using System;
using Raven.Client;
using System.Collections.Generic;
using System.Linq;

namespace SaxxBoard.Widgets
{
    public abstract class WidgetCollectorBase<TDataPoint> : IWidgetCollector where TDataPoint : WidgetCollectorBaseDataPoint
    {
        public abstract IEnumerable<TDataPoint> Collect();

        public IEnumerable<IWidgetCollectorDataPoint> Collect(IDocumentSession dbSession)
        {
            var newDataPoints = Collect().ToList();

            DeleteOldDataPoints(dbSession);

            foreach (var d in newDataPoints)
            {
                d.Date = d.Date.ToUniversalTime();
                dbSession.Store(d);
            }
            dbSession.SaveChanges();

            return newDataPoints;
        }

        private void DeleteOldDataPoints(IDocumentSession dbSession)
        {
            var availableSeriesIndexes = (from x in dbSession.Query<WidgetCollectorBaseDataPoint>()
                where x.WidgetIdentifier == Widget.InternalIdentifier
                select x.SeriesIndex).Distinct().ToList();

            foreach (var seriesIndex in availableSeriesIndexes)
            {
                var seriesIndexClosure = seriesIndex;
                var query =
                    dbSession.Query<TDataPoint>()
                        .Where(
                            x => x.WidgetIdentifier == Widget.InternalIdentifier && x.SeriesIndex == seriesIndexClosure)
                        .OrderBy(x => x.Date);

                var count = query.Count();
                var difference = count - Widget.Configuration.MaxDataPointsToStore;

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

            var reallyOldDataPoints = (from x in dbSession.Query<WidgetCollectorBaseDataPoint>()
                where x.Date <= DateTime.Now.AddDays(-10)
                select x);
            foreach (var dataPoint in reallyOldDataPoints)
                dbSession.Delete(dataPoint);
            dbSession.SaveChanges();
        }

        public IWidget Widget { get; set; }
    }

    public class WidgetCollectorBaseDataPoint : IWidgetCollectorDataPoint
    {
        public DateTime Date { get; set; }
        public double? Value { get; set; }
        public string WidgetIdentifier { get; set; }
        public int SeriesIndex { get; set; }
    }
}