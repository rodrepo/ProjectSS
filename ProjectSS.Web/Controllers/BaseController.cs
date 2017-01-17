using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using ProjectSS.Db.Contracts;
using System;
using System.Web;
using System.Web.Mvc;

namespace ProjectSS.Web.Controllers
{
    public class BaseController : Controller
    {
        protected IMapper _mapper;
        protected IDataRepo _repo;

        public Func<string> GetUserId;
        private UserManager _userManager;
        private ApplicationSignInManager _signInManager;

        public BaseController(IDataRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
            GetUserId = () => User.Identity.GetUserId();
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

    }
}