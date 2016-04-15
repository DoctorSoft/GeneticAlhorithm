namespace Data.Migration.ModelConfigurations.TicTacToeConfigurations
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using Data.Model.TicTacToe;

    public class FieldConfiguration : EntityTypeConfiguration<Field>
    {
        public FieldConfiguration()
        {
            this.ToTable("Field");

            this.HasKey(field => field.FieldId);
            this.Property(field => field.FieldId).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(field => field.FirstVariant).IsRequired();
            this.Property(field => field.SecondVariant).IsRequired();
            this.Property(field => field.ThirdVariant).IsRequired();
            this.Property(field => field.FourthVariant).IsRequired();

            this.HasOptional(field => field.FieldStatistic).WithRequired(statistic => statistic.Field);
            this.HasMany(field => field.Games).WithRequired(game => game.Field);
            this.HasMany(field => field.GeneticFactors).WithRequired(factor => factor.Field).HasForeignKey(factor => factor.FieldId);
        }
    }
}
