namespace ProjectSS.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsDisapprovedboolToBudgetRequest : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BudgetRequests", "IsDisapproved", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BudgetRequests", "IsDisapproved");
        }
    }
}
