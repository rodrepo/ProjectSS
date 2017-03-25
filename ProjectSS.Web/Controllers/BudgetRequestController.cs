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
                // Add item
                if(model.Item != null && model.IsCreate == "addItem")
                {
                    if (model.Items.Count > 0)
                    {
                        // Asign tempId
                        model.Item.TempId = model.Items[model.Items.Count - 1].TempId += 1;
                    }
                    else
                    {
                        // Asign defualt value
                        model.Item.TempId = 1;
                    }
                    // Add item to item list
                    model.Items.Add(model.Item);
                    TempData["Success"] = "New item added";
                    return View(model);
                }
                // Removed Item
                else if(model.IsCreate != "true")
                {
                    // Get item to be removed
                    var toBeRemoved = model.Items.Where(m => m.TempId.ToString() == model.IsCreate).FirstOrDefault();
                    // Remove Item
                    model.Items.Remove(toBeRemoved);
                    TempData["Success"] = "Item Removed";
                    return View(model);
                }
                // Save Budget Request
                if (ModelState.IsValid)
                {
                    BudgetRequest request = await _repo.AddBudGetRequest(_mapper.Map<BudgetRequest>(model), CurrentUser.Id);
                    foreach (var item in model.Items)
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