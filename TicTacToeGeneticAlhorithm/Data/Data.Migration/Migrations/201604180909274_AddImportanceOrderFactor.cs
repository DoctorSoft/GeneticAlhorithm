namespace Data.Migration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddImportanceOrderFactor : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GeneticIndividual", "ImportanceOrder", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GeneticIndividual", "ImportanceOrder");
        }
    }
}
