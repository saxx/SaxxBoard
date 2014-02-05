using System.Collections.Generic;
using SaxxBoard.Widgets.Interfaces;

namespace SaxxBoard.Widgets.TrelloWidget
{
    public class TrelloWidgetConfiguration : WidgetConfigurationBase
    {
        public TrelloWidgetConfiguration(dynamic widgetConfiguration)
            : base((object)widgetConfiguration)
        {
            ChartConfiguration.SumInsteadOfAverage = true;

            var series = new List<IWidgetConfigurationSeries>();
            foreach (var s in widgetConfiguration.series)
                series.Add(new TrelloWidgetConfigurationSeries
                {
                    Label = s.label,
                    AppKey = s.appKey,
                    UserToken = s.userToken,
                    Board = s.board,
                    Lists = s.lists.Values<string>()
                });
            Series = series;
        }
    }

    public class TrelloWidgetConfigurationSeries : WidgetConfigurationSeriesBase
    {
        public string AppKey { get; set; }
        public string UserToken { get; set; }
        public string Board { get; set; }
        public IEnumerable<string> Lists { get; set; } 
    }
}