app.controller('userManagementCTRL', function ($scope, $timeout, coreService, userManagementService) {
    document.getElementById("template").style.visibility = "visible";

    $scope.initialize = function () {
        $('#bootstrap-data-table').DataTable().clear().destroy();
        coreService.showInd();
        userManagementService.getList()
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
        $scope.oUser = {};
        $scope.oUser.UserID = 0;
        clearValidation();
    };

    $scope.add = function () {
        $scope.header = 'ADD';
        $scope.submitTxt = 'Confirm';
        $scope.isEditable = false;
        $scope.reset()
    };

    $scope.edit = function (userId) {

        $scope.header = 'Change Password';
        $scope.submitTxt = 'Update';
        $scope.isEditable = true;
        $scope.reset();

        coreService.showInd();
        userManagementService.get(userId)
        .then(function (response) {
            coreService.hideInd();
            $scope.oUser = response.data;
            $scope.oUser.Password = '';

        }, function (err) {
            coreService.hideInd();
            coreService.error(coreService.message.error);
        });
    };

    $scope.submit = function () {
        coreService.showInd();
        userManagementService.updatePassword($scope.oUser)
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
    };

    $scope.changeStatus = function (userId, data) {
       
        var status = data ? 'Acivate' : 'Deacivate';

        if (confirm("Are you sure to " + status + " this user?")) {

            $scope.oUser = {};
            $scope.oUser.UserID = userId;
            $scope.oUser.IsActive = data;

            coreService.showInd();
            userManagementService.updateStatus($scope.oUser)
            .then(function (response) {
                coreService.hideInd();
                if (response.data)
                    coreService.success(coreService.message.updated);
                else
                    coreService.error(coreService.message.error);

                $scope.initialize();

            }, function (err) {
                coreService.hideInd();
                coreService.error(coreService.message.error);
            });
        } else {

        }
    }

    $scope.delete = function () {

        if (confirm("Are you sure to delete this record?")) {
            coreService.showInd();
            userManagementService.delete($scope.oUser)
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