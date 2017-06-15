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
    public class RequestSummaryController :BaseController
    {
        public RequestSummaryController(IMapper mapper, IDataRepo repo, TelemetryClient telemetryClient)
            : base(repo, mapper, telemetryClient)
        {
            _mapper = mapper;
            _repo = repo;
        }

        public async Task<ActionResult> Index()
        {
            await RunNotifications();
            var requests = _mapper.Map<List<BudgetRequestViewModel>>(await _repo.GetBudGetRequests());
            ViewBag.Projects = ViewBag.Projects ?? await GetProjectsAsync();
            var number = 1;
            foreach (var val in requests)
            {
                val.TableNumber = number;
                number += 1;
            }
            return View(requests);
        }
    }
}