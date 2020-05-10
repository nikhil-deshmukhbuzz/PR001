app.service('deviceService', function ($http) {

    this.add = function (oDevice) {
        return $http.post('/Device/Add', oDevice);

    };

    this.update = function (oDevice) {
        return $http.post('/Device/Update', oDevice);

    };

    this.getList = function () {
        return $http.get('/Device/GetList');

    };

    this.get = function (deviceId) {
        return $http.get('/Device/Get?deviceId=' + deviceId);

    };

    this.existance = function (deviceId) {
        return $http.get('/Device/Existance?deviceId=' + deviceId);

    };

    this.delete = function (odevice) {
        return $http.post('/Device/Delete', odevice);

    };
});