namespace Core.TicTacToe.Declaration
{
    using System.Collections.Generic;

    using Core.TicTacToe.Constants;

    public interface IGameFieldCellsStatisticProvider
    {
        Dictionary<CellCondition, int> GetStatistic(CellCondition[,] gameField);
    }
}
