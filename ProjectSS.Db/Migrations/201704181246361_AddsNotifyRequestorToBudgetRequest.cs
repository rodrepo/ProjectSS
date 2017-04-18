namespace ProjectSS.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddsNotifyRequestorToBudgetRequest : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BudgetRequests", "NotifyRequestor", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BudgetRequests", "NotifyRequestor");
        }
    }
}
