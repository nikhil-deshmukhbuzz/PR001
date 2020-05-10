app.service('districtService', function ($http) {

    this.add = function (oDistrict) {
        return $http.post('/District/Add', oDistrict);

    };

    this.update = function (oDistrict) {
        return $http.post('/District/Update', oDistrict);

    };

    this.getList = function () {
        return $http.get('/District/GetList');

    };

    this.getDistrictList = function (stateId) {
        return $http.get('/District/GetStateWiseList?stateId=' + stateId);
    };

    this.get = function (districtId) {
        return $http.get('/District/Get?districtId=' + districtId);
    };
});