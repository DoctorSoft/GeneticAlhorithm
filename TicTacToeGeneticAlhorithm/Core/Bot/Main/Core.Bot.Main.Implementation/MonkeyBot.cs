using System;
using Core.Bot.Main.Implementation.Declaration;
using Core.TicTacToe.Constants;
using Core.TicTacToe.Declaration;
using Core.TicTacToe.Models;

namespace Core.Bot.Main.Implementation
{
    public class MonkeyBot : IMonkeyBot
    {
        private readonly IPossibleStepsProvider possibleStepsProvider;

        private readonly Random random;

        public MonkeyBot(IPossibleStepsProvider possibleStepsProvider)
        {
            this.possibleStepsProvider = possibleStepsProvider;
            this.random = new Random();
        }

        public Coordinates GetStep(CellCondition[,] gameField)
        {
            var steps = this.possibleStepsProvider.GetPossibleSteps(gameField);

            var nextStepNumber = random.Next(steps.Count);

            return steps[nextStepNumber];
        }
    }
}
