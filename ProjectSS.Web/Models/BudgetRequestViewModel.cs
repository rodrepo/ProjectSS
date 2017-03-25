using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectSS.Web.Models
{
    public class BudgetRequestViewModel
    {
        public BudgetRequestViewModel()
        {
            Item = new BudgetRequestItemViewModel();
            Items = new List<BudgetRequestItemViewModel>();
        }
        public string RequestNumber { get; set; }
        public int RNumber { get; set; }

        public DateTime? DateNeeded { get; set; }
        public decimal TotalAmount { get; set; }
        public string Purpose { get; set; }

        public bool StatusRecommendingApproval { get; set; }
        public bool StatusApproval { get; set; }
        public bool StatusRelease { get; set; }

        public string RequestorName { get; set; }
        public string RequestorId { get; set; }

        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string ProjectNumber { get; set; }
        public string IsCreate { get; set; }
        public BudgetRequestItemViewModel Item { get; set; }
        public List<BudgetRequestItemViewModel> Items { get; set; }
    }

    public class BudgetRequestItemViewModel
    {
        public string Category { get; set; }
        public string Item { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }

        public int TempId { get; set; }
        public int Id { get; set; }
        public int BudgetRequestId { get; set; }
    }

}