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

            var query = (from x in dbSession.Query<SimpleCollectorDataPoint>()
                         where x.WidgetIdentifier == Widget.InternalIdentifier
                         orderby x.Date descending
                         select x).ToList();
            var groupedQuery = (from x in query
                                group x by x.SeriesIndex
                                    into g
                                    select new
                                        {
                                            SeriesIndex = g.Key,
                                            DataPoints = g.OrderBy(x => x.Date).Take(Widget.GetConfiguration().MaxDataPointsInChart).ToList()
                                        });
            var result = new List<SimplePresenterSeries>();
            foreach (var x in groupedQuery)
            {
                var serie = new SimplePresenterSeries
                    {
                        Label = config.SeriesLabels.Count() > x.SeriesIndex ? config.SeriesLabels.ElementAt(x.SeriesIndex) : "",
                        DataPoints = from y in x.DataPoints
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