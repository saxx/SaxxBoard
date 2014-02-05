using System;
using System.Collections.Generic;
using System.Linq;
using SaxxBoard.Models;

namespace SaxxBoard.Widgets
{
    public abstract class WidgetCollectorBase<TDataPoint> : IWidgetCollector where TDataPoint : DataPoint
    {
        public abstract IEnumerable<TDataPoint> Collect();

        public IEnumerable<DataPoint> Collect(Db dbSession)
        {
            var newDataPoints = Collect().ToList();

            DeleteOldDataPoints(dbSession);

            foreach (var d in newDataPoints)
            {
                d.DateTime = d.DateTime.ToUniversalTime();
                dbSession.DataPoints.Add(d);
            }
            dbSession.SaveChanges();

            return newDataPoints;
        }

        private void DeleteOldDataPoints(Db dbSession)
        {
            var availableSeriesIndexes = (from x in dbSession.DataPoints
                                          where x.WidgetIdentifier == Widget.InternalIdentifier
                                          select x.SeriesIndex).Distinct().ToList();

            foreach (var seriesIndex in availableSeriesIndexes)
            {
                var seriesIndexClosure = seriesIndex;
                var query =
                    dbSession.DataPoints
                        .Where(
                            x => x.WidgetIdentifier == Widget.InternalIdentifier && x.SeriesIndex == seriesIndexClosure)
                        .OrderBy(x => x.DateTime);

                var count = query.Count();
                var difference = count - Widget.Configuration.MaxDataPointsInChart;

                //delete at most 10 datapoints at once
                if (difference > 10)
                    difference = 10;

                if (difference > 0)
                {
                    foreach (var dataPointToDelete in query.Take(difference).ToList())
                        dbSession.DataPoints.Remove(dataPointToDelete);
                    dbSession.SaveChanges();
                }
            }

            var dateForReallyOldDataPoints = DateTime.Now.AddDays(-10);
            var reallyOldDataPoints = (from x in dbSession.DataPoints
                                       where x.DateTime <= dateForReallyOldDataPoints
                                       select x);
            foreach (var dataPoint in reallyOldDataPoints.ToList())
                dbSession.DataPoints.Remove(dataPoint);
            dbSession.SaveChanges();
        }

        public IWidget Widget { get; set; }
    }
}