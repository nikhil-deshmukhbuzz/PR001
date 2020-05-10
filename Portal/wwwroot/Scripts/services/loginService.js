app.service('loginService', function ($http) {

    this.validateUser = function (oLogin) {
        return $http.post('/Account/ValidateUser',oLogin);

    };

    this.signOff = function () {
        return $http.post('/Account/SignOff');

    };
});