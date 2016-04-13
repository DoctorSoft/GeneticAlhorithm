namespace Core.TicTacToe.Declaration
{
    using Core.TicTacToe.Constants;

    public interface INextStepCalculator
    {
        CellCondition GetNextTurnCondition(CellCondition[,] gameFiled);
    }
}
