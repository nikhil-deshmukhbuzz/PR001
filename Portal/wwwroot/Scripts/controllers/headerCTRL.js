app.controller('headerCTRL', function ($scope, coreService, loginService, userManagementService) {
    document.getElementById("header").style.visibility = "visible";

    $scope.username = coreService.getUser().Name;
    var userId = coreService.getUserManagement().Id;

    $scope.changePassword = function () {
        $scope.oUser = {};
        $scope.oUser.Id = userId;
        $scope.form.$setPristine();
    };

    $scope.updatePassword = function () {
        coreService.showInd();
        userManagementService.changePassword($scope.oUser)
        .then(function (response) {
            coreService.hideInd();
            if (response.data == true) {
                coreService.success(coreService.message.passwordchanged);
            }
            else {
                coreService.error(coreService.message.wrong);
            }
        }, function (err) {
            coreService.hideInd();
            coreService.error(coreService.message.error);
        });
    };

    $scope.logout = function () {

        window.location.href = '@Url.Action("SignOff", "Account")/';//'/Account/SignOff';
    };



});