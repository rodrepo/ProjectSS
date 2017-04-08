using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSS.Db.Entities
{
    public class BudgetRequestItem: Base
    {
        public string Category { get; set; }
        public string Item { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public int ItemId { get; set; }

        public int BudgetRequestId { get; set; }
        public virtual BudgetRequest BudgetRequest { get; set; }
    }
}
