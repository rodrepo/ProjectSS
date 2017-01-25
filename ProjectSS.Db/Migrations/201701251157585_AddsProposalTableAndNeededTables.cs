namespace ProjectSS.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddsProposalTableAndNeededTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Proposals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ContactPerson = c.String(),
                        CompanyName = c.String(),
                        Industry = c.String(),
                        ContactPersonPosition = c.String(),
                        Title = c.String(),
                        Description = c.String(),
                        Location = c.String(),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Cost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BDId = c.String(),
                        TSId = c.String(),
                        THId = c.String(),
                        PJNumber = c.Int(nullable: false),
                        PPNumber = c.Int(nullable: false),
                        RVNumber = c.Int(nullable: false),
                        ProjectNumber = c.String(),
                        ProposalNumber = c.String(),
                        RevisionNumber = c.String(),
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
                "dbo.ProposalContractors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        NoOfDay = c.Int(nullable: false),
                        Rate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Factor = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ProposalId = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(),
                        ModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Proposals", t => t.ProposalId, cascadeDelete: true)
                .Index(t => t.ProposalId);
            
            CreateTable(
                "dbo.ProposalExpenses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Item = c.String(),
                        Description = c.String(),
                        Factor = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DirectCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ProposalId = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(),
                        ModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Proposals", t => t.ProposalId, cascadeDelete: true)
                .Index(t => t.ProposalId);
            
            CreateTable(
                "dbo.ProposalStaffs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ManHours = c.Int(nullable: false),
                        ManMonths = c.Int(nullable: false),
                        Factor = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UserId = c.String(maxLength: 128),
                        ProposalId = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(),
                        ModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Proposals", t => t.ProposalId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.ProposalId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProposalStaffs", "UserId", "dbo.Users");
            DropForeignKey("dbo.ProposalStaffs", "ProposalId", "dbo.Proposals");
            DropForeignKey("dbo.ProposalExpenses", "ProposalId", "dbo.Proposals");
            DropForeignKey("dbo.ProposalContractors", "ProposalId", "dbo.Proposals");
            DropForeignKey("dbo.Proposals", "CRMId", "dbo.CRMs");
            DropIndex("dbo.ProposalStaffs", new[] { "ProposalId" });
            DropIndex("dbo.ProposalStaffs", new[] { "UserId" });
            DropIndex("dbo.ProposalExpenses", new[] { "ProposalId" });
            DropIndex("dbo.ProposalContractors", new[] { "ProposalId" });
            DropIndex("dbo.Proposals", new[] { "CRMId" });
            DropTable("dbo.ProposalStaffs");
            DropTable("dbo.ProposalExpenses");
            DropTable("dbo.ProposalContractors");
            DropTable("dbo.Proposals");
        }
    }
}
