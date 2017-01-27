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

        [HttpGet]
        public async Task<ActionResult> Manage(int id)
        {
            var mergeValues = await MergeToProposal(_mapper.Map<ProposalViewModel>(await _repo.GetProposalByIdAsync(id)));
            await DropdownListForUsers();
            await SetListItemsAsync(mergeValues);
            return View(mergeValues);
        }

        [HttpPost]
        public async Task<ActionResult> CreateStaff(ProposalStaffViewModel model)
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
            foreach(var value in ModelState.Values)
            {
                foreach(var error in value.Errors)
                {
                    TempData["Error"] = error.ErrorMessage;
                }
            }
            return RedirectToAction("Index");
        }

        #region Private Helpers
        private async Task<ProposalViewModel> MergeToProposal(ProposalViewModel model)
        {
            if (model != null)
            {
                //Staff
                var staff = await MapNeedFieldForProposalStaff(_mapper.Map<List<ProposalStaffViewModel>>(await _repo.GetProposalStaffsByProposalIdAsync(model.Id)));
                if (staff?.Any() ?? false)
                {
                    model.ProposalStaffs = staff;
                    model.TotalStaffBilledToClient = staff.Select(m => m.BilledToClient).Sum();
                    model.TotalStaffDirectCost = staff.Select(m => m.DirectCost).Sum();
                }
            }
            return model;
        }

        private async Task<List<ProposalStaffViewModel>> MapNeedFieldForProposalStaff(List<ProposalStaffViewModel> staffs)
        {
            if (staffs?.Any() ?? false)
            {
                foreach (var staff in staffs)
                {
                    if (staff != null && !staff.UserId.IsEmpty())
                    {
                        var user = await _repo.GetUserByIdAsync(staff.UserId);
                        staff.Name = user.FirstName + " " + user.MiddleName + " " + user.LastName;
                        staff.BillingRate = user.Rate;
                        staff.TotalManHours = staff.ManHours * staff.ManMonths;
                        staff.DirectCost = user.Rate * staff.TotalManHours;
                        staff.BilledToClient =  staff.DirectCost * staff.Factor;
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