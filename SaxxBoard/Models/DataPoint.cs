using System;

namespace SaxxBoard.Models
{
    public class DataPoint
    {
        public int Id { get; set; }
        public string WidgetIdentifier { get; set; }
        public int SeriesIndex { get; set; }
        public DateTime DateTime { get; set; }
        public double? Value { get; set; }
    }
}