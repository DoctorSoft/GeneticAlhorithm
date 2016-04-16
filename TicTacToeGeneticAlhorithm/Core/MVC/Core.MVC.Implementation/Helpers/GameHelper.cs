using System.Collections.Generic;
using System.Linq;
using Core.TicTacToe.Constants;
using Core.TicTacToe.Models;
using Data.Migration.Contexts;
using Data.Model.TicTacToe;

namespace Core.MVC.Implementation.Helpers
{
    public static class GameHelper
    {
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
            return context.Set<Field>().FirstOrDefault(field1 => field1.FirstVariant == fieldCode
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
