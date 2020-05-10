app.service('spareService', function ($http) {

    this.add = function (oSpare) {
        return $http.post('/Spare/Add', oSpare);

    };

    this.update = function (oSpare) {
        return $http.post('/Spare/Update', oSpare);

    };

    this.getList = function () {
        return $http.get('/Spare/GetList');

    };

    this.existance = function (spareId) {
        return $http.get('/Spare/Existance?spareId=' + spareId);

    };

    this.get = function (spareId) {
        return $http.get('/Spare/Get?spareId=' + spareId);

    };

    this.delete = function (oSpare) {
        return $http.post('/Spare/Delete', oSpare);

    };
});