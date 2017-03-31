namespace ProjectSS.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddsNeedFieldsForBudget : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Projects", "Budget", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Projects", "RemainingBudget", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.ProposalCommissions", "Budget", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.ProposalCommissions", "RemainingBudget", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.ProposalContractors", "Budget", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.ProposalContractors", "RemainingBudget", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.ProposalEquipments", "Budget", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.ProposalEquipments", "RemainingBudget", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.ProposalExpenses", "Budget", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.ProposalExpenses", "RemainingBudget", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.ProposalLaboratories", "Budget", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.ProposalLaboratories", "RemainingBudget", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.ProposalStaffs", "Budget", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.ProposalStaffs", "RemainingBudget", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProposalStaffs", "RemainingBudget");
            DropColumn("dbo.ProposalStaffs", "Budget");
            DropColumn("dbo.ProposalLaboratories", "RemainingBudget");
            DropColumn("dbo.ProposalLaboratories", "Budget");
            DropColumn("dbo.ProposalExpenses", "RemainingBudget");
            DropColumn("dbo.ProposalExpenses", "Budget");
            DropColumn("dbo.ProposalEquipments", "RemainingBudget");
            DropColumn("dbo.ProposalEquipments", "Budget");
            DropColumn("dbo.ProposalContractors", "RemainingBudget");
            DropColumn("dbo.ProposalContractors", "Budget");
            DropColumn("dbo.ProposalCommissions", "RemainingBudget");
            DropColumn("dbo.ProposalCommissions", "Budget");
            DropColumn("dbo.Projects", "RemainingBudget");
            DropColumn("dbo.Projects", "Budget");
        }
    }
}
