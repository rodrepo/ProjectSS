namespace ProjectSS.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddsProjectTypeToProposal : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Proposals", "ProjectType", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Proposals", "ProjectType");
        }
    }
}
