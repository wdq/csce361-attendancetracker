using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AttendanceTracker.Startup))]
namespace AttendanceTracker
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
