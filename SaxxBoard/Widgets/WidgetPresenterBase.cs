using System;
using Raven.Client;
using Raven.Client.Linq;
using System.Collections.Generic;
using System.Linq;

namespace SaxxBoard.Widgets
{
    public class WidgetPresenterBase : IWidgetPresenter
    {
        public IEnumerable<IWidgetPresenterSeries> GetData(IDocumentSession dbSession)
        {
            var availableSeriesIndexes = (from x in dbSession.Query<WidgetCollectorBaseDataPoint>()
                                          where x.WidgetIdentifier == Widget.InternalIdentifier
                                          select x.SeriesIndex).Distinct().ToList();

            var result = new List<WidgetPresenterBaseSeries>();
            foreach (var seriesIndex in availableSeriesIndexes)
            {
                var seriesIndexClosure = seriesIndex;
                var dataPoints = (from x in dbSession.Query<WidgetCollectorBaseDataPoint>()
                                  where x.WidgetIdentifier == Widget.InternalIdentifier && x.SeriesIndex == seriesIndexClosure
                                  orderby x.Date descending
                                  select x).Take(Widget.Configuration.MaxDataPointsInChart).ToList();

                var serie = new WidgetPresenterBaseSeries
                    {
                        Label = Widget.Configuration.Series.Count() > seriesIndex ? Widget.Configuration.Series.ElementAt(seriesIndex).Label : "",
                        DataPoints = from y in dataPoints
                                     select new WidgetPresenterBaseDataPoint
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

    public class WidgetPresenterBaseDataPoint : IWidgetPresenterDataPoint
    {
        public DateTime Date { get; set; }
        public double? RawValue { get; set; }
    }

    public class WidgetPresenterBaseSeries : IWidgetPresenterSeries
    {
        public string Label { get; set; }
        public IEnumerable<IWidgetPresenterDataPoint> DataPoints { get; set; }
    }
}