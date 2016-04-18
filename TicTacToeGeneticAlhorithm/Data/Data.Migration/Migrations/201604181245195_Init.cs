namespace Data.Migration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Field",
                c => new
                    {
                        FieldId = c.Int(nullable: false, identity: true),
                        FirstVariant = c.String(nullable: false),
                        SecondVariant = c.String(nullable: false),
                        ThirdVariant = c.String(nullable: false),
                        FourthVariant = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.FieldId);
            
            CreateTable(
                "dbo.FieldStatistic",
                c => new
                    {
                        FieldId = c.Int(nullable: false),
                        Score = c.Double(nullable: false),
                        PlayedGames = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.FieldId)
                .ForeignKey("dbo.Field", t => t.FieldId)
                .Index(t => t.FieldId);
            
            CreateTable(
                "dbo.Game",
                c => new
                    {
                        GameId = c.Int(nullable: false, identity: true),
                        FieldNumber = c.Int(nullable: false),
                        Proccess = c.String(),
                        Field_FieldId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.GameId)
                .ForeignKey("dbo.Field", t => t.Field_FieldId, cascadeDelete: true)
                .Index(t => t.Field_FieldId);
            
            CreateTable(
                "dbo.GeneticFactor",
                c => new
                    {
                        GeneticFactorId = c.Int(nullable: false, identity: true),
                        Factor = c.Int(nullable: false),
                        FieldId = c.Int(nullable: false),
                        GeneticIndividualId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.GeneticFactorId)
                .ForeignKey("dbo.GeneticIndividual", t => t.GeneticIndividualId, cascadeDelete: true)
                .ForeignKey("dbo.Field", t => t.FieldId, cascadeDelete: true)
                .Index(t => t.FieldId)
                .Index(t => t.GeneticIndividualId);
            
            CreateTable(
                "dbo.GeneticIndividual",
                c => new
                    {
                        GeneticIndividualId = c.Int(nullable: false, identity: true),
                        GenerationNumber = c.Int(nullable: false),
                        ImportanceOrder = c.Int(nullable: false),
                        Score = c.Double(nullable: false),
                        PlayedGames = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.GeneticIndividualId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GeneticFactor", "FieldId", "dbo.Field");
            DropForeignKey("dbo.GeneticFactor", "GeneticIndividualId", "dbo.GeneticIndividual");
            DropForeignKey("dbo.Game", "Field_FieldId", "dbo.Field");
            DropForeignKey("dbo.FieldStatistic", "FieldId", "dbo.Field");
            DropIndex("dbo.GeneticFactor", new[] { "GeneticIndividualId" });
            DropIndex("dbo.GeneticFactor", new[] { "FieldId" });
            DropIndex("dbo.Game", new[] { "Field_FieldId" });
            DropIndex("dbo.FieldStatistic", new[] { "FieldId" });
            DropTable("dbo.GeneticIndividual");
            DropTable("dbo.GeneticFactor");
            DropTable("dbo.Game");
            DropTable("dbo.FieldStatistic");
            DropTable("dbo.Field");
        }
    }
}
