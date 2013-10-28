using System;
using System.Collections.Generic;
using System.Linq;

namespace SaxxBoard.Widgets.RandomWidget
{
    public class RandomWidgetCollector : WidgetCollectorBase<WidgetCollectorBaseDataPoint>
    {
        public override IEnumerable<WidgetCollectorBaseDataPoint> Collect()
        {
            var random = new Random();
            var result = new List<WidgetCollectorBaseDataPoint>();

            for (var i = 0; i <= Widget.Configuration.Series.Count() - 1; i++)
                result.Add(new WidgetCollectorBaseDataPoint
                    {
                        Date = DateTime.Now,
                        Value = random.Next(0, 100),
                        WidgetIdentifier = Widget.InternalIdentifier,
                        SeriesIndex = i
                    });

            return result;
        }
    }
}