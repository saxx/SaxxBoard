using Elmah;
using ePunkt.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SaxxBoard.Widgets.Pop3Widget;
using TrelloNet;

namespace SaxxBoard.Widgets.TrelloWidget
{
    public class TrelloWidgetCollector : WidgetCollectorBase<WidgetCollectorBaseDataPoint>
    {
        public override IEnumerable<WidgetCollectorBaseDataPoint> Collect()
        {
            var newDataPoints = new List<WidgetCollectorBaseDataPoint>();
            var config = (TrelloWidgetConfiguration)Widget.Configuration;

            for (var i = 0; i < config.Series.Count(); i++)
            {
                var seriesConfig = ((TrelloWidgetConfigurationSeries)config.Series.ElementAt(i));

                try
                {
                    var trello = new Trello(seriesConfig.AppKey);
                    trello.Authorize(seriesConfig.UserToken);

                    var boards = trello.Boards.ForMe();
                    var board = boards.FirstOrDefault(x => x.Name.Equals(seriesConfig.Board, StringComparison.InvariantCultureIgnoreCase));
                    if (board == null)
                        throw new System.ApplicationException("Board '" + seriesConfig.Board + "' not found.");

                    if (seriesConfig.Lists.Any())
                    {
                        var value = 0;
                        foreach (var list in trello.Lists.ForBoard(board))
                            if (seriesConfig.Lists.Any(x => x.Is(list.Name)))
                                value += trello.Cards.ForList(list).Count();
                        newDataPoints.Add(new WidgetCollectorBaseDataPoint
                        {
                            Date = DateTime.Now,
                            SeriesIndex = i,
                            WidgetIdentifier = Widget.InternalIdentifier,
                            Value = value
                        });
                    }
                    else
                    {
                        newDataPoints.Add(new WidgetCollectorBaseDataPoint
                        {
                            Date = DateTime.Now,
                            SeriesIndex = i,
                            WidgetIdentifier = Widget.InternalIdentifier,
                            Value = trello.Cards.ForBoard(board).Count()
                        });
                    }
                }
                catch (Exception ex)
                {
                    ErrorLog.GetDefault(HttpContext.Current).Log(new Error(ex));
                }
            }

            return newDataPoints;
        }
    }
}