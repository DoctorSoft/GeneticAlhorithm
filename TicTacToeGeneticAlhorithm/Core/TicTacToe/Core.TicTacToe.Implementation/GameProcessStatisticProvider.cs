namespace Core.TicTacToe.Implementation
{
    using System.Linq;

    using Core.TicTacToe.Constants;
    using Core.TicTacToe.Declaration;
    using Core.TicTacToe.Models;

    public class GameProcessStatisticProvider : IGameProcessStatisticProvider
    {
        private readonly IGameFieldCellsStatisticProvider gameFieldCellsStatisticProvider;

        private readonly ICorrectCoordinatesChecker correctCoordinatesChecker;

        public GameProcessStatisticProvider(IGameFieldCellsStatisticProvider gameFieldCellsStatisticProvider, ICorrectCoordinatesChecker correctCoordinatesChecker)
        {
            this.gameFieldCellsStatisticProvider = gameFieldCellsStatisticProvider;
            this.correctCoordinatesChecker = correctCoordinatesChecker;
        }

        public GameProcessStatistic GetGameProcessStatistic(CellCondition[,] gameField)
        {
            var statistic = this.gameFieldCellsStatisticProvider.GetStatistic(gameField);

            var winnerInRow = this.GetWinnerInRow(gameField);
            if (winnerInRow != null)
            {
                return winnerInRow;
            }

            var winnerInColumn = this.GetWinnerInColumn(gameField);
            if (winnerInColumn != null)
            {
                return winnerInColumn;
            }

            var winnerInRightDownDiaginal = this.GetWinnerInRightDownDiagonal(gameField);
            if (winnerInRightDownDiaginal != null)
            {
                return winnerInRightDownDiaginal;
            }

            var winnerInLeftDownDiagonal = this.GetWinnerInLeftDownDiagonal(gameField);
            if (winnerInLeftDownDiagonal != null)
            {
                return winnerInLeftDownDiagonal;
            }

            if (statistic.Where(pair => pair.Key != CellCondition.Empty).Select(pair => pair.Value).Sum() == GameFieldConstants.CellsCount)
            {
                return new GameProcessStatistic
                {
                    WinStatistic = null,
                    GameStatus = GameStatus.Draw
                };
            }

            return new GameProcessStatistic
                       {
                           WinStatistic = null,
                           GameStatus = GameStatus.InProgress
                       };
        }

        public GameProcessStatistic GetWinnerInRow(CellCondition[,] gameField)
        {
            for (var y = 0; y <= GameFieldConstants.MaxCoordinate; y++)
            {
                var cellConditionToCalculate = CellCondition.Empty;
                var cellsTogetherCount = 0;

                for (var x = 0; x <= GameFieldConstants.MaxCoordinate; x++)
                {
                    if (gameField[x, y] == CellCondition.Empty)
                    {
                        cellConditionToCalculate = CellCondition.Empty;
                        cellsTogetherCount = 0;
                        continue;
                    }

                    if (gameField[x, y] == cellConditionToCalculate)
                    {
                        cellsTogetherCount++;
                        if (cellsTogetherCount == GameFieldConstants.CellsCountToWin)
                        {
                            return new GameProcessStatistic
                                       {
                                           GameStatus = cellConditionToCalculate == CellCondition.Circle ? GameStatus.CircleWon : GameStatus.CrossWon,
                                           WinStatistic = new WinStatistic
                                                              {
                                                                  MoveDirection = MoveDirection.Left,
                                                                  X = x,
                                                                  Y = y
                                                              }
                                       };
                        }
                    }
                    else
                    {
                        cellConditionToCalculate = gameField[x, y];
                        cellsTogetherCount = 1;
                    }
                }
            }

            return null;
        }

        public GameProcessStatistic GetWinnerInColumn(CellCondition[,] gameField)
        {
            for (var x = 0; x <= GameFieldConstants.MaxCoordinate; x++)
            {
                var cellConditionToCalculate = CellCondition.Empty;
                var cellsTogetherCount = 0;

                for (var y = 0; y <= GameFieldConstants.MaxCoordinate; y++)
                {
                    if (gameField[x, y] == CellCondition.Empty)
                    {
                        cellConditionToCalculate = CellCondition.Empty;
                        cellsTogetherCount = 0;
                        continue;
                    }

                    if (gameField[x, y] == cellConditionToCalculate)
                    {
                        cellsTogetherCount++;
                        if (cellsTogetherCount == GameFieldConstants.CellsCountToWin)
                        {
                            return new GameProcessStatistic
                                       {
                                           GameStatus = cellConditionToCalculate == CellCondition.Circle ? GameStatus.CircleWon : GameStatus.CrossWon,
                                           WinStatistic = new WinStatistic
                                                              {
                                                                  MoveDirection = MoveDirection.Up,
                                                                  Y = y,
                                                                  X = x
                                                              }
                                       };
                        }
                    }
                    else
                    {
                        cellConditionToCalculate = gameField[x, y];
                        cellsTogetherCount = 1;
                    }
                }
            }

            return null;
        }

        public GameProcessStatistic GetWinnerInRightDownDiagonal(CellCondition[,] gameField)
        {
            for (var x = -GameFieldConstants.MaxCoordinate; x <= GameFieldConstants.MaxCoordinate; x++)
            {
                var cellConditionToCalculate = CellCondition.Empty;
                var cellsTogetherCount = 0;

                for (var y = 0; y <= GameFieldConstants.MaxCoordinate; y++)
                {
                    if (!this.correctCoordinatesChecker.CoordinatesAreCorrect(x, x + y))
                    {
                        cellConditionToCalculate = CellCondition.Empty;
                        cellsTogetherCount = 0;
                        continue;
                    }

                    if (gameField[x + y, y] == CellCondition.Empty)
                    {
                        cellConditionToCalculate = CellCondition.Empty;
                        cellsTogetherCount = 0;
                        continue;
                    }

                    if (gameField[x + y, y] == cellConditionToCalculate)
                    {
                        cellsTogetherCount++;
                        if (cellsTogetherCount == GameFieldConstants.CellsCountToWin)
                        {
                            return new GameProcessStatistic
                            {
                                GameStatus = cellConditionToCalculate == CellCondition.Circle ? GameStatus.CircleWon : GameStatus.CrossWon,
                                WinStatistic = new WinStatistic
                                {
                                    MoveDirection = MoveDirection.LeftUp,
                                    Y = y,
                                    X = x + y
                                }
                            };
                        }
                    }
                    else
                    {
                        cellConditionToCalculate = gameField[x + y, y];
                        cellsTogetherCount = 1;
                    }
                }
            }

            return null;
        }

        public GameProcessStatistic GetWinnerInLeftDownDiagonal(CellCondition[,] gameField)
        {
            const int DiagonalLineCount = GameFieldConstants.MaxCoordinate * 2 + 1;

            for (var x = 0; x <= DiagonalLineCount; x++)
            {
                var cellConditionToCalculate = CellCondition.Empty;
                var cellsTogetherCount = 0;

                for (var y = 0; y <= GameFieldConstants.MaxCoordinate; y++)
                {
                    if (!this.correctCoordinatesChecker.CoordinatesAreCorrect(x, x - y))
                    {
                        cellConditionToCalculate = CellCondition.Empty;
                        cellsTogetherCount = 0;
                        continue;
                    }

                    if (gameField[x - y, y] == CellCondition.Empty)
                    {
                        cellConditionToCalculate = CellCondition.Empty;
                        cellsTogetherCount = 0;
                        continue;
                    }

                    if (gameField[x - y, y] == cellConditionToCalculate)
                    {
                        cellsTogetherCount++;
                        if (cellsTogetherCount == GameFieldConstants.CellsCountToWin)
                        {
                            return new GameProcessStatistic
                            {
                                GameStatus = cellConditionToCalculate == CellCondition.Circle ? GameStatus.CircleWon : GameStatus.CrossWon,
                                WinStatistic = new WinStatistic
                                {
                                    MoveDirection = MoveDirection.RightUp,
                                    Y = y,
                                    X = x - y
                                }
                            };
                        }
                    }
                    else
                    {
                        cellConditionToCalculate = gameField[x - y, y];
                        cellsTogetherCount = 1;
                    }
                }
            }

            return null;
        }
    }
}
