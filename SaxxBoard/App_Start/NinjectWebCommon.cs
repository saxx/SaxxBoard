using Microsoft.AspNet.SignalR;
using SaxxBoard.Models;
using System.Collections.Generic;
using System.Linq;

[assembly: WebActivator.PreApplicationStartMethod(typeof(SaxxBoard.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(SaxxBoard.App_Start.NinjectWebCommon), "Stop")]

namespace SaxxBoard.App_Start
{
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Ninject;
    using Ninject.Web.Common;
    using System;
    using System.Web;

    public static class NinjectWebCommon
    {
        public static readonly Bootstrapper Bootstrapper = new Bootstrapper();

        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            Bootstrapper.Initialize(CreateKernel);
        }

        public static void Stop()
        {
            Bootstrapper.ShutDown();
        }

        public static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

            RegisterServices(kernel);

            return kernel;
        }

        private static void RegisterServices(IKernel kernel)
        {
            //this is required to make SignalR work on Azure
            //kernel.Bind<IProtectedData>().To<MachineKeyProtectedData>();

            kernel.Bind<Db>().ToMethod(x => new Db());

            kernel.Bind<Collector>().ToMethod(x =>
                {
                    var collector = new Collector(kernel.Get<Db>(), kernel.Get<WidgetCollection>());
                    return collector;
                });

            kernel.Bind<WidgetCollection>().ToSelf().InSingletonScope();
        }
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
