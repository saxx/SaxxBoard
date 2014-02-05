using System.Collections.Generic;
using SaxxBoard.Models;

namespace SaxxBoard.Widgets.Interfaces
{
    public interface IWidgetCollector
    {
        IEnumerable<DataPoint> Collect(Db dbSession);
        IWidget Widget { get; set; }
    }
}