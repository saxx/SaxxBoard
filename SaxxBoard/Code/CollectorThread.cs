﻿using System;
using System.Threading;

namespace SaxxBoard
{
    public class CollectorThread : IDisposable
    {
        public Thread Thread { get; set; }
        private bool _isStopping;

        public void Start(Collector collector)
        {
            Thread = new Thread(x =>
                {
                    var count = 0;
                    while (!_isStopping)
                    {
                        if (count >= 10)
                        {
                            collector.Collect();
                            count = 0;
                        }

                        count++;
                        Thread.Sleep(100);
                    }
                });
            Thread.Start();
        }

        public void Stop()
        {
            _isStopping = true;
        }

        public void Dispose()
        {
            Stop();
        }
    }
}