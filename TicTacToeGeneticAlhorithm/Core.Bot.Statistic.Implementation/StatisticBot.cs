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
        private readonly IFieldStateConverter fieldStateConverter;

        private readonly IStepMaker stepMaker;

        private readonly IPossibleStepsProvider possibleStepsProvider;

        public StatisticBot(IPossibleStepsProvider possibleStepsProvider, IStepMaker stepMaker, IFieldStateConverter fieldStateConverter)
        {
            this.possibleStepsProvider = possibleStepsProvider;
            this.stepMaker = stepMaker;
            this.fieldStateConverter = fieldStateConverter;
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

                var currentFactor = this.CalculateFactor(field.FieldStatistic.PlayedGames, field.FieldStatistic.Score);
                if (currentFactor > factor)
                {
                    factor = currentFactor;
                    nextStep = step;
                }
            }

            return nextStep;
        }

        public double CalculateFactor(int playedGames, double score)
        {
            if (playedGames == 0)
            {
                return 0.5;
            }

            return score / playedGames;
        }
    }
}
