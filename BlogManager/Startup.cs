using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BlogManager.Startup))]
namespace BlogManager
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
