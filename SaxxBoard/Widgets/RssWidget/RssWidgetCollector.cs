using Elmah;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using SaxxBoard.Models;

namespace SaxxBoard.Widgets.RssWidget
{
    public class RssWidgetCollector : WidgetCollectorBase<DataPoint>
    {
        public override IEnumerable<DataPoint> Collect()
        {
            var newDataPoints = new List<DataPoint>();

            var config = (RssWidgetConfiguration)Widget.Configuration;
            for (var i = 0; i < config.Series.Count(); i++)
            {
                try
                {
                    var url = ((RssWidgetConfigurationSeries)config.Series.ElementAt(i)).Url;
                    var xml = XDocument.Load(url);
                    var count = xml.Elements().First().Elements().First().Elements().Count() - 4;

                    newDataPoints.Add(new DataPoint
                        {
                            DateTime = DateTime.Now,
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