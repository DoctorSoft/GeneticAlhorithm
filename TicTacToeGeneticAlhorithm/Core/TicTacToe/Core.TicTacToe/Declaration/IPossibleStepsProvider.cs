namespace Core.TicTacToe.Declaration
{
    using System.Collections.Generic;

    using Core.TicTacToe.Constants;
    using Core.TicTacToe.Models;

    public interface IPossibleStepsProvider
    {
        List<Coordinates> GetPossibleSteps(CellCondition[,] gameField);
    }
}
