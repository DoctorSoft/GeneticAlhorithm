namespace Data.Migration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNewFactorScale : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FieldStatistic", "Score", c => c.Double(nullable: false));
            AddColumn("dbo.FieldStatistic", "PlayedGames", c => c.Int(nullable: false));
            AddColumn("dbo.GeneticIndividual", "Score", c => c.Double(nullable: false));
            AddColumn("dbo.GeneticIndividual", "PlayedGames", c => c.Int(nullable: false));
            DropColumn("dbo.FieldStatistic", "Draws");
            DropColumn("dbo.FieldStatistic", "Wins");
            DropColumn("dbo.FieldStatistic", "Loses");
            DropColumn("dbo.GeneticIndividual", "Draws");
            DropColumn("dbo.GeneticIndividual", "Wins");
            DropColumn("dbo.GeneticIndividual", "Loses");
        }
        
        public override void Down()
        {
            AddColumn("dbo.GeneticIndividual", "Loses", c => c.Int(nullable: false));
            AddColumn("dbo.GeneticIndividual", "Wins", c => c.Int(nullable: false));
            AddColumn("dbo.GeneticIndividual", "Draws", c => c.Int(nullable: false));
            AddColumn("dbo.FieldStatistic", "Loses", c => c.Int(nullable: false));
            AddColumn("dbo.FieldStatistic", "Wins", c => c.Int(nullable: false));
            AddColumn("dbo.FieldStatistic", "Draws", c => c.Int(nullable: false));
            DropColumn("dbo.GeneticIndividual", "PlayedGames");
            DropColumn("dbo.GeneticIndividual", "Score");
            DropColumn("dbo.FieldStatistic", "PlayedGames");
            DropColumn("dbo.FieldStatistic", "Score");
        }
    }
}
