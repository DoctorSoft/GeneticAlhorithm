namespace Data.Migration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFieldToSaveGameProccess : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Game", "Proccess", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Game", "Proccess");
        }
    }
}
