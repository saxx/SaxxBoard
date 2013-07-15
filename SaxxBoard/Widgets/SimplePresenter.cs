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
            var query = dbSession.Query<SimpleDataPoint>().Where(x => x.WidgetIdentifier == Widget.InternalIdentifier).OrderByDescending(x => x.Date).Take(Widget.NumberOfDataPoints);
            return from x in query.OrderBy(x => x.Date)
                   orderby x.Date
                   select x;
        }

        public IWidget Widget { get; set; }
    }
}