namespace Data.Migration.Contexts
{
    using System.Data.Entity;

    using Data.Migration.ModelConfigurations.GeneticBotConfigurations;
    using Data.Migration.ModelConfigurations.StatisticBotConfigurations;
    using Data.Migration.ModelConfigurations.TicTacToeConfigurations;

    public class TicTacToeContext : DbContext
    {
        public TicTacToeContext()
            : base("TicTacToe")
        {
        }

        public DbSet<TEntity> GetModel<TEntity>() where TEntity : class
        {
            return this.Set<TEntity>();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new FieldConfiguration());
            modelBuilder.Configurations.Add(new GameConfiguration());

            modelBuilder.Configurations.Add(new FieldStatisticConfiguration());

            modelBuilder.Configurations.Add(new GeneticFactorConfiguration());
            modelBuilder.Configurations.Add(new GeneticIndividualConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
