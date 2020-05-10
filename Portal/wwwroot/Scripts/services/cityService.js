app.service('cityService', function ($http) {

    this.add = function (oCity) {
        return $http.post('/City/Add', oCity);

    };

    this.update = function (oCity) {
        return $http.post('/City/Update', oCity);

    };

    this.getList = function (districtId) {
        return $http.get('/City/GetList?districtId=' + districtId);
    };

    this.getAllList = function () {
        return $http.get('/City/GetAllList');
    };

    this.get = function (cityId) {
        return $http.get('/City/Get?cityId=' + cityId);
    };
});