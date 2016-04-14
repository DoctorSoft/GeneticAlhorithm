namespace Core.TicTacToe.Implementation
{
    using Core.TicTacToe.Constants;
    using Core.TicTacToe.Declaration;
    using Core.TicTacToe.Exceptions;

    public class StepMaker : IStepMaker
    {
        private readonly INextStepConditionCalculator nextStepConditionCalculator;

        private readonly ICorrectCoordinatesChecker correctCoordinatesChecker;

        private readonly IGameFieldCopyMaker gameFieldCopyMaker;

        public StepMaker(INextStepConditionCalculator nextStepConditionCalculator, ICorrectCoordinatesChecker correctCoordinatesChecker, IGameFieldCopyMaker gameFieldCopyMaker)
        {
            this.nextStepConditionCalculator = nextStepConditionCalculator;
            this.correctCoordinatesChecker = correctCoordinatesChecker;
            this.gameFieldCopyMaker = gameFieldCopyMaker;
        }

        public CellCondition[,] MakeStep(CellCondition[,] gameField, int x, int y)
        {
            this.CheckIfCoordinatesAreCorrect(x, y);
            this.CheckIfCellIsEmpty(gameField, x, y);

            var stepCondition = this.nextStepConditionCalculator.GetNextTurnCondition(gameField);

            var resultField = this.gameFieldCopyMaker.MakeFieldCopy(gameField);
            resultField[x, y] = stepCondition;

            return resultField;
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
