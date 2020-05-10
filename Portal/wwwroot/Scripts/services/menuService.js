app.service('menuService', function ($http) {

    this.add = function (oDeviceMenu) {
        return $http.post('/Menu/AddDeviceMenu', oDeviceMenu);

    };

    this.configSave = function (_listOfDeviceMenu) {
        return $http.post('/Menu/AddDeviceMenuLink', _listOfDeviceMenu);

    };

    this.update = function (oDeviceMenu) {
        return $http.post('/Menu/UpdateDeviceMenu', oDeviceMenu);

    };

    this.getList = function (clientId) {
        return $http.get('/Menu/GetList?clientId=' + clientId);

    };

    this.getInstallationList = function (clientId) {
        return $http.get('/Menu/GetInstallationList?clientId=' + clientId);

    };

    this.get = function (deviceMenuId) {
        return $http.get('/Menu/Get?deviceMenuId=' + deviceMenuId);
    };

    this.getMenu = function (deviceMenuId) {
        return $http.get('/Menu/GetMenu?deviceMenuId=' + deviceMenuId);
    };

    this.getDeviceMenuLink = function (deviceMenuId) {
        return $http.get('/Menu/GetDeviceMenuLink?deviceMenuId=' + deviceMenuId);
    };

    this.save = function (oDeviceMenu) {
        return $http.post('/Menu/AddMenu', oDeviceMenu);
    };

    this.getClientList = function () {
        return $http.get('/Menu/GetClientList');

    };
});