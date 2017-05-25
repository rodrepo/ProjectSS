namespace ProjectSS.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddsInventoryNumber : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Inventories", "InvNumber", c => c.Int(nullable: false));
            AddColumn("dbo.Inventories", "InventoryNumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Inventories", "InventoryNumber");
            DropColumn("dbo.Inventories", "InvNumber");
        }
    }
}
