using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSS.Db.Entities
{
    public class BudgetRequest : Base
    {
        public string RequestNumber { get; set; }
        public int RNumber { get; set; }

        public DateTime? DateNeeded { get; set; }
        public decimal TotalAmount { get; set; }
        public string Purpose { get; set; }

        public bool StatusRecommendingApproval { get; set; }
        public bool StatusApproval { get; set; }
        public bool StatusRelease { get; set; }

        //Disapproved
        public bool IsDisapproved { get; set; }
        public string DisapprovedBy { get; set; }
        public string DisapproverRole { get; set; }
        public string DisapprovedNote { get; set; }

        public string RequestorName { get; set; }
        public string RequestorId { get; set; }

        public int ProjectId { get; set; }
        public string ProjectNumber { get; set; }
        public virtual Project Project { get; set; }

        public virtual ICollection<BudgetRequestItem> BudgetRequestItems { get; set; }
        public BudgetRequest()
        {
            BudgetRequestItems = new HashSet<BudgetRequestItem>();
        }
    }
}
