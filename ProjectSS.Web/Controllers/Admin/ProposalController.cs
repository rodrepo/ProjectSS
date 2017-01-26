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
        public async  Task<ActionResult> Index()
        {
            var model = _mapper.Map<List<ProposalViewModel>>(await _repo.GetProposalAsync());
            await SetListItemsAsync(new ProposalViewModel());
            return View(model);
        }

        #region SetListItems
        private async Task SetListItemsAsync(ProposalViewModel model)
        {
            ViewBag.CRMs = ViewBag.CRMs ?? await GetCRMsAsync(model.CRMId);
        }
        #endregion

    }
}