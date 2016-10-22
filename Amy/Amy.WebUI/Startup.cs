using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Amy.WebUI.Startup))]
namespace Amy.WebUI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
