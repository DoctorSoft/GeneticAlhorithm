namespace Core.TicTacToe.Declaration
{
    using Core.TicTacToe.Constants;

    public interface INextStepConditionCalculator
    {
        CellCondition GetNextTurnCondition(CellCondition[,] gameField);
    }
}
