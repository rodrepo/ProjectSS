using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSS.Db.Entities
{
    public class Proposal : Base
    {
        public string ContactPerson { get; set; }
        public string CompanyName { get; set; }
        public string Industry { get; set; }
        public string ContactPersonPosition { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public decimal Amount { get; set; }
        public decimal Cost { get; set; }
        public decimal NegotiationAllowance { get; set; }
        public string BDId { get; set; }
        public string TSId { get; set; }
        public string THId { get; set; }

        public int PJNumber { get; set; }
        public int PPNumber { get; set; }
        public int RVNumber { get; set; }

        public string ProjectNumber { get; set; }
        public string ProposalNumber { get; set; }
        public string RevisionNumber { get; set; }

        public int CRMId { get; set; }
        public virtual CRM CRM { get; set; }

        public virtual ICollection<ProposalStaff> ProposalStaffs { get; set; }
        public virtual ICollection<ProposalContractor> ProposalContractors { get; set; }
        public virtual ICollection<ProposalExpense> ProposalExpenses { get; set; }
        public virtual ICollection<ProposalEquipment> ProposalEquipments { get; set; }
        public virtual ICollection<ProposalLaboratory> ProposalLaboratories { get; set; }
        public virtual ICollection<ProposalCommission> ProposalCommissions { get; set; }

        public Proposal()
        {
            ProposalStaffs = new HashSet<ProposalStaff>();
            ProposalContractors = new HashSet<ProposalContractor>();
            ProposalExpenses = new HashSet<ProposalExpense>();
            ProposalEquipments = new HashSet<ProposalEquipment>();
            ProposalLaboratories = new HashSet<ProposalLaboratory>();
            ProposalCommissions = new HashSet<ProposalCommission>();
        }
    }
}
