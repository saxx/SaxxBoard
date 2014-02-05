using SaxxBoard.Models;
using System.Collections.Generic;

namespace SaxxBoard.Widgets
{
    public interface IWidgetCollector
    {
        IEnumerable<DataPoint> Collect(Db dbSession);
        IWidget Widget { get; set; }
    }
}