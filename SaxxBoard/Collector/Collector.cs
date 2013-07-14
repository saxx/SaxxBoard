using Raven.Client;
using SaxxBoard.Widgets;

namespace SaxxBoard.Collector
{
    public class Collector
    {
        private readonly IDocumentStore _db;
        private readonly WidgetCollection _widgets;

        public Collector(IDocumentStore db, WidgetCollection widgets)
        {
            _widgets = widgets;
            _db = db;
        }

        public void Collect()
        {
            using (var dbSession = _db.OpenSession())
            {
                foreach (var widget in _widgets.AvailableWidgets)
                {
                    var widgetCollector = widget.GetCollector();
                    widgetCollector.Collect(dbSession);
                }
            }
        }
    }
}