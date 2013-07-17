using Elmah;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Xml.Linq;

namespace SaxxBoard.Widgets.NewRelicWidget
{
    public class NewRelicWidgetCollector : SimpleCollector<SimpleCollectorDataPoint>
    {
        public override IEnumerable<SimpleCollectorDataPoint> Collect()
        {
            var newDataPoints = new List<SimpleCollectorDataPoint>();

            try
            {
                var config = (NewRelicConfiguration)Widget.GetConfiguration();

                var interval = config.RefreshIntervalInSeconds;
                if (interval < 60)
                    interval = 60;
                var startDate = DateTime.Now.AddSeconds(-interval).ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
                var endDate = DateTime.Now.AddMinutes(1).ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");

                var url = "https://api.newrelic.com/api/v1/accounts/" + config.Account + "/metrics/data.xml?begin=" + startDate + "&end=" + endDate + "&summary=1&field=" + config.Field;
                url = config.Agents.Aggregate(url, (current, agent) => current + ("&agent_id[]=" + agent));
                url = config.Metrics.Aggregate(url, (current, metric) => current + ("&metrics[]=" + metric));

                var request = WebRequest.Create(url);
                request.Headers.Add("x-api-key", config.ApiKey);

                var response = request.GetResponse();
                using (var responseReader = new StreamReader(response.GetResponseStream()))
                {
                    var xml = XDocument.Load(responseReader);

                    for (var i = 0; i < config.Agents.Count(); i++)
                    {
                        var agent = config.Agents.ElementAt(i);

                        var value = 0.0;
                        foreach (var metricNodes in xml.Elements("metrics").Elements("metric").Where(x => x.Attributes().Any(y => y.Name == "agent_id" && y.Value == agent)))
                            value += double.Parse(metricNodes.Element("field").Value, CultureInfo.InvariantCulture);

                        newDataPoints.Add(new SimpleCollectorDataPoint
                            {
                                Date = DateTime.Now,
                                SeriesIndex = i,
                                Value = value,
                                WidgetIdentifier = Widget.InternalIdentifier
                            });
                    }
                }
            }
            catch (WebException ex)
            {
                using (var response = ex.Response)
                {
                    using (var data = response.GetResponseStream())
                    using (var reader = new StreamReader(data))
                    {
                        var errorMessage = reader.ReadToEnd();
                        ErrorLog.GetDefault(HttpContext.Current).Log(new Error(new System.ApplicationException("Unable to call NewRelic API: " + errorMessage, ex)));
                    }
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