$(document).ready(function () {
    BindDriverRiderRideData();
    $("#currentlyRunningTableRow").hide();
    CompletedCancelledRidesGraph();
    EarningGraph();
    OrdersStatusPercentagePieGraph();
    DriverRatingGraph();
    DriverEarningGraph()
});


function BindDriverRiderRideData() {
    $.ajax({
        url: "/DashBoard/GetDriverRiderCounts",
        type: "Get",
        async: true, 
        success: function (data) {
            $("#totalCompletedRides").text(data.totalCompleteRides);
            $("#runningRides").text(data.runningRides);
            $("#totalRider").text(data.totalRiders);
            $("#totalDriver").text(data.totalDrivers);

        }
    });
}

function CompletedCancelledRidesGraph() {
    debugger;

    $.ajax({
        url: "/DashBoard/GetMonthlyCompletedCanceledRides",
        type: "Get",
        async: true,
        success: function (record) {
            if (record.data != null && record.data.length > 0) {
                // Assuming record.data is an array of objects
                var dates = [];
                var completedOrders = [];
                var canceledOrders = [];
                $.each(record.data, function (index, orderData) {
                    if ($.inArray(orderData.date, dates) === -1) {
                        dates.push(orderData.date);
                        completedOrders.push(0); // Initialize with 0
                        canceledOrders.push(0); // Initialize with 0
                    }
                    if (orderData.status === 'COMPLETED') {
                        completedOrders[$.inArray(orderData.date, dates)] = orderData.count;
                    } else if (orderData.status === 'CANCELLED') {
                        canceledOrders[$.inArray(orderData.date, dates)] = orderData.count;
                    }
                });
                var ctx = $('#columnChart');
                var columnChart = new Chart(ctx, {
                    type: 'bar',
                    data: {
                        labels: dates,
                        datasets: [{
                            label: 'Completed Orders',
                            data: completedOrders,
                            backgroundColor: '#0fa949',
                            borderColor: 'rgba(0, 123, 255, 1.5)',
                            borderWidth: 1
                        }, {
                            label: 'Canceled Orders',
                            data: canceledOrders,
                            backgroundColor: 'rgba(255, 0, 0, 0.75)',
                            borderColor: 'rgba(255, 0, 0, 1)',
                            borderWidth: 1
                        }]
                    },
                    options: {
                        scales: {
                            y: {
                                beginAtZero: true
                            }
                        }
                    }
                });

            }
        }
    });
}
function EarningGraph() {
    $.ajax({
        url: "/DashBoard/TotalEarningsDetails",
        type: "Get",
        async: true,
        success: function (record) {
            debugger;
            var data = [];

            $.each(record, function (index, item) {
              
                var newEntry = {
                    "date": item.newDate,
                    "value": parseInt(item.totalAmount,10)
                };
                data.push(newEntry);
            });
            debugger;

            am5.ready(function () {
                // Create root element
                var root = am5.Root.new("chartdiv");

                const myTheme = am5.Theme.new(root);

                // Move minor label a bit down
                myTheme.rule("AxisLabel", ["minor"]).setAll({
                    dy: 1
                });
                // Tweak minor grid opacity
                myTheme.rule("Grid", ["minor"]).setAll({
                    strokeOpacity: 0.08
                });

                // Set themes

                root.setThemes([
                    am5themes_Animated.new(root),
                    myTheme
                ]);
                // Create chart

                var chart = root.container.children.push(am5xy.XYChart.new(root, {
                    panX: false,
                    panY: false,
                    wheelX: "panX",
                    wheelY: "zoomX",
                    paddingLeft: 0
                }));


                // Add cursor
                // https://www.amcharts.com/docs/v5/charts/xy-chart/cursor/
                var cursor = chart.set("cursor", am5xy.XYCursor.new(root, {
                    behavior: "zoomX"
                }));
                cursor.lineY.set("visible", false);


                // Convert date strings to Date objects
                var data2 = data.map(function (item) {
                    var dateParts = item.date.split('/');
                    var date = new Date(+dateParts[2], dateParts[0] - 1, +dateParts[1]);
                    return { date: date.getTime(), value: item.value };
                });

                // Set data
                var xAxis = chart.xAxes.push(am5xy.DateAxis.new(root, {
                    maxDeviation: 0,
                    baseInterval: {
                        timeUnit: "day",
                        count: 1
                    },
                    renderer: am5xy.AxisRendererX.new(root, {
                        minorGridEnabled: true,
                        minGridDistance: 200,
                        minorLabelsEnabled: true
                    }),
                    tooltip: am5.Tooltip.new(root, {})
                }));

                xAxis.set("minorDateFormats", {
                    day: "dd",
                    month: "MM"
                });

                var yAxis = chart.yAxes.push(am5xy.ValueAxis.new(root, {
                    renderer: am5xy.AxisRendererY.new(root, {})
                }));


                // Add series
                // https://www.amcharts.com/docs/v5/charts/xy-chart/series/
                var series = chart.series.push(am5xy.LineSeries.new(root, {
                    name: "Series",
                    xAxis: xAxis,
                    yAxis: yAxis,
                    valueYField: "value",
                    valueXField: "date",
                    tooltip: am5.Tooltip.new(root, {
                        labelText: "{valueY}"
                    })
                }));

                // Actual bullet
                series.bullets.push(function () {
                    var bulletCircle = am5.Circle.new(root, {
                        radius: 5,
                        fill: series.get("fill")
                    });
                    return am5.Bullet.new(root, {
                        sprite: bulletCircle
                    })
                })

                // Add scrollbar
                // https://www.amcharts.com/docs/v5/charts/xy-chart/scrollbars/
                chart.set("scrollbarX", am5.Scrollbar.new(root, {
                    orientation: "horizontal"
                }));


                series.data.setAll(data2);


                // Make stuff animate on load
                // https://www.amcharts.com/docs/v5/concepts/animations/
                series.appear(1000);
                chart.appear(1000, 100);

            });
        }
    });
}
function OrdersStatusPercentagePieGraph() {
    $.ajax({
        url: "/DashBoard/GetOrdersPercentageWithStatus",
        type: "Get",
        async: true,
        success: function (record) {
            
            var data = [];

            $.each(record, function (index, item) {
                var newEntry = {
                    "value": item.orderPercentage,
                    "category": item.status
                };
                data.push(newEntry);
            });
            am5.ready(function () {

                var root = am5.Root.new("pieChart");
                root.setThemes([
                    am5themes_Animated.new(root)
                ]);
                var chart = root.container.children.push(am5percent.PieChart.new(root, {
                    layout: root.verticalLayout,
                    innerRadius: am5.percent(50)
                }));
                var series = chart.series.push(am5percent.PieSeries.new(root, {
                    valueField: "value",
                    categoryField: "category",
                    alignLabels: false
                }));
                series.labels.template.setAll({
                    visible: false, // This will hide the labels
                    textType: "circular",
                    centerX: 0,
                    centerY: 0
                });


                series.data.setAll(data);
                var legend = chart.children.push(am5.Legend.new(root, {
                    centerX: am5.percent(50),
                    x: am5.percent(50),
                    marginTop: 15,
                    marginBottom: 15,
                }));
                legend.data.setAll(series.dataItems);
                series.appear(1000, 100);

            });

        }
    });
}

function DriverRatingGraph() {
    $.ajax({
        url: "/DashBoard/Top10RatedDrivers",
        type: "Get",
        async: true,
        success: function (record) {

            am5.ready(function () {


                // Create root element
                // https://www.amcharts.com/docs/v5/getting-started/#Root_element
                var root = am5.Root.new("driversRating");


                // Set themes
                // https://www.amcharts.com/docs/v5/concepts/themes/
                root.setThemes([
                    am5themes_Animated.new(root)
                ]);


                // Create chart
                // https://www.amcharts.com/docs/v5/charts/xy-chart/
                var chart = root.container.children.push(am5xy.XYChart.new(root, {
                    panX: true,
                    panY: true,
                    wheelX: "panX",
                    wheelY: "zoomX",
                    pinchZoomX: true,
                    paddingLeft: 0,
                    paddingRight:1
                   // layout: root.verticalLayout
                }));

                var colors = chart.get("colors");

              
                var data = record.map(function (item) {
                    return {
                        country: item.driverName,
                        visits: item.rating,
                        icon: item.profilePhoto,
                        columnSettings: { fill: colors.next() }
                    };
                });
                var xRenderer = am5xy.AxisRendererX.new(root, {
                    minGridDistance: 30,
                    minorGridEnabled: true
                })

                var xAxis = chart.xAxes.push(am5xy.CategoryAxis.new(root, {
                    categoryField: "country",
                    renderer: xRenderer,
                    bullet: function (root, axis, dataItem) {
                        return am5xy.AxisBullet.new(root, {
                            location: 0.5,
                            sprite: am5.Picture.new(root, {
                                width: 24,
                                height: 24,
                                centerY: am5.p50,
                                centerX: am5.p50,
                                src: dataItem.dataContext.icon
                            })
                        });
                    }
                }));
                var yRenderer = am5xy.AxisRendererY.new(root, {
                    strokeOpacity: 0.1
                })
                xRenderer.grid.template.setAll({
                    location: 1
                })

                xRenderer.labels.template.setAll({
                    rotation: -405,
                    centerY: am5.p50,
                    centerX: am5.p100,
                    paddingRight: 15
                });

                xAxis.data.setAll(data);

                var yAxis = chart.yAxes.push(am5xy.ValueAxis.new(root, {
                    maxDeviation: 0.3,
                    renderer: yRenderer
                   
                }));


                // Add series
                // https://www.amcharts.com/docs/v5/charts/xy-chart/series/
                var series = chart.series.push(am5xy.ColumnSeries.new(root, {
                    xAxis: xAxis,
                    yAxis: yAxis,
                    valueYField: "visits",
                    categoryXField: "country"
                }));

                series.columns.template.setAll({
                    tooltipText: "{categoryX}: {valueY}",
                    tooltipY: 0,
                    strokeOpacity: 0,
                    templateField: "columnSettings"
                });

                series.data.setAll(data);


                // Make stuff animate on load
                // https://www.amcharts.com/docs/v5/concepts/animations/
                series.appear();
                chart.appear(1000, 100);

            }); // end am5.ready() 





        }
    });
}

function bindData(data) {
    
    let formattedData = data.map(item => {
        return {
            name: item.driverName,
            steps: item.rating,
            pictureSettings: {
                src: item.profilePhoto
            }
        };
    });
    return formattedData;
}

function DriverEarningGraph() {
    $.ajax({
        url: "/DashBoard/Top5EarnedDrivers",
        type: "Get",
        async: true,
        success: function (record) {

            am5.ready(function () {

                // Create root element
                // https://www.amcharts.com/docs/v5/getting-started/#Root_element
                var root = am5.Root.new("driversEarning");

                // Set themes
                // https://www.amcharts.com/docs/v5/concepts/themes/
                root.setThemes([
                    am5themes_Animated.new(root)
                ]);

                // Create chart
                // https://www.amcharts.com/docs/v5/charts/xy-chart/
                var chart = root.container.children.push(am5xy.XYChart.new(root, {
                    panX: true,
                    panY: true,
                    wheelX: "panX",
                    wheelY: "zoomX",
                    pinchZoomX: true,
                    paddingLeft: 0,
                    paddingRight: 1
                }));

                // Add cursor
                // https://www.amcharts.com/docs/v5/charts/xy-chart/cursor/
                var cursor = chart.set("cursor", am5xy.XYCursor.new(root, {}));
                cursor.lineY.set("visible", false);


                // Create axes
                // https://www.amcharts.com/docs/v5/charts/xy-chart/axes/
                var xRenderer = am5xy.AxisRendererX.new(root, {
                    minGridDistance: 30,
                    minorGridEnabled: true
                });

                xRenderer.labels.template.setAll({
                    rotation: -45,
                    centerY: am5.p50,
                    centerX: am5.p100,
                    paddingRight: 15
                });

                xRenderer.grid.template.setAll({
                    location: 1
                })

                var xAxis = chart.xAxes.push(am5xy.CategoryAxis.new(root, {
                    maxDeviation: 0.3,
                    categoryField: "country",
                    renderer: xRenderer,
                    tooltip: am5.Tooltip.new(root, {})
                }));

                var yRenderer = am5xy.AxisRendererY.new(root, {
                    strokeOpacity: 0.1
                })

                var yAxis = chart.yAxes.push(am5xy.ValueAxis.new(root, {
                    maxDeviation: 0.3,
                    renderer: yRenderer
                }));

                // Create series
                // https://www.amcharts.com/docs/v5/charts/xy-chart/series/
                var series = chart.series.push(am5xy.ColumnSeries.new(root, {
                    name: "Series 1",
                    xAxis: xAxis,
                    yAxis: yAxis,
                    valueYField: "value",
                    sequencedInterpolation: true,
                    categoryXField: "country",
                    tooltip: am5.Tooltip.new(root, {
                        labelText: "{valueY}"
                    })
                }));

                series.columns.template.setAll({ cornerRadiusTL: 5, cornerRadiusTR: 5, strokeOpacity: 0 });
                series.columns.template.adapters.add("fill", function (fill, target) {
                    return chart.get("colors").getIndex(series.columns.indexOf(target));
                });

                series.columns.template.adapters.add("stroke", function (stroke, target) {
                    return chart.get("colors").getIndex(series.columns.indexOf(target));
                });
                var data = record.map(function (item) {
                    return {
                        country: item.driverName,
                        value: item.totalEarnings,
                    };
                });
                // Set data
                //var data = [{
                //    country: "USA",
                //    value: 2025
                //}, {
                //    country: "China",
                //    value: 1882
                //}, {
                //    country: "Japan",
                //    value: 1809
                //}, {
                //    country: "Germany",
                //    value: 1322
                //}, {
                //    country: "UK",
                //    value: 1122
                //}, {
                //    country: "France",
                //    value: 1114
                //}, {
                //    country: "India",
                //    value: 984
                //}, {
                //    country: "Spain",
                //    value: 711
                //}, {
                //    country: "Netherlands",
                //    value: 665
                //}, {
                //    country: "South Korea",
                //    value: 443
                //}, {
                //    country: "Canada",
                //    value: 441
                //}];

                xAxis.data.setAll(data);
                series.data.setAll(data);


                // Make stuff animate on load
                // https://www.amcharts.com/docs/v5/concepts/animations/
                series.appear(1000);
                chart.appear(1000, 100);



            })

            }
            
    });

}
