using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSS.Db.Entities
{
    public class ProposalEquipment : Base
    {
        public int Hours { get; set; }
        public int Months { get; set; }
        public decimal Rate { get; set; }
        public decimal Factor { get; set; }

        public int ProposalId { get; set; }
        public virtual Proposal Proposal { get; set; }

        public decimal Budget { get; set; }
        public decimal RemainingBudget { get; set; }

        public int InventoryId { get; set; }
        public virtual Inventory Inventory { get; set; }
    }
}
