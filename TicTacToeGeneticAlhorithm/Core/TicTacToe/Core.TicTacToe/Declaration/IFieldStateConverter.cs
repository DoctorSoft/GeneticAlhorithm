namespace Core.TicTacToe.Declaration
{
    using Core.TicTacToe.Constants;

    public interface IFieldStateConverter
    {
        string GameFieldToString(CellCondition[,] gameField);

        CellCondition[,] StringToGameField(string value);
    }
}
