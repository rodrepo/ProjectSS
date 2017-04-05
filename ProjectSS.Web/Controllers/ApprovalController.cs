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
            var user = CurrentUser;
            var role = await _repo.GetRolesByUserId(user.Id);
            List<BudgetRequestViewModel> request = new List<BudgetRequestViewModel>();
            if(role.Select(r => r.Name).FirstOrDefault() == "OM")
            {
                request = _mapper.Map<List<BudgetRequestViewModel>>(await _repo.GetBudGetRequestsForOMAsync());
            }
            else if(role.Select(r => r.Name).FirstOrDefault() == "TH")
            {
                request = _mapper.Map<List<BudgetRequestViewModel>>(await _repo.GetBudGetRequestsForTHAsync(user.Id));

            }
            else if (role.Select(r => r.Name).FirstOrDefault() == "AH")
            {
                request = _mapper.Map<List<BudgetRequestViewModel>>(await _repo.GetBudGetRequestsForAHAsync());

            }
            return View(request);
        }
    }
}