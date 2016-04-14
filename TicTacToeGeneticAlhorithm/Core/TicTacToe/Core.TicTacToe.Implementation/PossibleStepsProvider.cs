namespace Core.TicTacToe.Implementation
{
    using System.Collections.Generic;

    using Core.TicTacToe.Constants;
    using Core.TicTacToe.Declaration;
    using Core.TicTacToe.Models;

    public class PossibleStepsProvider : IPossibleStepsProvider
    {
        public List<Coordinates> GetPossibleSteps(CellCondition[,] gameField)
        {
            var coordinatesList = new List<Coordinates>();

            for (var x = 0; x <= GameFieldConstants.MaxCoordinate; x++)
            {
                for (var y = 0; y <= GameFieldConstants.MaxCoordinate; y++)
                {
                    if (gameField[x, y] == CellCondition.Empty)
                    {
                        coordinatesList.Add(new Coordinates { X = x, Y = y });
                    }
                }
            }

            return coordinatesList;
        }
    }
}
