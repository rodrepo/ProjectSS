using AutoMapper;
using Microsoft.ApplicationInsights;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using ProjectSS.Common;
using ProjectSS.Db.Contracts;
using ProjectSS.Db.Entities;
using ProjectSS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;

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

        //Remove all letters and return decimal
        public decimal ConvertStringToDecimal(string str)
        {
            decimal dec = 0;
            if (str.Any(char.IsDigit) && !str.IsEmpty())
            {
                str = Regex.Replace(str, @"[^0-9\.]+", string.Empty);
                dec = decimal.Parse(str);
            }
            return dec;
        }

        public string ConvertDecimalToPesos(decimal dec)
        {
            string str = "P 0.00";
            if(dec > 0)
            {
                str = string.Format("{0:P" + " " + "#,0.00}", dec);
            }
            return str;
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

        protected async Task<List<SelectListItem>> GetUsersAsync(string id = null)
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

                    if (user.Id == id)
                    {
                        item.Selected = true;
                    }
                    result.Add(item);
                }
            }
            return result;
        }

        protected async Task<List<SelectListItem>> GetInventoriesAsync()
        {
            var result = new List<SelectListItem>();
            var inventories = await _repo.GetInventoriesAsync();
            if (inventories != null)
            {
                foreach (var invtory in inventories)
                {
                    var item = new SelectListItem
                    {
                        Value = invtory.Id.ToString(),
                        Text = invtory.Name
                    };
                    result.Add(item);
                }
            }
            return result;
        }

        protected async Task<List<SelectListItem>> GetContractors( int projectId, int id = 0)
        {
            var result = new List<SelectListItem>();
            var project = await _repo.GetProjectByIdAsync(projectId);
            if (project != null)
            {
                var contractors = _mapper.Map<List<ProposalContractorModel>>(await _repo.GetProposalContractorsByProposalIdAsync(project.ProposalId));
                foreach (var con in contractors)
                {
                    var item = new SelectListItem
                    {
                        Value = con.Id.ToString(),
                        Text = con.Name
                    };

                    if (con.Id == id)
                    {
                        item.Selected = true;
                    }
                    result.Add(item);
                }
            }
            return result;
        }

        protected async Task<List<SelectListItem>> GetExpenses(int projectId, int id = 0)
        {
            var result = new List<SelectListItem>();
            var project = await _repo.GetProjectByIdAsync(projectId);
            if (project != null)
            {
                var expenses = _mapper.Map<List<ProposalExpenseModel>>(await _repo.GetProposalExpensesByProposalIdAsync(project.ProposalId));
                foreach (var exp in expenses)
                {
                    var item = new SelectListItem
                    {
                        Value = exp.Id.ToString(),
                        Text = exp.Item
                    };

                    if (exp.Id == id)
                    {
                        item.Selected = true;
                    }
                    result.Add(item);
                }
            }
            return result;
        }

        protected async Task<List<SelectListItem>> GetEquipments(int projectId, int id = 0)
        {
            var result = new List<SelectListItem>();
            var project = await _repo.GetProjectByIdAsync(projectId);
            if (project != null)
            {
                var equipments = _mapper.Map<List<ProposalEquipmentModel>>(await _repo.GetProposalEquipmentsByProposalIdAsync(project.ProposalId));
                foreach (var equipment in equipments)
                {
                    var inventory = await _repo.GetInventoryByIdAsync(equipment.InventoryId);
                    var item = new SelectListItem
                    {
                        Value = equipment.InventoryId.ToString(),
                        Text = inventory.Name
                };

                    if (equipment.Id == id)
                    {
                        item.Selected = true;
                    }
                    result.Add(item);
                }
            }
            return result;
        }

        protected async Task<List<SelectListItem>> GetLabarotories(int projectId, int id = 0)
        {
            var result = new List<SelectListItem>();
            var project = await _repo.GetProjectByIdAsync(projectId);
            if (project != null)
            {
                var labarotories = _mapper.Map<List<ProposalLaboratoryModel>>(await _repo.GetProposalLaboratoriesByProposalAsync(project.ProposalId));
                foreach (var lab in labarotories)
                {
                    var item = new SelectListItem
                    {
                        Value = lab.Id.ToString(),
                        Text = lab.Name
                    };

                    if (lab.Id == id)
                    {
                        item.Selected = true;
                    }
                    result.Add(item);
                }
            }
            return result;
        }

        protected async Task<List<SelectListItem>> GetCommissions(int projectId, int id = 0)
        {
            var result = new List<SelectListItem>();
            var project = await _repo.GetProjectByIdAsync(projectId);
            if (project != null)
            {
                var commissions = _mapper.Map<List<ProposalCommissionModel>>(await _repo.GetProposalCommissionsByProposalIdAsync(project.ProposalId));
                foreach (var com in commissions)
                {
                    var item = new SelectListItem
                    {
                        Value = com.Id.ToString(),
                        Text = com.Name
                    };

                    if (com.Id == id)
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