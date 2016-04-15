namespace Data.Migration.ModelConfigurations.TicTacToeConfigurations
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using Data.Model.TicTacToe;

    public class GameConfiguration : EntityTypeConfiguration<Game>
    {
        public GameConfiguration()
        {
            this.ToTable("Game");

            this.HasKey(game => game.GameId);
            this.Property(game => game.GameId).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(game => game.FieldNumber).IsRequired();

            this.HasRequired(game => game.Field).WithMany(field => field.Games);
        }
    }
}
