using Elmah;
using ePunkt.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrelloNet;

namespace SaxxBoard.Widgets.TrelloWidget
{
    public class TrelloWidgetCollector : SimpleCollector<SimpleCollectorDataPoint>
    {
        public override IEnumerable<SimpleCollectorDataPoint> Collect()
        {
            var newDataPoints = new List<SimpleCollectorDataPoint>();

            try
            {
                var config = (TrelloConfiguration) Widget.GetConfiguration();
                var trello = new Trello(config.AppKey);
                trello.Authorize(config.UserToken);

                var boards = trello.Boards.ForMe();
                var board = boards.FirstOrDefault(x => x.Name.Equals(config.Board, StringComparison.InvariantCultureIgnoreCase));
                if (board == null)
                    throw new System.ApplicationException("Board '" + config.Board + "' not found.");

                if (config.Lists.Any())
                {
                    for (var i = 0; i < config.Lists.Count(); i++)
                    {
                        var value = 0;
                        foreach (var list in trello.Lists.ForBoard(board))
                            if (config.Lists.ElementAt(i).Any(x => x.Is(list.Name)))
                                value += trello.Cards.ForList(list).Count();
                        newDataPoints.Add(new SimpleCollectorDataPoint
                        {
                            Date = DateTime.Now,
                            SeriesIndex = i,
                            WidgetIdentifier = Widget.InternalIdentifier,
                            Value = value
                        });
                    }
                }
                else
                {
                    newDataPoints.Add(new SimpleCollectorDataPoint
                        {
                            Date = DateTime.Now,
                            SeriesIndex = 0,
                            WidgetIdentifier = Widget.InternalIdentifier,
                            Value = trello.Cards.ForBoard(board).Count()
                        });
                }
            }
            catch (Exception ex)
            {
                ErrorLog.GetDefault(HttpContext.Current).Log(new Error(ex));
            }

            return newDataPoints;
        }
    }
}