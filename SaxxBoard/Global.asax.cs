using Microsoft.AspNet.SignalR;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using SaxxBoard.App_Start;

namespace SaxxBoard
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            // SignalR needs a special kernel to work properly on Azure Websites
            var kernel = NinjectWebCommon.Bootstrapper.Kernel;
            GlobalHost.DependencyResolver = new SignalRNinjectDependencyResolver(kernel);

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}