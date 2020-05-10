app.controller('licenseCTRL', function ($scope, $timeout, coreService, clientService, productService, deviceService, licenseService) {
     
    document.getElementById("template").style.visibility = "visible";

    var clientId = 0;
    $scope.isEditable = false;

    $scope.initialize = function () {
        $('#bootstrap-data-table').DataTable().clear().destroy();
        coreService.showInd();
        licenseService.getList()
        .then(function (response) {
            var result = response.data;
          for (var i = 0; i < result.length; i++) {
              if (result[i].LicenseDueDate != null)
                  result[i].LicenseDueDate = new Date(result[i].LicenseDueDate);
            }

            $scope.data = result;
            console.log(response.data);
            isTableLoaded = true;
            coreService.hideInd();
        }, function (err) {
            coreService.hideInd();
            coreService.error(coreService.message.error);
        });
    };

    $scope.masters = function () {
        ddlClient();
        $scope.listOfProduct = [];
    };

    var clearValidation = function () {
        $scope.form.$setPristine();
    };

    $scope.reset = function () {
        $scope.oDevice = {};
        $scope.oDevice.DeviceID = 0;
        $scope.oDevice.DeviceName = '';
        $scope.oDevice.Stock = '';
        clearValidation();
    };

    $scope.add = function () {
        $scope.header = 'ADD';
        $scope.submitTxt = 'Confirm';
        $scope.isEditable = false;
        $scope.IsLicenseExists = false;
        $scope.reset();
        $scope.masters();
    };

    $scope.edit = function (licenseId) {

        $scope.header = 'EDIT';
        $scope.submitTxt = 'Update';
        $scope.isEditable = true;
        $scope.IsLicenseExists = false;
        $scope.reset();

        coreService.showInd();
        licenseService.get(licenseId)
        .then(function (response) {
            coreService.hideInd();
            var result = response.data;
            clientId = result[0].ClientID.toString();

            $('#client').val(clientId);
            $scope.form.client.$setDirty();

            $('.standardSelect').trigger("chosen:updated");

            $scope.listOfProduct = [];
            for (var i = 0; i < result.length; i++) {

                if (result[i].LicenseDueDate != null)
                    result[i].LicenseDueDate = new Date(result[i].LicenseDueDate);

                $scope.listOfProduct.push({
                    LicenseID:result[i].LicenseID,
                    ClientID: clientId,
                    ProductID: result[i].ProductID,
                    ProductName: result[i].ProductMaster.ProductName,
                    TotalLicense: result[i].TotalLicense,
                    LicenseDueDate: result[i].LicenseDueDate
                });
            }


        }, function (err) {
            coreService.hideInd();
            coreService.error(coreService.message.error);
        });
    };

    $scope.submit = function () {

        if (!$scope.isEditable) {
            coreService.showInd();
            licenseService.add($scope.listOfProduct)
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
            licenseService.update($scope.listOfProduct)
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
           console.log(err.data);
       });
    };

    var ddlProduct = function (clientId) {
        licenseService.getProductList(clientId)
       .then(function (response) {
           
           var result = response.data;
           $scope.IsLicenseExists = false;
           $scope.listOfProduct = [];

           if (result.length > 0) {
               var expireDate = new Date();
               expireDate.setFullYear(expireDate.getFullYear() + 1);
               expireDate.setMonth(expireDate.getMonth() + 1)
               expireDate.setDate(expireDate.getDate());

               for (var i = 0; i < result.length; i++) {
                   $scope.listOfProduct.push({
                       ClientID: clientId,
                       ProductID: result[i].ProductID,
                       ProductName: result[i].ProductName,
                       TotalLicense: 0,
                       LicenseDueDate: expireDate
                   });
               }
               console.log(result);
           }
           else {
               $scope.IsLicenseExists = true;
           }
       }, function (err) {
           coreService.hideInd();
           console.log(err.data);
       });
    }

    $("#client").change(function () {
        $scope.form.client.$setDirty();

        var clientId = $('#client').find(":selected").val();
        ddlProduct(clientId);
    });
});