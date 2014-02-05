using System.Collections.Generic;
using SaxxBoard.Widgets.Interfaces;

namespace SaxxBoard.Widgets.RssWidget
{
    public class RssWidgetConfiguration : WidgetConfigurationBase
    {
        public RssWidgetConfiguration(dynamic widgetConfiguration) : base((object)widgetConfiguration)
        {
            ChartConfiguration.SumInsteadOfAverage = true;

            var series = new List<IWidgetConfigurationSeries>();
            foreach (var s in widgetConfiguration.series)
                series.Add(new RssWidgetConfigurationSeries
                {
                    Label = s.label,
                    Url = s.url
                });
            Series = series;
        }
    }

    public class RssWidgetConfigurationSeries : WidgetConfigurationSeriesBase
    {
        public string Url { get; set; }
    }
}