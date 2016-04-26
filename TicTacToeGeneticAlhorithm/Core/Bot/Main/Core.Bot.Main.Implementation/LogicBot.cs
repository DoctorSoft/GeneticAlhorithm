namespace Core.Bot.Main.Implementation
{
    using System.Collections.Generic;
    using System.Linq;

    using Core.Bot.Main.Implementation.Declaration;
    using Core.TicTacToe.Constants;
    using Core.TicTacToe.Declaration;
    using Core.TicTacToe.Models;

    public class LogicBot : ILogicBot
    {
        private readonly IMonkeyBot monkeyBot;

        private readonly INextStepConditionCalculator nextStepConditionCalculator;

        public LogicBot(IMonkeyBot monkeyBot, INextStepConditionCalculator nextStepConditionCalculator)
        {
            this.monkeyBot = monkeyBot;
            this.nextStepConditionCalculator = nextStepConditionCalculator;
        }

        public Coordinates GetStep(CellCondition[,] gameField)
        {
            var result = this.TwoInLine(gameField);
            if (result != null)
            {
                return result;
            }

            return this.monkeyBot.GetStep(gameField);
        }

        private Coordinates SetCenter(CellCondition[,] gameField)
        {
            return gameField[1, 1] == CellCondition.Empty ? new Coordinates { X = 1, Y = 1 } : null;
        }

        private Coordinates TwoInLine(CellCondition[,] gameField)
        {
            var nextStepCondition = this.nextStepConditionCalculator.GetNextTurnCondition(gameField);

            var conditions = nextStepCondition == CellCondition.Circle
                                 ? new[] { CellCondition.Circle, CellCondition.Cross }
                                 : new[] { CellCondition.Cross, CellCondition.Circle };

            foreach (var cellCondition in conditions)
            {
                var coordinates = this.TwoInRow(gameField, cellCondition);
                if (coordinates != null)
                {
                    return coordinates;
                }

                coordinates = this.TwoInColumn(gameField, cellCondition);
                if (coordinates != null)
                {
                    return coordinates;
                }

                coordinates = this.TwoInRightDownDiagonal(gameField, cellCondition);
                if (coordinates != null)
                {
                    return coordinates;
                }

                coordinates = this.TwoInLeftDownDiagonal(gameField, cellCondition);
                if (coordinates != null)
                {
                    return coordinates;
                }
            }

            return null;
        }

        private Coordinates TwoInRow(CellCondition[,] gameField, CellCondition condition)
        {
            for (var y = 0; y <= 2; y++)
            {
                var conditionCells = new List<Coordinates>();
                var emptyCells = new List<Coordinates>();

                for (var x = 0; x <= 2; x++)
                {
                    var cell = new Coordinates { X = x, Y = y };

                    if (gameField[x, y] == condition)
                    {
                        conditionCells.Add(cell);
                        continue;
                    }

                    if (gameField[x, y] == CellCondition.Empty)
                    {
                        emptyCells.Add(cell);
                    }
                }

                if (conditionCells.Count != 2)
                {
                    continue;
                }

                if (emptyCells.Any())
                {
                    return emptyCells.FirstOrDefault();
                }
            }

            return null;
        }

        private Coordinates TwoInColumn(CellCondition[,] gameField, CellCondition condition)
        {
            for (var x = 0; x <= 2; x++)
            {
                var conditionCells = new List<Coordinates>();
                var emptyCells = new List<Coordinates>();

                for (var y = 0; y <= 2; y++)
                {
                    var cell = new Coordinates { X = x, Y = y };

                    if (gameField[x, y] == condition)
                    {
                        conditionCells.Add(cell);
                        continue;
                    }

                    if (gameField[x, y] == CellCondition.Empty)
                    {
                        emptyCells.Add(cell);
                    }
                }

                if (conditionCells.Count != 2)
                {
                    continue;
                }

                if (emptyCells.Any())
                {
                    return emptyCells.FirstOrDefault();
                }
            }

            return null;
        }

        private Coordinates TwoInRightDownDiagonal(CellCondition[,] gameField, CellCondition condition)
        {
            var conditionCells = new List<Coordinates>();
            var emptyCells = new List<Coordinates>();

            for (var factor = 0; factor <= 2; factor++)
            {
                var cell = new Coordinates { X = factor, Y = factor };

                if (gameField[factor, factor] == condition)
                {
                    conditionCells.Add(cell);
                    continue;
                }

                if (gameField[factor, factor] == CellCondition.Empty)
                {
                    emptyCells.Add(cell);
                }
            }

            if (conditionCells.Count != 2)
            {
                return null;
            }

            return emptyCells.Any() ? emptyCells.FirstOrDefault() : null;
        }

        private Coordinates TwoInLeftDownDiagonal(CellCondition[,] gameField, CellCondition condition)
        {
            var conditionCells = new List<Coordinates>();
            var emptyCells = new List<Coordinates>();

            for (var factor = 0; factor <= 2; factor++)
            {
                var cell = new Coordinates { X = factor, Y = 2 - factor };

                if (gameField[factor, 2 - factor] == condition)
                {
                    conditionCells.Add(cell);
                    continue;
                }

                if (gameField[factor, 2 - factor] == CellCondition.Empty)
                {
                    emptyCells.Add(cell);
                }
            }

            if (conditionCells.Count != 2)
            {
                return null;
            }

            return emptyCells.Any() ? emptyCells.FirstOrDefault() : null;
        }
    }
}
