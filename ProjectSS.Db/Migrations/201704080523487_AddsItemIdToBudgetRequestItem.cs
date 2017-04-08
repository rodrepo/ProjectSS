namespace ProjectSS.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddsItemIdToBudgetRequestItem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BudgetRequestItems", "ItemId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BudgetRequestItems", "ItemId");
        }
    }
}
