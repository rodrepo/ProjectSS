using AutoMapper;
using Microsoft.ApplicationInsights;
using ProjectSS.Db.Contracts;
using ProjectSS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ProjectSS.Web.Controllers
{
    public class ApprovalController : BaseController
    {
        public ApprovalController(IMapper mapper, IDataRepo repo, TelemetryClient telemetryClient)
            : base(repo, mapper, telemetryClient)
        {
            _mapper = mapper;
            _repo = repo;
        }

        public async Task<ActionResult> Index()
        {
            await RunNotifications();
            var user = CurrentUser;
            var role = await _repo.GetRolesByUserId(user.Id);
            List<BudgetRequestViewModel> request = new List<BudgetRequestViewModel>();
            if (role.Select(r => r.Name).FirstOrDefault() == "OM")
            {
                request = _mapper.Map<List<BudgetRequestViewModel>>(await _repo.GetBudGetRequestsForOMAsync());
            }
            else if (role.Select(r => r.Name).FirstOrDefault() == "TH")
            {
                request = _mapper.Map<List<BudgetRequestViewModel>>(await _repo.GetBudGetRequestsForTHAsync(user.Id));

            }
            else if (role.Select(r => r.Name).FirstOrDefault() == "AH")
            {
                request = _mapper.Map<List<BudgetRequestViewModel>>(await _repo.GetBudGetRequestsForAHAsync());

            }
            var number = 1;
            foreach (var val in request)
            {
                val.TableNumber = number;
                number += 1;
            }
            return View(request);
        }

        public async Task<ActionResult> Approved(int id)
        {
            var role = await _repo.GetRolesByUserId(CurrentUser.Id);
            var roleName = role.Select(r => r.Name).FirstOrDefault();
            await _repo.ApprovedBudgetRequest(id, roleName);
            await _repo.SaveAllAsync();
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Disapproved(DisapprovedViewModel model)
        {
            var role = await _repo.GetRolesByUserId(CurrentUser.Id);
            var roleName = GetProperRoleName(role.Select(r => r.Name).FirstOrDefault());
            await _repo.DisapprovedBudgetRequest(model.BudgetRequestId , model.DisapprovedNote, CurrentUser.DisplayName, roleName);
            await _repo.SaveAllAsync();
            return RedirectToAction("Index");
        }
    }
}
