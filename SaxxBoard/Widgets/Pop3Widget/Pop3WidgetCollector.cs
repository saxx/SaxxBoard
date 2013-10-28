using System.Linq;
using Elmah;
using ePunkt.Utilities;
using OpenPop.Pop3;
using System;
using System.Collections.Generic;
using System.Web;

namespace SaxxBoard.Widgets.Pop3Widget
{
    public class Pop3WidgetCollector : WidgetCollectorBase<WidgetCollectorBaseDataPoint>
    {
        public override IEnumerable<WidgetCollectorBaseDataPoint> Collect()
        {
            var newDataPoints = new List<WidgetCollectorBaseDataPoint>();
            var config = (Pop3WidgetConfiguration)Widget.Configuration;
            
            for (var i = 0; i < config.Series.Count(); i++)
            {
                var seriesConfig = ((Pop3WidgetConfigurationSeries)config.Series.ElementAt(i));

                var host = seriesConfig.Host;
                var port = seriesConfig.Port;
                var useSsl = seriesConfig.UseSsl;
                var username = seriesConfig.Username;
                var password = seriesConfig.Password;

                try
                {
                    using (var pop3Client = new Pop3Client())
                    {
                        pop3Client.Connect(host, port, useSsl);
                        if (username.HasValue())
                            pop3Client.Authenticate(username, password);

                        var messageCount = pop3Client.GetMessageCount();
                        pop3Client.Disconnect();

                        newDataPoints.Add(new WidgetCollectorBaseDataPoint
                        {
                            Date = DateTime.Now,
                            SeriesIndex = i,
                            Value = messageCount,
                            WidgetIdentifier = Widget.InternalIdentifier
                        });
                    }
                }
                catch (Exception ex)
                {
                    ErrorLog.GetDefault(HttpContext.Current).Log(new Error(new System.ApplicationException("Unable to fetch POP3. Host: " + host + ", Username: " + username + ", Password: " + password + ".", ex)));
                }
            }


            return newDataPoints;
        }
    }
}