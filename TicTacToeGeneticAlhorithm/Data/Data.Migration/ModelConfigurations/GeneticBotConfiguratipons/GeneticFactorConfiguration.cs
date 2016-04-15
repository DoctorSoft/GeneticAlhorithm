namespace Data.Migration.ModelConfigurations.GeneticBotConfiguratipons
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using Data.Model.GeneticBot;

    public class GeneticFactorConfiguration : EntityTypeConfiguration<GeneticFactor>
    {
        public GeneticFactorConfiguration()
        {
            this.ToTable("GeneticFactor");

            this.HasKey(factor => factor.GeneticFactorId);
            this.Property(factor => factor.GeneticFactorId).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(factor => factor.Factor).IsRequired();

            this.HasRequired(factor => factor.Field).WithMany(field => field.GeneticFactors);
            this.HasRequired(factor => factor.GeneticIndividual).WithMany(individual => individual.GeneticFactors);
        }
    }
}
