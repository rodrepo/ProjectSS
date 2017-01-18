using AutoMapper;
using Microsoft.ApplicationInsights;
using ProjectSS.Db.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectSS.Web.Controllers.Admin
{
    [Authorize]
    public class ProjectController : BaseController
    {

        public ProjectController(IMapper mapper, IDataRepo repo, TelemetryClient telemetryClient)
            : base(repo, mapper)
        {
            _mapper = mapper;
            _repo = repo;
        }

        public ActionResult Index()
        {

            return View();
        }
    }
}