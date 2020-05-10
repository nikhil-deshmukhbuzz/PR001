app.service('distributorService', function ($http) {

    this.add = function (oDistributor) {
        return $http.post('/Distributor/Add', oDistributor);

    };

    this.update = function (oDistributor) {
        return $http.post('/Distributor/Update', oDistributor);

    };

    this.get = function (distributorId) {
        return $http.get('/Distributor/Get?distributorId=' + distributorId);

    };

    this.getList = function () {
        return $http.get('/Distributor/GetList');

    };

    this.existance = function (distributorId) {
        return $http.get('/Distributor/Existance?distributorId=' + distributorId);

    };

    this.delete = function (oDistributor) {
        return $http.post('/Distributor/Delete', oDistributor);

    };

});