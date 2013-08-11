using Raven.Client;
using System;

namespace SaxxBoard
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
                foreach (var widget in _widgets.CurrentWidgets)
                {
                    var config = widget.GetConfiguration();
                    var random = new Random();

                    if (!widget.NextUpdate.HasValue)
                        widget.NextUpdate = DateTime.Now.AddSeconds(random.Next(1, config.RefreshIntervalInSeconds));


                    if (widget.NextUpdate < DateTime.Now)
                    {
                        var widgetCollector = widget.GetCollector();
                        widgetCollector.Collect(dbSession);
                        widget.LastUpdate = DateTime.Now;
                        widget.NextUpdate = DateTime.Now.AddSeconds(config.RefreshIntervalInSeconds);
                        if (_widgets.OnCollectedCallback != null)
                            _widgets.OnCollectedCallback.Invoke(widget, false);

                        //if we already have a collection this cycle, we wait for the next cycle to do the next collection
                        return;
                    }
                }
            }
        }
    }
}