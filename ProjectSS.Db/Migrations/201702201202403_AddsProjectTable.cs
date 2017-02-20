namespace ProjectSS.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddsProjectTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProjectNo = c.String(),
                        PJNumber = c.Int(nullable: false),
                        ProposalId = c.Int(nullable: false),
                        CRMId = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(),
                        ModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CRMs", t => t.CRMId, cascadeDelete: false)
                .ForeignKey("dbo.Proposals", t => t.ProposalId, cascadeDelete: false)
                .Index(t => t.ProposalId)
                .Index(t => t.CRMId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Projects", "ProposalId", "dbo.Proposals");
            DropForeignKey("dbo.Projects", "CRMId", "dbo.CRMs");
            DropIndex("dbo.Projects", new[] { "CRMId" });
            DropIndex("dbo.Projects", new[] { "ProposalId" });
            DropTable("dbo.Projects");
        }
    }
}
