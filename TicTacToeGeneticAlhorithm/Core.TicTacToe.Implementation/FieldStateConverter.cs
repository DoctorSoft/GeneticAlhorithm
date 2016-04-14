namespace Core.TicTacToe.Implementation
{
    using Core.TicTacToe.Constants;
    using Core.TicTacToe.Declaration;

    public class FieldStateConverter : IFieldStateConverter
    {
        private readonly INewGameFieldCreator newGameFieldCreator;

        public FieldStateConverter(INewGameFieldCreator newGameFieldCreator)
        {
            this.newGameFieldCreator = newGameFieldCreator;
        }

        public string GameFieldToString(CellCondition[,] gameField)
        {
            var resultLine = string.Empty;

            for (var x = GameFieldConstants.MinXCoordinate; x <= GameFieldConstants.MaxXCoordinate; x++)
            {
                for (var y = GameFieldConstants.MinYCoordinate; y <= GameFieldConstants.MaxYCoordinate; y++)
                {
                    switch (gameField[x, y])
                    {
                        case CellCondition.Cross:
                            resultLine += GameFieldConvertingNames.Cross;
                            continue;
                        case CellCondition.Circle:
                            resultLine += GameFieldConvertingNames.Circle;
                            continue;
                    }

                    resultLine += GameFieldConvertingNames.Empty;
                }
            }

            return resultLine;
        }

        public CellCondition[,] StringToGameField(string value)
        {
            var gameField = this.newGameFieldCreator.CreateNewGameField();

            var x = GameFieldConstants.MinXCoordinate;
            var y = GameFieldConstants.MinYCoordinate;

            foreach (var nextSymbol in value)
            {
                if (x >= GameFieldConstants.MaxXCoordinate)
                {
                    x = GameFieldConstants.MinXCoordinate;
                    y++;

                    if (y > GameFieldConstants.MaxYCoordinate)
                    {
                        break;
                    }
                }
                else
                {
                    x++;
                }

                var cellCondition = CellCondition.Empty;

                if (nextSymbol == GameFieldConvertingNames.Cross)
                {
                    cellCondition = CellCondition.Cross;
                }

                if (nextSymbol == GameFieldConvertingNames.Circle)
                {
                    cellCondition = CellCondition.Circle;
                }

                gameField[x, y] = cellCondition;
            }

            return gameField;
        }
    }
}
