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

namespace ProjectSS.Web.Controllers.Admin
{
    [Authorize]
    public class ProposalController : BaseController
    {
        public ProposalController(IMapper mapper, IDataRepo repo, TelemetryClient telemetryClient)
            : base(repo, mapper, telemetryClient)
        {
            _mapper = mapper;
            _repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var model = _mapper.Map<List<ProposalViewModel>>(await _repo.GetProposalAsync());
            await SetListItemsAsync(new ProposalViewModel());
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Create(ProposalViewModel model)
        {
            if (model.CRMId != 0)
            {
                var proposal = await _repo.AddPorposalAsync(_mapper.Map<Proposal>(model), CurrentUser.Id);
                if (await _repo.SaveAllAsync())
                {

                    TempData["Success"] = string.Format("Proposal has been successfully Created");
                    return RedirectToAction("Manage", new { @id = proposal.Id });
                }
                TempData["Error"] = "Unable to create Proposal due to some internal issues.";
            }
            return RedirectToAction("Index");

        }

        [HttpPost]
        public async Task<ActionResult> CreateStaff(ProposalStaffModel model)
        {
            if (ModelState.IsValid)
            {
                _repo.AddProposalStaff(_mapper.Map<ProposalStaff>(model), CurrentUser.Id);
                if (await _repo.SaveAllAsync())
                {

                    TempData["Success"] = string.Format("Staff has been successfully Created");
                    return RedirectToAction("Manage", new { @id = model.ProposalId });
                }
                TempData["Error"] = "Unable to create Staff due to some internal issues.";
            }
            foreach (var value in ModelState.Values)
            {
                foreach (var error in value.Errors)
                {
                    TempData["Error"] = error.ErrorMessage;
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteStaff(int id , int proposalId)
        {
            try
            {
                await _repo.DeleteProposalStaff(id);
                if (await _repo.SaveAllAsync())
                {
                    TempData["Success"] = $"Successfully deleted staff";
                }
                else
                {
                    TempData["Error"] = "Unable to delete staff due to some internal issues.";
                }
                return RedirectToAction("Manage", new { @id = proposalId });
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
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _repo.DeleteProposal(id);
                if (await _repo.SaveAllAsync())
                {
                    TempData["Success"] = $"Successfully deleted proposal";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Error"] = "Unable to delete proposal due to some internal issues.";
                    return RedirectToAction("Manage", new { @id = id });
                }
            }
            catch (Exception e)
            {
                _telemetryClient.TrackException(e);
                ModelState.AddModelError("error", e.Message);
                return ServerError();
            }
        }

        [HttpGet]
        public async Task<ActionResult> Manage(int id)
        {
            var getValues = await _repo.GetProposalByIdAsync(id);
            var proposal = _mapper.Map<ProposalViewModel>(getValues);
            if (proposal != null)
            {
                proposal.Staffs = _mapper.Map<List<ProposalStaffModel>>(getValues.ProposalStaffs);
                var mergeValues = await MergeToProposal(proposal);
                await DropdownListForUsers();
                await SetListItemsAsync(mergeValues);
                return View(mergeValues);
            }
            return View(proposal);
        }

        [HttpPost]
        public async Task<ActionResult> Manage(ProposalViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _repo.UpdateProposal(_mapper.Map<Proposal>(model), CurrentUser.Id);
                    if (await _repo.SaveAllAsync())
                    {
                        TempData["Success"] = string.Format("CRM has been successfully Updated");
                        return RedirectToAction("Manage", new { id = model.Id });
                    }
                }
                foreach(var value in ModelState.Values)
                {
                    foreach(var error in value.Errors)
                    {
                        TempData["Error"] = error.ErrorMessage;
                        return RedirectToAction("Manage", new { id = model.Id });
                    }
                }
                return RedirectToAction("Manage", new { id = model.Id });
            }
            catch (Exception e)
            {
                _telemetryClient.TrackException(e);
                ModelState.AddModelError("error", e.Message);
                return ServerError();
            }
        }

        #region Private Helpers
        private async Task<ProposalViewModel> MergeToProposal(ProposalViewModel model)
        {
            if (model != null)
            {
                if(model.Cost > 0)
                {
                    model.MangementFeeBilledToClient = decimal.Parse("0.4") * model.Cost;
                }

                #region Staff
                var result = model.Staffs.Where(s => !s.IsDeleted).ToList();
                if (result?.Any() ?? false)
                {
                    var staff = await MapNeedFieldForProposalStaff(result);
                    if (staff?.Any() ?? false)
                    {
                        model.TotalStaffBilledToClient = staff.Select(m => m.BilledToClient).Sum();
                        model.TotalStaffDirectCost = staff.Select(m => m.DirectCost).Sum();
                    }
                }
                model.Staffs = result;
                #endregion
            }
            return model;
        }

        private async Task<List<ProposalStaffModel>> MapNeedFieldForProposalStaff(List<ProposalStaffModel> staffs)
        {
            if (staffs?.Any() ?? false)
            {
                foreach (var staff in staffs)
                {
                    var user = await _repo.GetUserByIdAsync(staff.UserId);
                    if (user != null)
                    {
                        if (staff != null && !staff.UserId.IsEmpty())
                        {

                            staff.Name = user.FirstName + " " + user.MiddleName + " " + user.LastName;
                            staff.BillingRate = user.Rate;
                            staff.TotalManHours = staff.ManHours * staff.ManMonths;
                            staff.DirectCost = user.Rate * staff.TotalManHours;
                            staff.BilledToClient = staff.DirectCost * staff.Factor;
                        }
                    }
                    else
                    {
                        if (staff.Id > 0)
                        {
                            await _repo.DeleteProposalStaff(staff.Id);
                            await _repo.SaveAllAsync();
                        }
                    }
                }
            }
            return staffs;
        }

        #endregion

        #region SetListItems
        private async Task SetListItemsAsync(ProposalViewModel model)
        {
            ViewBag.CRMs = ViewBag.CRMs ?? await GetCRMsAsync(model.CRMId);
            ViewBag.BDs = ViewBag.BDs ?? await GetBDUsersAsync(model.BDId);
            ViewBag.TSs = ViewBag.TSs ?? await GetTSUsersAsync(model.TSId);
            ViewBag.THs = ViewBag.THs ?? await GetTHUsersAsync(model.THId);
        }

        private async Task DropdownListForUsers()
        {
            ViewBag.Users = ViewBag.Users ?? await GetUsersAsync();
        }
        #endregion

    }
}