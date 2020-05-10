app.controller('stateCTRL', function ($scope, $timeout, coreService, stateService) {
    document.getElementById("template").style.visibility = "visible";

    $scope.initialize = function () {
        $('#bootstrap-data-table').DataTable().clear().destroy();
        coreService.showInd();
        stateService.getList()
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
        $scope.oState = {};
        $scope.oState.StateID = 0;
        $scope.oState.StateName = '';
        clearValidation();
    };

    $scope.add = function () {
        $scope.header = 'ADD';
        $scope.submitTxt = 'Confirm';
        $scope.reset()
    };

    $scope.edit = function (stateId) {

        $scope.header = 'EDIT';
        $scope.submitTxt = 'Update';
        $scope.reset();
       

        coreService.showInd();
        stateService.get(stateId)
        .then(function (response) {
            coreService.hideInd();
            $scope.oState = response.data;
        }, function (err) {
            coreService.hideInd();
            coreService.error(coreService.message.error);
        });
    };

    $scope.submit = function () {

        if ($scope.oState.StateID == 0) {
            coreService.showInd();
            stateService.add($scope.oState)
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
            stateService.update($scope.oState)
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
            stateService.delete($scope.oState)
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
   
});