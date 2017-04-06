using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSS.Db.Entities
{
    public class ProposalExpense : Base
    {
        public string Item { get; set; }
        public string Description { get; set; }
        public decimal Factor { get; set; }
        public decimal Cost { get; set; }
        public int Quantity { get; set; }
        public int NoOfDay { get; set; }

        public decimal Budget { get; set; }
        public decimal RemainingBudget { get; set; }

        public int ProposalId { get; set; }
        public virtual Proposal Proposal { get; set; }
    }
}
