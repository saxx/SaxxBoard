using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SaxxBoard.Widgets
{
    public interface ICollectorDataPoint
    {
        DateTime Date { get; set; }
    }
}