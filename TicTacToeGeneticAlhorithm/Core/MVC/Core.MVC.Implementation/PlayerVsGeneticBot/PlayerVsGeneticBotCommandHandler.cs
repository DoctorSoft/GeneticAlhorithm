using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

using Core.Bot.Main.Implementation.Helpers;
using Core.MVC.PlayerVsGeneticBot.Declarations;
using Core.MVC.PlayerVsGeneticBot.Models;
using Core.TicTacToe.Constants;
using Core.TicTacToe.Declaration;
using Data.Migration.Contexts;
using Data.Model.TicTacToe;

namespace Core.MVC.Implementation.PlayerVsGeneticBot
{
    using Core.Bot.Genetic.Implementation.Declaration;

    public class PlayerVsGeneticBotCommandHandler : IPlayerVsGeneticBotCommandHandler
    {
        private readonly Random random;

        private readonly INewGameFieldCreator newGameFieldCreator;

        private readonly IFieldStateConverter fieldStateConverter;

        private readonly IStepMaker stepMaker;

        private readonly IGameProcessStatisticProvider gameProcessStatisticProvider;

        private readonly IGeneticBot GeneticBot;

        public PlayerVsGeneticBotCommandHandler(INewGameFieldCreator newGameFieldCreator, IFieldStateConverter fieldStateConverter, IStepMaker stepMaker, IGameProcessStatisticProvider gameProcessStatisticProvider, IGeneticBot GeneticBot)
        {
            this.newGameFieldCreator = newGameFieldCreator;
            this.fieldStateConverter = fieldStateConverter;
            this.stepMaker = stepMaker;
            this.gameProcessStatisticProvider = gameProcessStatisticProvider;
            this.GeneticBot = GeneticBot;
            this.random = new Random();
        }

        public PlayerVsGeneticBotNewGameCommandResult ExecuteCommand(PlayerVsGeneticBotNewGameCommand command)
        {
            PlayerVsGeneticBotNewGameCommandResult result;

            using (var context = new TicTacToeContext())
            {
                var gameField = this.newGameFieldCreator.CreateNewGameField();
                var fieldCode = this.fieldStateConverter.GameFieldToString(gameField);

                var field = GameHelper.GetFieldByCode(fieldCode, context);
                var number = GameHelper.GetCodeNumber(fieldCode, field);

                var game = new Game
                {
                    Field = field, 
                    FieldNumber = number, 
                    Proccess = field.FieldId.ToString()
                };
                context.Set<Game>().Add(game);
                context.SaveChanges();

                var gameId = game.GameId;

                result = new PlayerVsGeneticBotNewGameCommandResult
                {
                    GameField = gameField,
                    GameId = gameId,
                    CellSize = GameFieldConstants.LineLength,
                    IsBotTurn = random.Next(2) == 0
                };
            }

            return result;
        }

        public PlayerVsGeneticBotMakeStepCommandResult ExecuteCommand(PlayerVsGeneticBotMakeStepCommand command)
        {
            PlayerVsGeneticBotMakeStepCommandResult result;

            using (var context = new TicTacToeContext())
            {
                var game = context.Set<Game>().Include(game1 => game1.Field).FirstOrDefault(game1 => game1.GameId == command.GameId);
                var field = game.Field;

                var fieldCode = GameHelper.GetFieldByNumber(game.FieldNumber, field);
                var gameField = this.fieldStateConverter.StringToGameField(fieldCode);

                CellCondition[,] nextGameField;

                try
                {
                    nextGameField = this.stepMaker.MakeStep(gameField, command.X, command.Y);
                }
                catch (Exception)
                {
                    var previousProcessStatistic = this.gameProcessStatisticProvider.GetGameProcessStatistic(gameField);
                    return new PlayerVsGeneticBotMakeStepCommandResult
                    {
                        CellSize = GameFieldConstants.LineLength,
                        GameId = command.GameId,
                        GameField = gameField,
                        GameProcessStatistic = previousProcessStatistic,
                    };

                }

                var nextFieldCode = this.fieldStateConverter.GameFieldToString(nextGameField);
                var nextField = GameHelper.GetFieldByCode(nextFieldCode, context);
                var nextNumber = GameHelper.GetCodeNumber(nextFieldCode, nextField);

                var gameProcessStatistic = this.gameProcessStatisticProvider.GetGameProcessStatistic(nextGameField);

                game.Field = nextField;
                game.FieldNumber = nextNumber;
                game.Proccess += "|" + nextField.FieldId;
                context.Set<Game>().AddOrUpdate(game);
                context.SaveChanges();

                result = new PlayerVsGeneticBotMakeStepCommandResult
                {
                    CellSize = GameFieldConstants.LineLength,
                    GameId = command.GameId,
                    GameField = nextGameField,
                    GameProcessStatistic = gameProcessStatistic
                };
            }

            return result;
        }

        public PlayerVsGeneticBotMakeBotStepCommandResult ExecuteCommand(PlayerVsGeneticBotMakeBotStepCommand command)
        {
            PlayerVsGeneticBotMakeBotStepCommandResult result;

            using (var context = new TicTacToeContext())
            {
                var game = context.Set<Game>().Include(game1 => game1.Field).FirstOrDefault(game1 => game1.GameId == command.GameId);
                var field = game.Field;

                var fieldCode = GameHelper.GetFieldByNumber(game.FieldNumber, field);
                var gameField = this.fieldStateConverter.StringToGameField(fieldCode);

                var nextStep = GeneticBot.GetStep(gameField, context);
                var nextGameField = this.stepMaker.MakeStep(gameField, nextStep.X, nextStep.Y);

                var nextFieldCode = this.fieldStateConverter.GameFieldToString(nextGameField);
                var nextField = GameHelper.GetFieldByCode(nextFieldCode, context);
                var nextNumber = GameHelper.GetCodeNumber(nextFieldCode, nextField);

                var gameProcessStatistic = this.gameProcessStatisticProvider.GetGameProcessStatistic(nextGameField);

                game.Field = nextField;
                game.FieldNumber = nextNumber;
                game.Proccess += "|" + nextField.FieldId;
                context.Set<Game>().AddOrUpdate(game);
                context.SaveChanges();

                result = new PlayerVsGeneticBotMakeBotStepCommandResult
                {
                    CellSize = GameFieldConstants.LineLength,
                    GameId = command.GameId,
                    GameField = nextGameField,
                    GameProcessStatistic = gameProcessStatistic,
                    BotStep = nextStep
                };
            }

            return result;
        }

        public PlayerVsGeneticBotTakeDrawCommandResult ExecuteCommand(PlayerVsGeneticBotTakeDrawCommand command)
        {
            PlayerVsGeneticBotTakeDrawCommandResult result;

            using (var context = new TicTacToeContext())
            {
                var game = context.Set<Game>().Include(game1 => game1.Field).FirstOrDefault(game1 => game1.GameId == command.GameId);
                var field = game.Field;

                var fieldCode = GameHelper.GetFieldByNumber(game.FieldNumber, field);
                var gameField = this.fieldStateConverter.StringToGameField(fieldCode);

                GameHelper.RefreshStatistic(game.GameId, GameStatus.Draw, context);

                context.Set<Game>().Remove(game);
                context.SaveChanges();

                result = new PlayerVsGeneticBotTakeDrawCommandResult
                {
                    CellSize = GameFieldConstants.LineLength,
                    GameField = gameField,
                };
            }

            return result;
        }

        public PlayerVsGeneticBotWinGameCommandResult ExecuteCommand(PlayerVsGeneticBotWinGameCommand command)
        {
            PlayerVsGeneticBotWinGameCommandResult result;

            using (var context = new TicTacToeContext())
            {
                var game = context.Set<Game>().Include(game1 => game1.Field).FirstOrDefault(game1 => game1.GameId == command.GameId);
                var field = game.Field;

                var fieldCode = GameHelper.GetFieldByNumber(game.FieldNumber, field);
                var gameField = this.fieldStateConverter.StringToGameField(fieldCode);

                var gameProcessStatistic = this.gameProcessStatisticProvider.GetGameProcessStatistic(gameField);

                var winCoordinates = GameHelper.GetWinCoordinates(gameProcessStatistic);

                GameHelper.RefreshStatistic(game.GameId, gameProcessStatistic.GameStatus, context);

                context.Set<Game>().Remove(game);
                context.SaveChanges();

                result = new PlayerVsGeneticBotWinGameCommandResult
                {
                    CellSize = GameFieldConstants.LineLength,
                    GameField = gameField,
                    WinCoordinates = winCoordinates
                };
            }

            return result;
        }
    }
}
