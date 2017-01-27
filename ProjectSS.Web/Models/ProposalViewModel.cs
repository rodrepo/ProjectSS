using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectSS.Web.Models
{
    public class ProposalViewModel
    {
        public ProposalViewModel()
        {
            ProposalModelContractors = new List<ProposalContractorViewModel>();
            ProposalModelExpenses = new List<ProposalExpenseViewModel>();
            ProposalModelStaffs = new List<ProposalStaffViewModel>();
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

        public int CRMId { get; set; }
        public int Id { get; set; }

        public List<ProposalContractorViewModel> ProposalModelContractors { get; set; }
        public List<ProposalExpenseViewModel> ProposalModelExpenses { get; set; }
        public List<ProposalStaffViewModel> ProposalModelStaffs { get; set; }
    }


    public class ProposalContractorViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int NoOfDay { get; set; }
        public decimal Rate { get; set; }
        public decimal Factor { get; set; }

        public int ProposalId { get; set; }
        public int Id { get; set; }
    }

    public class ProposalExpenseViewModel
    {
        public string Item { get; set; }
        public string Description { get; set; }
        public decimal Factor { get; set; }
        public decimal DirectCost { get; set; }

        public int ProposalId { get; set; }
        public int Id { get; set; }
    }

    public class ProposalStaffViewModel
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
}