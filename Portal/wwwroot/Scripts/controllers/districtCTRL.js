app.controller('districtCTRL', function ($scope, $timeout, coreService, stateService, districtService) {
    document.getElementById("template").style.visibility = "visible";

    var stateId = 0;
    $scope.initialize = function () {
        $('#bootstrap-data-table').DataTable().clear().destroy();
        coreService.showInd();
        districtService.getList()
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
        ddlState();
    };

    var clearValidation = function () {
        $scope.form.$setPristine();
    };

    $scope.reset = function () {
        $scope.oDistrict = {};
        $scope.oDistrict.DistrictID = 0;
        $scope.oDistrict.DistrictName = '';
        clearValidation();
    };

    $scope.add = function () {
        $scope.header = 'ADD';
        $scope.submitTxt = 'Confirm';
        $scope.isEditable = false;
        $scope.reset();
        $scope.masters();
    };

    $scope.edit = function (districtId) {

        $scope.header = 'EDIT';
        $scope.submitTxt = 'Update';
        $scope.isEditable = true;
        $scope.reset();


        coreService.showInd();
        districtService.get(districtId)
        .then(function (response) {
            coreService.hideInd();
            $scope.oDistrict = response.data;
            stateId = response.data.StateID;
            $scope.masters();

        }, function (err) {
            coreService.hideInd();
            coreService.error(coreService.message.error);
        });
    };

    $scope.submit = function () {

        $scope.oDistrict.StateID = $('#state').find(":selected").val();
        if ($scope.oDistrict.DistrictID == 0) {
            coreService.showInd();
            districtService.add($scope.oDistrict)
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
            districtService.update($scope.oDistrict)
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
           }

           $('.standardSelect').trigger("chosen:updated");

       }, function (err) {
           coreService.hideInd();
           console.log(error.data);
       });
    }

    $("#state").change(function () {
        $scope.form.state.$setDirty();
    });

});