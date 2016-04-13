namespace Core.TicTacToe.Implementation
{
    using Core.TicTacToe.Constants;
    using Core.TicTacToe.Declaration;

    public class NextStepConditionCalculator : INextStepConditionCalculator
    {
        public CellCondition GetNextTurnCondition(CellCondition[,] gameField)
        {
            var emptyCellCount = 0;

            for (var x = GameFieldConstants.MinXCoordinate; x <= GameFieldConstants.MaxXCoordinate; x++)
            {
                for (var y = GameFieldConstants.MinYCoordinate; y <= GameFieldConstants.MaxYCoordinate; y++)
                {
                    if (gameField[x, y] == CellCondition.Empty)
                    {
                        emptyCellCount++;
                    }
                }
            }

            return emptyCellCount % 2 == 0 ? CellCondition.Circle : CellCondition.Cross;
        }
    }
}
