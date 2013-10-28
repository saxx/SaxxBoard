using ePunkt.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SaxxBoard.Widgets.NewRelicWidget
{
    public class NewRelicWidgetConfiguration : WidgetConfigurationBase
    {

        public NewRelicWidgetConfiguration(dynamic widgetConfiguration)
            : base((object)widgetConfiguration)
        {
            var series = new List<NewRelicWidgetConfigurationSeries>();
            foreach (var s in widgetConfiguration.series)
                series.Add(new NewRelicWidgetConfigurationSeries
                {
                    Label = s.label,
                    Account = s.account,
                    Agents = s.agents.Values<string>(),
                    ApiKey = s.apiKey,
                    Field = s.field ?? "average_value",
                    Metrics = s.metrics.Values<string>()
                });
            Series = series;

            ValueIsSeconds = series.Any(x => x.Field.Is("average_response_time"));
            ValueIsBytes =
                series.Any(x => x.Metrics.Any(y => y.EndsWith("bytes/sec", StringComparison.InvariantCultureIgnoreCase)));
            ValueIsApdex = series.Any(x => x.Metrics.Any(y => y.Is("apdex")));
            ValueIsPercent = series.Any(x => x.Metrics.Any(y => y.EndsWith("percent", StringComparison.InvariantCultureIgnoreCase)));

            ChartConfiguration.SumInsteadOfAverage = ValueIsBytes || series.Any(x => x.Field.Is("requests_per_minute"));
            if (ValueIsPercent)
                ChartConfiguration.MaxValue = 100;
            if (ValueIsApdex)
                ChartConfiguration.MaxValue = 1.3;
        }

        public bool ValueIsPercent
        {
            get;
            private set;
        }

        public bool ValueIsApdex
        {
            get;
            private set;
        }

        public bool ValueIsBytes
        {
            get;
            private set;
        }

        public bool ValueIsSeconds { get; private set; }
    }

    public class NewRelicWidgetConfigurationSeries : WidgetConfigurationSeriesBase
    {
        public string ApiKey { get; set; }
        public string Account { get; set; }
        public IEnumerable<string> Metrics { get; set; }
        public IEnumerable<string> Agents { get; set; }
        public string Field { get; set; }
    }
}