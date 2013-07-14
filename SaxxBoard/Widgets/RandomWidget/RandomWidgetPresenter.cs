using Raven.Client;
using Raven.Client.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SaxxBoard.Widgets.RandomWidget
{
    public class RandomWidgetPresenter : IPresenter
    {
        public IEnumerable<IPresenterDataPoint> GetData(IDocumentSession dbSession, DateTime startDate, DateTime endDate)
        {
            var query = dbSession.Query<SimpleDataPoint>().Where(x => x.WidgetIdentifier == Widget.Identifier && x.Date >= startDate && x.Date <= endDate).OrderByDescending(x => x.Date).Take(100);
            return from x in query.OrderBy(x => x.Date)
                   orderby x.Date
                   select x;
        }

        public IWidget Widget { get; set; }
    }
}