using System;
using Core.Bot.Main.Implementation.Helpers;
using Core.Bot.Statistic.Implementation.Declaration;
using Core.TicTacToe.Constants;
using Core.TicTacToe.Declaration;
using Core.TicTacToe.Models;
using Data.Migration.Contexts;

namespace Core.Bot.Statistic.Implementation
{
    public class StatisticBot : IStatisticBot
    {
        private readonly Random random;

        private readonly INewGameFieldCreator newGameFieldCreator;

        private readonly IFieldStateConverter fieldStateConverter;

        private readonly IStepMaker stepMaker;

        private readonly IGameProcessStatisticProvider gameProcessStatisticProvider;

        private readonly IPossibleStepsProvider possibleStepsProvider;

        public StatisticBot(IPossibleStepsProvider possibleStepsProvider)
        {
            this.possibleStepsProvider = possibleStepsProvider;
        }

        public Coordinates GetStep(CellCondition[,] gameField, TicTacToeContext context)
        {
            var allPossibleSteps = this.possibleStepsProvider.GetPossibleSteps(gameField);

            var factor = -1.0;
            Coordinates nextStep = null;

            foreach (var step in allPossibleSteps)
            {
                var currentField = this.stepMaker.MakeStep(gameField, step.X, step.Y);
                var currentFieldCode = this.fieldStateConverter.GameFieldToString(currentField);

                var field = GameHelper.GetFieldByCode(currentFieldCode, context);

                var currentFactor = this.CalculateFactor(field.FieldStatistic.Draws, field.FieldStatistic.Wins, field.FieldStatistic.Loses);
                if (currentFactor > factor)
                {
                    factor = currentFactor;
                    nextStep = step;
                }
            }

            return nextStep;
        }

        public double CalculateFactor(int draws, int wins, int loses)
        {
            var count = draws + wins + loses;

            if (count == 0)
            {
                return 0.5;
            }

            return (wins + 0.5*draws)/count;
        }
    }
}
