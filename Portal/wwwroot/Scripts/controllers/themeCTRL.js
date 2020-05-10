app.controller('themeCTRL', function ($scope, $timeout, coreService, themeService, menuService) {

    document.getElementById("template").style.visibility = "visible";

    $scope.masters = function () {
        clear();
        ddlClient();
        getDayList();
    };

    $scope.initialize = function(){
        deviceId = $('#device').find(":selected").val();
        loadTheme(deviceId);
    }

    $scope.close = function () {
        var x = document.getElementById("snackbar");
        x.className = "";
    };

    $scope.edit = function (selectedTheme) {
        $scope.oTheme = {};
        $scope.validSelectedDays = false;
        $scope.validSelectedWeekDays = false;
        $scope.selectedTheme = selectedTheme;

        for (var i = 0; i < $scope.days.length; i++) {
            $scope.days[i].IsActive = false;
        }

        var x = document.getElementById("snackbar");
        x.className = "show";
    };

    $scope.submit = function () {

        if ($scope.oTheme.Days == 'weekend') {
            deviceId = $('#device').find(":selected").val();

            var selectedDays = $.grep($scope.days, function (a) { return a.IsActive == true; });
            console.log(selectedDays);
            var oDeviceWeekendTheme = [];
            for (var i = 0; i < selectedDays.length; i++) {
                oDeviceWeekendTheme.push({ InstallationID: deviceId, ThemeID: $scope.selectedTheme.ThemeID, DayID: selectedDays[i].DayID })
            }

            coreService.showInd();
            themeService.updateDeviceWeekendTheme(oDeviceWeekendTheme)
            .then(function (response) {
                if (response.data == 1) {
                    coreService.success(coreService.message.added);
                }
                else {
                    coreService.error(coreService.message.error);
                }

                var x = document.getElementById("snackbar");
                x.className = "";

                var installationId = $('#device').find(":selected").val();
                today(installationId);
                weekend(installationId);
            }, function (err) {
                coreService.hideInd();
                coreService.error(coreService.message.error);
               });

        }
        else {
            var oDeviceTheme = {};
            oDeviceTheme.ThemeID = $scope.selectedTheme.ThemeID;
            oDeviceTheme.InstallationID = $('#device').find(":selected").val();
            coreService.showInd();
            themeService.updateDeviceTheme(oDeviceTheme)
            .then(function (response) {
                coreService.hideInd();
                if (response.data == true) {
                    coreService.success(coreService.message.updated);
                }
                else {
                    coreService.error(coreService.message.error);
                }

                var x = document.getElementById("snackbar");
                x.className = "";

                var installationId = $('#device').find(":selected").val();
                today(installationId);
                weekend(installationId)
            }, function (err) {
                coreService.hideInd();
                coreService.error(coreService.message.error);
            });
        }
    };

    $scope.daySelectedChanged = function () {
        $scope.validSelectedDays = true;

        if ($scope.oTheme.Days == 'weekend') {
            var selectedDays = $.grep($scope.days, function (a) { return a.IsActive == true; });
            if (selectedDays.length > 0)
                $scope.validSelectedWeekDays = true;
            else
                $scope.validSelectedWeekDays = false;
        }
        else
            $scope.validSelectedWeekDays = true;
    };

    $scope.weekSelectedChanged = function () {
        
        var selectedDays = $.grep($scope.days, function (a) { return a.IsActive == true; });
        if(selectedDays.length > 0)
            $scope.validSelectedWeekDays = true;
        else
            $scope.validSelectedWeekDays = false;
    };

    var today = function (installationId) {
        $scope.today = {};

        coreService.showInd();
        themeService.getToday(installationId)
           .then(function (response) {
               coreService.hideInd();
               if (response.data.length > 0) {
                  var result =  getBase64(response.data);
                  $scope.today = result[0];
               }
           }, function (err) {
               console.log(err.data);
               coreService.hideInd();
               coreService.error(coreService.message.error);
           });
    }
    var weekend = function (installationId) {
        $scope.weekend = {};

        coreService.showInd();
        themeService.getWeekend(installationId)
           .then(function (response) {
               coreService.hideInd();
               if (response.data.length > 0) {
                   var result = getBase64(response.data);
                   for (var i = 0; i < result.length > 0; i++) {
                       result[i].DayTitle = getWeekDay(result[i].DayTitle);
                   }
                   $scope.weekend = result;
               }
           }, function (err) {
               console.log(err.data);
               coreService.hideInd();
               coreService.error(coreService.message.error);
           });
    };
    var year2018 = function (installationId) {
        $scope.year2018 = {};

        coreService.showInd();
        themeService.getYear(installationId, 2018)
           .then(function (response) {
               coreService.hideInd();
               if (response.data.length > 0) {
                   var result = getBase64(response.data);
                   $scope.year2018 = result;
               }
           }, function (err) {
               console.log(err.data);
               coreService.hideInd();
               coreService.error(coreService.message.error);
           });
    };

    var clear = function () {
        $scope.today = {};
        $scope.weekend = [];
        $scope.year2018 = [];
    };

    var loadTheme = function (installationId) {
        today(installationId);
        weekend(installationId);
        year2018(installationId);
    }

    var getBase64 = function (data) {
        for (var i = 0; i < data.length; i++) {
            data[i].BackgroundImage = 'data:image/jpg;base64,' + data[i].BackgroundImage;
        }
        return data;
    };
    var getWeekDay = function (day) {
        if (day == 'Thursday') {
            day = day.substring(0, 4);
        }
        else {
            day = day.substring(0, 3);
        }
        return day;
    };

    var ddlClient = function () {
        menuService.getClientList()
       .then(function (response) {

           var result = response.data;
           $('#client').empty();
           $('#client').append('<option value=""></option>');
           $.each(result, function (k, v) {
               $('#client').append('<option value="' + v.ClientID + '">' + v.ClientName + '</option>');
           });

           $('.standardSelect').trigger("chosen:updated");

       }, function (err) {
           coreService.hideInd();
           console.log(error.data);
       });
    };
    var ddlInstallation = function (clientId) {
        menuService.getInstallationList(clientId)
       .then(function (response) {

           var result = response.data;
           $('#device').empty();
           $('#device').append('<option value=""></option>');
           $.each(result, function (k, v) {
               $('#device').append('<option value="' + v.InstallationID + '">' + v.DeviceName + '</option>');
           });

           $('.standardSelect').trigger("chosen:updated");

       }, function (err) {
           coreService.hideInd();
           console.log(error.data);
       });
    };
    var getDayList = function () {
        themeService.getDayList()
         .then(function (response) {
             var result = response.data;

             for (var i = 0; i < result.length; i++) {
                 result[i].DayTitle = getWeekDay(result[i].DayTitle);
             }
             
             $scope.days = result;
             
         }, function (err) {
             coreService.hideInd();
             console.log(error.data);
         });
    };

    $("#client").change(function () {
        clientId = $('#client').find(":selected").val();
        ddlInstallation(clientId);
    });

    $("#device").change(function () {
        deviceId = $('#device').find(":selected").val();
        loadTheme(deviceId);
    });
});