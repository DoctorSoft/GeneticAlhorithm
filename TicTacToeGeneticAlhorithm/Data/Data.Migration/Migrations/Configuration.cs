namespace Data.Migration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.Linq;

    using Core.TicTacToe.Declaration;

    using Data.Model.GeneticBot;
    using Data.Model.StatisticBot;
    using Data.Model.TicTacToe;

    using Infrastructure.DI;

    using Ninject;

    internal sealed class Configuration : DbMigrationsConfiguration<Data.Migration.Contexts.TicTacToeContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Data.Migration.Contexts.TicTacToeContext context)
        {
            IKernel kernel = new StandardKernel(new TicTacToeNinjectModule());
            var provider = kernel.Get<IAllPossibleGameFieldsProvider>();
            var random = new Random();

            if (!context.Set<Field>().Any())
            { 

                var combinations = provider.GenerateAllPossibleGameFieldCombanations();

                var fieldModels = combinations.Select((combination, i) => new Field
                                                                    {
                                                                        FirstVariant = combination.Fields[0],
                                                                        SecondVariant = combination.Fields[1],
                                                                        ThirdVariant = combination.Fields[2],
                                                                        FourthVariant = combination.Fields[3],

                                                                        FieldStatistic = new FieldStatistic
                                                                                             {
                                                                                                 Draws = 0,
                                                                                                 Loses = 0,
                                                                                                 Wins = 0
                                                                                             }
                                                                    }).ToList();

                context.Set<Field>().AddRange(fieldModels);
                context.SaveChanges();

                var generationModels = Enumerable.Range(0, 10)
                    .Select(i => new GeneticIndividual
                                     {
                                         Draws = 0,
                                         Loses = 0,
                                         Wins = 0,
                                         GenerationNumber = 0
                                     }).ToList();
                context.Set<GeneticIndividual>().AddRange(generationModels);
                context.SaveChanges();

                var generationFactors = generationModels.SelectMany(individual => fieldModels.Select(field => new GeneticFactor
                                                                                                                {
                                                                                                                    Field = field,
                                                                                                                    Factor = random.Next(100) + 1,
                                                                                                                    GeneticIndividual = individual
                                                                                                                })).ToList();
                context.Set<GeneticFactor>().AddRange(generationFactors);
                context.SaveChanges();
            }
        }
    }
}
