namespace Core.TicTacToe.Declaration
{
    using Core.TicTacToe.Constants;

    public interface IGameFieldCopyMaker
    {
        CellCondition[,] MakeFieldCopy(CellCondition[,] field);
    }
}
