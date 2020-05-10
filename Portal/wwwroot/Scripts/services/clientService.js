app.service('clientService', function ($http) {

    this.add = function (oClient) {
        return $http.post('/Client/Add', oClient);

    };

    this.update = function (oClient) {
        return $http.post('/Client/Update', oClient);

    };

    this.get = function (clientId) {
        return $http.get('/Client/Get?clientId=' + clientId);

    };

    this.getList = function () {
        return $http.get('/Client/GetList');

    };

    this.getClientTypeList = function () {
        return $http.get('/Client/GetClientTypeList');

    };

    this.existance = function (clientId) {
        return $http.get('/Client/Existance?clientId=' + clientId);

    };

    this.delete = function (oClient) {
        return $http.post('/Client/Delete', oClient);

    };

});