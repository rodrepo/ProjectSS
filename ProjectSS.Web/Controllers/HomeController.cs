using AutoMapper;
using Microsoft.ApplicationInsights;
using ProjectSS.Db.Contracts;
using ProjectSS.Web.Helpers;
using ProjectSS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ProjectSS.Web.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        private MvcApplication _mvcApplication;

        public HomeController(IMapper mapper, IDataRepo repo, TelemetryClient telemetryClient, MvcApplication mvcApplication)
            : base(repo, mapper, telemetryClient)
        {
            _mapper = mapper;
            _repo = repo;
            _mvcApplication = mvcApplication;
        }
        public async Task<ActionResult> Index()
        {
            await RunNotifications();
            HomeViewModel model = new HomeViewModel();
            if (OnlineVisitorsContainer.Visitors != null)
            {
                var visitors = OnlineVisitorsContainer.Visitors.Values.OrderByDescending(x => x.SessionStarted);
                model.TotalOnlineUsers = visitors.Count().ToString();
            }
            model.TotalProjects = await _repo.GetProjectsCountAsync();
            model.TotalNumberOfItemsAssigned = await _repo.GetInventoriesAssignedCountAsync(CurrentUser.Id);
            model.TotalPeddingRequst = await _repo.GetBudGetPeddingRequestsCountByUserIdAsync(CurrentUser.Id);
            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}