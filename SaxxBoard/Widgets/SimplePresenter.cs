using Raven.Client;
using Raven.Client.Linq;
using System.Collections.Generic;
using System.Linq;

namespace SaxxBoard.Widgets
{
    public class SimplePresenter : IPresenter
    {
        public IEnumerable<IPresenterSeries> GetData(IDocumentSession dbSession)
        {
            var config = Widget.GetConfiguration();

            var availableSeriesIndexes = (from x in dbSession.Query<SimpleCollectorDataPoint>()
                                          where x.WidgetIdentifier == Widget.InternalIdentifier
                                          select x.SeriesIndex).Distinct().ToList();

            var result = new List<SimplePresenterSeries>();
            foreach (var seriesIndex in availableSeriesIndexes)
            {
                var seriesIndexClosure = seriesIndex;
                var dataPoints = (from x in dbSession.Query<SimpleCollectorDataPoint>()
                                  where x.WidgetIdentifier == Widget.InternalIdentifier && x.SeriesIndex == seriesIndexClosure
                                  orderby x.Date descending
                                  select x).Take(Widget.GetConfiguration().MaxDataPointsInChart).ToList();

                var serie = new SimplePresenterSeries
                    {
                        Label = config.SeriesLabels.Count() > seriesIndex ? config.SeriesLabels.ElementAt(seriesIndex) : "",
                        DataPoints = from y in dataPoints
                                     select new SimplePresenterDataPoint
                                         {
                                             Date = y.Date,
                                             RawValue = CalculateValue(y.Value)
                                         }
                    };

                result.Add(serie);
            }

            return result;
        }

        public virtual string FormatValue(double? rawValue)
        {
            return rawValue.HasValue ? rawValue.Value.ToString("N0") : null;
        }

        protected virtual double? CalculateValue(double? rawValue)
        {
            return rawValue;
        }

        public IWidget Widget { get; set; }
    }
}