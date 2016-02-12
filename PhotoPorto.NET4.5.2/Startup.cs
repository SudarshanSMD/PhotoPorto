using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PhotoPorto.Startup))]
namespace PhotoPorto
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
