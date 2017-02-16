namespace ProjectSS.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddsQuantityAndNoOfDayToExpense : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProposalExpenses", "Quantity", c => c.Int(nullable: false));
            AddColumn("dbo.ProposalExpenses", "NoOfDay", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProposalExpenses", "NoOfDay");
            DropColumn("dbo.ProposalExpenses", "Quantity");
        }
    }
}
