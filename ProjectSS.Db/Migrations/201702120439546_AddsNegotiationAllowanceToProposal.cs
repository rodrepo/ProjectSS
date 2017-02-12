namespace ProjectSS.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddsNegotiationAllowanceToProposal : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Proposals", "NegotiationAllowance", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Proposals", "NegotiationAllowance");
        }
    }
}
