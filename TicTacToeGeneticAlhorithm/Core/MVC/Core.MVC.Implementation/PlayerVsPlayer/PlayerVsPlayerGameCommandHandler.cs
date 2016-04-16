﻿using System.Collections.Generic;
using Core.TicTacToe.Models;

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

                var field = this.GetFieldByCode(fieldCode, context);
                var number = this.GetCodeNumber(fieldCode, field);

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

                var fieldCode = this.GetFieldByNumber(game.FieldNumber, field);
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
                var nextField = this.GetFieldByCode(nextFieldCode, context);
                var nextNumber = this.GetCodeNumber(nextFieldCode, nextField);

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

                var fieldCode = this.GetFieldByNumber(game.FieldNumber, field);
                var gameField = this.fieldStateConverter.StringToGameField(fieldCode);

                var gameProcessStatistic = this.gameProcessStatisticProvider.GetGameProcessStatistic(gameField);

                var winCoordinates = this.GetWinCoordinates(gameProcessStatistic);

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

                var fieldCode = this.GetFieldByNumber(game.FieldNumber, field);
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

        private List<Coordinates> GetWinCoordinates(GameProcessStatistic gameProcessStatistic)
        {
            var resultList = new List<Coordinates>();

            if (gameProcessStatistic.WinStatistic.MoveDirection == MoveDirection.Left)
            {
                for (var x = 0; x < GameFieldConstants.CellsCountToWin; x++)
                {
                    resultList.Add(new Coordinates
                    {
                        X = gameProcessStatistic.WinStatistic.X - x,
                        Y = gameProcessStatistic.WinStatistic.Y
                    });
                }

                return resultList;
            }

            if (gameProcessStatistic.WinStatistic.MoveDirection == MoveDirection.Up)
            {
                for (var y = 0; y < GameFieldConstants.CellsCountToWin; y++)
                {
                    resultList.Add(new Coordinates
                    {
                        X = gameProcessStatistic.WinStatistic.X,
                        Y = gameProcessStatistic.WinStatistic.Y - y 
                    });
                }

                return resultList;
            }

            if (gameProcessStatistic.WinStatistic.MoveDirection == MoveDirection.LeftUp)
            {
                for (var factor = 0; factor < GameFieldConstants.CellsCountToWin; factor++)
                {
                    resultList.Add(new Coordinates
                    {
                        X = gameProcessStatistic.WinStatistic.X - factor,
                        Y = gameProcessStatistic.WinStatistic.Y - factor
                    });
                }

                return resultList;
            }

            for (var factor = 0; factor < GameFieldConstants.CellsCountToWin; factor++)
            {
                resultList.Add(new Coordinates
                {
                    X = gameProcessStatistic.WinStatistic.X + factor,
                    Y = gameProcessStatistic.WinStatistic.Y - factor
                });
            }

            return resultList;
        }

        private Field GetFieldByCode(string fieldCode, TicTacToeContext context)
        {
            return context.Set<Field>().FirstOrDefault(field1 => field1.FirstVariant == fieldCode
                                                                          || field1.SecondVariant == fieldCode
                                                                          || field1.ThirdVariant == fieldCode
                                                                          || field1.FourthVariant == fieldCode);
        }

        private int GetCodeNumber(string fieldCode, Field field)
        {
            if (field.FirstVariant == fieldCode)
            {
                return 1;
            }

            if (field.SecondVariant == fieldCode)
            {
                return 2;
            }

            if (field.ThirdVariant == fieldCode)
            {
                return 3;
            }

            return 4;
        }

        private string GetFieldByNumber(int number, Field field)
        {
            switch (number)
            {
                case 1:
                    return field.FirstVariant;
                case 2:
                    return field.SecondVariant;
                case 3:
                    return field.ThirdVariant;
            }

            return field.FourthVariant;
        }
    }
}
