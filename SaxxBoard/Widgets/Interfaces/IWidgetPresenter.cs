using System;
using System.Collections.Generic;
using SaxxBoard.Models;

namespace SaxxBoard.Widgets.Interfaces
{
    public interface IWidgetPresenter
    {
        IEnumerable<IWidgetPresenterSeries> GetData(Db dbSession);
        string FormatValue(double? rawValue);
        IWidget Widget { get; set; }
    }

    public interface IWidgetPresenterDataPoint
    {
        DateTime Date { get; set; }
        double? RawValue { get; }
    }

    public interface IWidgetPresenterSeries
    {
        string Label { get; set; }
        IEnumerable<IWidgetPresenterDataPoint> DataPoints { get; set; }
    }
}