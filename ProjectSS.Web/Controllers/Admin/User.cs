using AutoMapper;
using Microsoft.ApplicationInsights;
using ProjectSS.Db.Contracts;
using System.Web.Mvc;

namespace ProjectSS.Web.Controllers.Admin
{
    [Authorize(Roles = "OM")]
    public class User : BaseController
    {
        public User(IMapper mapper, IDataRepo repo, TelemetryClient telemetryClient)
            : base(repo, mapper)
        {
            _mapper = mapper;
            _repo = repo;
        }

        public ActionResult Index()
        {

            return View()
        };
    }
}