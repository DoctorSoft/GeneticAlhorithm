namespace Core.Bot.Genetic.Implementation
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    using Core.Bot.Genetic.Implementation.Declaration;
    using Core.Bot.Main.Implementation.Declaration;
    using Core.Bot.Main.Implementation.Helpers;
    using Core.TicTacToe.Constants;
    using Core.TicTacToe.Declaration;
    using Core.TicTacToe.Models;

    using Data.Migration.Contexts;
    using Data.Model.GeneticBot;
    using Data.Model.TicTacToe;

    public class GeneticBotDeveloper : IGeneticBotDeveloper
    {
        private readonly INewGameFieldCreator newGameFieldCreator;

        private readonly IFieldStateConverter fieldStateConverter;

        private readonly IGeneticBot geneticBot;

        private readonly IStepMaker stepMaker;

        private readonly IGameProcessStatisticProvider gameProcessStatisticProvider;

        private readonly IMonkeyBot monkeyBot;

        public GeneticBotDeveloper(
            INewGameFieldCreator newGameFieldCreator,
            IFieldStateConverter fieldStateConverter,
            IGeneticBot geneticBot,
            IStepMaker stepMaker,
            IGameProcessStatisticProvider gameProcessStatisticProvider,
            IMonkeyBot monkeyBot)
        {
            this.newGameFieldCreator = newGameFieldCreator;
            this.fieldStateConverter = fieldStateConverter;
            this.geneticBot = geneticBot;
            this.stepMaker = stepMaker;
            this.gameProcessStatisticProvider = gameProcessStatisticProvider;
            this.monkeyBot = monkeyBot;
        }

        public void GenerateNextGeneration(TicTacToeContext context)
        {
            var indivadualIds = this.GetIndividualIds(context);

            foreach (var indivadualId in indivadualIds)
            {
                var isCross = true;

                for (var gameNumber = 0; gameNumber < 20; gameNumber++)
                {
                    var gameId = this.CreateNewGame(context);
                    var isGeneticBotTurn = isCross;
                    GameProcessStatistic statistic = null;

                    do
                    {
                        statistic = isGeneticBotTurn ? this.MakeGeneticBotStep(gameId, context) : this.MakeMonkeyBotStep(gameId, context);
                        isGeneticBotTurn = !isGeneticBotTurn;
                    }
                    while (statistic.GameStatus == GameStatus.InProgress);

                    isCross = !isCross;
                }
            }

            var winners = this.TakeThreeWinners(context);
        }

        public List<GeneticIndividual> TakeThreeWinners(TicTacToeContext context)
        {
            var result = context.Set<GeneticIndividual>()
                .OrderBy(individual => individual.Score / individual.PlayedGames)
                .Take(3)
                .ToList();

            for (var order = 0; order < result.Count; order++)
            {
                result[order].ImportanceOrder = order;
            }

            return result;
        } 

        public List<int> GetIndividualIds(TicTacToeContext context)
        {
            var result = context.Set<GeneticIndividual>().Select(individual => individual.GeneticIndividualId).ToList();
            return result;
        }

        public int CreateNewGame(TicTacToeContext context)
        {
            var gameField = this.newGameFieldCreator.CreateNewGameField();
            var fieldCode = this.fieldStateConverter.GameFieldToString(gameField);

            var field = GameHelper.GetFieldByCode(fieldCode, context);
            var number = GameHelper.GetCodeNumber(fieldCode, field);

            var game = new Game { Field = field, FieldNumber = number, Proccess = field.FieldId.ToString() };
            context.Set<Game>().Add(game);
            context.SaveChanges();

            var gameId = game.GameId;

            return gameId;
        }

        public GameProcessStatistic MakeGeneticBotStep(int gameId, TicTacToeContext context)
        {
            var game = context.Set<Game>().Include(game1 => game1.Field).FirstOrDefault(game1 => game1.GameId == gameId);
            var field = game.Field;

            var fieldCode = GameHelper.GetFieldByNumber(game.FieldNumber, field);
            var gameField = this.fieldStateConverter.StringToGameField(fieldCode);

            var nextStep = this.geneticBot.GetStep(gameField, context);
            var nextGameField = this.stepMaker.MakeStep(gameField, nextStep.X, nextStep.Y);

            var nextFieldCode = this.fieldStateConverter.GameFieldToString(nextGameField);
            var nextField = GameHelper.GetFieldByCode(nextFieldCode, context);
            var nextNumber = GameHelper.GetCodeNumber(nextFieldCode, nextField);

            var result = this.gameProcessStatisticProvider.GetGameProcessStatistic(nextGameField);

            game.Field = nextField;
            game.FieldNumber = nextNumber;
            game.Proccess += "|" + nextField.FieldId;
            context.Set<Game>().AddOrUpdate(game);
            context.SaveChanges();


            return result;
        }

        public GameProcessStatistic MakeMonkeyBotStep(int gameId, TicTacToeContext context)
        {
            var game = context.Set<Game>().Include(game1 => game1.Field).FirstOrDefault(game1 => game1.GameId == gameId);
            var field = game.Field;

            var fieldCode = GameHelper.GetFieldByNumber(game.FieldNumber, field);
            var gameField = this.fieldStateConverter.StringToGameField(fieldCode);

            var nextStep = this.monkeyBot.GetStep(gameField);
            var nextGameField = this.stepMaker.MakeStep(gameField, nextStep.X, nextStep.Y);

            var nextFieldCode = this.fieldStateConverter.GameFieldToString(nextGameField);
            var nextField = GameHelper.GetFieldByCode(nextFieldCode, context);
            var nextNumber = GameHelper.GetCodeNumber(nextFieldCode, nextField);

            var result = this.gameProcessStatisticProvider.GetGameProcessStatistic(nextGameField);

            game.Field = nextField;
            game.FieldNumber = nextNumber;
            game.Proccess += "|" + nextField.FieldId;
            context.Set<Game>().AddOrUpdate(game);
            context.SaveChanges();


            return result;
        }

        public void TakeDraw(int gameId, int geneticIndividualId, TicTacToeContext context)
        {
            var game = context.Set<Game>().Include(game1 => game1.Field).FirstOrDefault(game1 => game1.GameId == gameId);

            GameHelper.RefreshStatistic(game.GameId, GameStatus.Draw, context);

            context.Set<Game>().Remove(game);
            context.SaveChanges();

            var individual =
                context.Set<GeneticIndividual>()
                    .FirstOrDefault(geneticIndividual => geneticIndividual.GeneticIndividualId == geneticIndividualId);
            individual.Score += 0.5;
            individual.PlayedGames++;

            context.Set<GeneticIndividual>().AddOrUpdate(individual);
        }

        public void Win(int gameId, int geneticIndividualId, bool isCross, TicTacToeContext context)
        {
            var game = context.Set<Game>().Include(game1 => game1.Field).FirstOrDefault(game1 => game1.GameId == gameId);
            var field = game.Field;

            var fieldCode = GameHelper.GetFieldByNumber(game.FieldNumber, field);
            var gameField = this.fieldStateConverter.StringToGameField(fieldCode);

            var gameProcessStatistic = this.gameProcessStatisticProvider.GetGameProcessStatistic(gameField);

            GameHelper.RefreshStatistic(game.GameId, gameProcessStatistic.GameStatus, context);

            context.Set<Game>().Remove(game);
            context.SaveChanges();

            var fieldIds = game.Proccess.Split('|').Select(int.Parse).ToList();
            var winPowerFactor = 0.6 - fieldIds.Count * 0.1;
            var isCrossWinner = fieldIds.Count % 2 == 0;

            var individual =
                context.Set<GeneticIndividual>()
                    .FirstOrDefault(geneticIndividual => geneticIndividual.GeneticIndividualId == geneticIndividualId);
            individual.Score += (isCross == isCrossWinner) ? (1 + winPowerFactor) : (0 - winPowerFactor);
            individual.PlayedGames++;

            context.Set<GeneticIndividual>().AddOrUpdate(individual);
        }

    }
}
