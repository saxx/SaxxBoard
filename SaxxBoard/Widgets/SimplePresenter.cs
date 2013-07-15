using Raven.Client;
using Raven.Client.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SaxxBoard.Widgets
{
    public class SimplePresenter : IPresenter
    {
        public IEnumerable<IPresenterDataPoint> GetData(IDocumentSession dbSession)
        {
            var query = dbSession.Query<SimpleDataPoint>().Where(x => x.WidgetIdentifier == Widget.InternalIdentifier).OrderByDescending(x => x.Date).Take(Widget.NumberOfDataPoints).ToList();
            return from x in query.OrderBy(x => x.Date)
                   orderby x.Date
                   select new SimpleDataPoint
                       {
                           Value = x.Value,
                           WidgetIdentifier = x.WidgetIdentifier,
                           Date = TimeZoneInfo.ConvertTimeFromUtc(x.Date, TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time"))
                       };
        }

        public IWidget Widget { get; set; }
    }
}