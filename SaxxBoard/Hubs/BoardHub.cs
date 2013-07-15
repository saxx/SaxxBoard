using Microsoft.AspNet.SignalR;
using Raven.Client;
using SaxxBoard.Widgets;
using System;
using System.Linq;

namespace SaxxBoard.Hubs
{
    public class BoardHub : Hub
    {
        private readonly WidgetCollection _widgets;
        private readonly IDocumentStore _db;

        public BoardHub(WidgetCollection widgets, IDocumentStore db)
        {
            _db = db;
            _widgets = widgets;

            _widgets.OnCollectedCallback = UpdateBoard;
        }

        private void UpdateBoard()
        {
            using (var dbSession = _db.OpenSession())
            {
                var result = new
                    {
                        widgets = from x in _widgets.AvailableWidgets
                                  select new
                                      {
                                          x.Title,
                                          DataPoints = x.GetPresenter().GetData(dbSession).ToList(),
                                          x.IsScaledToPercents
                                      },
                        dateTime = DateTime.Now
                    };
                Clients.All.updateBoard(result);
            }
        }

        public void Refresh()
        {
            UpdateBoard();
        }
    }
}