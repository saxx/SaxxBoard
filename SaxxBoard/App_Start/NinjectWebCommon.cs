using Microsoft.AspNet.SignalR.Infrastructure;
using Microsoft.AspNet.SignalR.SystemWeb.Infrastructure;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;
using System.Reflection;
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
            Boot(kernel);

            return kernel;
        }

        private static void RegisterServices(IKernel kernel)
        {
            //this is required to make SignalR work on Azure
            kernel.Bind<IProtectedData>().To<MachineKeyProtectedData>();

            kernel.Bind<IDocumentStore>().ToMethod(x =>
                {
                    var store = new DocumentStore {ConnectionStringName = "RavenDB"};
                    store.Initialize();
                    IndexCreation.CreateIndexes(Assembly.GetCallingAssembly(), store);
                    return store;
                }).InSingletonScope();

            kernel.Bind<Collector>().ToMethod(x =>
                {
                    var collector = new Collector(kernel.Get<IDocumentStore>(), kernel.Get<WidgetCollection>());
                    return collector;
                });

            kernel.Bind<CollectorThread>().ToSelf().InSingletonScope();
            kernel.Bind<WidgetCollection>().ToSelf().InSingletonScope();
        }

        private static void Boot(IKernel kernel)
        {
            var collectorThread = kernel.Get<CollectorThread>();
            var collector = kernel.Get<Collector>();

            collectorThread.Start(collector);
        }
    }
}
