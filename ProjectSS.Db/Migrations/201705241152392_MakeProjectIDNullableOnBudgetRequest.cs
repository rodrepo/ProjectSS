namespace ProjectSS.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakeProjectIDNullableOnBudgetRequest : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.BudgetRequests", "ProjectId", "dbo.Projects");
            DropIndex("dbo.BudgetRequests", new[] { "ProjectId" });
            AlterColumn("dbo.BudgetRequests", "ProjectId", c => c.Int());
            CreateIndex("dbo.BudgetRequests", "ProjectId");
            AddForeignKey("dbo.BudgetRequests", "ProjectId", "dbo.Projects", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BudgetRequests", "ProjectId", "dbo.Projects");
            DropIndex("dbo.BudgetRequests", new[] { "ProjectId" });
            AlterColumn("dbo.BudgetRequests", "ProjectId", c => c.Int(nullable: false));
            CreateIndex("dbo.BudgetRequests", "ProjectId");
            AddForeignKey("dbo.BudgetRequests", "ProjectId", "dbo.Projects", "Id", cascadeDelete: true);
        }
    }
}
