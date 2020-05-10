app.controller('loginCTRL', function ($scope, $window, $timeout, coreService, loginService) {
    document.getElementById("login").style.visibility = "visible";

    $scope.isProcessing = false;
    $scope.oUser = {};
    $scope.Menu = [];

    $scope.doLogin = function () {
       
        $scope.Menu = [];
        $scope.isProcessing = true;
        loginService.validateUser($scope.oUser)
        .then(function (response) {
            $scope.isProcessing = false;
            var result = response.data;
            if (result != null) {
                if (result.Status == 'Success') {
                    coreService.setUserManagement(result);
                    coreService.setUser(result.UserMaster);
                    /*menu*/
                    var menu = result.ListOfMenuMaster;
                    var parentMenu = $.grep(menu, function (a) { return a.ParentMenuID == 0; });

                    for (var i = 0; i < parentMenu.length; i++) {
                        var subMenu = $.grep(menu, function (a) { return a.ParentMenuID == parentMenu[i].MenuID; });

                        $scope.Menu.push({ MenuName: parentMenu[i].MenuName, ImageUrl: parentMenu[i].ImageUrl, Menu: subMenu });

                        coreService.setMenu($scope.Menu);
                    }
                    $window.location = '/Dashboard';
                }
                else {
                    error('Invalid login');
                }
            }
            else {
                error('Something went wrong! contact to administrator');
            }
        }, function (err) {
            $scope.isProcessing = false;
        });
    };

    var error = function (msg) {
        $scope.error = true;
        $scope.errorMsg = msg;
        $timeout(function () {
            $scope.errorMsg = '';
            $scope.error = false;
        }, 5000);
    };
});