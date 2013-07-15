﻿using ePunkt.Utilities;
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

            try
            {
                using (var pop3Client = new Pop3Client())
                {
                    var host = Settings.Get("Pop3Widget::" + Widget.InternalIdentifier + "::Host", "");
                    var port = Settings.Get("Pop3Widget::" + Widget.InternalIdentifier + "::Port", 110);
                    var useSsl = Settings.Get("Pop3Widget::" + Widget.InternalIdentifier + "::UseSsl", false);
                    var username = Settings.Get("Pop3Widget::" + Widget.InternalIdentifier + "::Username", "");
                    var password = Settings.Get("Pop3Widget::" + Widget.InternalIdentifier + "::Password", "");

                    pop3Client.Connect(host, port, useSsl);
                    if (username.HasValue())
                        pop3Client.Authenticate(username, password);

                    var messageCount = pop3Client.GetMessageCount();
                    newDataPoint.Value = messageCount;

                    pop3Client.Disconnect();
                }
            }
            catch
            {
                newDataPoint.Value = -1;
            }

            return newDataPoint;
        }
    }
}