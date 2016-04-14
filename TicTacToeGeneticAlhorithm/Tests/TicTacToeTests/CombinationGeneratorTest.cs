namespace TicTacToeTests
{
    using System.Linq;

    using Core.TicTacToe.Implementation;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CombinationGeneratorTest
    {
        [TestMethod]
        public void GenerateCombinations()
        {
            var provider = new AllPossibleGameFieldsProvider(
                new NewGameFieldCreator(),
                new FieldStateConverter(new NewGameFieldCreator(), new GameFieldTransparator(new NewGameFieldCreator())),
                new StepMaker(
                    new NextStepConditionCalculator(new GameFieldCellsStatisticProvider()),
                    new CorrectCoordinatesChecker(),
                    new GameFieldCopyMaker()),
                new PossibleStepsProvider(),
                new GameProcessStatisticProvider(new GameFieldCellsStatisticProvider(), new CorrectCoordinatesChecker()));

            var combinations = provider.GenerateAllPossibleGameFieldCombanations();

            var combinationsWithoutMirror =
                combinations.SelectMany(combination => combination.Fields).Distinct().ToList();
        }
    }
}
