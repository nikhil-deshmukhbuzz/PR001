app.service('orderService', function ($http) {

    this.add = function (oOrder) {
        return $http.post('/Order/Add', oOrder);

    };

    this.update = function (oOrder) {
        return $http.post('/Order/Update', oOrder);

    };

    this.getList = function () {
        return $http.get('/Order/GetList');

    };

    this.getListByDistributor = function (distributorId) {
        return $http.get('/Order/GetListByDistributor?distributorId=' + distributorId);

    };

    this.getOrderStatusList = function () {
        return $http.get('/Order/GetOrderStatusList');

    };

    this.get = function (orderId) {
        return $http.get('/Order/Get?orderId=' + orderId);

    };

    this.existance = function (orderId) {
        return $http.get('/Order/Existance?orderId=' + orderId);

    };

    this.delete = function (oOrder) {
        return $http.post('/Order/Delete', oOrder);

    };
});