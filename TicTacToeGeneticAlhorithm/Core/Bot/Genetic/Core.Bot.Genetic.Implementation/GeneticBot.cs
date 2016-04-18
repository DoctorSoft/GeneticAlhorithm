namespace Core.Bot.Genetic.Implementation
{
    using System.Linq;

    using Core.Bot.Genetic.Implementation.Declaration;
    using Core.Bot.Main.Implementation.Helpers;
    using Core.TicTacToe.Constants;
    using Core.TicTacToe.Declaration;
    using Core.TicTacToe.Models;

    using Data.Migration.Contexts;
    using Data.Model.GeneticBot;

    public class GeneticBot : IGeneticBot
    {
        private readonly IFieldStateConverter fieldStateConverter;

        private readonly IStepMaker stepMaker;

        private readonly IPossibleStepsProvider possibleStepsProvider;

        public GeneticBot(IFieldStateConverter fieldStateConverter, IStepMaker stepMaker, IPossibleStepsProvider possibleStepsProvider)
        {
            this.fieldStateConverter = fieldStateConverter;
            this.stepMaker = stepMaker;
            this.possibleStepsProvider = possibleStepsProvider;
        }

        public Coordinates GetStep(CellCondition[,] gameField, TicTacToeContext context)
        {
            var allPossibleSteps = this.possibleStepsProvider.GetPossibleSteps(gameField);
            var individual =
                context.GetModel<GeneticIndividual>()
                    .FirstOrDefault(geneticIndividual => geneticIndividual.ImportanceOrder == 0);

            var factor = -1.0;
            Coordinates nextStep = null;

            foreach (var step in allPossibleSteps)
            {
                var currentField = this.stepMaker.MakeStep(gameField, step.X, step.Y);
                var currentFieldCode = this.fieldStateConverter.GameFieldToString(currentField);

                var field = GameHelper.GetFieldByCode(currentFieldCode, context);
                var individualFactor =
                    context.Set<GeneticFactor>()
                        .FirstOrDefault(geneticFactor => geneticFactor.Field.FieldId == field.FieldId
                                                         && geneticFactor.GeneticIndividual.GeneticIndividualId == individual.GeneticIndividualId);

                var currentFactor = individualFactor.Factor;
                if (currentFactor > factor)
                {
                    factor = currentFactor;
                    nextStep = step;
                }
            }

            return nextStep;
        }
    }
}
