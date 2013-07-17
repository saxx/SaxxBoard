using System;
using System.Collections.Generic;

namespace SaxxBoard.Widgets
{
    public class SimpleCollectorDataPoint : ICollectorDataPoint
    {
        public DateTime Date { get; set; }
        public double? Value { get; set; }
        public string WidgetIdentifier { get; set; }
        public int SeriesIndex { get; set; }
    }
    
    public class SimplePresenterDataPoint : IPresenterDataPoint
    {
        public DateTime Date { get; set; }
        public double? RawValue { get; set; }
    }

    public class SimplePresenterSeries : IPresenterSeries
    {
        public string Label { get; set; }
        public IEnumerable<IPresenterDataPoint> DataPoints { get; set; }
    }
}