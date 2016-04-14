namespace Core.TicTacToe.Declaration
{
    using Core.TicTacToe.Constants;

    public interface IGameFieldTransparator
    {
        CellCondition[,] GetTransparation(CellCondition[,] gameField);
    }
}
