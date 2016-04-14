namespace Core.TicTacToe.Implementation
{
    using System.Collections.Generic;

    using Core.TicTacToe.Constants;
    using Core.TicTacToe.Declaration;

    public class GameFieldCellsStatisticProvider : IGameFieldCellsStatisticProvider
    {
        public Dictionary<CellCondition, int> GetStatistic(CellCondition[,] gameField)
        {
            var statistic = new Dictionary<CellCondition, int>
                                {
                                    {CellCondition.Circle, 0},
                                    {CellCondition.Cross, 0},
                                    {CellCondition.Empty, 0}
                                };

            for (var x = 0; x <= GameFieldConstants.MaxCoordinate; x++)
            {
                for (var y = 0; y <= GameFieldConstants.MaxCoordinate; y++)
                {
                    statistic[gameField[x, y]]++;
                }
            }

            return statistic;
        }
    }
}
