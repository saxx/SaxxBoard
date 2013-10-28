using System.Collections.Generic;
using SaxxBoard.Widgets.RssWidget;

namespace SaxxBoard.Widgets.Pop3Widget
{
    public class Pop3WidgetConfiguration : WidgetConfigurationBase
    {

        public Pop3WidgetConfiguration(dynamic widgetConfiguration)
            : base((object)widgetConfiguration)
        {
            ChartConfiguration.MinTickSize = 1;
            ChartConfiguration.SumInsteadOfAverage = true;

            var series = new List<IWidgetConfigurationSeries>();
            foreach (var s in widgetConfiguration.series)
                series.Add(new Pop3WidgetConfigurationSeries
                {
                    Label = s.label,
                    Host = s.host ?? "",
                    Port = s.port ?? 110,
                    UseSsl = s.useSsl ?? false,
                    Username = s.username ?? "",
                    Password = s.password ?? ""
                });
            Series = series;

        }
    }

    public class Pop3WidgetConfigurationSeries : WidgetConfigurationSeriesBase
    {
        public string Host
        {
            get;
            set;
        }

        public int Port
        {
            get;
            set;
        }

        public bool UseSsl
        {
            get;
            set;
        }

        public string Username
        {
            get;
            set;
        }

        public string Password
        {
            get;
            set;
        }
    }
}