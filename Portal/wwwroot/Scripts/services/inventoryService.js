app.service('inventoryService', function ($http) {

    this.add = function (oInventory) {
        return $http.post('/Inventory/Add', oInventory);

    };

    this.update = function (oInventory) {
        return $http.post('/Inventory/Update', oInventory);

    };

    this.getList = function () {
        return $http.get('/Inventory/GetList');

    };

    this.existance = function (inventoryId) {
        return $http.get('/Inventory/Existance?inventoryId=' + inventoryId);

    };

    this.get = function (inventoryId) {
        return $http.get('/Inventory/Get?inventoryId=' + inventoryId);

    };

    this.delete = function (oInventory) {
        return $http.post('/Inventory/Delete', oInventory);

    };

    this.getHardwareList = function () {
        return $http.get('/Inventory/GetHardwareList');

    };

    this.getDeviceSpareList = function () {
        return $http.get('/Inventory/GetDeviceSpareList');

    };
});