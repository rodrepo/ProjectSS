namespace ProjectSS.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddsIsClosedToProject : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Projects", "IsClosed", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Projects", "IsClosed");
        }
    }
}
