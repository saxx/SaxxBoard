using Raven.Client;
using System.Collections.Generic;
using System.Linq;

namespace SaxxBoard.Widgets
{
    public abstract class SimpleCollector<TDataPoint> : ICollector where TDataPoint : SimpleDataPoint
    {
        public abstract TDataPoint Collect();

        public IEnumerable<ICollectorDataPoint> Collect(IDocumentSession dbSession)
        {
            var newDataPoint = Collect();
            var newDataPoints = new[]
                {
                    newDataPoint
                };

            DeleteOldDataPoints(dbSession);

            foreach (var d in newDataPoints)
            {
                d.Date = d.Date.ToUniversalTime();
                dbSession.Store(d);
            }
            dbSession.SaveChanges();

            if (OnCollected != null)
                OnCollected();
            return newDataPoints;
        }

        private void DeleteOldDataPoints(IDocumentSession dbSession)
        {
            var query = dbSession.Query<TDataPoint>().Where(x => x.WidgetIdentifier == Widget.InternalIdentifier).OrderBy(x => x.Date);

            var count = query.Count();
            var difference = count - Widget.NumberOfDataPoints;

            //delete at most 10 datapoints at once
            if (difference > 10)
                difference = 10;

            if (difference > 0)
            {
                foreach (var dataPointToDelete in query.Take(difference))
                    dbSession.Delete(dataPointToDelete);
                dbSession.SaveChanges();
            }
        }

        public event CollectedDelegate OnCollected;
        public IWidget Widget { get; set; }
    }
}