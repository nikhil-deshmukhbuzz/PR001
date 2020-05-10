app.controller('spareCTRL', function ($scope, $timeout, coreService, spareService) {

    document.getElementById("template").style.visibility = "visible";

    $scope.initialize = function () {
        $('#bootstrap-data-table').DataTable().clear().destroy();
        coreService.showInd();
        spareService.getList()
        .then(function (response) {
            $scope.data = response.data;
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
        $scope.oSpare = {};
        $scope.oSpare.SpareID = 0;
        $scope.oSpare.SpareName = '';
        $scope.oSpare.Stock = '';
        $scope.isDelete = false;
        clearValidation();
    };

    $scope.add = function () {
        $scope.header = 'ADD';
        $scope.submitTxt = 'Confirm';
        $scope.reset()
    };

    $scope.edit = function (spareId) {

        $scope.header = 'EDIT';
        $scope.submitTxt = 'Update';
        $scope.reset();
        existance(spareId);

        coreService.showInd();
        spareService.get(spareId)
        .then(function (response) {
            coreService.hideInd();
            $scope.oSpare = response.data;


        }, function (err) {
            coreService.hideInd();
            coreService.error(coreService.message.error);
        });
    };

    $scope.submit = function () {

        if ($scope.oSpare.SpareID == 0) {
            coreService.showInd();
            spareService.add($scope.oSpare)
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
            spareService.update($scope.oSpare)
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
            spareService.delete($scope.oSpare)
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
    var existance = function (spareId) {

        coreService.showInd();
        spareService.existance(spareId)
        .then(function (response) {
            coreService.hideInd();
            $scope.isDelete = response.data ? false : true;
        }, function (err) {
            coreService.hideInd();
        });
    };
});