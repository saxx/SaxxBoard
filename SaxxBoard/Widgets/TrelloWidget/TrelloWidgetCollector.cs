using Elmah;
using ePunkt.Utilities;
using System;
using System.Linq;
using System.Web;
using TrelloNet;

namespace SaxxBoard.Widgets.TrelloWidget
{
    public class TrelloWidgetCollector : SimpleCollector<SimpleDataPoint>
    {
        public override SimpleDataPoint Collect()
        {
            var newDataPoint = new SimpleDataPoint
                {
                    Date = DateTime.Now,
                    WidgetIdentifier = Widget.InternalIdentifier,
                    Value = 0
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

                if (config.Lists.Any())
                {
                    foreach (var list in trello.Lists.ForBoard(board))
                        if (config.Lists.Any(x => x.Is(list.Name)))
                            newDataPoint.Value += trello.Cards.ForList(list).Count();
                }
                else
                    newDataPoint.Value = trello.Cards.ForBoard(board).Count();
            }
            catch (Exception ex)
            {
                newDataPoint.Value = -1;
                ErrorLog.GetDefault(HttpContext.Current).Log(new Error(ex));
            }

            return newDataPoint;
        }
    }
}