using SaxxBoard.Models;
using System;

namespace SaxxBoard
{
    public class Collector
    {
        private readonly Db _db;
        private readonly WidgetCollection _widgets;

        public Collector(Db db, WidgetCollection widgets)
        {
            _widgets = widgets;
            _db = db;
        }

        public void Collect()
        {
            foreach (var widget in _widgets.Widgets)
            {
                var config = widget.Configuration;

                if (!widget.NextUpdate.HasValue)
                    widget.NextUpdate = DateTime.Now;

                if (widget.NextUpdate < DateTime.Now)
                {
                    var widgetCollector = widget.GetCollector();
                    widgetCollector.Collect(_db);
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