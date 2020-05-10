app.service('productService', function ($http) {

    this.getList = function (clientId) {
        return $http.get('/Product/GetList?clientId=' + clientId);

    };

    this.getClientTypeList = function (productId) {
        return $http.get('/Product/GetClientTypeList?productId=' + productId);

    };

    this.getLicenseList = function (clientId) {
        return $http.get('/License/GetProductList?clientId=' + clientId);

    };
});