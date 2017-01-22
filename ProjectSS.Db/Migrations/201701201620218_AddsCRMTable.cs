namespace ProjectSS.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddsCRMTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CRMs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Reference = c.String(),
                        Number = c.String(),
                        CompanyName = c.String(),
                        Industry = c.String(),
                        Name = c.String(),
                        Position = c.String(),
                        Address = c.String(),
                        Municipality = c.String(),
                        Province = c.String(),
                        Region = c.String(),
                        Email = c.String(),
                        OfficeNo = c.String(),
                        FaxNo = c.String(),
                        MobileNo = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        CreatedDate = c.DateTime(),
                        ModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CRMs");
        }
    }
}
