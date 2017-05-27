using AutoMapper;
using Microsoft.ApplicationInsights;
using ProjectSS.Db.Contracts;
using ProjectSS.Db.Entities;
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
            var commets = _mapper.Map<List<CommentViewModel>>(await _repo.GetAllCommentsAsync());
            if(commets.Count > 10)
            {
                model.Comments = commets.Take(10).ToList();
                var toBeDeleted = commets.Where(c => !model.Comments.Any(m => m.Id == c.Id)).ToList();
                await _repo.DeleteComments(_mapper.Map<List<Comment>>(toBeDeleted), CurrentUser.Id);
                await _repo.SaveAllAsync();
            }
            else
            {
                model.Comments = commets;
            }
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> CreateSubComment(SubCommentViewModel model)
        {
            try
            {
                model.CommentBy = CurrentUser.DisplayName;
                _repo.AddSubComments(_mapper.Map<SubComment>(model), CurrentUser.Id);
                if (await _repo.SaveAllAsync())
                {
                    TempData["Success"] = string.Format("Comment has been successfully Sent");
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Error"] = "Unable to create Comment due to some internal issues.";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                _telemetryClient.TrackException(e);
                ModelState.AddModelError("error", e.Message);
                return ServerError();
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateComment(CommentViewModel model)
        {
            try
            {
                model.CommentBy = CurrentUser.DisplayName;
                _repo.AddComments(_mapper.Map<Comment>(model), CurrentUser.Id);
                if (await _repo.SaveAllAsync())
                {
                    TempData["Success"] = string.Format("Comment has been successfully Sent");
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Error"] = "Unable to create Comment due to some internal issues.";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                _telemetryClient.TrackException(e);
                ModelState.AddModelError("error", e.Message);
                return ServerError();
            }
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