app.service('licenseService', function ($http) {

    this.add = function (listOfLicense) {
        return $http.post('/License/Add', listOfLicense);

    };

    this.update = function (listOfLicense) {
        return $http.post('/License/Update', listOfLicense);

    };

    this.get = function (licenseId) {
        return $http.get('/License/Get?licenseId=' + licenseId);

    };

    this.getList = function () {
        return $http.get('/License/GetList');

    };

    this.getProductList = function (clientId) {
        return $http.get('/License/GetProductList?clientId=' + clientId);

    };
});