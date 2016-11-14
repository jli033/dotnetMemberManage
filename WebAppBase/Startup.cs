using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebAppBase.Startup))]
namespace WebAppBase
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
