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

            this.Property(statistic => statistic.Score).IsRequired();
            this.Property(statistic => statistic.PlayedGames).IsRequired();

            this.HasRequired(statistic => statistic.Field).WithOptional(field => field.FieldStatistic);
        }
    }
}
