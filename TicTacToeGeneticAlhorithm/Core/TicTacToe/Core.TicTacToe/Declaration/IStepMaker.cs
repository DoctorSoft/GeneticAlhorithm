namespace Core.TicTacToe.Declaration
{
    using Core.TicTacToe.Constants;

    public interface IStepMaker
    {
        CellCondition[,] MakeStep(CellCondition[,] gameFiled, int x, int y);
    }
}
