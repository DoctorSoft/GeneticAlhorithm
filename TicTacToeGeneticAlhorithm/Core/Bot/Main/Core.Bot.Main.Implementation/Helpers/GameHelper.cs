using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using Core.TicTacToe.Constants;
using Core.TicTacToe.Models;
using Data.Migration.Contexts;
using Data.Model.StatisticBot;
using Data.Model.TicTacToe;

namespace Core.Bot.Main.Implementation.Helpers
{
    public static class GameHelper
    {
        public static void RefreshStatistic(int gameId, GameStatus status, TicTacToeContext context)
        {
            var game = context.Set<Game>().FirstOrDefault(game1 => game1.GameId == gameId);

            var fieldIds = game.Proccess.Split('|').Select(int.Parse).ToList();
            //// var winPowerFactor = 0.6 - fieldIds.Count * 0.1;

            if (status == GameStatus.CircleWon)
            {
                var isCircle = true;
                foreach (var fieldId in fieldIds)
                {
                    var fieldStatistic = context.Set<FieldStatistic>().FirstOrDefault(field1 => field1.FieldId == fieldId);
                    if (isCircle)
                    {
                        fieldStatistic.Score += 1; //// + winPowerFactor;
                    }
                    else
                    {
                        fieldStatistic.Score += 0; //// - winPowerFactor;
                    }

                    fieldStatistic.PlayedGames++;

                    context.Set<FieldStatistic>().AddOrUpdate(fieldStatistic);
                    context.SaveChanges();
                    isCircle = !isCircle;
                }
                return;
            }

            if (status == GameStatus.CrossWon)
            {
                var isCross = false;
                foreach (var fieldId in fieldIds)
                {
                    var fieldStatistic = context.Set<FieldStatistic>().FirstOrDefault(field1 => field1.FieldId == fieldId);
                    if (isCross)
                    {
                        fieldStatistic.Score += 1; //// + winPowerFactor;
                    }
                    else
                    {
                        fieldStatistic.Score += 0; //// - winPowerFactor;
                    }

                    fieldStatistic.PlayedGames++;

                    context.Set<FieldStatistic>().AddOrUpdate(fieldStatistic);
                    context.SaveChanges();
                    isCross = !isCross;
                }
                return;
            }

            foreach (var fieldId in fieldIds)
            {
                var fieldStatistic = context.Set<FieldStatistic>().FirstOrDefault(field1 => field1.FieldId == fieldId);

                fieldStatistic.Score += 0.5;
                fieldStatistic.PlayedGames++;

                context.Set<FieldStatistic>().AddOrUpdate(fieldStatistic);
                context.SaveChanges();
            }
        } 

        public static List<Coordinates> GetWinCoordinates(GameProcessStatistic gameProcessStatistic)
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

        public static Field GetFieldByCode(string fieldCode, TicTacToeContext context)
        {
            return context.Set<Field>().Include(field => field.FieldStatistic).FirstOrDefault(field1 => field1.FirstVariant == fieldCode
                                                                                              || field1.SecondVariant == fieldCode
                                                                                              || field1.ThirdVariant == fieldCode
                                                                                              || field1.FourthVariant == fieldCode);
        }

        public static int GetCodeNumber(string fieldCode, Field field)
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

        public static string GetFieldByNumber(int number, Field field)
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
