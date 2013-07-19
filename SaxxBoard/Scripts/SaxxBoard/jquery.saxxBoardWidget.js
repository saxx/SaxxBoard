(function ($) {
    $.fn.saxxBoardWidget = function (options) {
        options = $.extend({
            title: "",
            series: [],
            height: "250px",
            trend: 0,
            lastValue: "",
            hasError: false,
            minTickSize: null,
            maxValue: null,
            higherIsBetter: false,
            lastUpdate: null,
            nextUpdate: null
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
            .css("margin-bottom", "30px")
            .html("");

        var titleDiv = $("<h3 />").css("margin", "0 5px 0 5px");
        var valueDiv = $("<div />")
            .css("float", "right")
            .html(options.lastValue);

        var trendDegrees = (options.trend * 90);
        if (trendDegrees < -90)
            trendDegrees = -90;
        if (trendDegrees > 90)
            trendDegrees = 90;
        var trendDiv = $("<div />")
            .css("transform", "rotate(" + trendDegrees + "deg)")
            .css("color", (trendDegrees < 0 ? (options.higherIsBetter ? "#9EB764" : "#B76474") : (trendDegrees == 0 ? "#eeeeee" : (options.higherIsBetter ? "#B76474" : "#9EB764"))))
            .css("padding-left", "10px")
            .css("font-size", "200%")
            .css("float", "right")
            .css("z-index", "1")
            .css("font-weight", "bold")
            .html("&#10148;");
        titleDiv.append(trendDiv);
        titleDiv.append(valueDiv);
        titleDiv.append(options.title);

        var lastUpdate = new Date(options.lastUpdate);
        var nextUpdate = new Date(options.nextUpdate);

        if (options.lastUpdate) {
            titleDiv.append(" <span class='lastUpdateTicker' data-lastupdate='" + lastUpdate.getTime() + "' style='font-size:40%;font-weight:normal;'></span>");
        }

        if (options.nextUpdate && options.lastUpdate) {
            var progressDiv = $("<div class='nextUpdateTicker' data-lastupdate='" + lastUpdate.getTime() + "' data-nextupdate='" + nextUpdate.getTime() + "' />")
                .css("width", "100%")
                .css("background-color", "white")
                .css("height", "31px")
                .css("margin-bottom", "-37px");
            container.append(progressDiv);
        }

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