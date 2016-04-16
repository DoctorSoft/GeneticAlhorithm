using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using Core.Bot.Main.Implementation.Declaration;
using Core.Bot.Main.Implementation.Helpers;
using Core.MVC.PlayerVsMonkeyBot.Declarations;
using Core.MVC.PlayerVsMonkeyBot.Models;
using Core.TicTacToe.Constants;
using Core.TicTacToe.Declaration;
using Data.Migration.Contexts;
using Data.Model.TicTacToe;

namespace Core.MVC.Implementation.PlayerVsMonkeyBot
{
    public class PlayerVsMonkeyBotCommandHandler : IPlayerVsMonkeyBotCommandHandler
    {
        private readonly Random random;

        private readonly INewGameFieldCreator newGameFieldCreator;

        private readonly IFieldStateConverter fieldStateConverter;

        private readonly IStepMaker stepMaker;

        private readonly IGameProcessStatisticProvider gameProcessStatisticProvider;

        private readonly IMonkeyBot monkeyBot;

        public PlayerVsMonkeyBotCommandHandler(INewGameFieldCreator newGameFieldCreator, IFieldStateConverter fieldStateConverter, IStepMaker stepMaker, IGameProcessStatisticProvider gameProcessStatisticProvider, IMonkeyBot monkeyBot)
        {
            this.newGameFieldCreator = newGameFieldCreator;
            this.fieldStateConverter = fieldStateConverter;
            this.stepMaker = stepMaker;
            this.gameProcessStatisticProvider = gameProcessStatisticProvider;
            this.monkeyBot = monkeyBot;
            this.random = new Random();
        }

        public PlayerVsMonkeyBotNewGameCommandResult ExecuteCommand(PlayerVsMonkeyBotNewGameCommand command)
        {
            PlayerVsMonkeyBotNewGameCommandResult result;

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

                result = new PlayerVsMonkeyBotNewGameCommandResult
                {
                    GameField = gameField,
                    GameId = gameId,
                    CellSize = GameFieldConstants.LineLength,
                    IsBotTurn = random.Next(2) == 0
                };
            }

            return result;
        }

        public PlayerVsMonkeyBotMakeStepCommandResult ExecuteCommand(PlayerVsMonkeyBotMakeStepCommand command)
        {
            PlayerVsMonkeyBotMakeStepCommandResult result;

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
                    return new PlayerVsMonkeyBotMakeStepCommandResult
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

                result = new PlayerVsMonkeyBotMakeStepCommandResult
                {
                    CellSize = GameFieldConstants.LineLength,
                    GameId = command.GameId,
                    GameField = nextGameField,
                    GameProcessStatistic = gameProcessStatistic
                };
            }

            return result;
        }

        public PlayerVsMonkeyBotMakeBotStepCommandResult ExecuteCommand(PlayerVsMonkeyBotMakeBotStepCommand command)
        {
            PlayerVsMonkeyBotMakeBotStepCommandResult result;

            using (var context = new TicTacToeContext())
            {
                var game = context.Set<Game>().Include(game1 => game1.Field).FirstOrDefault(game1 => game1.GameId == command.GameId);
                var field = game.Field;

                var fieldCode = GameHelper.GetFieldByNumber(game.FieldNumber, field);
                var gameField = this.fieldStateConverter.StringToGameField(fieldCode);

                var nextStep = monkeyBot.GetStep(gameField);
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

                result = new PlayerVsMonkeyBotMakeBotStepCommandResult
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

        public PlayerVsMonkeyBotTakeDrawCommandResult ExecuteCommand(PlayerVsMonkeyBotTakeDrawCommand command)
        {
            PlayerVsMonkeyBotTakeDrawCommandResult result;

            using (var context = new TicTacToeContext())
            {
                var game = context.Set<Game>().Include(game1 => game1.Field).FirstOrDefault(game1 => game1.GameId == command.GameId);
                var field = game.Field;

                var fieldCode = GameHelper.GetFieldByNumber(game.FieldNumber, field);
                var gameField = this.fieldStateConverter.StringToGameField(fieldCode);

                GameHelper.RefreshStatistic(game.GameId, GameStatus.Draw, context);

                context.Set<Game>().Remove(game);
                context.SaveChanges();

                result = new PlayerVsMonkeyBotTakeDrawCommandResult
                {
                    CellSize = GameFieldConstants.LineLength,
                    GameField = gameField,
                };
            }

            return result;
        }

        public PlayerVsMonkeyBotWinGameCommandResult ExecuteCommand(PlayerVsMonkeyBotWinGameCommand command)
        {
            PlayerVsMonkeyBotWinGameCommandResult result;

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

                result = new PlayerVsMonkeyBotWinGameCommandResult
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
