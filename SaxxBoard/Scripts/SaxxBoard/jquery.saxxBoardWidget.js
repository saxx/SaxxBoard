(function ($) {
    $.fn.saxxBoardWidget = function (options) {
        options = $.extend({
            title: "Chart",
            dataPoints: [],
            width: "100%",
            height: "250px",
            isScaledToPercents: true
        }, options);

        var container = $(this);
        container.html("");

        var titleDiv = $("<h3 />").html(options.title);
        var chartDiv = $("<div />").css("height", options.height).css("width", options.width);
        container.append(titleDiv);
        container.append(chartDiv);

        var plotSeries = {
            data: new Array(),
        };
        $.each(options.dataPoints, function (i, dataPoint) {
            var date = new Date(dataPoint.Date);
            plotSeries.data.push([date.getTime(), dataPoint.Value]);
        });

        var plotOptions = {
            series: {
                lines: { show: true }
            },
            grid: {
                backgroundColor: { colors: ["#fff", "#eee"] }
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
                minTickSize: [1, "minute"],
            }
        };
        chartDiv.plot([plotSeries], plotOptions);

        return container;
    };
}(jQuery));