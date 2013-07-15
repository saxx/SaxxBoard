using System.Threading;
using System.Web;
using Elmah;
using ePunkt.Utilities;
using OpenPop.Pop3;
using System;

namespace SaxxBoard.Widgets.Pop3Widget
{
    public class Pop3WidgetCollector : SimpleCollector<SimpleDataPoint>
    {
        public override SimpleDataPoint Collect()
        {
            var newDataPoint = new SimpleDataPoint
                {
                    Date = DateTime.Now,
                    WidgetIdentifier = Widget.InternalIdentifier
                };

            var host = Settings.Get("Pop3Widget::" + Widget.InternalIdentifier + "::Host", "");
            var port = Settings.Get("Pop3Widget::" + Widget.InternalIdentifier + "::Port", 110);
            var useSsl = Settings.Get("Pop3Widget::" + Widget.InternalIdentifier + "::UseSsl", false);
            var username = Settings.Get("Pop3Widget::" + Widget.InternalIdentifier + "::Username", "");
            var password = Settings.Get("Pop3Widget::" + Widget.InternalIdentifier + "::Password", "");

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
                newDataPoint.Value = -1;
                ErrorLog.GetDefault(HttpContext.Current).Log(new Error(new System.ApplicationException("Unable to fetch POP3. Host: " + host + ", Username: " + username + ", Password: " + password + ".", ex)));
            }

            return newDataPoint;
        }
    }
}