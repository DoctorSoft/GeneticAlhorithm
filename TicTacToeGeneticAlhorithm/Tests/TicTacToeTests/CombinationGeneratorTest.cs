namespace TicTacToeTests
{
    using System.Linq;

    using Core.TicTacToe.Declaration;

    using Infrastructure.DI;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Ninject;

    [TestClass]
    public class CombinationGeneratorTest
    {
        [TestMethod]
        public void GenerateCombinations()
        {
            IKernel kernel = new StandardKernel(new TicTacToeNinjectModule());
            var provider = kernel.Get<IAllPossibleGameFieldsProvider>();

            var combinations = provider.GenerateAllPossibleGameFieldCombanations();

            var combinationsWithoutMirror =
                combinations.SelectMany(combination => combination.Fields).Distinct().ToList();
        }
    }
}
