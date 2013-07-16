using Raven.Client;
using Raven.Client.Linq;
using System.Collections.Generic;
using System.Linq;

namespace SaxxBoard.Widgets
{
    public class SimplePresenter : IPresenter
    {
        public IEnumerable<IPresenterDataPoint> GetData(IDocumentSession dbSession)
        {
            var config = Widget.GetConfiguration();

            var query = dbSession.Query<SimpleCollectorDataPoint>().Where(x => x.WidgetIdentifier == Widget.InternalIdentifier).OrderByDescending(x => x.Date).Take(Widget.GetConfiguration().MaxDataPointsInChart).ToList();
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