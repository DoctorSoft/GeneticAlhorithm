namespace Data.Migration.ModelConfigurations.GeneticBotConfigurations
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using Data.Model.GeneticBot;

    public class GeneticIndividualConfiguration : EntityTypeConfiguration<GeneticIndividual>
    {
        public GeneticIndividualConfiguration()
        {
            this.ToTable("GeneticIndividual");

            this.HasKey(individual => individual.GeneticIndividualId);
            this.Property(individual => individual.GeneticIndividualId).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(individual => individual.PlayedGames).IsRequired();
            this.Property(individual => individual.Score).IsRequired();
            this.Property(individual => individual.ImportanceOrder).IsRequired();

            this.HasMany(individual => individual.GeneticFactors).WithRequired(factor => factor.GeneticIndividual).HasForeignKey(factor => factor.GeneticIndividualId);
        }
    }
}
