namespace Core.TicTacToe.Implementation
{
    using Core.TicTacToe.Constants;
    using Core.TicTacToe.Declaration;

    public class StepMaker : IStepMaker
    {
        private readonly INextStepCalculator nextStepCalculator;

        public StepMaker(INextStepCalculator nextStepCalculator)
        {
            this.nextStepCalculator = nextStepCalculator;
        }

        public CellCondition[,] MakeStep(CellCondition[,] gameField, int x, int y)
        {
            
        }

        private void CheckIfCoordinatesAreCorrect(int x, int y)
        {
            if (x < 0 || x > 3)
        }
    }
}
