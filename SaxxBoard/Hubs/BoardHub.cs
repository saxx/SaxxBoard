using Microsoft.AspNet.SignalR;
using Raven.Client;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SaxxBoard.Hubs
{
    public class BoardHub : Hub
    {
        private readonly WidgetCollection _widgets;
        private readonly IDocumentStore _db;

        public BoardHub(WidgetCollection widgets, IDocumentStore db)
        {
            _db = db;
            _widgets = widgets;

            _widgets.OnCollectedCallback = UpdateBoard;
        }

        private void UpdateBoard()
        {
            var result = new List<JsonWidget>();

            foreach (var widget in _widgets.CurrentWidgets)
            {
                var config = widget.GetConfiguration();

                var jsonWidget = new JsonWidget
                    {
                        title = widget.Title,
                        refreshIntervalInSeconds = config.RefreshIntervalInSeconds,
                        minTickSizeOnChart = config.MinTickSizeOnChart,
                        maxValueOnChart = config.MaxValueOnChart,
                        higherIsBetter = config.HigherValueIsBetter
                    };

                using (var dbSession = _db.OpenSession())
                {
                    var series = widget.GetPresenter().GetData(dbSession).ToList();
                    jsonWidget.series = (from x in series
                                         select new JsonSeries
                                             {
                                                 label = x.Label,
                                                 dataPoints = (from y in x.DataPoints
                                                               select new JsonDataPoint
                                                                   {
                                                                       date = y.Date,
                                                                       rawValue = y.RawValue
                                                                   }).ToList()
                                             }).ToList();
                }

                jsonWidget.hasError = jsonWidget.series.Any(x => x.dataPoints.Any() && x.dataPoints.Last().rawValue == null);

                if (config.SumInsteadOfAverage)
                {
                    var sum = jsonWidget.series.Where(x => x.dataPoints.Any()).Select(x => x.dataPoints.Last().rawValue).Sum();
                    jsonWidget.lastValue = widget.GetPresenter().FormatValue(sum);
                }
                else
                {
                    var average = jsonWidget.series.Where(x => x.dataPoints.Any()).Select(x => x.dataPoints.Last().rawValue).Average();
                    jsonWidget.lastValue = widget.GetPresenter().FormatValue(average);
                }

                jsonWidget.trend = jsonWidget.series.Average(x => CalculateTrend(x.dataPoints.ToList()));
                foreach (var series in jsonWidget.series)
                    series.dataPoints = FillMissingDataPointsWithNull(config.MaxDataPointsInChart, config.RefreshIntervalInSeconds, series.dataPoints.ToList()).ToList();

                result.Add(jsonWidget);
            }

            Clients.All.updateBoard(result);
        }

        private double CalculateTrend(IList<JsonDataPoint> dataPoints)
        {
            var count = dataPoints.Count;

            if (count > 2)
            {
                var firstPart = (int)Math.Round(count * 0.75);

                var firstPartAverage = dataPoints.Take(firstPart).Where(x => x.rawValue.HasValue).Average(x => x.rawValue.Value);
                var secondPartAverage = dataPoints.Skip(firstPart).Where(x => x.rawValue.HasValue).Average(x => x.rawValue.Value);
                var maxValue = dataPoints.Where(x => x.rawValue.HasValue).Average(x => x.rawValue.Value);
                if (maxValue <= 0)
                    return 0;

                return (firstPartAverage - secondPartAverage) / maxValue;
            }
            return 0;
        }

        private IEnumerable<JsonDataPoint> FillMissingDataPointsWithNull(int maxDataPoints, int refreshIntervalInSeconds, IList<JsonDataPoint> dataPoints)
        {
            for (var i = 0; i < dataPoints.Count - 1; i++)
            {
                var difference = (dataPoints[i + 1].date - dataPoints[i].date).TotalSeconds;
                while (difference >= refreshIntervalInSeconds * 2)
                {
                    difference -= refreshIntervalInSeconds;
                    dataPoints.Add(new JsonDataPoint
                        {
                            date = dataPoints[i].date.AddSeconds(difference)
                        });
                }
            }

            return dataPoints.OrderByDescending(x => x.date).Take(maxDataPoints).OrderBy(x => x.date);
        }

        public void Refresh()
        {
            UpdateBoard();
        }

        // ReSharper disable InconsistentNaming
        public class JsonWidget
        {
            public string title { get; set; }
            public int refreshIntervalInSeconds { get; set; }
            public IEnumerable<JsonSeries> series { get; set; }
            public double trend { get; set; }
            public string lastValue { get; set; }
            public bool hasError { get; set; }
            public double? minTickSizeOnChart { get; set; }
            public double? maxValueOnChart { get; set; }
            public bool higherIsBetter { get; set; }
        }

        public class JsonSeries
        {
            public string label { get; set; }
            public IEnumerable<JsonDataPoint> dataPoints { get; set; }
        }

        public class JsonDataPoint
        {
            public double? rawValue { get; set; }
            public DateTime date { get; set; }
        }
        // ReSharper restore InconsistentNaming
    }
}