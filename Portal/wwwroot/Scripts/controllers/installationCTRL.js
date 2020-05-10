jQuery(".standardSelect").chosen({
    disable_search_threshold: 1,
    no_results_text: "Oops, nothing found!",
    width: "100%"
});

app.controller('installationCTRL', function ($scope, $timeout, coreService, clientService, installationService, stateService, districtService, cityService, productService) {

    document.getElementById("template").style.visibility = "visible";

    var userId = 1;
    var clientId = 0;
    var productId = 0;
    var clientTypeId = 0;
    var stateId = 0;
    var districtId = 0;
    var cityId = 0;
    $scope.isEditable = false;
    $scope.header = '';

    $scope.initialize = function () {
        $('#bootstrap-data-table').DataTable().clear().destroy();
        coreService.showInd();
        installationService.getList(userId)
        .then(function (response) {
            var result = response.data;
            for (var i = 0; i < result.length; i++) {
                if (result[i].ActivationDate != null)
                    result[i].ActivationDate = new Date(result[i].ActivationDate.match(/\d+/)[0] * 1);

                if (result[i].ExpiryDate != null)
                    result[i].ExpiryDate = new Date(result[i].ExpiryDate.match(/\d+/)[0] * 1);
            }
            $scope.data = result;
            isTableLoaded = true;
            clearValidation();
            coreService.hideInd();
        }, function (err) {
            coreService.hideInd();
            error(coreService.message.error);
        });
    };

    var clearValidation = function () {
        $scope.form.$setPristine();
    };

    $scope.reset = function () {
        $scope.isActive = true;
        clearValidation();
    };

    $scope.add = function () {
        $scope.header = 'ADD';
        $scope.submitTxt = 'Confirm';
        $scope.isEditable = false;
        $scope.InstallationID = 0;
        $scope.reset();
        $scope.masters();
    }

    $scope.edit = function (installationId) {

        $scope.header = 'EDIT';
        $scope.submitTxt = 'Update';
        $scope.isEditable = true;
        $scope.reset();
        $scope.InstallationID = installationId;

        coreService.showInd();
        installationService.get(installationId)
        .then(function (response) {
            coreService.hideInd();
            var result = response.data;

            clientId = result.ClientID.toString();
            productId = result.ProductID.toString();
            clientTypeId = result.ClientTypeID.toString();
            stateId = result.StateID.toString();
            districtId = result.DistrictID.toString();
            cityId = result.CityID.toString();

           
            $('#client').val(clientId);
            $scope.form.client.$setDirty();
            ddlProduct(clientId);

            $('#state').val(stateId);
            $scope.form.state.$setDirty();
            ddlDistrict(userId, stateId);

            $scope.InstallationNumber = result.InstallationNumber;
            $scope.DeviceName = result.DeviceName;
            $scope.isActive = result.IsActive;
            $scope.isRegister = result.IsRegister;

            $('.standardSelect').trigger("chosen:updated");
            
        }, function (err) {
            coreService.hideInd();
            error(coreService.message.error);
        });
    };

    $scope.submit = function (form) {

        var oInstallation = {};
         oInstallation.InstallationID = $scope.InstallationID;
         oInstallation.ClientID = $('#client').find(":selected").val();
         oInstallation.ProductID = $('#product').find(":selected").val();
         oInstallation.StateID = $('#state').find(":selected").val();
         oInstallation.DistrictID = $('#district').find(":selected").val();
         oInstallation.CityID = $('#city').find(":selected").val();
         oInstallation.ClientTypeID = $('#clientType').find(":selected").val();
         oInstallation.DeviceAvailabilityID = 1;
         oInstallation.IsActive = $scope.isActive;

         if ($scope.InstallationID == 0) {
             coreService.showInd();
             installationService.add(oInstallation)
             .then(function (response) {
                 if (response.data == 1) {
                     success(coreService.message.added);
                 }
                 else if (response.data == 2) {
                     error(coreService.message.licenceExceed);
                 }
                 else {
                     error(coreService.message.error);
                 }


                 $scope.initialize();
             }, function (err) {
                 coreService.hideInd();
                 error(coreService.message.error);
             });
         }
         else {
             coreService.showInd();
             installationService.update(oInstallation)
             .then(function (response) {
                 coreService.hideInd();
                 if (response.data == true) {
                     success(coreService.message.updated);
                 }
                 else {
                     error(coreService.message.error);
                 }

                 $scope.initialize();
             }, function (err) {
                 coreService.hideInd();
                 error(coreService.message.error);
             });
         }
    };

    $scope.delete = function () {

        if (confirm("Are you sure to delete this record?")) {
            var oInstallation = {};
            oInstallation.InstallationID = $scope.InstallationID;
            coreService.showInd();
            installationService.delete(oInstallation)
            .then(function (response) {
                coreService.hideInd();
                if (response.data)
                    success(coreService.message.deleted);
                else
                    error(coreService.message.error);

                $scope.initialize();

            }, function (err) {
                coreService.hideInd();
                error(coreService.message.error);
            });
        } else {

        }
    };

    $scope.masters = function () {
        $('#client').empty();
        $('#product').empty();
        $('#clientType').empty();
        $('#state').empty();
        $('#district').empty();
        $('#city').empty();

        ddlState();
        ddlClient();
    };

    var ddlClient = function () {
        clientService.getList()
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
    var ddlProduct = function (clientId) {
        productService.getList(clientId)
       .then(function (response) {

           var result = response.data;
           console.log(result);
           $('#product').empty();
           $('#product').append('<option value=""></option>');
           $.each(result, function (k, v) {
               $('#product').append('<option value="' + v.ProductID + '">' + v.ProductName + '</option>');
           });
           if ($scope.isEditable) {
               $('#product').val(productId);
               $scope.form.product.$setDirty();
               var productName = $('#product').find(":selected").text();
               ddlClientType(productName);
           }

           $('.standardSelect').trigger("chosen:updated");

       }, function (err) {
           coreService.hideInd();
           console.log(error.data);
       });
    }
    var ddlClientType = function (productName) {
        if (productName == 'DIGIM') {
            clientService.getClientTypeList()
           .then(function (response) {
               var result = response.data;

               $('#clientType').empty();
               $('#clientType').append('<option value=""></option>');
               $.each(result, function (k, v) {
                   $('#clientType').append('<option value="' + v.ClientTypeID + '">' + v.ClientTypeName + '</option>');
               });

               if ($scope.isEditable) {
                   $('#clientType').val(clientTypeId);
                   $scope.form.clientType.$setDirty();
               }

               $('.standardSelect').trigger("chosen:updated");

           }, function (err) {
               coreService.hideInd();
               console.log(error.data);
           });
        }
        else {
            $('#clientType').empty();
            $('#clientType').append('<option value=""></option>');
            $('#clientType').append('<option value="0">None</option>');
           
            if ($scope.isEditable) {
                $('#clientType').val(clientTypeId);
                $scope.form.clientType.$setDirty();
            }

            $('.standardSelect').trigger("chosen:updated");
        }
    }
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
    var ddlDistrict = function (userId, stateId) {
        districtService.getList(userId, stateId)
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

    var success = function (msg) {
        $scope.successMsg = msg;
        $scope.success = true;
        $timeout(function () {
            $scope.success = false;
        }, 5000);
    }

    var error = function (msg) {
        $scope.errorMsg = msg;
        $scope.error = true;
        $timeout(function () {
            $scope.error = false;
        }, 5000);
    }

    $("#client").change(function () {
        $scope.form.client.$setDirty();

        var clientId = $('#client').find(":selected").val();
        ddlProduct(clientId);
    });

    $("#product").change(function () {
        $scope.form.product.$setDirty();
        $scope.form.clientType.$setPristine();
        var productName = $('#product').find(":selected").text();
        ddlClientType(productName);
    });

    $("#state").change(function () {
        $scope.form.state.$setDirty();

        var stateId = $('#state').find(":selected").val();
        ddlDistrict(userId, stateId);
    });

    $("#district").change(function () {
        $scope.form.district.$setDirty();
        
        var districtId = $("#district").val();
        ddlCity(districtId);
    });

    $("#city").change(function () {
        $scope.form.city.$setDirty();
    });

    $("#clientType").change(function () {
        $scope.form.clientType.$setDirty();
    });

});
