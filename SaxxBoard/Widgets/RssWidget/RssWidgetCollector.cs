using Elmah;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace SaxxBoard.Widgets.RssWidget
{
    public class RssWidgetCollector : SimpleCollector<SimpleCollectorDataPoint>
    {
        public override IEnumerable<SimpleCollectorDataPoint> Collect()
        {
            var newDataPoints = new List<SimpleCollectorDataPoint>();

            try
            {
                var config = (RssConfiguration)Widget.GetConfiguration();

                for (var i = 0; i < config.Urls.Count(); i++)
                {
                    var url = config.Urls.ElementAt(i);
                    var xml = XDocument.Load(url);
                    var count = xml.Elements().First().Elements().First().Elements().Count() - 4;

                    newDataPoints.Add(new SimpleCollectorDataPoint
                        {
                            Date = DateTime.Now,
                            SeriesIndex = i,
                            Value = count,
                            WidgetIdentifier = Widget.InternalIdentifier
                        });
                }
            }
            catch (Exception ex)
            {
                ErrorLog.GetDefault(HttpContext.Current).Log(new Error(ex));
            }

            return newDataPoints;
        }
    }
}