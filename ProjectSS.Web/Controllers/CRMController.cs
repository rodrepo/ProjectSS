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
    }
}