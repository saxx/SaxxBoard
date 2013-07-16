using Raven.Client;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SaxxBoard.Widgets.NewRelicWidget
{
    public class NewRelicPresenter : IPresenter
    {
        public IEnumerable<IPresenterDataPoint> GetData(IDocumentSession dbSession)
        {
            var query = (from x in dbSession.Query<SimpleCollectorDataPoint>()
                         where x.WidgetIdentifier == Widget.InternalIdentifier
                         orderby x.Date descending
                         select x).Take(Widget.GetConfiguration().MaxDataPointsInChart).ToList();

            var config = (NewRelicConfiguration)Widget.GetConfiguration();
            var valueIsBytes =
                config.Metrics.Any(x => x.EndsWith("bytes", StringComparison.InvariantCultureIgnoreCase) || x.EndsWith("bytes/sec", StringComparison.InvariantCultureIgnoreCase));

            if (valueIsBytes)
            {
                //if the value is bytes, calculate the MB value, because bytes are not very readable
                var result = (from x in query.OrderBy(x => x.Date)
                              orderby x.Date
                              select new SimplePresenterDataPoint
                                  {
                                      Date = x.Date,
                                      RawValue = x.Value.HasValue ? new double?(Math.Round(x.Value.Value / 1024.0 / 1024.0, 2)) : null
                                  }).ToList();

                foreach (var dataPoint in result)
                    if (dataPoint.RawValue.HasValue)
                        dataPoint.FormattedValue = dataPoint.RawValue.Value.ToString("N2") + " MB";

                return result;
            }

            return from x in query.OrderBy(x => x.Date)
                   orderby x.Date
                   select new SimplePresenterDataPoint
                   {
                       Date = x.Date,
                       RawValue = x.Value,
                       FormattedValue = x.Value.HasValue ? (x.Value.Value.ToString("N0") + (config.IsScaledToPercents ? " %" : "")) : null
                   };
        }

        public IWidget Widget { get; set; }
    }
}