(function ($) {
    $.fn.saxxBoardWidget = function (options) {
        options = $.extend({
            title: "Chart",
            dataPoints: [],
            height: "250px",
            isScaledToPercents: false,
            trend: 0
        }, options);

        var lastDataPointWasError = false;
        var lastDataPoint = options.dataPoints[options.dataPoints.length - 1];
        if (lastDataPoint && lastDataPoint.RawValue == null)
            lastDataPointWasError = true;

        var container = $(this);
        container.html("");

        var titleDiv = $("<h4 />").css("margin-bottom", "0");
        var valueDiv = $("<div />")
            .css("float", "right")
            .html(lastDataPoint.FormattedValue);

        var trendDegrees = (options.trend * 90);
        var trendDiv = $("<div />")
            .css("transform", "rotate(" + trendDegrees + "deg)")
            .css("color", (trendDegrees < 0 ? "#B76474" : (trendDegrees == 0 ? "#eeeeee" : "#9EB764")))
            .css("padding-left", "10px")
            .css("font-size", "800%")
            .css("float", "right")
            .css("z-index", "1")
            .css("font-weight", "bold")
            .html("&#8674;");
        titleDiv.append(trendDiv);
        titleDiv.append(valueDiv);
        titleDiv.append(options.title);
        container.append(titleDiv);


        var chartDiv = $("<div />").css("height", options.height).css("width", "100%");
        container.append(chartDiv);

        var plotSeries = {
            data: new Array(),
        };
        $.each(options.dataPoints, function (i, dataPoint) {
            var date = new Date(dataPoint.Date);
            plotSeries.data.push([date.getTime(), dataPoint.RawValue]);
        });

        var plotOptions = {
            series: {
                lines: { show: true }
            },
            grid: {
                backgroundColor: { colors: ["#fff", lastDataPointWasError ? "#f00" : "#eee"] }
            },
            yaxis: {
                min: 0,
                minTickSize: 1,
                tickDecimals: 0,
                max: (options.isScaledToPercents ? 100 : null),
                show: (options.isScaledToPercents ? false : true)
            },
            xaxis: {
                mode: "time",
                timezone: "browser",
                minTickSize: [1, "minute"]
            }
        };
        chartDiv.plot([plotSeries], plotOptions);

        return container;
    };
}(jQuery));