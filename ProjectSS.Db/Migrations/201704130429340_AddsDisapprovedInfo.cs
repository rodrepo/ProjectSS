namespace ProjectSS.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddsDisapprovedInfo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BudgetRequests", "DisapprovedBy", c => c.String());
            AddColumn("dbo.BudgetRequests", "DisapproverRole", c => c.String());
            AddColumn("dbo.BudgetRequests", "DisapprovedNote", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.BudgetRequests", "DisapprovedNote");
            DropColumn("dbo.BudgetRequests", "DisapproverRole");
            DropColumn("dbo.BudgetRequests", "DisapprovedBy");
        }
    }
}
