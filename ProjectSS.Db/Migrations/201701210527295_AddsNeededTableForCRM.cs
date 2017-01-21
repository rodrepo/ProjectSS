namespace ProjectSS.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddsNeededTableForCRM : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CRMCallHistories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateTime = c.DateTime(),
                        Remarks = c.String(),
                        Caller = c.String(),
                        CRMId = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(),
                        ModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CRMs", t => t.CRMId, cascadeDelete: true)
                .Index(t => t.CRMId);
            
            CreateTable(
                "dbo.CRMEmailHistories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateTime = c.DateTime(),
                        Remarks = c.String(),
                        Sender = c.String(),
                        CRMId = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(),
                        ModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CRMs", t => t.CRMId, cascadeDelete: true)
                .Index(t => t.CRMId);
            
            CreateTable(
                "dbo.CRMRevisionHistories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateTime = c.DateTime(),
                        Remarks = c.String(),
                        UserName = c.String(),
                        CRMId = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(),
                        ModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CRMs", t => t.CRMId, cascadeDelete: true)
                .Index(t => t.CRMId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CRMRevisionHistories", "CRMId", "dbo.CRMs");
            DropForeignKey("dbo.CRMEmailHistories", "CRMId", "dbo.CRMs");
            DropForeignKey("dbo.CRMCallHistories", "CRMId", "dbo.CRMs");
            DropIndex("dbo.CRMRevisionHistories", new[] { "CRMId" });
            DropIndex("dbo.CRMEmailHistories", new[] { "CRMId" });
            DropIndex("dbo.CRMCallHistories", new[] { "CRMId" });
            DropTable("dbo.CRMRevisionHistories");
            DropTable("dbo.CRMEmailHistories");
            DropTable("dbo.CRMCallHistories");
        }
    }
}
