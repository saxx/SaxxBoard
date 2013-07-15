using Raven.Client;
using System.Collections.Generic;

namespace SaxxBoard.Widgets
{
    public interface ICollector
    {
        IEnumerable<ICollectorDataPoint> Collect(IDocumentSession dbSession);
        event CollectedDelegate OnCollected;
        IWidget Widget { get; set; }
    }
}