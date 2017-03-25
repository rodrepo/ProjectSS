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

        public ActionResult Index(int id, string projectNo)
        {
           BudgetRequestViewModel  model = new BudgetRequestViewModel { ProjectId = id, ProjectNumber = projectNo };
           return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Index(BudgetRequestViewModel model)
        {
            try
            {
                // How this work evertime you add a new item it will store to Items note this list cant be modified to
                // as of deleting you need to add the TempId to ToBeDeleted List this list is for the deleted items you can only store id here
                // like the Items you cant modify this list 
                // to get the not deleted list you need to just filer the ToBeDeleted list to Items the get the result and add it to ShowItems
                // note if you refresh the will all temp data will be lost 


                // Add item
                if(model.Item != null && model.IsCreate == "addItem")
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
                    // Add item to item list
                    model.Items.Add(model.Item);
                    model.IsCreate = null;
                    TempData["Success"] = "New item added";
                    // Get not delete items
                    model.ShowItems = model.Items.Where(m => !model.ListOfDeleted.Any(xx => xx == m.TempId)).ToList();
                    return View(model);
                }
                // Removed Item
                else if(model.IsCreate != "true" && model.IsCreate != "addItem")
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
                if (ModelState.IsValid)
                {
                    BudgetRequest request = await _repo.AddBudGetRequest(_mapper.Map<BudgetRequest>(model), CurrentUser.Id);
                    // Get proper list
                    model.ShowItems = model.Items.Where(m => !model.ListOfDeleted.Any(xx => xx == m.TempId)).ToList();
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
                        TempData["Error"] = "Unable to send BudgetRequest due to some internal issues.";
                        return View(model);
                    }
                }
                else
                {
                    foreach(var state in ModelState)
                    {
                        foreach(var error in state.Value.Errors)
                        {
                            TempData["Error"] = error.ErrorMessage;
                        }
                    }
                }
                return View(model);
            }
            catch (Exception e)
            {
                TempData["Error"] = e.Message;
                return View(model);

            }
        }
    }
}