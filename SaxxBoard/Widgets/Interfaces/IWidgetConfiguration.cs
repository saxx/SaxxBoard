
using System.Collections.Generic;

namespace SaxxBoard.Widgets
{
    public interface IWidgetConfiguration
    {
        int RefreshIntervalInSeconds { get; }
        int MaxDataPointsInChart { get; }
        int MaxDataPointsToStore { get; }

        IEnumerable<IWidgetConfigurationSeries> Series { get; set; }

        IChartConfiguration ChartConfiguration { get; set; }
        IWidget Widget { get; set; }
    }

    public interface IWidgetConfigurationSeries
    {
        string Label { get; }
    }

    public interface IChartConfiguration
    {
        double? MinTickSize { get; set; }
        double? MaxValue { get; set; }
        bool SumInsteadOfAverage { get; set; }
    }
}