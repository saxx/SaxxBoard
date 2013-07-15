using System.Web;
using Elmah;
using ePunkt.Utilities;
using System;
using System.Linq;
using System.Xml.Linq;

namespace SaxxBoard.Widgets.RssWidget
{
    public class RssWidgetCollector : SimpleCollector<SimpleDataPoint>
    {
        public override SimpleDataPoint Collect()
        {
            var newDataPoint = new SimpleDataPoint
                {
                    Date = DateTime.Now,
                    WidgetIdentifier = Widget.InternalIdentifier
                };

            try
            {
                var url = Settings.Get("RssWidget::" + Widget.InternalIdentifier + "::Url", "");
                var xml = XDocument.Load(url);
                newDataPoint.Value = xml.Elements().First().Elements().First().Elements().Count() - 4;
            }
            catch (Exception ex)
            {
                newDataPoint.Value = -1;
                ErrorLog.GetDefault(HttpContext.Current).Log(new Error(ex));
            }

            return newDataPoint;
        }
    }
}