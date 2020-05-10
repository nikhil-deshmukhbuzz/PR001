app.service('distributorbillService', function ($http) {

    this.update = function (oDistributorBill) {
        return $http.post('/DistributorBill/Update', oDistributorBill);

    };

    this.get = function (distributorbillId) {
        return $http.get('/DistributorBill/Get?distributorbillId=' + distributorbillId);

    };

    this.getList = function () {
        return $http.get('/DistributorBill/GetList');

    };

    this.filterByDate = function (distributorId, month, year) {
        return $http.get('/DistributorBill/FilterByDate?distributorId=' + distributorId + '&month=' + month + '&year=' + year);

    };

});
