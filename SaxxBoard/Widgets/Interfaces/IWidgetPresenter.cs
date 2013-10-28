using Raven.Client;
using System;
using System.Collections.Generic;

namespace SaxxBoard.Widgets
{
    public interface IWidgetPresenter
    {
        IEnumerable<IWidgetPresenterSeries> GetData(IDocumentSession dbSession);
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