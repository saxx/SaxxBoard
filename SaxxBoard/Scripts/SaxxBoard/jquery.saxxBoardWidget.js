(function ($) {
    $.fn.saxxBoardWidget = function (options) {
        options = $.extend({
            title: "",
            series: [],
            height: "250px",
            lastValue: "",
            hasError: false,
            minTickSize: null,
            maxValue: null,
        }, options);

        function getColorForSeries(index) {
            if (index == 0)
                return "#edc240";
            if (index == 1)
                return "#afd8f8";
            if (index == 2)
                return "#cb4b4b";
            if (index == 3)
                return "#4da74d";
            if (index == 4)
                return "#9440ed";
            if (index == 5)
                return "#568765";
            if (index == 6)
                return "#a67f45";
            if (index == 7)
                return "#13e551";
            if (index == 8)
                return "#4578f9";
            if (index == 9)
                return "#f85611";
            return null;
        }

        var container = $(this);
        container
            .css("margin-bottom", "15px")
            .html("");

        var titleDiv = $("<h4 />").css("text-align", "center").css("margin","0");
        titleDiv.append(options.title + " &raquo; " + options.lastValue);
        container.append(titleDiv);

        var chartDiv = $("<div />").css("height", options.height).css("width", "100%");
        container.append(chartDiv);

        var plotSeries = new Array(); $.each(options.series, function (i, serie) {
            var plotSerie = {
                color: getColorForSeries(i),
                data: new Array(),
                label: serie.label + "&nbsp;"
            };
            $.each(serie.dataPoints, function (j, dataPoint) {
                var date = new Date(dataPoint.date);
                plotSerie.data.push([date.getTime(), dataPoint.rawValue]);
            });
            plotSeries.push(plotSerie);
        });

        var plotOptions = {
            series: {
                lines: { show: true }
            },
            grid: {
                backgroundColor: { colors: ["#fff", options.hasError ? "#f00" : "#eee"] }
            },
            yaxis: {
                min: 0,
                minTickSize: options.minTickSize,
                tickDecimals: 0,
                max: options.maxValue,
                show: true
            },
            xaxis: {
                mode: "time",
                timezone: "browser",
                minTickSize: [1, "minute"]
            },
            legend: {
                show: options.series.length > 1 ? true : false,
                position: "nw",
                margin: [7, 3],
                sorted: true,
                backgroundOpacity: 0.6,
                noColumns: 2
            }
        };
        chartDiv.plot(plotSeries, plotOptions);

        return container;
    };
}(jQuery));