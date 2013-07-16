using Elmah;
using ePunkt.Utilities;
using System;
using System.Linq;
using System.Web;
using TrelloNet;

namespace SaxxBoard.Widgets.TrelloWidget
{
    public class TrelloWidgetCollector : SimpleCollector<SimpleCollectorDataPoint>
    {
        public override SimpleCollectorDataPoint Collect()
        {
            var newDataPoint = new SimpleCollectorDataPoint
                {
                    Date = DateTime.Now,
                    WidgetIdentifier = Widget.InternalIdentifier,
                };

            try
            {
                var config = (TrelloConfiguration) Widget.GetConfiguration();
                var trello = new Trello(config.AppKey);
                trello.Authorize(config.UserToken);

                var boards = trello.Boards.ForMe();
                var board = boards.FirstOrDefault(x => x.Name.Equals(config.Board, StringComparison.InvariantCultureIgnoreCase));
                if (board == null)
                    throw new System.ApplicationException("Board '" + config.Board + "' not found.");

                var value = 0;
                if (config.Lists.Any())
                {
                    foreach (var list in trello.Lists.ForBoard(board))
                        if (config.Lists.Any(x => x.Is(list.Name)))
                            value += trello.Cards.ForList(list).Count();
                }
                else
                    value = trello.Cards.ForBoard(board).Count();
                newDataPoint.Value = value;
            }
            catch (Exception ex)
            {
                ErrorLog.GetDefault(HttpContext.Current).Log(new Error(ex));
            }

            return newDataPoint;
        }
    }
}