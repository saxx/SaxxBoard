using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using MailSendbox;
using Microsoft.AspNet.SignalR;
using Ninject;
using Ninject.Web.Mvc;
using SaxxBoard.App_Start;

namespace SaxxBoard
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RouteTable.Routes.MapHubs();
            GlobalHost.DependencyResolver = new SignalRNinjectDependencyResolver(NinjectWebCommon.Bootstrapper.Kernel);

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }


        public class SignalRNinjectDependencyResolver : DefaultDependencyResolver
        {
            private readonly IKernel _kernel;

            public SignalRNinjectDependencyResolver(IKernel kernel)
            {
                _kernel = kernel;
            }

            public override object GetService(Type serviceType)
            {
                return _kernel.TryGet(serviceType) ?? base.GetService(serviceType);
            }

            public override IEnumerable<object> GetServices(Type serviceType)
            {
                return _kernel.GetAll(serviceType).Concat(base.GetServices(serviceType));
            }
        }
    }
}