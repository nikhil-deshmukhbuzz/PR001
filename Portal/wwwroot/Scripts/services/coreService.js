app.service('coreService', function ($http, $rootScope, $timeout) {

    this.message = {
        added: 'successfully added',
        updated: 'successfully updated',
        passwordchanged: 'password changed successfully',
        deleted: 'successfully deleted',
        licenceExceed: 'selected product licence exceed',
        limitExceed: 'Maximum limit exceed',
        outOfStock: 'Out of stock',
        error: 'please contact to administrator due to error in application',
        wrong:'something went wrong'
    }

    this.hideInd = function (lUserId) {
        document.getElementById("overlay").style.display = "none";
    };

    this.showInd = function (lUserId) {
        document.getElementById("overlay").style.display = "block";
    };

    this.success = function (msg) {
        $rootScope.successMsg = msg;
        $rootScope.success = true;
        $timeout(function () {
            $rootScope.success = false;
        }, 5000);
    }

    this.error = function (msg) {
        $rootScope.errorMsg = msg;
        $rootScope.error = true;
        $timeout(function () {
            $rootScope.error = false;
        }, 5000);
    }

    this.setMenu = function (data) {
        localStorage.Menu = angular.toJson(data);
    };

    this.getMenu = function () {
        return angular.fromJson(localStorage.Menu);
    };

    this.setUser = function (data) {
        localStorage.User = angular.toJson(data);
    };

    this.getUser = function () {
        return angular.fromJson(localStorage.User);
    };

    this.setUserManagement = function (data) {
        localStorage.UserManagement = angular.toJson(data);
    };

    this.getUserManagement = function () {
        return angular.fromJson(localStorage.UserManagement);
    };
});