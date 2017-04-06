namespace ProjectSS.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedToCost : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProposalExpenses", "Cost", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.ProposalExpenses", "DirectCost");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ProposalExpenses", "DirectCost", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.ProposalExpenses", "Cost");
        }
    }
}
