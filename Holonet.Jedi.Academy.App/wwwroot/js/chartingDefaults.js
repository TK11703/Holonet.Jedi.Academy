
function AdjustSizesBeforeDraw(chart) {
    var width = chart.width,
        height = chart.height,
        ctx = chart.ctx;
    ctx.restore();
    var fontSize = (height / 70).toFixed(2);
    ctx.font = fontSize + "em sans-serif";
    ctx.textBaseline = "middle";
    //var text = chart.config.data.datasets[0].data[0] + "%",
    //    textX = Math.round((width - ctx.measureText(text).width) / 2),
    //    textY = height / 2;
    //ctx.fillText(text, textX, textY);
    ctx.save();
}

function ShowLineGraph(canvasObj, enableDrillDown, chartResponse, tooltipLabelFunction, yAxisFormatFunction) {
    var config = {
        type: chartResponse.type,
        data: {
            labels: chartResponse.labels,
            datasets: chartResponse.datasets
        },
        plugins: [{
            beforeDraw: AdjustSizesBeforeDraw
        }],
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                title: {
                    display: true,
                    text: chartResponse.title,
                    padding: {
                        top: 10,
                        bottom: 10
                    }
                },
                tooltip: {
                    callbacks: {
                        label: tooltipLabelFunction
                    }
                }
            },
            tooltips: {
                mode: 'index',
                intersect: false,
            },
            hover: {
                mode: 'nearest',
                intersect: true
            },
            scales: {
                x: {
                    display: true,
                    title: {
                        display: true,
                        text: chartResponse.xAxisLabel
                    }
                },
                y: {
                    display: true,
                    title: {
                        display: true,
                        text: chartResponse.yAxisLabel
                    },
                    ticks: {
                        callback: yAxisFormatFunction
                    }
                }
            }
        }
    };

    var ctx = $(canvasObj).get(0).getContext('2d');
    if ($(canvasObj).first().data("maintainaspectratio") != null) {
        config.options.maintainAspectRatio = $(canvasObj).first().data("maintainaspectratio");
    }
    if ($(canvasObj).first().data("responsive") != null) {
        config.options.responsive = $(canvasObj).first().data("responsive");
    }
    var chartObj = new Chart(ctx, config);
    if (enableDrillDown) {
        $(canvasObj).get(0).onclick = function (evt) {
            var firstPoint = chartObj.getElementAtEvent(evt)[0];

            if (firstPoint) {
                var label = chartObj.data.labels[firstPoint._index];
                var datasetName = chartObj.data.datasets[firstPoint._datasetIndex].label;
                var value = chartObj.data.datasets[firstPoint._datasetIndex].data[firstPoint._index];
                var msg = "A chart interaction was captured, and the specific data targeted was:\nDataset: " + datasetName + "\n X Axis: " + label + "\n Y Axis: " + value;
                siteAlert(msg);
            }
        };
    }
    return chartObj;
}

function ShowPieChart(canvasObj, enableDrillDown, chartResponse, tooltipLabelFunction) {
    var config = {
        type: chartResponse.type,
        data: {
            labels: chartResponse.labels,
            datasets: chartResponse.datasets
        },
        plugins: [{
            beforeDraw: AdjustSizesBeforeDraw
        }],
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                title: {
                    display: true,
                    text: chartResponse.title,
                    padding: {
                        top: 10,
                        bottom: 10
                    }
                },
                legend: {
                    display: true,
                    position: 'top'
                    //labels: {
                    //    color: 'rgb(255, 99, 132)'
                    //}
                },
                tooltip: {
                    callbacks: {
                        label: tooltipLabelFunction
                    }
                }
            },
            animation: {
                animateScale: true,
                animateRotate: true
            },
            tooltips: {
                callbacks: {
                    label: function (item, data) {
                        console.log(data.labels, item);
                        return data.datasets[item.datasetIndex].label + ": " + data.labels[item.index] + ": " + data.datasets[item.datasetIndex].data[item.index];
                    }
                }
            }
        }
    };

    var ctx = $(canvasObj).get(0).getContext('2d');
    if ($(canvasObj).first().data("maintainaspectratio") != null) {
        config.options.maintainAspectRatio = $(canvasObj).first().data("maintainaspectratio");
    }
    if ($(canvasObj).first().data("responsive") != null) {
        config.options.responsive = $(canvasObj).first().data("responsive");
    }
    var chartObj = new Chart(ctx, config);
    if (enableDrillDown) {
        $(canvasObj).get(0).onclick = function (evt) {
            var firstPoint = chartObj.getElementAtEvent(evt)[0];

            if (firstPoint) {
                var label = chartObj.data.labels[firstPoint._index];
                var datasetName = chartObj.data.datasets[firstPoint._datasetIndex].label;
                var value = chartObj.data.datasets[firstPoint._datasetIndex].data[firstPoint._index];
                var msg = "A chart interaction was captured, and the specific data targeted was:\nDataset: " + datasetName + "\n Data Target: " + label + "\n Value: " + value;
                siteAlert(msg);
            }
        };
    }
    return chartObj;
}

function ShowRadarChart(canvasObj, enableDrillDown, chartResponse) {
    var config = {
        type: chartResponse.type,
        data: {
            labels: chartResponse.labels,
            datasets: chartResponse.datasets
        },
        plugins: [{
            beforeDraw: AdjustSizesBeforeDraw
        }],
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                title: {
                    display: true,
                    text: chartResponse.title,
                    padding: {
                        top: 10,
                        bottom: 10
                    }
                },
                legend: {
                    display: true,
                    position: 'top'
                    //labels: {
                    //    color: 'rgb(255, 99, 132)'
                    //}
                }
            },
            animation: {
                animateScale: true,
                animateRotate: true
            },
            tooltips: {
                callbacks: {
                    label: function (item, data) {
                        console.log(data.labels, item);
                        return data.datasets[item.datasetIndex].label + ": " + data.labels[item.index] + ": " + data.datasets[item.datasetIndex].data[item.index];
                    }
                }
            },
            elements: {
                "line":
                {
                    "tension": 0, "borderWidth": 3
                }
            }
        }
    };

    var ctx = $(canvasObj).get(0).getContext('2d');
    if ($(canvasObj).first().data("maintainaspectratio") != null) {
        config.options.maintainAspectRatio = $(canvasObj).first().data("maintainaspectratio");
    }
    if ($(canvasObj).first().data("responsive") != null) {
        config.options.responsive = $(canvasObj).first().data("responsive");
    }
    var chartObj = new Chart(ctx, config);
    if (enableDrillDown) {
        $(canvasObj).get(0).onclick = function (evt) {
            var firstPoint = chartObj.getElementAtEvent(evt)[0];

            if (firstPoint) {
                var label = chartObj.data.labels[firstPoint._index];
                var datasetName = chartObj.data.datasets[firstPoint._datasetIndex].label;
                var value = chartObj.data.datasets[firstPoint._datasetIndex].data[firstPoint._index];
                var msg = "A chart interaction was captured, and the specific data targeted was:\nDataset: " + datasetName + "\n Data Target: " + label + "\n Value: " + value;
                siteAlert(msg);
            }
        };
    }
    return chartObj;
}
