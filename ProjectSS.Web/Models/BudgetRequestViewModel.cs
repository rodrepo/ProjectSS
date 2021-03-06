﻿using System;
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
            ShowItems = new List<BudgetRequestItemViewModel>();
            ListOfDeleted = new List<int>();
        }
        public int TableNumber { get; set; }
        public string RequestNumber { get; set; }
        public int RNumber { get; set; }

        public DateTime? DateNeeded { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalAmountForView
        {
            get
            {
                return (ShowItems.Sum(a => a.Amount));
            }
        }
        public string Purpose { get; set; }

        public bool StatusRecommendingApproval { get; set; }
        public bool StatusApproval { get; set; }
        public bool StatusRelease { get; set; }

        //Disapproval
        public bool IsDisapproved { get; set; }
        public string DisapprovedBy { get; set; }
        public string DisapproverRole { get; set; }
        public string DisapprovedNote { get; set; }

        public string RequestorName { get; set; }
        public string RequestorId { get; set; }

        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string ProjectNumber { get; set; }
        public string IsCreate { get; set; }
        public int TobeDeleted { get; set; }
        public BudgetRequestItemViewModel Item { get; set; }
        public List<BudgetRequestItemViewModel> Items { get; set; }
        public List<BudgetRequestItemViewModel> ShowItems { get; set; }

        //Use in mapping do not use 
        public List<BudgetRequestItemViewModel> BudgetRequestItems { get; set; }

        public List<int> ListOfDeleted { get; set; }

        public string CategoryDropdown { get; set; }
        public int ItemId { get; set; }
        public int ItemId1 { get; set; }
        public int ItemId2 { get; set; }
        public int ItemId3 { get; set; }
        public int ItemId4 { get; set; }
        public int ItemId5 { get; set; }

        public bool ItemNameIsNull { get; set; }
        public DateTime? CreatedDate { get; set; }

        // For Validation
        public string Error { get; set; }

        // For Approval
        public string CurrentUserRole { get; set; }

    }

    public class BudgetRequestItemViewModel
    {
        public string Category { get; set; }
        public string Item { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public int ItemId { get; set; }

        public int TempId { get; set; }
        public int Id { get; set; }
        public int BudgetRequestId { get; set; }
    }

    public class DisapprovedViewModel
    {
        public int BudgetRequestId { get; set; }
        public bool IsDisapproved { get; set; }
        public string DisapprovedBy { get; set; }
        public string DisapproverRole { get; set; }
        public string DisapprovedNote { get; set; }
    }

    public class NewBudgetRequestModel
    {
        public int ProjectId { get; set; }
        public string ProjectNumber { get; set; }
    }

}