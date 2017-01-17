using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ProjectSS.Web.Startup))]
namespace ProjectSS.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
