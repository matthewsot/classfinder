using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Classfinder.Startup))]
namespace Classfinder
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
