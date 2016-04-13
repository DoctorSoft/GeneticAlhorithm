namespace Core.TicTacToe.Implementation
{
    using Core.TicTacToe.Constants;
    using Core.TicTacToe.Declaration;

    public class NewGameFieldCreator : INewGameFieldCreator
    { 
        public CellCondition[,] CreateNewGameField()
        {
            var gameField = new CellCondition[GameFieldConstants.MaxXCoordinate + 1, GameFieldConstants.MaxYCoordinate + 1];

            for (var x = GameFieldConstants.MinXCoordinate; x <= GameFieldConstants.MaxXCoordinate; x++)
            {
                for (var y = GameFieldConstants.MinYCoordinate; y <= GameFieldConstants.MaxYCoordinate; y++)
                {
                    gameField[x, y] = CellCondition.Empty;
                }
            }

            return gameField;
        }
    }
}
