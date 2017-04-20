using System.Web.Mvc;
using System.Web.Routing;

namespace ProjectSS.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            
            routes.MapRoute(
            name: "Accounts",
            url: "Accounts",
            defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional });

            routes.MapRoute(
            name: "BudgetRequest",
            url: "Budget-Request/{projectId}/{projectNo}/{action}/{id}",
            defaults: new { controller = "BudgetRequest", action = "Index", id = UrlParameter.Optional }); 

            routes.MapRoute(
            name: "Default",
            url: "{controller}/{action}/{id}",
            defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional });
        }
    }
}
