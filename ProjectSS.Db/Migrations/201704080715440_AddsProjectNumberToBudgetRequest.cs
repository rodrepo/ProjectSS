namespace ProjectSS.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddsProjectNumberToBudgetRequest : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BudgetRequests", "ProjectNumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.BudgetRequests", "ProjectNumber");
        }
    }
}
