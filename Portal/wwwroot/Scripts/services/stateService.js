app.service('stateService', function ($http) {

    this.add = function (oState) {
        return $http.post('/State/Add', oState);

    };

    this.update = function (oState) {
        return $http.post('/State/Update', oState);

    };

    this.getList = function () {
        return $http.get('/State/GetList');

    };

    this.get = function (stateId) {
        return $http.get('/State/Get?stateId=' + stateId);
    };
});