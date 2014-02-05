using Elmah;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Xml.Linq;
using SaxxBoard.Models;

namespace SaxxBoard.Widgets.NewRelicWidget
{
    public class NewRelicWidgetCollector : WidgetCollectorBase<DataPoint>
    {
        public override IEnumerable<DataPoint> Collect()
        {
            var newDataPoints = new List<DataPoint>();


            var config = (NewRelicWidgetConfiguration)Widget.Configuration;
            for (var seriesIndex = 0; seriesIndex < config.Series.Count(); seriesIndex++)
            {
                var seriesConfig = (NewRelicWidgetConfigurationSeries)config.Series.ElementAt(seriesIndex);

                try
                {

                    var interval = config.RefreshIntervalInSeconds;
                    if (interval < 60)
                        interval = 60;
                    var startDate = DateTime.Now.AddSeconds(-interval).ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
                    var endDate = DateTime.Now.AddMinutes(1).ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");

                    var url = "https://api.newrelic.com/api/v1/accounts/" + seriesConfig.Account + "/metrics/data.xml?begin=" + startDate + "&end=" + endDate + "&summary=1&field=" + seriesConfig.Field;
                    url = seriesConfig.Agents.Aggregate(url, (current, agent) => current + ("&agent_id[]=" + agent));
                    url = seriesConfig.Metrics.Aggregate(url, (current, metric) => current + ("&metrics[]=" + metric));

                    var request = WebRequest.Create(url);
                    request.Headers.Add("x-api-key", seriesConfig.ApiKey);

                    var response = request.GetResponse();
                    var responseStream = response.GetResponseStream();
                    if (responseStream == null)
                        throw new ArgumentException("ResponseStream is null.");

                    using (var responseReader = new StreamReader(responseStream))
                    {
                        var xml = XDocument.Load(responseReader);

                        for (var agentIndex = 0; agentIndex < seriesConfig.Agents.Count(); agentIndex++)
                        {
                            var agent = seriesConfig.Agents.ElementAt(agentIndex);

                            var value = 0.0;
                            // ReSharper disable once LoopCanBeConvertedToQuery
                            foreach (var metricNodes in xml.Elements("metrics").Elements("metric").Where(x => x.Attributes().Any(y => y.Name == "agent_id" && y.Value == agent)))
                            {
                                var fieldElement = metricNodes.Element("field");
                                if (fieldElement != null)
                                    value += double.Parse(fieldElement.Value, CultureInfo.InvariantCulture);
                                value += 0;
                            }

                            newDataPoints.Add(new DataPoint
                            {
                                DateTime = DateTime.Now,
                                SeriesIndex = seriesIndex,
                                Value = value,
                                WidgetIdentifier = Widget.InternalIdentifier
                            });
                        }
                    }
                }
                catch (WebException ex)
                {
                    using (var response = ex.Response)
                    using (var responseStream = response.GetResponseStream())
                        if (responseStream != null)
                            using (var reader = new StreamReader(responseStream))
                            {
                                var errorMessage = reader.ReadToEnd();
                                ErrorLog.GetDefault(HttpContext.Current).Log(new Error(new System.ApplicationException("Unable to call NewRelic API: " + errorMessage, ex)));
                            }
                        else
                            throw new ArgumentException("ResponseStream is null.");
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