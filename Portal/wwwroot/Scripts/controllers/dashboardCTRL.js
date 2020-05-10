
app.controller('dashboardCTRL', function ($scope, coreService, dashboardService) {
    document.getElementById("dashboard").style.visibility = "visible";
    var profileName = coreService.getUser().ProfileMaster.ProfileName;
    var distributorID = coreService.getUser().DistributorID;

    $scope.isMainDashboard = false;
    $scope.isStockRegister = false;
    $scope.isAccountManger = false;
    $scope.isDistributor = false;
    $scope.stockRegister = {};
    $scope.accountManager = {};
    $scope.distributor = {};
    $scope.main = {};

    $scope.initialize = function () {
        if (profileName == 'Superadmin' || profileName == 'Administrator') {
            main();
        }
        else if (profileName == 'Inventory Supervisor') {
            stockRegister();
        }
        else if (profileName == 'Account Manager') {
            accountManager();
        }
        else if (profileName == 'Distributor') {
            distributor();
        }
    };

    var stockRegister = function () {
        dashboardService.stockRegister()
        .then(function (response) {
            $scope.isStockRegister = true;
            $scope.stockRegister = response.data;
        }, function (err) {

        });
    };

    var accountManager = function () {
        dashboardService.accontManager()
        .then(function (response) {
            $scope.isAccountManger = true;
            $scope.accountManager = response.data;
        }, function (err) {

        });
    };

    var distributor = function () {
        dashboardService.distributor(distributorID)
      .then(function (response) {
          $scope.isDistributor = true;
          $scope.distributor = response.data;
      }, function (err) {

      });
    };

    var main = function () {
        dashboardService.main()
      .then(function (response) {
          $scope.isMainDashboard = true;
          $scope.main = response.data;

          var month = [];
          var count = [];
          for (var i = 0; i < $scope.main.OrdersChart.length; i++) {
              month.push($scope.main.OrdersChart[i].sMonth + ' ' + $scope.main.OrdersChart[i].iYear.toString());
              count.push($scope.main.OrdersChart[i].Count);
          }
          orderChart(month, count);

           month = [];
           count = [];
           for (var i = 0; i < $scope.main.DispatchChart.length; i++) {
               month.push($scope.main.DispatchChart[i].sMonth + ' ' + $scope.main.DispatchChart[i].iYear.toString());
               count.push($scope.main.DispatchChart[i].Count);
          }
           dispatchChart(month, count);
           callLogChart(month, count);

           month = [];
           count = [];
           for (var i = 0; i < $scope.main.PaymentDuesChart.length; i++) {
               month.push($scope.main.PaymentDuesChart[i].sMonth + ' ' + $scope.main.PaymentDuesChart[i].iYear.toString());
               count.push($scope.main.PaymentDuesChart[i].Count);
           }
           paymentChart(month, count);
           
           //month = [];
           //count = [];
           //for (var i = 0; i < $scope.main.CallLogsChart.length; i++) {
           //    month.push($scope.main.CallLogsChart[i].sMonth + ' ' + $scope.main.CallLogsChart[i].iYear.toString());
           //    count.push($scope.main.CallLogsChart[i].Count);
           //}
           //callLogChart(month, count);


          
      }, function (err) {

      });
    };

    var orderChart = function (month,count) {
        //WidgetChart 1
        var ctx = document.getElementById("widgetChart1");
        ctx.height = 150;
        var myChart = new Chart(ctx, {
            type: 'line',
            data: {
                labels: month, //['January', 'February', 'March', 'April', 'May', 'June', 'July'],
                type: 'line',
                datasets: [{
                    data: count, //[65, 59, 84, 84, 51, 55, 40],
                    label: 'Order',
                    backgroundColor: 'transparent',
                    borderColor: 'rgba(255,255,255,.55)',
                }, ]
            },
            options: {

                maintainAspectRatio: false,
                legend: {
                    display: false
                },
                responsive: true,
                tooltips: {
                    mode: 'index',
                    titleFontSize: 12,
                    titleFontColor: '#000',
                    bodyFontColor: '#000',
                    backgroundColor: '#fff',
                    titleFontFamily: 'Montserrat',
                    bodyFontFamily: 'Montserrat',
                    cornerRadius: 3,
                    intersect: false,
                },
                scales: {
                    xAxes: [{
                        gridLines: {
                            color: 'transparent',
                            zeroLineColor: 'transparent'
                        },
                        ticks: {
                            fontSize: 2,
                            fontColor: 'transparent'
                        }
                    }],
                    yAxes: [{
                        display: false,
                        ticks: {
                            display: false,
                        }
                    }]
                },
                title: {
                    display: false,
                },
                elements: {
                    line: {
                        borderWidth: 1
                    },
                    point: {
                        radius: 4,
                        hitRadius: 10,
                        hoverRadius: 4
                    }
                }
            }
        });
    };
    var dispatchChart = function (month, count) {
        //WidgetChart 2
        var ctx = document.getElementById("widgetChart2");
        ctx.height = 150;
        var myChart = new Chart(ctx, {
            type: 'line',
            data: {
                labels: month,//['January', 'February', 'March', 'April', 'May', 'June', 'July'],
                type: 'line',
                datasets: [{
                    data: count,//[1, 18, 9, 17, 34, 22, 11],
                    label: 'Dispatched',
                    backgroundColor: '#63c2de',
                    borderColor: 'rgba(255,255,255,.55)',
                }, ]
            },
            options: {

                maintainAspectRatio: false,
                legend: {
                    display: false
                },
                responsive: true,
                tooltips: {
                    mode: 'index',
                    titleFontSize: 12,
                    titleFontColor: '#000',
                    bodyFontColor: '#000',
                    backgroundColor: '#fff',
                    titleFontFamily: 'Montserrat',
                    bodyFontFamily: 'Montserrat',
                    cornerRadius: 3,
                    intersect: false,
                },
                scales: {
                    xAxes: [{
                        gridLines: {
                            color: 'transparent',
                            zeroLineColor: 'transparent'
                        },
                        ticks: {
                            fontSize: 2,
                            fontColor: 'transparent'
                        }
                    }],
                    yAxes: [{
                        display: false,
                        ticks: {
                            display: false,
                        }
                    }]
                },
                title: {
                    display: false,
                },
                elements: {
                    line: {
                        tension: 0.00001,
                        borderWidth: 1
                    },
                    point: {
                        radius: 4,
                        hitRadius: 10,
                        hoverRadius: 4
                    }
                }
            }
        });
    };
    var paymentChart = function (month, count) {
        var ctx = document.getElementById("widgetChart3");
        ctx.height = 70;
        var myChart = new Chart(ctx, {
            type: 'line',
            data: {
                labels: month,//['Jan', 'Feb', 'March', 'April', 'May', 'Jun', 'Jul', 'Aug', 'Sept', 'Oct', 'Nov', 'Dec'],
                type: 'line',
                datasets: [{
                    data: count,//[78, 81, 80, 45, 34, 12, 40, 78, 81, 80, 45, 34, 12],
                    label: 'Dues',
                    backgroundColor: 'rgba(255,255,255,.2)',
                    borderColor: 'rgba(255,255,255,.55)',
                }, ]
            },
            options: {

                maintainAspectRatio: true,
                legend: {
                    display: false
                },
                responsive: true,
                 tooltips: {
                     mode: 'index',
                     titleFontSize: 12,
                     titleFontColor: '#000',
                     bodyFontColor: '#000',
                     backgroundColor: '#fff',
                     titleFontFamily: 'Montserrat',
                     bodyFontFamily: 'Montserrat',
                     cornerRadius: 3,
                     intersect: false,
                 },
                scales: {
                    xAxes: [{
                        gridLines: {
                            color: 'transparent',
                            zeroLineColor: 'transparent'
                        },
                        ticks: {
                            fontSize: 2,
                            fontColor: 'transparent'
                        }
                    }],
                    yAxes: [{
                        display: false,
                        ticks: {
                            display: false,
                        }
                    }]
                },
                title: {
                    display: false,
                },
                elements: {
                    line: {
                        borderWidth: 2
                    },
                    point: {
                        radius: 0,
                        hitRadius: 10,
                        hoverRadius: 4
                    }
                }
            }
        });
    };
    var callLogChart = function (month, count) {
        //WidgetChart 4
        var ctx = document.getElementById("widgetChart4");
        ctx.height = 70;
        var myChart = new Chart(ctx, {
            type: 'bar',
            data: {
                labels: month,//['', '', '', '', '', '', '', '', '', '', '', '', '', '', '', ''],
                datasets: [
                    {
                        label: "Call log",
                        data: count,//[78, 81, 80, 45, 34, 12, 40, 75, 34, 89, 32, 68, 54, 72, 18, 98],
                        borderColor: "rgba(0, 123, 255, 0.9)",
                        //borderWidth: "0",
                        backgroundColor: "rgba(255,255,255,.3)"
                    }
                ]
            },
            options: {
                maintainAspectRatio: true,
                legend: {
                    display: false
                },
                scales: {
                    xAxes: [{
                        display: false,
                        categoryPercentage: 1,
                        barPercentage: 0.5
                    }],
                    yAxes: [{
                        display: false
                    }]
                }
            }
        });

    };

});