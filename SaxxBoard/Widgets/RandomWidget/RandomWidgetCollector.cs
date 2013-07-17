using System;
using System.Collections.Generic;
using System.Linq;

namespace SaxxBoard.Widgets.RandomWidget
{
    public class RandomWidgetCollector : SimpleCollector<SimpleCollectorDataPoint>
    {
        public override IEnumerable<SimpleCollectorDataPoint> Collect()
        {
            var random = new Random();

            var config = Widget.GetConfiguration();
            var result = new List<SimpleCollectorDataPoint>();

            for (var i = 0; i <= config.SeriesLabels.Count() - 1; i++)
                result.Add(new SimpleCollectorDataPoint
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