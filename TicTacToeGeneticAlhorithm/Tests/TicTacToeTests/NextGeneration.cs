using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TicTacToeTests
{
    using Core.Bot.Genetic.Implementation.Declaration;

    using Data.Migration.Contexts;

    using Infrastructure.DI;
    using Infrastructure.MVCDependencyInjection;

    using Ninject;

    [TestClass]
    public class NextGeneration
    {
        [TestMethod]
        public void CreateNextGeneration()
        {
            IKernel kernel = new StandardKernel(new TicTacToeNinjectModule(), new MVCNinjectModule());

            var developer = kernel.Get<IGeneticBotDeveloper>();

            for (var i = 0; i < 5; i++)
            {
                using (var context = new TicTacToeContext())
                {
                    developer.GenerateNextGeneration(context);
                }
            }
        }
    }
}
