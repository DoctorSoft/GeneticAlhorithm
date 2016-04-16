using Core.MVC.Implementation.Helpers;

namespace Core.MVC.Implementation.PlayerVsPlayer
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    using Core.MVC.PlayerVsPlayer.Declarations;
    using Core.MVC.PlayerVsPlayer.Models;
    using Core.TicTacToe.Constants;
    using Core.TicTacToe.Declaration;

    using Data.Migration.Contexts;
    using Data.Model.TicTacToe;

    public class PlayerVsPlayerGameCommandHandler : IPlayerVsPlayerGameCommandHandler
    {
        private readonly INewGameFieldCreator newGameFieldCreator;

        private readonly IFieldStateConverter fieldStateConverter;

        private readonly IStepMaker stepMaker;

        private readonly IGameProcessStatisticProvider gameProcessStatisticProvider;

        public PlayerVsPlayerGameCommandHandler(INewGameFieldCreator newGameFieldCreator, IFieldStateConverter fieldStateConverter, IStepMaker stepMaker, IGameProcessStatisticProvider gameProcessStatisticProvider)
        {
            this.newGameFieldCreator = newGameFieldCreator;
            this.fieldStateConverter = fieldStateConverter;
            this.stepMaker = stepMaker;
            this.gameProcessStatisticProvider = gameProcessStatisticProvider;
        }

        public PlayerVsPlayerNewGameCommandResult ExecuteCommand(PlayerVsPlayerNewGameCommand command)
        {
            PlayerVsPlayerNewGameCommandResult result;

            using (var context = new TicTacToeContext())
            {
                var gameField = this.newGameFieldCreator.CreateNewGameField();
                var fieldCode = this.fieldStateConverter.GameFieldToString(gameField);

                var field = GameHelper.GetFieldByCode(fieldCode, context);
                var number = GameHelper.GetCodeNumber(fieldCode, field);

                var game = new Game { Field = field, FieldNumber = number };
                context.Set<Game>().Add(game);
                context.SaveChanges();

                var gameId = game.GameId;

                result = new PlayerVsPlayerNewGameCommandResult
                             {
                                 GameField = gameField,
                                 GameId = gameId,
                                 CellSize = GameFieldConstants.LineLength
                             };
            }

            return result;
        }

        public PlayerVsPlayerMakeStepCommandResult ExecuteCommand(PlayerVsPlayerMakeStepCommand command)
        {
            PlayerVsPlayerMakeStepCommandResult result;

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
                    return new PlayerVsPlayerMakeStepCommandResult
                               {
                                   CellSize = GameFieldConstants.LineLength,
                                   GameId = command.GameId,
                                   GameField = gameField,
                                   GameProcessStatistic = previousProcessStatistic
                    };

                }

                var nextFieldCode = this.fieldStateConverter.GameFieldToString(nextGameField);
                var nextField = GameHelper.GetFieldByCode(nextFieldCode, context);
                var nextNumber = GameHelper.GetCodeNumber(nextFieldCode, nextField);

                var gameProcessStatistic = this.gameProcessStatisticProvider.GetGameProcessStatistic(nextGameField);

                game.Field = nextField;
                game.FieldNumber = nextNumber;
                context.Set<Game>().AddOrUpdate(game);
                context.SaveChanges();

                result = new PlayerVsPlayerMakeStepCommandResult
                             {
                                 CellSize = GameFieldConstants.LineLength,
                                 GameId = command.GameId,
                                 GameField = nextGameField,
                                 GameProcessStatistic = gameProcessStatistic
                             };
            }

            return result;
        }

        public PlayerVsPlayerWinGameCommandResult ExecuteCommand(PlayerVsPlayerWinGameCommand command)
        {
            PlayerVsPlayerWinGameCommandResult result;

            using (var context = new TicTacToeContext())
            {
                var game = context.Set<Game>().Include(game1 => game1.Field).FirstOrDefault(game1 => game1.GameId == command.GameId);
                var field = game.Field;

                var fieldCode = GameHelper.GetFieldByNumber(game.FieldNumber, field);
                var gameField = this.fieldStateConverter.StringToGameField(fieldCode);

                var gameProcessStatistic = this.gameProcessStatisticProvider.GetGameProcessStatistic(gameField);

                var winCoordinates = GameHelper.GetWinCoordinates(gameProcessStatistic);

                context.Set<Game>().Remove(game);
                context.SaveChanges();

                result = new PlayerVsPlayerWinGameCommandResult
                {
                    CellSize = GameFieldConstants.LineLength,
                    GameField = gameField,
                    WinCoordinates = winCoordinates
                };
            }

            return result;
        }

        public PlayerVsPlayerTakeDrawCommandResult ExecuteCommand(PlayerVsPlayerTakeDrawCommand command)
        {
            PlayerVsPlayerTakeDrawCommandResult result;

            using (var context = new TicTacToeContext())
            {
                var game = context.Set<Game>().Include(game1 => game1.Field).FirstOrDefault(game1 => game1.GameId == command.GameId);
                var field = game.Field;

                var fieldCode = GameHelper.GetFieldByNumber(game.FieldNumber, field);
                var gameField = this.fieldStateConverter.StringToGameField(fieldCode);

                context.Set<Game>().Remove(game);
                context.SaveChanges();

                result = new PlayerVsPlayerTakeDrawCommandResult
                {
                    CellSize = GameFieldConstants.LineLength,
                    GameField = gameField,
                };
            }

            return result;
        }
        
    }
}
