namespace ProjectSS.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCommissionTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProposalCommissions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Note = c.String(),
                        Cost = c.Decimal(nullable: false, precision: 18, scale: 2),
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProposalCommissions", "ProposalId", "dbo.Proposals");
            DropIndex("dbo.ProposalCommissions", new[] { "ProposalId" });
            DropTable("dbo.ProposalCommissions");
        }
    }
}
