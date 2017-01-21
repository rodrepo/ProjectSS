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

       
        public async Task<ActionResult> Index()
        {
            var model = _mapper.Map<List<CRMViewModel>>(await _repo.GetCRM());
            return View(model);
        }
    }
}