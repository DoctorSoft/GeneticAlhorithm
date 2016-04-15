namespace Data.Migration.ModelConfigurations.StatisticBotConfigurations
{
    using System.Data.Entity.ModelConfiguration;

    using Data.Model.StatisticBot;

    public class FieldStatisticConfiguration : EntityTypeConfiguration<FieldStatistic>
    {
        public FieldStatisticConfiguration()
        {
            this.ToTable("FieldStatistic");

            this.HasKey(statistic => statistic.FieldId);
            this.Property(statistic => statistic.FieldId).IsRequired();

            this.Property(statistic => statistic.Draws).IsRequired();
            this.Property(statistic => statistic.Loses).IsRequired();
            this.Property(statistic => statistic.Wins).IsRequired();

            this.HasRequired(statistic => statistic.Field).WithOptional(field => field.FieldStatistic);
        }
    }
}
