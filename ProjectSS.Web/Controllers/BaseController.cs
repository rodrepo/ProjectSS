using AutoMapper;
using Microsoft.ApplicationInsights;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using ProjectSS.Db.Contracts;
using ProjectSS.Db.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ProjectSS.Web.Controllers
{
    public class BaseController : Controller
    {
        protected IMapper _mapper;
        protected IDataRepo _repo;

        protected TelemetryClient _telemetryClient;
        public Func<string> GetUserId;
        private UserManager _userManager;
        private ApplicationSignInManager _signInManager;

        public BaseController(IDataRepo repo, IMapper mapper, TelemetryClient telemetryClient)
        {
            _repo = repo;
            _mapper = mapper;
            _telemetryClient = telemetryClient;
            GetUserId = () => User.Identity.GetUserId();
        }

        public User CurrentUser
        {
            get
            {
                ViewBag.CurrentUser = ViewBag.CurrentUser ?? _repo.GetUser(GetUserId());
                return ViewBag.CurrentUser;
            }
        }

        public UserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<UserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        protected IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        protected void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        public ActionResult ServerError()
        {
            return RedirectToAction("ServerError", "Error");
        }

        #region SetListItems
        protected async Task<List<SelectListItem>> GetRolesAsync(bool includeBlank, string id = null)
        {
            var result = new List<SelectListItem>();
            if (includeBlank)
            {
                result.Add(new SelectListItem { Value = "0", Text = "Choose Roles..." });
            }

            var Roles = await _repo.GetRolesAsync();
            if (Roles != null)
            {
                foreach (var role in Roles)
                {
                    var item = new SelectListItem
                    {
                        Value = role.Id.ToString(),
                        Text = GetProperRoleName(role.Name)
                    };

                    if (role.Id == id)
                    {
                        item.Selected = true;
                    }

                    result.Add(item);
                }
            }
            return result;
        }

        #endregion

        public string GetProperRoleName(string roleName)
        {
            string name = "";
            if(roleName == "OM")
            {
                name = "Operations Manager";
            }
            else if(roleName == "TH")
            {
                name = "Technicial Head";
            }
            else if(roleName == "BD")
            {
                name = "Business Development";
            }
            else if(roleName == "AH")
            {
                name = "Admin Head";
            }
            else if(roleName == "TS")
            {
                name = "Technical Sepecialist";
            }
            return name;
        }
    }
}