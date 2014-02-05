using System;
using System.Collections.Generic;
using System.Linq;
using SaxxBoard.Models;

namespace SaxxBoard.Widgets.RandomWidget
{
    public class RandomWidgetCollector : WidgetCollectorBase<DataPoint>
    {
        public override IEnumerable<DataPoint> Collect()
        {
            var random = new Random();
            var result = new List<DataPoint>();

            for (var i = 0; i <= Widget.Configuration.Series.Count() - 1; i++)
                result.Add(new DataPoint
                    {
                        DateTime = DateTime.Now,
                        Value = random.Next(0, 100),
                        WidgetIdentifier = Widget.InternalIdentifier,
                        SeriesIndex = i
                    });

            return result;
        }
    }
}