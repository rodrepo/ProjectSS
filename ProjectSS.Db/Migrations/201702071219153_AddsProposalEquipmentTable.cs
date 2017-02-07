namespace ProjectSS.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddsProposalEquipmentTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProposalEquipments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Hours = c.Int(nullable: false),
                        Months = c.Int(nullable: false),
                        Rate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Factor = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ProposalId = c.Int(nullable: false),
                        InventoryId = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(),
                        ModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Inventories", t => t.InventoryId, cascadeDelete: true)
                .ForeignKey("dbo.Proposals", t => t.ProposalId, cascadeDelete: true)
                .Index(t => t.ProposalId)
                .Index(t => t.InventoryId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProposalEquipments", "ProposalId", "dbo.Proposals");
            DropForeignKey("dbo.ProposalEquipments", "InventoryId", "dbo.Inventories");
            DropIndex("dbo.ProposalEquipments", new[] { "InventoryId" });
            DropIndex("dbo.ProposalEquipments", new[] { "ProposalId" });
            DropTable("dbo.ProposalEquipments");
        }
    }
}
