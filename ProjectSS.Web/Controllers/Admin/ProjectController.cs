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

namespace ProjectSS.Web.Controllers.Admin
{
    [Authorize]
    public class ProjectController : BaseController
    {

        public ProjectController(IMapper mapper, IDataRepo repo, TelemetryClient telemetryClient)
            : base(repo, mapper, telemetryClient)
        {
            _mapper = mapper;
            _repo = repo;
        }

        public async Task<ActionResult> Index()
        {
            await RunNotifications();
            var model = _mapper.Map<List<ProjectViewModel>>(await _repo.GetProjectsAsync());
            return View(model);
        }

        public async Task<ActionResult> Show(int id)
        {
            await RunNotifications();
            var role = await _repo.GetRolesByUserId(CurrentUser.Id);
            ViewBag.Role = role.Select(r => r.Name).FirstOrDefault();
            var model = _mapper.Map<ProjectViewModel>(await _repo.GetProjectByIdAsync(id));
            var proposal = await _repo.GetProposalByIdAsync(model.ProposalId);
            model = MapProposal(proposal, model);
            model.BDName = GetUserName(model.ProposalModel.BDId);
            model.THName = GetUserName(model.ProposalModel.THId);
            model.TSName = GetUserName(model.ProposalModel.TSId);
            return View(model);
        }

        public async Task<ActionResult>CloseProject(int id)
        {
            await _repo.CloseProject(id);
            if(await _repo.SaveAllAsync())
            {
                TempData["Success"] = string.Format("Project has been successfully closed");
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Error"] = "Unable to update due to some internal issues.";
                return RedirectToAction("Show", new { @id = id });
            }
        }

        #region Helper
        private ProjectViewModel MapProposal(Proposal proposal, ProjectViewModel model)
        {
            model.ProposalModel = _mapper.Map<ProposalViewModel>(proposal);
            if (proposal.ProposalStaffs?.Any() ?? false)
            {
                model.ProposalModel.Staffs = _mapper.Map<List<ProposalStaffModel>>(proposal.ProposalStaffs);
            }
            if (proposal.ProposalContractors?.Any() ?? false)
            {
                model.ProposalModel.Contractors = _mapper.Map<List<ProposalContractorModel>>(proposal.ProposalContractors);
            }
            if (proposal.ProposalExpenses?.Any() ?? false)
            {
                model.ProposalModel.Expenses = _mapper.Map<List<ProposalExpenseModel>>(proposal.ProposalExpenses);
            }
            if (proposal.ProposalEquipments?.Any() ?? false)
            {
                model.ProposalModel.Equipments = _mapper.Map<List<ProposalEquipmentModel>>(proposal.ProposalEquipments);
            }
            if (proposal.ProposalLaboratories?.Any() ?? false)
            {
                model.ProposalModel.Laboratories = _mapper.Map<List<ProposalLaboratoryModel>>(proposal.ProposalLaboratories);
            }
            if (proposal.ProposalCommissions?.Any() ?? false)
            {
                model.ProposalModel.Commissions = _mapper.Map<List<ProposalCommissionModel>>(proposal.ProposalCommissions);
            }
            return model;
        }

        public string GetUserName(string userId)
        {
            return _repo.GetUserName(userId);
        }
        #endregion
    }
}