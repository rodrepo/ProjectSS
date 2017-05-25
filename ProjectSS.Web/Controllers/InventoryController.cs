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
    public class InventoryController : BaseController
    {
        public InventoryController(IMapper mapper, IDataRepo repo, TelemetryClient telemetryClient)
            : base(repo, mapper, telemetryClient)
        {
            _mapper = mapper;
            _repo = repo;
        }

        public async Task<ActionResult> Index()
        {
            await RunNotifications();
            ViewBag.Role = await GetCurrentUserRole();
            var model = MapNeededValue(await _repo.GetInventoriesAsync());
            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> Create()
        {
            await RunNotifications();
            var model = new InventoryViewModel();
            await SetListItemsAsync(model);
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Create(InventoryViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _repo.AddInventory(_mapper.Map<Inventory>(model), CurrentUser.Id);
                    if (await _repo.SaveAllAsync())
                    {
                        TempData["Success"] = string.Format("Item has been successfully Created");
                        return RedirectToAction("Index");
                    }
                }
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        TempData["Error"] = error.ErrorMessage;
                    }
                }
                return View(model);
            }
            catch (Exception e)
            {
                _telemetryClient.TrackException(e);
                ModelState.AddModelError("error", e.Message);
                return ServerError();
            }
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            await RunNotifications();
            var model = _mapper.Map<InventoryViewModel>(await _repo.GetInventoryByIdAsync(id));
            await SetListItemsAsync(model);
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(InventoryViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _repo.UpdateInventory(_mapper.Map<Inventory>(model), CurrentUser.Id);
                    if (await _repo.SaveAllAsync())
                    {
                        TempData["Success"] = string.Format("Item has been successfully updated");
                        return RedirectToAction("Index");
                    }
                }
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        TempData["Error"] = error.ErrorMessage;
                    }
                }
                return View(model);
            }
            catch (Exception e)
            {
                _telemetryClient.TrackException(e);
                ModelState.AddModelError("error", e.Message);
                return ServerError();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(InventoryViewModel model)
        {
            try
            {
                await _repo.DeleteInventory(model.Id);
                if (await _repo.SaveAllAsync())
                {
                    TempData["Success"] = $"Successfully deleted item";
                }
                else
                {
                    TempData["Error"] = "Unable to delete item due to some internal issues.";
                }
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                _telemetryClient.TrackException(e);
                ModelState.AddModelError("error", e.Message);
                return ServerError();
            }
        }

        #region Helper
        public List<InventoryViewModel> MapNeededValue(List<Inventory> inventory)
        {
            var inventories = new List<InventoryViewModel>();
            if (inventory?.Any() ?? false)
            {
                foreach (var inv in inventory)
                {
                    var invt = new InventoryViewModel();
                    invt.Id = inv.Id;
                    invt.Name = inv.Name;
                    invt.Brand = inv.Brand;
                    invt.ItemModel = inv.ItemModel;
                    invt.SerialNo = inv.SerialNo;
                    invt.Location = inv.Location;
                    invt.UserId = inv.UserId;
                    invt.Quantity = inv.Quantity;
                    invt.CreatedBy = inv.CreatedBy;
                    invt.IsDeleted = inv.IsDeleted;
                    invt.InventoryNumber = inv.InventoryNumber;
                    invt.InvNumber = inv.InvNumber;
                    if (!inv.ModifiedBy.IsEmpty())
                    {
                        var user = _repo.GetUser(inv.ModifiedBy);
                        invt.ModifiedBy = user.FirstName +" "+ user.MiddleName +" "+ user.LastName;
                    }
                    else
                    {
                        var user = _repo.GetUser(inv.CreatedBy);
                        invt.ModifiedBy = user.FirstName + " " + user.MiddleName + " " + user.LastName;
                    }
                    if (inv.ModifiedDate != null)
                    {
                        invt.ModifiedDate = inv.ModifiedDate;
                    }
                    else
                    {
                        inv.ModifiedDate = inv.CreatedDate;
                    }
                    if (inv.User != null)
                    {
                        invt.Costodian = inv.User.FirstName + " " + inv.User.MiddleName + " " + inv.User.LastName;
                    }
                    inventories.Add(invt);
                }
            }
            return inventories;
        }
        #endregion
        private async Task SetListItemsAsync(InventoryViewModel model)
        {
            ViewBag.Users = ViewBag.Users ?? await GetUsersAsync(model.UserId);
        }

    }
}