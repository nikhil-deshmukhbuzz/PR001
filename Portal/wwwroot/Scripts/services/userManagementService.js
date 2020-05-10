app.service('userManagementService', function ($http) {

    this.add = function (oUserManagement) {
        return $http.post('/UserManagement/Add', oUserManagement);
    };

    this.update = function (oUserManagement) {
        return $http.post('/UserManagement/Update', oUserManagement);
    };

    this.updateStatus = function (oUserManagement) {
        return $http.post('/UserManagement/UpdateStatus', oUserManagement);
    };

    this.updatePassword = function (oUserManagement) {
        return $http.post('/UserManagement/UpdatePassword', oUserManagement);
    };

    this.changePassword = function (userManagement) {
        return $http.post('/User/ChangePassword', userManagement);
    };

    this.getList = function () {
        return $http.get('/UserManagement/GetList');
    };

    this.get = function (userId) {
        return $http.get('/UserManagement/Get?userId=' + userId);
    };

    this.getProfileMasterList = function () {
        return $http.get('/UserManagement/GetProfileMasterList');
    };

    this.getDistributorList = function (isEditable) {
        return $http.get('/UserManagement/GetDistributorList?isEditable=' + isEditable);
    };

    this.delete = function (oUserManagement) {
        return $http.post('/UserManagement/Delete', oUserManagement);
    };
});