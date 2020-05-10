app.controller('menuCTRL', function ($scope, coreService) {
    document.getElementById("left-panel").style.visibility = "visible";

    $scope.Menu = coreService.getMenu();

    console.log(JSON.stringify($scope.Menu));
    
    $scope.doNavigate = function (url) {
        window.location.href = url;
    };

});