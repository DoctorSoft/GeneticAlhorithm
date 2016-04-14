namespace Core.TicTacToe.Implementation
{
    using Core.TicTacToe.Constants;
    using Core.TicTacToe.Declaration;

    public class GameFieldTransparator : IGameFieldTransparator
    {
        private readonly INewGameFieldCreator newGameFieldCreator;

        public GameFieldTransparator(INewGameFieldCreator newGameFieldCreator)
        {
            this.newGameFieldCreator = newGameFieldCreator;
        }

        public CellCondition[,] GetTransparation(CellCondition[,] gameField)
        {
            var transparatedGameField = this.newGameFieldCreator.CreateNewGameField();

            for (var x = 0; x <= GameFieldConstants.MaxCoordinate; x++)
            {
                for (var y = 0; y <= GameFieldConstants.MaxCoordinate; y++)
                {
                    transparatedGameField[x, y] = gameField[y, GameFieldConstants.MaxCoordinate - x];
                }
            }

            return transparatedGameField;
        }
    }
}
