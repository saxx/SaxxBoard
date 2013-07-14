using System;

namespace SaxxBoard.Widgets
{
    public interface IPresenterDataPoint
    {
        DateTime Date { get; set; }
        int Value { get; set; }
    }
}