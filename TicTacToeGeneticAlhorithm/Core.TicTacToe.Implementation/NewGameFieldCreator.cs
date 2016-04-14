namespace Core.TicTacToe.Implementation
{
    using Core.TicTacToe.Constants;
    using Core.TicTacToe.Declaration;

    public class NewGameFieldCreator : INewGameFieldCreator
    { 
        public CellCondition[,] CreateNewGameField()
        {
            var gameField = new CellCondition[GameFieldConstants.LineLength, GameFieldConstants.LineLength];

            for (var x = 0; x <= GameFieldConstants.MaxCoordinate; x++)
            {
                for (var y = 0; y <= GameFieldConstants.MaxCoordinate; y++)
                {
                    gameField[x, y] = CellCondition.Empty;
                }
            }

            return gameField;
        }
    }
}
