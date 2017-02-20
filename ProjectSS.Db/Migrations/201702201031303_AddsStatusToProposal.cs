namespace ProjectSS.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddsStatusToProposal : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Proposals", "Status", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Proposals", "Status");
        }
    }
}
