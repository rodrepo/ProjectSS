using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjectSS.Web.Models
{
    public class ProposalViewModel
    {
        public ProposalViewModel()
        {
           Contractors = new List<ProposalContractorModel>();
           Expenses = new List<ProposalExpenseModel>();
           Staffs = new List<ProposalStaffModel>();
           Equipments = new List<ProposalEquipmentModel>();
           Laboratories = new List<ProposalLaboratoryModel>();
           Commissions = new List<ProposalCommissionModel>();
        }

        public string ContactPerson { get; set; }
        public string CompanyName { get; set; }
        public string Industry { get; set; }
        public string ContactPersonPosition { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public decimal Amount { get; set; }
        public decimal Cost { get; set; }

        public decimal MangementFeeBilledToClient { get; set; }
        public string BDId { get; set; }
        public string TSId { get; set; }
        public string THId { get; set; }
        public string Status { get; set; }

        public int PJNumber { get; set; }
        public int PPNumber { get; set; }
        public int RVNumber { get; set; }

        public string ProjectNumber { get; set; }
        public string ProposalNumber { get; set; }
        public string RevisionNumber { get; set; }

        public decimal TotalStaffBilledToClient { get; set; }
        public decimal TotalStaffDirectCost { get; set; }

        public decimal TotalContractorBilledToClient { get; set; }
        public decimal TotalContractorDirectCost { get; set; }

        public decimal TotalExpenseBilledToClient { get; set; }
        public decimal TotalExpenseDirectCost { get; set; }

        public decimal TotalEquipmentBilledToClient { get; set; }
        public decimal TotalEquipmentDirectCost { get; set; }

        public decimal TotalLaboratoryBilledToClient { get; set; }
        public decimal TotalLaboratoryDirectCost { get; set; }

        public decimal TotalCommissionBilledToClient { get; set; }
        public decimal TotalCommissionDirectCost { get; set; }

        public int CRMId { get; set; }
        public int Id { get; set; }

        public List<ProposalContractorModel> Contractors { get; set; }
        public List<ProposalExpenseModel> Expenses { get; set; }
        public List<ProposalStaffModel> Staffs { get; set; }
        public List<ProposalEquipmentModel> Equipments { get; set; }
        public List<ProposalLaboratoryModel> Laboratories { get; set; }
        public List<ProposalCommissionModel> Commissions { get; set; }

    }


    public class ProposalContractorModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int NoOfDay { get; set; }
        public decimal Rate { get; set; }
        public decimal Factor { get; set; }
        public decimal DirectCost { get; set; }
        public decimal BilledToClient { get; set; }
        public int ProposalId { get; set; }
        public int Id { get; set; }
        public bool IsDeleted { get; set; }

    }

    public class ProposalExpenseModel
    {
        public string Item { get; set; }
        public string Description { get; set; }
        public decimal Factor { get; set; }
        public decimal DirectCost { get; set; }
        public bool IsDeleted { get; set; }
        public decimal BilledToClient { get; set; }
        public int ProposalId { get; set; }
        public int Id { get; set; }
    }

    public class ProposalStaffModel
    {
        public string Name { get; set; }
        public int ManHours { get; set; }
        public int ManMonths { get; set; }
        public decimal Factor { get; set; }
        public int TotalManHours { get; set; }
        public decimal BillingRate { get; set; }
        public decimal DirectCost { get; set; }
        public decimal BilledToClient { get; set; }
        public string UserId { get; set; }
        public int ProposalId { get; set; }
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class ProposalEquipmentModel
    {
        public string Name { get; set; }
        public int Hours { get; set; }
        public int Months { get; set; }
        public decimal Rate { get; set; }
        public decimal Factor { get; set; }
        public decimal DirectCost { get; set; }
        public decimal BilledToClient { get; set; }
        public int TotalHours { get; set; }
        public bool IsDeleted { get; set; }

        public int Id { get; set; }
        public int ProposalId { get; set; }
        public int InventoryId { get; set; }
    }

    public class ProposalLaboratoryModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Parameters { get; set; }
        public int NoOfStations { get; set; }
        public string Replicate { get; set; }
        public int Cost { get; set; }
        public decimal Factor { get; set; }

        public decimal DirectCost { get; set; }
        public decimal BilledToClient { get; set; }

        public bool IsDeleted { get; set; }
        public int ProposalId { get; set; }
    }

    public class ProposalCommissionModel
    {
        public string Name { get; set; }
        public string Note { get; set; }
        public decimal Cost { get; set; }
        public decimal Factor { get; set; }
        public bool IsDeleted { get; set; }
        public decimal BilledToClient { get; set; }

        public int Id { get; set; }
        public int ProposalId { get; set; }
    }
}