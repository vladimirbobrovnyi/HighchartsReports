using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DexReportsSolution.Startup))]
namespace DexReportsSolution
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
