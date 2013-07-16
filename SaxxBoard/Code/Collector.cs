using Raven.Client;
using System.Collections.Generic;

namespace SaxxBoard
{
    public class Collector
    {
        private readonly IDocumentStore _db;
        private readonly WidgetCollection _widgets;

        private Dictionary<string, int> _remainingInterval = new Dictionary<string, int>();

        public Collector(IDocumentStore db, WidgetCollection widgets)
        {
            _widgets = widgets;
            _db = db;
        }

        public void Collect()
        {
            using (var dbSession = _db.OpenSession())
            {
                foreach (var widget in _widgets.CurrentWidgets)
                {
                    if (!_remainingInterval.ContainsKey(widget.InternalIdentifier))
                        _remainingInterval[widget.InternalIdentifier] = 0;
                    _remainingInterval[widget.InternalIdentifier] -= 1;

                    if (_remainingInterval[widget.InternalIdentifier] <= 0)
                    {
                        var widgetCollector = widget.GetCollector();
                        widgetCollector.Collect(dbSession);
                        _remainingInterval[widget.InternalIdentifier] = widget.GetConfiguration().RefreshIntervalInSeconds;
                    }
                }
            }
        }
    }
}