app.controller('clientCTRL', function ($scope, $timeout, coreService, clientService, distributorService, stateService, districtService, cityService) {

    document.getElementById("template").style.visibility = "visible";

    var districtId = 0;
    var cityId = 0;
    var distributorId = 0;

    $scope.isEditable = false;
    $scope.isDelete = false;

    $scope.initialize = function () {
        $('#bootstrap-data-table').DataTable().clear().destroy();
        coreService.showInd();
        clientService.getList()
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
        $('#state').empty();
        $('#district').empty();
        $('#city').empty();
        $('#distributor').empty();
        ddlState();
        ddlDistributor();
    };

    var clearValidation = function () {
        $scope.form.$setPristine();
    };

    $scope.reset = function () {
        $scope.oClient = {};
        $scope.oClient.ClientID = 0;
        $scope.oClient.ClientName = '';
        $scope.oClient.MobileNo = '';
        $scope.oClient.Email = '';
        $scope.oClient.Address = '';
        $scope.oClient.IsActive = false;
        $scope.isDelete = false;
        clearValidation();
    };

    $scope.add = function () {
        $scope.header = 'ADD';
        $scope.submitTxt = 'Confirm';
        $scope.isEditable = false;
        $scope.reset();
        $scope.masters();
    };

    $scope.edit = function (clientId) {

        $scope.header = 'EDIT';
        $scope.submitTxt = 'Update';
        $scope.isEditable = true;
        $scope.reset();
        existance(clientId);

        coreService.showInd();
        clientService.get(clientId)
        .then(function (response) {
            coreService.hideInd();
            var result = response.data;
            $scope.oClient = result;

            stateId = result.StateID.toString();
            districtId = result.DistrictID.toString();
            cityId = result.CityID.toString();
            distributorId = result.DistributorID;

            $('#state').val(stateId);
            $scope.form.state.$setDirty();
            ddlDistrict(stateId);
            ddlDistributor();

        }, function (err) {
            coreService.hideInd();
            coreService.error(coreService.message.error);
        });
    };

    $scope.submit = function () {
        $scope.oClient.StateID = $('#state').find(":selected").val();
        $scope.oClient.DistrictID = $('#district').find(":selected").val();
        $scope.oClient.CityID = $('#city').find(":selected").val();
        $scope.oClient.DistributorID = $('#distributor').find(":selected").val();

        if ($scope.oClient.ClientID == 0) {
            coreService.showInd();
            clientService.add($scope.oClient)
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
            clientService.update($scope.oClient)
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

    $scope.delete = function () {

        if (confirm("Are you sure to delete this record?")) {
            coreService.showInd();
            clientService.delete($scope.oClient)
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
    var existance = function (clientId) {

        coreService.showInd();
        clientService.existance(clientId)
        .then(function (response) {
            coreService.hideInd();
            $scope.isDelete = response.data ? false : true;
        }, function (err) {
            coreService.hideInd();
        });
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
               ddlCity(districtId);
           }

           $('.standardSelect').trigger("chosen:updated");

       }, function (err) {
           coreService.hideInd();
           console.log(error.data);
       });
    }
    var ddlCity = function (districtId) {
        cityService.getList(districtId)
       .then(function (response) {
           var result = response.data;

           $('#city').empty();
           $('#city').append('<option value=""></option>');
           $.each(result, function (k, v) {
               $('#city').append('<option value="' + v.CityID + '">' + v.CityName + '</option>');
           });

           if ($scope.isEditable) {
               $('#city').val(cityId);
               $scope.form.city.$setDirty();
           }

           $('.standardSelect').trigger("chosen:updated");

       }, function (err) {
           coreService.hideInd();
           console.log(error.data);
       });
    }
    var ddlDistributor = function () {
        distributorService.getList()
       .then(function (response) {
           var result = response.data;

           $('#distributor').empty();
           $('#distributor').append('<option value=""></option>');
           $.each(result, function (k, v) {
               $('#distributor').append('<option value="' + v.DistributorID + '">' + v.DistributorName + '</option>');
           });

           if ($scope.isEditable) {
               $('#distributor').val(distributorId);
           }

           $('.standardSelect').trigger("chosen:updated");

       }, function (err) {
           coreService.hideInd();
           console.log(err.data);
       });
    }

    $("#state").change(function () {
        $scope.form.state.$setDirty();

        var stateId = $('#state').find(":selected").val();
        ddlDistrict(stateId);
    });

    $("#district").change(function () {
        $scope.form.district.$setDirty();

        var districtId = $("#district").val();
        ddlCity(districtId);
    });

    $("#city").change(function () {
        $scope.form.city.$setDirty();
    });

});