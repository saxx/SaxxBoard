using Microsoft.AspNet.SignalR;
using SaxxBoard.Models;
using SaxxBoard.Widgets.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace SaxxBoard.Hubs
{
    public class BoardHub : Hub
    {
        private readonly WidgetCollection _widgets;
        private readonly Db _db;

        public BoardHub(WidgetCollection widgets, Db db)
        {
            _db = db;
            _widgets = widgets;

            _widgets.OnCollectedCallback = UpdateBoard;
        }

        private void UpdateBoard(bool updateCallerOnly)
        {
            foreach (var widget in _widgets.Widgets)
                UpdateBoard(widget, updateCallerOnly);
        }

        private void UpdateBoard(IWidget widget, bool updateCallerOnly)
        {
            var config = widget.Configuration;

            var jsonWidget = new JsonWidget
                {
                    identifier = widget.InternalIdentifier,
                    title = widget.Title,
                    minTickSizeOnChart = config.ChartConfiguration.MinTickSize,
                    maxValueOnChart = config.ChartConfiguration.MaxValue
                };

            var series = widget.GetPresenter().GetData(_db).ToList();
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

            jsonWidget.hasError = jsonWidget.series.Any(x => x.dataPoints.Any() && x.dataPoints.Last().rawValue == null);

            if (config.ChartConfiguration.SumInsteadOfAverage)
            {
                var sum = jsonWidget.series.Where(x => x.dataPoints.Any()).Select(x => x.dataPoints.Last().rawValue).Sum();
                jsonWidget.lastValue = widget.GetPresenter().FormatValue(sum);
            }
            else
            {
                var average = jsonWidget.series.Where(x => x.dataPoints.Any()).Select(x => x.dataPoints.Last().rawValue).Average();
                jsonWidget.lastValue = widget.GetPresenter().FormatValue(average);
            }

            foreach (var s in jsonWidget.series)
                s.dataPoints = FillMissingDataPointsWithNull(config.RefreshIntervalInSeconds, s.dataPoints.ToList()).ToList();

            Clients.All.updateBoard(jsonWidget);
        }

        public void Refresh()
        {
            UpdateBoard(true);
        }

        private IEnumerable<JsonDataPoint> FillMissingDataPointsWithNull(int refreshIntervalInSeconds, IList<JsonDataPoint> dataPoints)
        {
            dataPoints = dataPoints.OrderBy(x => x.date).ToList();

            var newDataPoints = new List<JsonDataPoint>();
            for (var i = 0; i < dataPoints.Count - 1; i++)
            {
                var difference = (dataPoints[i + 1].date - dataPoints[i].date).TotalSeconds;
                if (difference >= refreshIntervalInSeconds * 2)
                {
                    newDataPoints.Add(new JsonDataPoint
                        {
                            date = dataPoints[i].date.AddSeconds(1)
                        });
                    newDataPoints.Add(new JsonDataPoint
                        {
                            date = dataPoints[i + 1].date.AddSeconds(-1)
                        });
                }
            }

            newDataPoints.AddRange(dataPoints);
            dataPoints = newDataPoints.OrderBy(x => x.date).ToList();

            //500 datapoints is the absolute max. we're gonna paint, even with the empty datapoints included
            var diffToMax = dataPoints.Count - 500;
            dataPoints = dataPoints.Skip(diffToMax).ToList();

            return dataPoints;
        }

        // ReSharper disable InconsistentNaming
        public class JsonWidget
        {
            public string identifier { get; set; }
            public string title { get; set; }
            public IEnumerable<JsonSeries> series { get; set; }
            public string lastValue { get; set; }
            public bool hasError { get; set; }
            public double? minTickSizeOnChart { get; set; }
            public double? maxValueOnChart { get; set; }
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

            public override string ToString()
            {
                return date.ToString(CultureInfo.CurrentCulture) + ": " + (rawValue.HasValue ? rawValue.Value.ToString("N3") : "null");
            }
        }
        // ReSharper restore InconsistentNaming
    }
}