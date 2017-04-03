using AutoMapper;
using Microsoft.ApplicationInsights;
using ProjectSS.Db.Contracts;
using ProjectSS.Db.Entities;
using ProjectSS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;



namespace ProjectSS.Web.Controllers
{
    public class BudgetRequestController : BaseController
    {
        public BudgetRequestController(IMapper mapper, IDataRepo repo, TelemetryClient telemetryClient)
            : base(repo, mapper, telemetryClient)
        {
            _mapper = mapper;
            _repo = repo;
        }

        public async Task<ActionResult> Index(int id, string projectNo)
        {
            BudgetRequestViewModel model = new BudgetRequestViewModel { ProjectId = id, ProjectNumber = projectNo };
            var project = await _repo.GetProjectByIdAsync(id);
            if(project.RemainingBudget <= 0)
            {
                TempData["Error"] = "Insufficient funds";
                return RedirectToAction("Show", "Project", new { id = model.ProjectId });
            }
            await SetListItemsAsync(model);
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Index(BudgetRequestViewModel model)
        {
            try
            {
                // How this work everytime you add a new item it will store to Items note this list cant be modified to
                // as of deleting you need to add the TempId to ToBeDeleted List this list is for the deleted items you can only store id here
                // like the Items you cant modify this list 
                // to get the not deleted list you need to just filer the ToBeDeleted list to Items the get the result and add it to ShowItems
                // note if you refresh the will all temp data will be lost 

                // Add item
                await SetListItemsAsync(model);
                if (model.Item != null && model.IsCreate == "addItem")
                {
                    if (model.Items.Count > 0 && model.IsCreate != "delete")
                    {
                        // Asign tempId
                        int count = model.Items.Last().TempId;
                        model.Item.TempId = count += 1;
                    }
                    else
                    {
                        // Asign defualt value
                        model.Item.TempId = 1;
                    }

                    #region Dropdown
                    // Add Category to item list
                    if (!model.CategoryDropdown.IsEmpty())
                    {
                        model.Item.Category = model.CategoryDropdown;
                    }
                    else
                    {
                        model.ShowItems = model.Items.Where(m => !model.ListOfDeleted.Any(xx => xx == m.TempId)).ToList();
                        TempData["Error"] = "Please select a Category";
                        return View(model);
                    }

                    // Add item to item list
                    model = await ManageDropDownValues(model);
                    if (model.ItemNameIsNull == true)
                    {
                        model.ShowItems = model.Items.Where(m => !model.ListOfDeleted.Any(xx => xx == m.TempId)).ToList();
                        TempData["Error"] = "Please select a item";
                        return View(model);
                    }

                    // Description Validation
                    if (model.Item.Description.IsEmpty())
                    {
                        model.ShowItems = model.Items.Where(m => !model.ListOfDeleted.Any(xx => xx == m.TempId)).ToList();
                        TempData["Error"] = "Description is required";
                        return View(model);
                    }

                    // Ammount Validtion
                    if (model.Item.Amount <= 0)
                    {
                        model.ShowItems = model.Items.Where(m => !model.ListOfDeleted.Any(xx => xx == m.TempId)).ToList();
                        TempData["Error"] = "Please enter amount";
                        return View(model);
                    }
                    else if(!model.AmmountError.IsEmpty())
                    {
                        model.ShowItems = model.Items.Where(m => !model.ListOfDeleted.Any(xx => xx == m.TempId)).ToList();
                        TempData["Error"] = model.AmmountError;
                        return View(model);
                    }
                    #endregion

                    model.Items.Add(model.Item);
                    model.IsCreate = null;
                    TempData["Success"] = "New item added";
                    // Get not delete items
                    model.ShowItems = model.Items.Where(m => !model.ListOfDeleted.Any(xx => xx == m.TempId)).ToList();
                    return View(model);
                }

                // Removed Item
                else if (model.IsCreate != "true" && model.IsCreate != "addItem")
                {
                    // Get item to be removed
                    int toBeDeleted = model.Items.Where(m => m.TempId == model.TobeDeleted).First().TempId;
                    // Add tempid to list of deleted
                    model.ListOfDeleted.Add(toBeDeleted);
                    // Get not delete items
                    model.ShowItems = model.Items.Where(m => !model.ListOfDeleted.Any(xx => xx == m.TempId)).ToList();
                    model.TobeDeleted = 0;
                    TempData["Success"] = "Item Removed";
                    return View(model);
                }

                // Save Budget Request
                // Purpose an Date needed validation
                if (model.Purpose.IsEmpty())
                {
                    model.ShowItems = model.Items.Where(m => !model.ListOfDeleted.Any(xx => xx == m.TempId)).ToList();
                    TempData["Error"] = "Please enter Purpose of request";
                    return View(model);
                }
                if (model.DateNeeded == null)
                {
                    model.ShowItems = model.Items.Where(m => !model.ListOfDeleted.Any(xx => xx == m.TempId)).ToList();
                    TempData["Error"] = "Please enter Date Needed";
                    return View(model);
                }
                if(model.Items.Count <= 0)
                {
                    model.ShowItems = model.Items.Where(m => !model.ListOfDeleted.Any(xx => xx == m.TempId)).ToList();
                    TempData["Error"] = "Please add request item";
                    return View(model);
                }
                model.RequestorName = CurrentUser.DisplayName;
                model.RequestorId = CurrentUser.Id;
                model.ShowItems = model.Items.Where(m => !model.ListOfDeleted.Any(xx => xx == m.TempId)).ToList();
                model.TotalAmount = model.TotalAmountForView;
                BudgetRequest request = await _repo.AddBudGetRequest(_mapper.Map<BudgetRequest>(model), CurrentUser.Id);

                // Get proper list
                foreach (var item in model.ShowItems)
                {
                    item.BudgetRequestId = request.Id;
                    _repo.AddBudGetRequestItem(_mapper.Map<BudgetRequestItem>(item), CurrentUser.Id);
                }

                if (await _repo.SaveAllAsync())
                {
                    TempData["Success"] = string.Format("BudgetRequest has been successfully Sent");
                    return RedirectToAction("Show", "Project", new { id = model.ProjectId });
                }
                else
                {

                    await SetListItemsAsync(model);
                    TempData["Error"] = "Unable to send BudgetRequest due to some internal issues.";
                    return View(model);
                }
            }
            catch (Exception e)
            {
                TempData["Error"] = e.Message;
                await SetListItemsAsync(model);
                return View(model);

            }
        }

        public async Task<ActionResult> MyTransactions(int projectId, string projectNo)
        {
            ViewBag.ProjectId = projectId;
            ViewBag.ProjectNo = projectNo;
            var requests = _mapper.Map<List<BudgetRequestViewModel>>(await _repo.GetBudGetRequestsByProjectIdAndUserIdAsync(projectId, CurrentUser.Id));
            return View(requests);
        }

        public async Task<ActionResult> TransactionsSummary(int projectId, string projectNo)
        {
            ViewBag.ProjectId = projectId;
            ViewBag.ProjectNo = projectNo;
            var requests = _mapper.Map<List<BudgetRequestViewModel>>(await _repo.GetBudGetRequestsByProjectIdAsync(projectId));
            return View(requests);
        }

        #region Helper
        private async Task<BudgetRequestViewModel> ManageDropDownValues(BudgetRequestViewModel model)
        {
            // Add Item Name
            string name = "";
            if (model.CategoryDropdown == "CONTRACTORS/OUTSOURCE")
            {
                var result = await _repo.GetProposalContractorByIdAsync(model.ItemId1);
                name = result.Name;
                // Ammount Validation base on remaining budget
                if(model.Item.Amount > result.RemainingBudget)
                {
                    model.AmmountError = "The only allowed budget request for this item is P "+ result.RemainingBudget;
                }
                else if(result.RemainingBudget <= 0)
                {
                    model.AmmountError = "No more allocated fouds for this item";
                }
            }
            else if (model.CategoryDropdown == "OPERATING EXPENSES")
            {
                var result = await _repo.GetProposalExpenseByIdAsync(model.ItemId2);
                name = result.Item;
                // Ammount Validation base on remaining budget
                if (model.Item.Amount > result.RemainingBudget)
                {
                    model.AmmountError = "The only allowed budget request for this item is P " + result.RemainingBudget;
                }
                else if (result.RemainingBudget <= 0)
                {
                    model.AmmountError = "No more allocated fouds for this item";
                }
            }
            else if (model.CategoryDropdown == "EQUIPMENT")
            {
                var pEquip = await _repo.GetProposalEquipmentByIdAsync(model.ItemId3);
                var result = await _repo.GetInventoryByIdAsync(pEquip.InventoryId);
                name = result.Name;
                // Ammount Validation base on remaining budget
                if (model.Item.Amount > pEquip.RemainingBudget)
                {
                    model.AmmountError = "The only allowed budget request for this item is P " + pEquip.RemainingBudget;
                }
                else if (pEquip.RemainingBudget <= 0)
                {
                    model.AmmountError = "No more allocated fouds for this item";
                }
            }
            else if (model.CategoryDropdown == "LABORATORY ANALYSIS")
            {
                var result = await _repo.GetProposalLaboratoryByIdAync(model.ItemId4);
                name = result.Name;
                // Ammount Validation base on remaining budget
                if (model.Item.Amount > result.RemainingBudget)
                {
                    model.AmmountError = "The only allowed budget request for this item is P " + result.RemainingBudget;
                }
                else if (result.RemainingBudget <= 0)
                {
                    model.AmmountError = "No more allocated fouds for this item";
                }

            }
            else if (model.CategoryDropdown == "COMMISSIONS/REPRESENTATIONS")
            {
                var result = await _repo.GetProposalCommissionByIdAsync(model.ItemId5);
                name = result.Name;
                // Ammount Validation base on remaining budget
                if (model.Item.Amount > result.RemainingBudget)
                {
                    model.AmmountError = "The only allowed budget request for this item is P " + result.RemainingBudget;
                }
                else if (result.RemainingBudget <= 0)
                {
                    model.AmmountError = "No more allocated fouds for this item";
                }
            }
            if (name.IsEmpty())
            {
                model.ItemNameIsNull = true;
            }
            else
            {
                model.Item.ItemName = name;
            }
            model.CategoryDropdown = "";
            model.ItemId = 0;

            return model;
        }
        #endregion
        private async Task SetListItemsAsync(BudgetRequestViewModel model)
        {
            ViewBag.Contractors = ViewBag.Contractors ?? await GetContractors(model.ProjectId, model.ItemId);
            ViewBag.Expenses = ViewBag.Expenses ?? await GetExpenses(model.ProjectId, model.ItemId);
            ViewBag.Equipments = ViewBag.Equipments ?? await GetEquipments(model.ProjectId, model.ItemId);
            ViewBag.Labarotories = ViewBag.Labarotories ?? await GetLabarotories(model.ProjectId, model.ItemId);
            ViewBag.Commissions = ViewBag.Commissions ?? await GetCommissions(model.ProjectId, model.ItemId);

        }
    }
}