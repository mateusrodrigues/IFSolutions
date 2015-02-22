using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(IFSolutions.Startup))]
namespace IFSolutions
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
