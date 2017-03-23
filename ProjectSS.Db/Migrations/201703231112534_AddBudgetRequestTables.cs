namespace ProjectSS.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBudgetRequestTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BudgetRequestItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Category = c.String(),
                        Item = c.String(),
                        Description = c.String(),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BudgetRequestId = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(),
                        ModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BudgetRequests", t => t.BudgetRequestId, cascadeDelete: true)
                .Index(t => t.BudgetRequestId);
            
            CreateTable(
                "dbo.BudgetRequests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RequestNumber = c.String(),
                        RNumber = c.Int(nullable: false),
                        DateNeeded = c.DateTime(),
                        TotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Purpose = c.String(),
                        StatusRecommendingApproval = c.Boolean(nullable: false),
                        StatusApproval = c.Boolean(nullable: false),
                        StatusRelease = c.Boolean(nullable: false),
                        RequestorName = c.String(),
                        RequestorId = c.String(),
                        ProjectId = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(),
                        ModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Projects", t => t.ProjectId, cascadeDelete: true)
                .Index(t => t.ProjectId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BudgetRequests", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.BudgetRequestItems", "BudgetRequestId", "dbo.BudgetRequests");
            DropIndex("dbo.BudgetRequests", new[] { "ProjectId" });
            DropIndex("dbo.BudgetRequestItems", new[] { "BudgetRequestId" });
            DropTable("dbo.BudgetRequests");
            DropTable("dbo.BudgetRequestItems");
        }
    }
}
