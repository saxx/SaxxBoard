using Elmah;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace SaxxBoard.Widgets.RssWidget
{
    public class RssWidgetCollector : WidgetCollectorBase<WidgetCollectorBaseDataPoint>
    {
        public override IEnumerable<WidgetCollectorBaseDataPoint> Collect()
        {
            var newDataPoints = new List<WidgetCollectorBaseDataPoint>();

            var config = (RssWidgetConfiguration)Widget.Configuration;
            for (var i = 0; i < config.Series.Count(); i++)
            {
                try
                {
                    var url = ((RssWidgetConfigurationSeries)config.Series.ElementAt(i)).Url;
                    var xml = XDocument.Load(url);
                    var count = xml.Elements().First().Elements().First().Elements().Count() - 4;

                    newDataPoints.Add(new WidgetCollectorBaseDataPoint
                        {
                            Date = DateTime.Now,
                            SeriesIndex = i,
                            Value = count,
                            WidgetIdentifier = Widget.InternalIdentifier
                        });
                }
                catch (Exception ex)
                {
                    ErrorLog.GetDefault(HttpContext.Current).Log(new Error(ex));
                }
            }

            return newDataPoints;
        }
    }
}