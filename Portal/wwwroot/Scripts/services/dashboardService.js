app.service('dashboardService', function ($http) {

    this.stockRegister = function () {
        return $http.get('/Dashboard/StockRegister');

    };

    this.accontManager = function () {
        return $http.get('/Dashboard/AccontManager');

    };

    this.distributor = function (distributorId) {
        return $http.get('/Dashboard/Distributor?distributorId=' + distributorId);
    };

    this.main = function () {
        return $http.get('/Dashboard/MainDashboard');
    };
});