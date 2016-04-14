namespace Core.TicTacToe.Implementation
{
    using Core.TicTacToe.Constants;
    using Core.TicTacToe.Declaration;
    using Core.TicTacToe.Exceptions;

    public class StepMaker : IStepMaker
    {
        private readonly INextStepConditionCalculator nextStepConditionCalculator;

        private readonly ICorrectCoordinatesChecker correctCoordinatesChecker;

        public StepMaker(INextStepConditionCalculator nextStepConditionCalculator, ICorrectCoordinatesChecker correctCoordinatesChecker)
        {
            this.nextStepConditionCalculator = nextStepConditionCalculator;
            this.correctCoordinatesChecker = correctCoordinatesChecker;
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
            if (!this.correctCoordinatesChecker.CoordinatesAreCorrect(x, y))
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
