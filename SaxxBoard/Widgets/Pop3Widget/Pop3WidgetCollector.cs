using Elmah;
using ePunkt.Utilities;
using OpenPop.Pop3;
using System;
using System.Collections.Generic;
using System.Web;

namespace SaxxBoard.Widgets.Pop3Widget
{
    public class Pop3WidgetCollector : SimpleCollector<SimpleCollectorDataPoint>
    {
        public override IEnumerable<SimpleCollectorDataPoint> Collect()
        {
            var newDataPoint = new SimpleCollectorDataPoint
                {
                    Date = DateTime.Now,
                    WidgetIdentifier = Widget.InternalIdentifier
                };

            var config = (Pop3Configuration)Widget.GetConfiguration();
            var host = config.Host;
            var port = config.Port;
            var useSsl = config.UseSsl;
            var username = config.Username;
            var password = config.Password;

            try
            {
                using (var pop3Client = new Pop3Client())
                {
                    pop3Client.Connect(host, port, useSsl);
                    if (username.HasValue())
                        pop3Client.Authenticate(username, password);

                    var messageCount = pop3Client.GetMessageCount();
                    newDataPoint.Value = messageCount;

                    pop3Client.Disconnect();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.GetDefault(HttpContext.Current).Log(new Error(new System.ApplicationException("Unable to fetch POP3. Host: " + host + ", Username: " + username + ", Password: " + password + ".", ex)));
            }

            return new[] { newDataPoint };
        }
    }
}