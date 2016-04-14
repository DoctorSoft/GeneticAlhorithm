namespace Core.TicTacToe.Implementation
{
    using Core.TicTacToe.Constants;
    using Core.TicTacToe.Declaration;

    public class NextStepConditionCalculator : INextStepConditionCalculator
    {
        private readonly IGameFieldCellsStatisticProvider gameFieldCellsStatisticProvider;

        public NextStepConditionCalculator(IGameFieldCellsStatisticProvider gameFieldCellsStatisticProvider)
        {
            this.gameFieldCellsStatisticProvider = gameFieldCellsStatisticProvider;
        }

        public CellCondition GetNextTurnCondition(CellCondition[,] gameField)
        {
            var emptyCellCount = this.gameFieldCellsStatisticProvider.GetStatistic(gameField)[CellCondition.Empty];

            return emptyCellCount % 2 == GameFieldConstants.CellsCount % 2 ? CellCondition.Circle : CellCondition.Cross;
        }
    }
}
