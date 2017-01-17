using System.Web.Mvc;
using System.Web.Routing;

namespace ProjectSS.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            #region Account Controllers
            routes.MapRoute(
               name: "Accounts",
               url: "Login",
               defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional }
            );
            #endregion

            routes.MapRoute(
              name: "Default",
              url: "{controller}/{action}/{id}",
              defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
          );

        }
    }
}
