app.service('themeService', function ($http) {

    this.getToday = function (installationId) {
        return $http.get('/Theme/GetToday?installationId=' + installationId);

    };

    this.getWeekend = function (installationId) {
        return $http.get('/Theme/GetWeekend?installationId=' + installationId);

    };

    this.getYear = function (installationId, year) {
        return $http.get('/Theme/GetYear?installationId=' + installationId + '&year=' + year);

    };

    this.getDayList = function () {
        return $http.get('/Theme/GetDayList');

    };

    this.updateDeviceWeekendTheme = function (oDeviceWeekendTheme) {
        return $http.post('/Theme/UpdateDeviceWeekendTheme', oDeviceWeekendTheme);

    };

    this.updateDeviceTheme = function (oDeviceTheme) {
        return $http.post('/Theme/UpdateDeviceTheme', oDeviceTheme);

    };
});