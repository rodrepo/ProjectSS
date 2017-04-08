using System.Web.Mvc;
using System.Web.Routing;

namespace ProjectSS.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute("Account", "{page}", new  { controller = "Account", action = "Login" }, new { page = @"?ReturnUrl=%2F.*\.html"});

            //  #region Public
            //  routes.MapRoute(
            //     name: "Home",
            //     url: "Home",
            //     defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            //  );

            //  routes.MapRoute(
            //     name: "CRM",
            //     url: "CRM",
            //  defaults: new { controller = "CRM", action = "Index", id = UrlParameter.Optional }
            //  );
            //  #endregion

            //  #region User
            //  routes.MapRoute(
            //     name: "Users",
            //     url: "Users",
            //     defaults: new { controller = "User", action = "Index", id = UrlParameter.Optional }
            //  );
            //  #endregion

            routes.MapRoute(
              name: "Default",
              url: "{controller}/{action}/{id}",
              defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
          );
            routes.MapRoute(
             name: "Accounts",
             url: "Accounts",
             defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional }
          );
            routes.MapRoute(
               name: "BudgetRequest",
               url: "{controller}/{action}/{id}/{projectNo}",
               defaults: new { controller = "BudgetRequest", action = "Index", id = UrlParameter.Optional }
           );
            routes.MapRoute(
             name: "Projects",
             url: "{controller}/{action}",
             defaults: new { controller = "Project", action = "Index" }
         );

        }
    }
}
