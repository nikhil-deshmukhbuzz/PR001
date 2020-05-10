app.controller('cityCTRL', function ($scope, $timeout, coreService, stateService, districtService, cityService) {
    document.getElementById("template").style.visibility = "visible";

    var stateId = 0;
    var districtId = 0;

    $scope.initialize = function () {
        $('#bootstrap-data-table').DataTable().clear().destroy();
        coreService.showInd();
        cityService.getAllList()
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
        $('#district').empty();
        ddlState();
    };

    var clearValidation = function () {
        $scope.form.$setPristine();
    };

    $scope.reset = function () {
        $scope.oCity = {};
        $scope.oCity.CityID = 0;
        $scope.oCity.CityName = '';
        clearValidation();
    };

    $scope.add = function () {
        $scope.header = 'ADD';
        $scope.submitTxt = 'Confirm';
        $scope.isEditable = false;
        $scope.reset();
        $scope.masters();
    };

    $scope.edit = function (cityId) {

        $scope.header = 'EDIT';
        $scope.submitTxt = 'Update';
        $scope.isEditable = true;
        $scope.reset();

        coreService.showInd();
        cityService.get(cityId)
        .then(function (response) {
            coreService.hideInd();
            $scope.oCity = response.data;
            stateId = response.data.StateID;
            districtId = response.data.DistrictID;
            $scope.masters();

        }, function (err) {
            coreService.hideInd();
            coreService.error(coreService.message.error);
        });
    };

    $scope.submit = function () {

        $scope.oCity.DistrictID = $('#district').find(":selected").val();
        $scope.oCity.StateID = $('#state').find(":selected").val();
        if ($scope.oCity.CityID == 0) {
            coreService.showInd();
            cityService.add($scope.oCity)
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
            cityService.update($scope.oCity)
            .then(function (response) {
                coreService.hideInd();
                //if (response.data == true) {
                    coreService.success(coreService.message.updated);
                //}
                //else {
                //    coreService.error(coreService.message.error);
                //}

                $scope.initialize();
            }, function (err) {
                coreService.hideInd();
                coreService.error(coreService.message.error);
            });
        }
    };

    var ddlState = function () {
        stateService.getList()
       .then(function (response) {

           var result = response.data;
           $('#state').empty();
           $('#state').append('<option value=""></option>');
           $.each(result, function (k, v) {
               $('#state').append('<option value="' + v.StateID + '">' + v.StateName + '</option>');
           });

           if ($scope.isEditable) {
               $('#state').val(stateId);
               $scope.form.state.$setDirty();
               ddlDistrict(stateId);
           }

           $('.standardSelect').trigger("chosen:updated");

       }, function (err) {
           coreService.hideInd();
           console.log(error.data);
       });
    }
    var ddlDistrict = function (stateId) {
        districtService.getDistrictList(stateId)
       .then(function (response) {
           var result = response.data;

           $('#district').empty();
           $('#district').append('<option value=""></option>');
           $.each(result, function (k, v) {
               $('#district').append('<option value="' + v.DistrictID + '">' + v.DistrictName + '</option>');
           });

           if ($scope.isEditable) {
               $('#district').val(districtId);
               $scope.form.district.$setDirty();
           }

           $('.standardSelect').trigger("chosen:updated");

       }, function (err) {
           coreService.hideInd();
           console.log(error.data);
       });
    }


    $("#state").change(function () {
        $scope.form.state.$setDirty();
        var stateId = $('#state').find(":selected").val();
        ddlDistrict(stateId);
    });

    $("#district").change(function () {
        $scope.form.district.$setDirty();
    });

});