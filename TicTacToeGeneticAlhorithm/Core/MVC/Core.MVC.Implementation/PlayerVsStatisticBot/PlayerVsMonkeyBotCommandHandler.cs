using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using Core.Bot.Main.Implementation.Helpers;
using Core.Bot.Statistic.Implementation.Declaration;
using Core.MVC.PlayerVsStatisticBot.Declarations;
using Core.MVC.PlayerVsStatisticBot.Models;
using Core.TicTacToe.Constants;
using Core.TicTacToe.Declaration;
using Data.Migration.Contexts;
using Data.Model.TicTacToe;

namespace Core.MVC.Implementation.PlayerVsStatisticBot
{
    public class PlayerVsStatisticBotCommandHandler : IPlayerVsStatisticBotCommandHandler
    {
        private readonly Random random;

        private readonly INewGameFieldCreator newGameFieldCreator;

        private readonly IFieldStateConverter fieldStateConverter;

        private readonly IStepMaker stepMaker;

        private readonly IGameProcessStatisticProvider gameProcessStatisticProvider;

        private readonly IStatisticBot statisticBot;

        public PlayerVsStatisticBotCommandHandler(INewGameFieldCreator newGameFieldCreator, IFieldStateConverter fieldStateConverter, IStepMaker stepMaker, IGameProcessStatisticProvider gameProcessStatisticProvider, IStatisticBot StatisticBot)
        {
            this.newGameFieldCreator = newGameFieldCreator;
            this.fieldStateConverter = fieldStateConverter;
            this.stepMaker = stepMaker;
            this.gameProcessStatisticProvider = gameProcessStatisticProvider;
            this.statisticBot = StatisticBot;
            this.random = new Random();
        }

        public PlayerVsStatisticBotNewGameCommandResult ExecuteCommand(PlayerVsStatisticBotNewGameCommand command)
        {
            PlayerVsStatisticBotNewGameCommandResult result;

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

                result = new PlayerVsStatisticBotNewGameCommandResult
                {
                    GameField = gameField,
                    GameId = gameId,
                    CellSize = GameFieldConstants.LineLength,
                    IsBotTurn = random.Next(2) == 0
                };
            }

            return result;
        }

        public PlayerVsStatisticBotMakeStepCommandResult ExecuteCommand(PlayerVsStatisticBotMakeStepCommand command)
        {
            PlayerVsStatisticBotMakeStepCommandResult result;

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
                    return new PlayerVsStatisticBotMakeStepCommandResult
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

                result = new PlayerVsStatisticBotMakeStepCommandResult
                {
                    CellSize = GameFieldConstants.LineLength,
                    GameId = command.GameId,
                    GameField = nextGameField,
                    GameProcessStatistic = gameProcessStatistic
                };
            }

            return result;
        }

        public PlayerVsStatisticBotMakeBotStepCommandResult ExecuteCommand(PlayerVsStatisticBotMakeBotStepCommand command)
        {
            PlayerVsStatisticBotMakeBotStepCommandResult result;

            using (var context = new TicTacToeContext())
            {
                var game = context.Set<Game>().Include(game1 => game1.Field).FirstOrDefault(game1 => game1.GameId == command.GameId);
                var field = game.Field;

                var fieldCode = GameHelper.GetFieldByNumber(game.FieldNumber, field);
                var gameField = this.fieldStateConverter.StringToGameField(fieldCode);

                var nextStep = statisticBot.GetStep(gameField, context);
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

                result = new PlayerVsStatisticBotMakeBotStepCommandResult
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

        public PlayerVsStatisticBotTakeDrawCommandResult ExecuteCommand(PlayerVsStatisticBotTakeDrawCommand command)
        {
            PlayerVsStatisticBotTakeDrawCommandResult result;

            using (var context = new TicTacToeContext())
            {
                var game = context.Set<Game>().Include(game1 => game1.Field).FirstOrDefault(game1 => game1.GameId == command.GameId);
                var field = game.Field;

                var fieldCode = GameHelper.GetFieldByNumber(game.FieldNumber, field);
                var gameField = this.fieldStateConverter.StringToGameField(fieldCode);

                GameHelper.RefreshStatistic(game.GameId, GameStatus.Draw, context);

                context.Set<Game>().Remove(game);
                context.SaveChanges();

                result = new PlayerVsStatisticBotTakeDrawCommandResult
                {
                    CellSize = GameFieldConstants.LineLength,
                    GameField = gameField,
                };
            }

            return result;
        }

        public PlayerVsStatisticBotWinGameCommandResult ExecuteCommand(PlayerVsStatisticBotWinGameCommand command)
        {
            PlayerVsStatisticBotWinGameCommandResult result;

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

                result = new PlayerVsStatisticBotWinGameCommandResult
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
