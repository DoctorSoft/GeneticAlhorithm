namespace Core.TicTacToe.Declaration
{
    using Core.TicTacToe.Constants;

    public interface INewGameFieldCreator
    {
        CellCondition[,] CreateNewGameField();
    }
}
