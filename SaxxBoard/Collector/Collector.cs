﻿using System.Collections.Generic;
using Raven.Client;
using SaxxBoard.Widgets;

namespace SaxxBoard.Collector
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
                foreach (var widget in _widgets.AvailableWidgets)
                {
                    if (!_remainingInterval.ContainsKey(widget.InternalIdentifier))
                        _remainingInterval[widget.InternalIdentifier] = 0;
                    _remainingInterval[widget.InternalIdentifier] -= 1;

                    if (_remainingInterval[widget.InternalIdentifier] <= 0)
                    {
                        var widgetCollector = widget.GetCollector();
                        widgetCollector.Collect(dbSession);
                        _remainingInterval[widget.InternalIdentifier] = widget.CollectIntervalInSeconds;
                    }
                }
            }
        }
    }
}