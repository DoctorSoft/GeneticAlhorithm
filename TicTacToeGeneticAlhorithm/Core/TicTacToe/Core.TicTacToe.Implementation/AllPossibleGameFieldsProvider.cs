namespace Core.TicTacToe.Implementation
{
    using System.Collections.Generic;
    using System.Linq;

    using Core.TicTacToe.Constants;
    using Core.TicTacToe.Declaration;
    using Core.TicTacToe.Models;

    public class AllPossibleGameFieldsProvider : IAllPossibleGameFieldsProvider
    {
        private readonly INewGameFieldCreator newGameFieldCreator;

        private readonly IFieldStateConverter fieldStateConverter;

        private readonly IPossibleStepsProvider possibleStepsProvider;

        private readonly IGameProcessStatisticProvider gameProcessStatisticProvider;

        private readonly IStepMaker stepMaker;

        public AllPossibleGameFieldsProvider(INewGameFieldCreator newGameFieldCreator, IFieldStateConverter fieldStateConverter, IStepMaker stepMaker, IPossibleStepsProvider possibleStepsProvider, IGameProcessStatisticProvider gameProcessStatisticProvider)
        {
            this.newGameFieldCreator = newGameFieldCreator;
            this.fieldStateConverter = fieldStateConverter;
            this.stepMaker = stepMaker;
            this.possibleStepsProvider = possibleStepsProvider;
            this.gameProcessStatisticProvider = gameProcessStatisticProvider;
        }

        public List<GameFieldCombination> GenerateAllPossibleGameFieldCombanations()
        {
            var firstField = this.newGameFieldCreator.CreateNewGameField();

            var fieldQueue = new Queue<CellCondition[,]>();
            var checkSet = new HashSet<string>();
            var resultList = new List<GameFieldCombination>();

            fieldQueue.Enqueue(firstField);

            while (fieldQueue.Any())
            {
                var currentField = fieldQueue.Dequeue();

                var combination = new GameFieldCombination
                                      {
                                          Fields = this.fieldStateConverter.GetSimiliarGameFieldStringList(currentField)
                                      };

                if (combination.Fields.Any(s => checkSet.Contains(s)))
                {
                    continue;
                }

                foreach (var field in combination.Fields)
                {
                    checkSet.Add(field);
                }

                resultList.Add(combination);

                if (this.gameProcessStatisticProvider.GetGameProcessStatistic(currentField).GameStatus
                    != GameStatus.InProgress)
                {
                    continue;
                }

                var possibleSteps = this.possibleStepsProvider.GetPossibleSteps(currentField);

                foreach (var possibleStep in possibleSteps)
                {
                    fieldQueue.Enqueue(this.stepMaker.MakeStep(currentField, possibleStep.X, possibleStep.Y));
                }
            }

            return resultList;
        }
    }
}
