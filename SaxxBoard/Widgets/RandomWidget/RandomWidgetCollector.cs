using System;

namespace SaxxBoard.Widgets.RandomWidget
{
    public class RandomWidgetCollector : SimpleCollector<SimpleCollectorDataPoint>
    {
        public override SimpleCollectorDataPoint Collect()
        {
            return new SimpleCollectorDataPoint
                {
                    Date = DateTime.Now,
                    Value = new Random().Next(0, 100),
                    WidgetIdentifier = Widget.InternalIdentifier
                };
        }
    }
}