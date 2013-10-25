using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SaxxBoard.Startup))]
namespace SaxxBoard
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}
