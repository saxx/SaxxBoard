using System.Collections.Generic;
using Microsoft.AspNet.SignalR;
using Raven.Client;
using SaxxBoard.Widgets;
using System;
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
            using (var dbSession = _db.OpenSession())
            {
                var result = new
                    {
                        widgets = (from x in _widgets.CurrentWidgets
                                   select new JsonResult
                                       {
                                           Title = x.Title,
                                           DataPoints = x.GetPresenter().GetData(dbSession).ToList(),
                                           IsScaledToPercents = x.GetConfiguration().IsScaledToPercents,
                                           RefreshIntervalInSeconds = x.GetConfiguration().RefreshIntervalInSeconds,
                                           MaxDataPoints = x.GetConfiguration().MaxDataPointsInChart
                                       }).ToList(),
                        dateTime = DateTime.Now
                    };

                foreach (var widget in result.widgets)
                    widget.Trend = CalculateTrend(widget.DataPoints.ToList());

                foreach (var widget in result.widgets)
                    widget.DataPoints = FillMissingDataPointsWithNull(widget.MaxDataPoints, widget.RefreshIntervalInSeconds, widget.DataPoints.ToList()).ToList();

                Clients.All.updateBoard(result);
            }
        }

        private double CalculateTrend(IList<IPresenterDataPoint> dataPoints)
        {
            var count = dataPoints.Count;

            if (count > 2)
            {
                var firstPart = (int)Math.Round(count * 0.75);

                var firstPartAverage = dataPoints.Take(firstPart).Where(x => x.RawValue.HasValue).Average(x => x.RawValue.Value);
                var secondPartAverage = dataPoints.Skip(firstPart).Where(x => x.RawValue.HasValue).Average(x => x.RawValue.Value);
                var maxValue = dataPoints.Where(x => x.RawValue.HasValue).Average(x => x.RawValue.Value);
                if (maxValue <= 0)
                    return 0;

                return (firstPartAverage - secondPartAverage) / maxValue;
            }
            return 0;
        }

        private IEnumerable<IPresenterDataPoint> FillMissingDataPointsWithNull(int maxDataPoints, int refreshIntervalInSeconds, IList<IPresenterDataPoint> dataPoints)
        {
            for (var i = 0; i < dataPoints.Count - 1; i++)
            {
                var difference = (dataPoints[i + 1].Date - dataPoints[i].Date).TotalSeconds;
                while (difference >= refreshIntervalInSeconds * 2)
                {
                    difference -= refreshIntervalInSeconds;
                    dataPoints.Add(new SimplePresenterDataPoint
                        {
                            Date = dataPoints[i].Date.AddSeconds(difference),
                            RawValue = null,
                            FormattedValue = null
                        });
                }
            }

            return dataPoints.OrderByDescending(x => x.Date).Take(maxDataPoints).OrderBy(x => x.Date);
        }



        public void Refresh()
        {
            UpdateBoard();
        }

        public class JsonResult
        {
            public string Title { get; set; }
            public int RefreshIntervalInSeconds { get; set; }
            public IEnumerable<IPresenterDataPoint> DataPoints { get; set; }
            public double Trend { get; set; }
            public bool IsScaledToPercents { get; set; }
            public int MaxDataPoints { get; set; }
        }
    }
}