using System;
using System.Threading;
using Elmah;
using Ninject;

[assembly: WebActivator.PostApplicationStartMethod(typeof(SaxxBoard.App_Start.CollectorThread), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(SaxxBoard.App_Start.CollectorThread), "Stop")]

namespace SaxxBoard.App_Start
{
    public static class CollectorThread
    {
        private static Thread _thread;
        private static bool _isStopping;

        public static void Start()
        {
            var kernel = NinjectWebCommon.Bootstrapper.Kernel;
            var collector = kernel.Get<Collector>();

            _thread = new Thread(x =>
            {
                try
                {
                    var count = 0;
                    while (!_isStopping)
                    {
                        if (count >= 10)
                        {
                            try
                            {
                                collector.Collect();
                            }
                            catch (Exception ex)
                            {
                                ErrorLog.GetDefault(null).Log(new Error(new System.ApplicationException("Exception while running collector.Collect().", ex)));
                            }
                            count = 0;
                        }

                        count++;
                        Thread.Sleep(100);
                    }
                }
                catch (Exception ex)
                {
                    ErrorLog.GetDefault(null).Log(new Error(new System.ApplicationException("Exception while running collector thread.", ex)));
                }
            });
            _thread.Start();

        }

        public static void Stop()
        {
            _isStopping = true;
        }
    }
}
