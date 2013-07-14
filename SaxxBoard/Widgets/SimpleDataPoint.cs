using System;

namespace SaxxBoard.Widgets
{
    public class SimpleDataPoint : IPresenterDataPoint, ICollectorDataPoint
    {
        public DateTime Date { get; set; }
        public int Value { get; set; }
        public string WidgetIdentifier { get; set; }
    }
}