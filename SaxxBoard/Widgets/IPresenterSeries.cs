using System.Collections.Generic;

namespace SaxxBoard.Widgets
{
    public interface IPresenterSeries
    {
        string Label { get; set; }
        IEnumerable<IPresenterDataPoint> DataPoints { get; set; }
    }
}