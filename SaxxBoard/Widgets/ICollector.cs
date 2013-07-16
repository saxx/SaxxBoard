using Raven.Client;
using System.Collections.Generic;

namespace SaxxBoard.Widgets
{
    public interface ICollector
    {
        IEnumerable<ICollectorDataPoint> Collect(IDocumentSession dbSession);
        IWidget Widget { get; set; }
    }
}