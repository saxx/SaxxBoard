using System;

namespace SaxxBoard.Widgets
{
    public interface IPresenterDataPoint
    {
        DateTime Date { get; set; }
        double? RawValue { get; }
        string FormattedValue { get; }
    }
}