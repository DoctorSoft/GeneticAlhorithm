namespace Core.TicTacToe.Implementation
{
    using Core.TicTacToe.Constants;
    using Core.TicTacToe.Declaration;
    using Core.TicTacToe.Exceptions;

    public class StepMaker : IStepMaker
    {
        private readonly INextStepConditionCalculator nextStepConditionCalculator;

        public StepMaker(INextStepConditionCalculator nextStepConditionCalculator)
        {
            this.nextStepConditionCalculator = nextStepConditionCalculator;
        }

        public CellCondition[,] MakeStep(CellCondition[,] gameField, int x, int y)
        {
            this.CheckIfCoordinatesAreCorrect(x, y);
            this.CheckIfCellIsEmpty(gameField, x, y);

            var stepCondition = this.nextStepConditionCalculator.GetNextTurnCondition(gameField);

            gameField[x, y] = stepCondition;

            return gameField;
        }

        private void CheckIfCoordinatesAreCorrect(int x, int y)
        {
            if (x < GameFieldConstants.MinXCoordinate || x > GameFieldConstants.MaxXCoordinate
                || y < GameFieldConstants.MinYCoordinate || y > GameFieldConstants.MaxYCoordinate)
            {
                throw new CoordinatesAreNotCorrectException();
            }
        }

        private void CheckIfCellIsEmpty(CellCondition[,] gameField, int x, int y)
        {
            if (gameField[x, y] != CellCondition.Empty)
            {
                throw new CellIsNotEmptyException();
            }
        }
    }
}
