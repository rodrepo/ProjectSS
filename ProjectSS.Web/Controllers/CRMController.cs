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
    [Authorize]
    public class CRMController : BaseController
    {
        public CRMController(IMapper mapper, IDataRepo repo, TelemetryClient telemetryClient)
            : base(repo, mapper, telemetryClient)
        {
            _mapper = mapper;
            _repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var model = _mapper.Map<List<CRMViewModel>>(await _repo.GetCRM());
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var model = new CRMViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Create(CRMViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _repo.AddCRM(_mapper.Map<CRM>(model), CurrentUser.Id);
                }
                if (await _repo.SaveAllAsync())
                {
                    TempData["Success"] = string.Format("CRM has been successfully Created");
                    return RedirectToAction("Index");
                }
                TempData["Error"] = "Unable to create CRM due to some internal issues.";
                return RedirectToAction("Index");
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
            var model = new ManageCRMViewModel();
            var crm = _mapper.Map<CRMViewModel>(await _repo.GetCRMById(id));
            if (crm != null)
            {
                model.CRM = crm;
            }

            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> ManageWithRevision(int id)
        {
            var model = new ManageCRMViewModel();
            var crm = _mapper.Map<CRMViewModel>(await _repo.GetCRMById(id));
            if (crm != null)
            {
                model.CRM = crm;
                var revision = _mapper.Map<List<CRMRevisionHistoryModel>>(await _repo.GetCRMRevisionHistoryByCRMId(crm.Id));
                if (revision.Count > 0)
                {
                    model.RevisionHistorys = revision;
                }
            }

            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> ManageWithEmail(int id)
        {
            var model = new ManageCRMViewModel();
            var crm = _mapper.Map<CRMViewModel>(await _repo.GetCRMById(id));
            if (crm != null)
            {
                model.CRM = crm;
                var emails = _mapper.Map<List<CRMEmailHistoryModel>>(await _repo.GetCRMEmailHistoryByCRMId(crm.Id));
                if (emails.Count > 0)
                {
                    model.EmailHistorys = emails;
                }
            }

            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> ManageWithCall(int id)
        {
            var model = new ManageCRMViewModel();
            var crm = _mapper.Map<CRMViewModel>(await _repo.GetCRMById(id));
            if (crm != null)
            {
                model.CRM = crm;
                var calls = _mapper.Map<List<CRMCallHistoryModel>>(await _repo.GetCRMCallHistoryByCRMId(crm.Id));
                if (calls.Count > 0)
                {
                    model.CallHistorys = calls;
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Manage(CRMViewModel model , string from)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _repo.UpdateCRM(_mapper.Map<CRM>(model), CurrentUser.Id);
                    var revision = new CRMRevisionHistory
                    {
                        UserName = CurrentUser.FirstName + " " + CurrentUser.MiddleName + " " + CurrentUser.LastName,
                        CRMId = model.Id
                    };
                    _repo.AddCRMRevisionHistory(revision, CurrentUser.Id);
                }
                if (await _repo.SaveAllAsync())
                {
                    TempData["Success"] = string.Format("CRM has been successfully Updated");
                    if(from == "Email")
                    {
                        return RedirectToAction("ManageWithEmail", new { id = model.Id });

                    }
                    else if(from == "Call")
                    {
                        return RedirectToAction("ManageWithCall", new { id = model.Id });
                    }
                    else if (from == "Revision")
                    {
                        return RedirectToAction("ManageWithRevision", new { id = model.Id });
                    }
                    else
                    {
                        return RedirectToAction("Manage", new { id = model.Id });
                    }

                }
                TempData["Error"] = "Unable to updated CRM due to some internal issues.";
                if (from == "Email")
                {
                    return RedirectToAction("ManageWithEmail", new { id = model.Id });

                }
                else if (from == "Call")
                {
                    return RedirectToAction("ManageWithCall", new { id = model.Id });
                }
                else if (from == "Revision")
                {
                    return RedirectToAction("ManageWithRevision", new { id = model.Id });
                }
                else
                {
                    return RedirectToAction("Manage", new { id = model.Id });
                }
            }
            catch (Exception e)
            {
                _telemetryClient.TrackException(e);
                ModelState.AddModelError("error", e.Message);
                return ServerError();
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(CRMViewModel umodel)
        {
            try
            {
                var model = await _repo.GetCRMById(umodel.Id);
                _repo.DeleteCRM(model, GetUserId());
                if (await _repo.SaveAllAsync())
                {
                    TempData["Success"] = $"Successfully deleted CRM";
                }
                else
                {
                    TempData["Error"] = "Unable to delete CRM due to some internal issues.";
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

        [HttpPost]
        public async Task<ActionResult> Email(CRMEmailHistoryModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.Sender = CurrentUser.FirstName + " " + CurrentUser.MiddleName + " " + CurrentUser.LastName;
                    _repo.AddCRMEmailHistory(_mapper.Map<CRMEmailHistory>(model), CurrentUser.Id);
                }
                if (await _repo.SaveAllAsync())
                {
                    TempData["Success"] = string.Format("History has been successfully Created");
                    return RedirectToAction("ManageWithEmail", new { id = model.CRMId });
                }
                TempData["Error"] = "Unable to updated history due to some internal issues.";
                return RedirectToAction("ManageWithEmail", new { id = model.CRMId });
            }
            catch (Exception e)
            {
                _telemetryClient.TrackException(e);
                ModelState.AddModelError("error", e.Message);
                return ServerError();
            }
        }

        [HttpPost]
        public async Task<ActionResult> Call(CRMCallHistoryModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.Caller = CurrentUser.FirstName + " " + CurrentUser.MiddleName + " " + CurrentUser.LastName;
                    _repo.AddCRMCallHistory(_mapper.Map<CRMCallHistory>(model), CurrentUser.Id);
                }
                if (await _repo.SaveAllAsync())
                {
                    TempData["Success"] = string.Format("History has been successfully Created");
                    return RedirectToAction("ManageWithCall", new { id = model.CRMId });
                }
                TempData["Error"] = "Unable to updated history due to some internal issues.";
                return RedirectToAction("ManageWithCall", new { id = model.CRMId });
            }
            catch (Exception e)
            {
                _telemetryClient.TrackException(e);
                ModelState.AddModelError("error", e.Message);
                return ServerError();
            }
        }
    }
}