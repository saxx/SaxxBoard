using System;

namespace SaxxBoard.Widgets.RandomWidget
{
    public class RandomWidgetCollector : SimpleCollector<SimpleDataPoint>
    {
        public override SimpleDataPoint Collect()
        {
            return new SimpleDataPoint
                {
                    Date = DateTime.Now,
                    Value = new Random().Next(1, 100),
                    WidgetIdentifier = Widget.InternalIdentifier
                };
        }
    }
}