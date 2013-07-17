﻿using Raven.Client;
using System.Collections.Generic;
using System.Linq;

namespace SaxxBoard.Widgets
{
    public abstract class SimpleCollector<TDataPoint> : ICollector where TDataPoint : SimpleCollectorDataPoint
    {
        public abstract IEnumerable<TDataPoint> Collect();

        public IEnumerable<ICollectorDataPoint> Collect(IDocumentSession dbSession)
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
            var query = dbSession.Query<TDataPoint>().Where(x => x.WidgetIdentifier == Widget.InternalIdentifier).OrderBy(x => x.Date);

            var count = query.Count();
            var difference = count - Widget.GetConfiguration().MaxDataPointsToStore;

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

        public IWidget Widget { get; set; }
    }
}