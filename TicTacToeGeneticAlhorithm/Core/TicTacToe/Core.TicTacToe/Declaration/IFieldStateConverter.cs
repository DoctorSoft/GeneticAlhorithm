namespace Core.TicTacToe.Declaration
{
    using System.Collections.Generic;

    using Core.TicTacToe.Constants;

    public interface IFieldStateConverter
    {
        string GameFieldToString(CellCondition[,] gameField);

        CellCondition[,] StringToGameField(string value);

        List<string> GetSimiliarGameFieldStringList(CellCondition[,] gameField);
    }
}
