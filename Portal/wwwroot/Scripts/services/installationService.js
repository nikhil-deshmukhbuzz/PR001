app.service('installationService', function ($http) {

    this.add = function (oInstallation) {
        return $http.post('/Installation/Add', oInstallation);

    };

    this.update = function (oInstallation) {
        return $http.post('/Installation/Update', oInstallation);

    };

    this.getList = function (lUserId) {
        return $http.get('/Installation/GetList?lUserId=' + lUserId);
  
    };

    this.get = function (installationId) {
        return $http.get('/Installation/Get?installationId=' + installationId);

    };

    this.delete = function (oInstallation) {
        return $http.post('/Installation/Delete', oInstallation);

    };
});