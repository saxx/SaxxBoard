using System;

namespace SaxxBoard.Widgets
{
    public class SimpleCollectorDataPoint : ICollectorDataPoint
    {
        public DateTime Date { get; set; }
        public double? Value { get; set; }
        public string WidgetIdentifier { get; set; }
    }
    
    public class SimplePresenterDataPoint : IPresenterDataPoint
    {
        public DateTime Date { get; set; }
        public double? RawValue { get; set; }
        public string FormattedValue { get; set; }
    }
}