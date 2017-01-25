namespace ProjectSS.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRateFieldToUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Rate", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "Rate");
        }
    }
}
