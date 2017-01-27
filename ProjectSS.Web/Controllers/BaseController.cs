﻿using AutoMapper;
using Microsoft.ApplicationInsights;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using ProjectSS.Common;
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
        protected async Task<List<SelectListItem>> GetRolesAsync(string id = null)
        {
            var result = new List<SelectListItem>();

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

        protected async Task<List<SelectListItem>> GetCRMsAsync(int id = 0)
        {
            var result = new List<SelectListItem>();
            var crms = await _repo.GetCRM();
            if (crms != null)
            {
                foreach (var crm in crms)
                {
                    var item = new SelectListItem
                    {
                        Value = crm.Id.ToString(),
                        Text = crm.Reference + " " + crm.CompanyName
                    };

                    if (crm.Id == id)
                    {
                        item.Selected = true;
                    }

                    result.Add(item);
                }
            }
            return result;
        }

        protected async Task<List<SelectListItem>> GetBDUsersAsync(string id = null)
        {
            var result = new List<SelectListItem>();
            var users = await _repo.GetUsersAsync();
            if (users != null)
            {
                foreach (var user in users)
                {
                    if (user.Roles.Count > 0)
                    {
                        foreach (var role in user.Roles)
                        {
                            var userRole = _repo.GetRoleById(role.RoleId);
                            if (userRole.Name == RoleType.BD.ToString())
                            {
                                var item = new SelectListItem
                                {
                                    Value = user.Id.ToString(),
                                    Text = user.FirstName + " " + user.MiddleName + " " + user.LastName
                                };

                                if (user.Id == id)
                                {
                                    item.Selected = true;
                                }
                                result.Add(item);
                            }
                        }
                    }
                }
            }
            return result;
        }

        protected async Task<List<SelectListItem>> GetTSUsersAsync(string id = null)
        {
            var result = new List<SelectListItem>();
            var users = await _repo.GetUsersAsync();
            if (users != null)
            {
                foreach (var user in users)
                {
                    if (user.Roles.Count > 0)
                    {
                        foreach (var role in user.Roles)
                        {
                            var userRole = _repo.GetRoleById(role.RoleId);
                            if (userRole.Name == RoleType.TS.ToString())
                            {
                                var item = new SelectListItem
                                {
                                    Value = user.Id.ToString(),
                                    Text = user.FirstName + " " + user.MiddleName + " " + user.LastName
                                };

                                if (user.Id == id)
                                {
                                    item.Selected = true;
                                }
                                result.Add(item);
                            }
                        }
                    }
                }
            }
            return result;
        }

        protected async Task<List<SelectListItem>> GetTHUsersAsync(string id = null)
        {
            var result = new List<SelectListItem>();
            var users = await _repo.GetUsersAsync();
            if (users != null)
            {
                foreach (var user in users)
                {
                    if (user.Roles.Count > 0)
                    {
                        foreach (var role in user.Roles)
                        {
                            var userRole = _repo.GetRoleById(role.RoleId);
                            if (userRole.Name == RoleType.TH.ToString())
                            {
                                var item = new SelectListItem
                                {
                                    Value = user.Id.ToString(),
                                    Text = user.FirstName + " " + user.MiddleName + " " + user.LastName
                                };

                                if (user.Id == id)
                                {
                                    item.Selected = true;
                                }
                                result.Add(item);
                            }
                        }
                    }
                }
            }
            return result;
        }

        protected async Task<List<SelectListItem>> GetUsersAsync()
        {
            var result = new List<SelectListItem>();
            var users = await _repo.GetUsersAsync();
            if (users != null)
            {
                foreach (var user in users)
                {
                    var item = new SelectListItem
                    {
                        Value = user.Id.ToString(),
                        Text = user.FirstName + " " + user.MiddleName + " " + user.LastName
                    };
                    result.Add(item);
                }
            }
            return result;
        }
        #endregion

        public string GetProperRoleName(string roleName)
        {
            string name = "";
            if (roleName == "OM")
            {
                name = "Operations Manager";
            }
            else if (roleName == "TH")
            {
                name = "Technicial Head";
            }
            else if (roleName == "BD")
            {
                name = "Business Development";
            }
            else if (roleName == "AH")
            {
                name = "Admin Head";
            }
            else if (roleName == "TS")
            {
                name = "Technical Sepecialist";
            }
            return name;
        }
    }
}