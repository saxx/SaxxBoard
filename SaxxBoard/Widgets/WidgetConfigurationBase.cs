using System.Collections.Generic;

namespace SaxxBoard.Widgets
{
    public class WidgetConfigurationBase : IWidgetConfiguration
    {
        public IWidget Widget { get; set; }

        public WidgetConfigurationBase(dynamic widgetConfiguration)
        {
            if (widgetConfiguration.refreshIntervalInSeconds != null)
                RefreshIntervalInSeconds = widgetConfiguration.refreshIntervalInSeconds;
            else
                RefreshIntervalInSeconds = 60 * 5;

            if (widgetConfiguration.maxDataPointsInChart != null)
                MaxDataPointsInChart = widgetConfiguration.maxDataPointsInChart;
            else
                MaxDataPointsInChart = 1000;

            var series = new List<IWidgetConfigurationSeries>();
            foreach (var s in widgetConfiguration.series)
                series.Add(new WidgetConfigurationSeriesBase
                {
                    Label = s.label
                });
            Series = series;

            ChartConfiguration = new ChartConfigurationBase();
        }


        public int RefreshIntervalInSeconds { get; private set; }
        public int MaxDataPointsInChart { get; private set; }

        public IEnumerable<IWidgetConfigurationSeries> Series
        {
            get;
            set;
        }

        public IChartConfiguration ChartConfiguration
        {
            get;
            set;
        }
    }


    public class WidgetConfigurationSeriesBase : IWidgetConfigurationSeries
    {
        public string Label { get; set; }
    }


    public class ChartConfigurationBase : IChartConfiguration
    {
        public double? MinTickSize
        {
            get;
            set;
        }

        public double? MaxValue
        {
            get;
            set;
        }

        public bool SumInsteadOfAverage
        {
            get;
            set;
        }
    }
}