using Raven.Client;
using System;
using System.Collections.Generic;

namespace SaxxBoard.Widgets.RandomWidget
{
    public class RandomWidgetCollector : ICollector
    {
        public event CollectedDelegate OnCollected;

        public IWidget Widget { get; set; }

        public IEnumerable<ICollectorDataPoint> Collect(IDocumentSession dbSession)
        {
            var newDataPoints = new[]
                {
                    new SimpleDataPoint
                        {
                            Date = DateTime.Now,
                            Value = new Random().Next(1, 100),
                            WidgetIdentifier = Widget.Identifier
                        }
                };

            foreach (var d in newDataPoints)
                dbSession.Store(d);
            dbSession.SaveChanges();

            if (OnCollected != null)
                OnCollected();

            return newDataPoints;
        }
    }
}