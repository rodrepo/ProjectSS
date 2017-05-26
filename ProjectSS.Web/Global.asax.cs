using ProjectSS.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ProjectSS.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start(Object sender, EventArgs e)
        {
            // get current context
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Session_Start(Object sender, EventArgs e)
        {
            HttpContext currentContext = HttpContext.Current;
            if (currentContext != null)
            {
                if (!currentContext.Request.Browser.Crawler)
                {
                    WebsiteVisitor currentVisitor = new WebsiteVisitor(currentContext);
                    OnlineVisitorsContainer.Visitors[currentVisitor.SessionId] = currentVisitor;
                }
            }
        }

        protected void Session_End(Object sender, EventArgs e)
        {
            // Code that runs when a session ends.
            // Note: The Session_End event is raised only when the sessionstate mode
            // is set to InProc in the Web.config file. If session mode is set to StateServer
            // or SQLServer, the event is not raised.

            if (this.Session != null)
            {
                WebsiteVisitor visitor;
                OnlineVisitorsContainer.Visitors.TryRemove(this.Session.SessionID, out visitor);
            }
        }

        protected void Application_PreRequestHandlerExecute(object sender, EventArgs eventArgs)
        {
            var session = HttpContext.Current.Session;
            if (session != null && HttpContext.Current.User != null && HttpContext.Current.User.Identity.IsAuthenticated)
            {
                if (OnlineVisitorsContainer.Visitors.ContainsKey(session.SessionID))
                {
                    OnlineVisitorsContainer.Visitors[session.SessionID].AuthUser = HttpContext.Current.User.Identity.Name;
                    OnlineVisitorsContainer.Visitors[session.SessionID].SessionStarted = DateTime.UtcNow;
                }
                else
                {
                    HttpContext currentContext = HttpContext.Current;
                    if (currentContext != null)
                    {
                        if (!currentContext.Request.Browser.Crawler)
                        {
                            WebsiteVisitor currentVisitor = new WebsiteVisitor(currentContext);
                            OnlineVisitorsContainer.Visitors[currentVisitor.SessionId] = currentVisitor;
                        }
                    }
                }
                var visitors = OnlineVisitorsContainer.Visitors.Values.Where(x => x.SessionStarted.AddMinutes(3) < DateTime.UtcNow).ToList();
                if(visitors.Any())
                {
                    foreach(var visitor in visitors)
                    {
                        WebsiteVisitor visitorx;
                        OnlineVisitorsContainer.Visitors.TryRemove(visitor.SessionId, out visitorx);
                    }
                }
            }
        }
    }
}
