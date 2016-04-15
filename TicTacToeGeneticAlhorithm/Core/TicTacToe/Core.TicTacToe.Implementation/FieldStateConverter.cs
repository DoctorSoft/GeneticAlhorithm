namespace Core.TicTacToe.Implementation
{
    using System.Collections.Generic;

    using Core.TicTacToe.Constants;
    using Core.TicTacToe.Declaration;

    using Shared.Common.Extensions;

    public class FieldStateConverter : IFieldStateConverter
    {
        private readonly INewGameFieldCreator newGameFieldCreator;

        private readonly IGameFieldTransparator gameFieldTransparator;

        public FieldStateConverter(INewGameFieldCreator newGameFieldCreator, IGameFieldTransparator gameFieldTransparator)
        {
            this.newGameFieldCreator = newGameFieldCreator;
            this.gameFieldTransparator = gameFieldTransparator;
        }

        public string GameFieldToString(CellCondition[,] gameField)
        {
            var resultLine = string.Empty;

            for (var x = 0; x <= GameFieldConstants.MaxCoordinate; x++)
            {
                for (var y = 0; y <= GameFieldConstants.MaxCoordinate; y++)
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

            var x = 0;
            var y = 0;

            foreach (var nextSymbol in value)
            {
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

                if (y >= GameFieldConstants.MaxCoordinate)
                {
                    y = 0;
                    x++;

                    if (x > GameFieldConstants.MaxCoordinate)
                    {
                        break;
                    }
                }
                else
                {
                    y++;
                }
            }

            return gameField;
        }

        public List<string> GetSimiliarGameFieldStringList(CellCondition[,] gameField)
        {
            var resultList = new List<string>();

            var orinalGameFieldCode = this.GameFieldToString(gameField);
            resultList.Add(orinalGameFieldCode);
            resultList.Add(orinalGameFieldCode.Reverse());

            var transparatedGameField = this.gameFieldTransparator.GetTransparation(gameField);
            var transparatedGameFieldCode = this.GameFieldToString(transparatedGameField);
            resultList.Add(transparatedGameFieldCode);
            resultList.Add(transparatedGameFieldCode.Reverse());

            return resultList;
        }
    }
}
