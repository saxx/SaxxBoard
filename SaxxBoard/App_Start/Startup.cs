using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SaxxBoard.App_Start.Startup))]
namespace SaxxBoard.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}
