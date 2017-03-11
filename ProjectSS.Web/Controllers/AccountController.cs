using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using AutoMapper;
using ProjectSS.Db.Contracts;
using ProjectSS.Web.Models;
using ProjectSS.Db.Entities;
using Microsoft.ApplicationInsights;
using ProjectSS.Web.Models.admin;
using System.Web.WebPages;

namespace ProjectSS.Web.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        private ApplicationSignInManager _signInManager;
        private UserManager _userManager;

        public AccountController(IMapper mapper, IDataRepo repo, TelemetryClient telemetryClient)
            : base(repo, mapper, telemetryClient)
        {
            _mapper = mapper;
            _repo = repo;
        }

        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            if (user != null)
            {
                if (user.IsDeleted == true)
                {
                    Session.Abandon();
                    AuthenticationManager.SignOut();
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
                }
                if (user.IsActive == false)
                {
                    Session.Abandon();
                    AuthenticationManager.SignOut();
                    ModelState.AddModelError("", "Account is not yet activated.");
                    return View(model);
                }
            }
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToAction("Index", "Home");
                //return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    if (user != null)
                    {
                        if (user.IsActive == false)
                        {
                            Session.Abandon();
                            AuthenticationManager.SignOut();
                            ModelState.AddModelError("", "Account is not yet activated.");
                            return View(model);
                        } 
                    }
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        [HttpGet]
        public async Task<ActionResult> ChangePassword(string userId)
        {
            var model = new ChangePasswordViewModel();
            if(!userId.IsEmpty())
            {
                model.UserId = userId;
                model.OldPassword = "Xxxxxx0xx";
            }
            model.UserDisplayName = await _repo.GetUserNameByIdAsync(userId);
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            bool admin = false;
            if (!ModelState.IsValid)
            {
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        TempData["Error"] = error.ErrorMessage;
                    }
                }
                return View(model);
            }

            string userId = "";
            var result = new IdentityResult();
            if(!model.UserId.IsEmpty())
            {
                userId = model.UserId;
                result = UserManager.RemovePassword(userId);
                if(result.Succeeded)
                {
                    result = UserManager.AddPassword(userId, model.ConfirmPassword);
                }
                admin = true;
            }
            else
            {
                userId = User.Identity.GetUserId();
                result = await UserManager.ChangePasswordAsync(userId, model.OldPassword, model.NewPassword);
            }
            if (result.Succeeded)
            {
                if(admin == false)
                {
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                    if (user != null)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }
                    TempData["Success"] = string.Format("Password successfully changed please log in using your new password");
                    return RedirectToAction("Login");
                }
                else
                {
                    TempData["Success"] = string.Format("Password successfully Updated");
                    return RedirectToAction("Edit", "User", new {@id = userId });
                }
            }
            AddErrors(result);
            return View(model);
        }
    }
}