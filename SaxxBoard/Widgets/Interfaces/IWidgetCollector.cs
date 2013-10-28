using Raven.Client;
using System;
using System.Collections.Generic;

namespace SaxxBoard.Widgets
{
    public interface IWidgetCollector
    {
        IEnumerable<IWidgetCollectorDataPoint> Collect(IDocumentSession dbSession);
        IWidget Widget { get; set; }
    }

    public interface IWidgetCollectorDataPoint
    {
        DateTime Date { get; set; }
    }
}