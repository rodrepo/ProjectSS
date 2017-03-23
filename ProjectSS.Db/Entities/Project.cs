using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSS.Db.Entities
{
    public class Project : Base
    {
        public string ProjectNo { get; set; }
        public int PJNumber { get; set; }

        public int ProposalId { get; set; }
        public virtual Proposal Proposal { get; set; }

        public int CRMId { get; set; }
        public virtual CRM CRM { get; set; }

        public virtual ICollection<BudgetRequest> BudgetRequests { get; set; }

        public Project()
        {
            BudgetRequests = new HashSet<BudgetRequest>();
        }
    }
}
