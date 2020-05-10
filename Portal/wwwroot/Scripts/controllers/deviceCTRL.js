app.controller('deviceCTRL', function ($scope, $timeout, coreService, deviceService) {
    document.getElementById("template").style.visibility = "visible";

    $scope.initialize = function () {
        $('#bootstrap-data-table').DataTable().clear().destroy();
        coreService.showInd();
        deviceService.getList()
        .then(function (response) {
            $scope.data  = response.data;
            isTableLoaded = true;
            coreService.hideInd();
        }, function (err) {
            coreService.hideInd();
            coreService.error(coreService.message.error);
        });
    };

    $scope.masters = function () {

    };

    var clearValidation = function () {
        $scope.form.$setPristine();
    };

    $scope.reset = function () {
        $scope.oDevice = {};
        $scope.oDevice.DeviceID = 0;
        $scope.oDevice.DeviceName = '';
        $scope.oDevice.Stock = '';
        $scope.isDelete = false;
        clearValidation();
    };

    $scope.add = function () {
        $scope.header = 'ADD';
        $scope.submitTxt = 'Confirm';
        $scope.reset()
    };

    $scope.edit = function (deviceId) {

        $scope.header = 'EDIT';
        $scope.submitTxt = 'Update';
        $scope.reset();
        existance(deviceId);

        coreService.showInd();
        deviceService.get(deviceId)
        .then(function (response) {
            coreService.hideInd();
            $scope.oDevice = response.data;


        }, function (err) {
            coreService.hideInd();
            coreService.error(coreService.message.error);
        });
    };

    $scope.submit = function () {
        
        if ($scope.oDevice.DeviceID == 0) {
            coreService.showInd();
            deviceService.add($scope.oDevice)
            .then(function (response) {
                if (response.data == 1) {
                    coreService.success(coreService.message.added);
                }
                else {
                    coreService.error(coreService.message.error);
                }

                $scope.initialize();
            }, function (err) {
                coreService.hideInd();
                coreService.error(coreService.message.error);
            });
        }
        else {
            coreService.showInd();
            deviceService.update($scope.oDevice)
            .then(function (response) {
                coreService.hideInd();
                if (response.data == true) {
                    coreService.success(coreService.message.updated);
                }
                else {
                    coreService.error(coreService.message.error);
                }

                $scope.initialize();
            }, function (err) {
                coreService.hideInd();
                coreService.error(coreService.message.error);
            });
        }
    };

    $scope.delete = function () {

        if (confirm("Are you sure to delete this record?")) {
            coreService.showInd();
            deviceService.delete($scope.oDevice)
            .then(function (response) {
                coreService.hideInd();
                if (response.data)
                    coreService.success(coreService.message.deleted);
                else
                    coreService.error(coreService.message.error);

                $scope.initialize();

            }, function (err) {
                coreService.hideInd();
                coreService.error(coreService.message.error);
            });
        } else {

        }
    };
    var existance = function (deviceId) {

        coreService.showInd();
        deviceService.existance(deviceId)
        .then(function (response) {
            coreService.hideInd();
            $scope.isDelete = response.data ? false : true;
        }, function (err) {
            coreService.hideInd();
        });
    };
});