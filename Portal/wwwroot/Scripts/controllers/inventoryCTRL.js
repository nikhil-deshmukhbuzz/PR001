jQuery(".standardSelect").chosen({
    disable_search_threshold: 1,
    no_results_text: "Oops, nothing found!",
    width: "100%"
});

app.controller('inventoryCTRL', function ($scope, $timeout, coreService, deviceService, inventoryService) {

    document.getElementById("template").style.visibility = "visible";

    $scope.isEditable = false;
    var deviceId = 0;

    $scope.initialize = function () {
        $('#bootstrap-data-table').DataTable().clear().destroy();
        coreService.showInd();
        inventoryService.getList()
        .then(function (response) {
            var result = response.data;

            //for (var i = 0; i < result.length; i++) {
            //    if (result[i].WarrantyDate != null)
            //        result[i].WarrantyDate = new Date(result[i].WarrantyDate.match(/\d+/)[0] * 1);
            //}
            $scope.data = result;
            isTableLoaded = true;
            coreService.hideInd();
        }, function (err) {
            coreService.hideInd();
            coreService.error(coreService.message.error);
        });
    };

    $scope.masters = function () {
        ddlHardware();
    };

    var clearValidation = function () {
        $scope.form.$setPristine();
    };

    $scope.reset = function () {
        $scope.oInventory = {};
        $scope.oInventory.InventoryID = 0;
        $scope.isDelete = false;

        $('#devicespare').empty();
        clearValidation();
    };

    $scope.add = function () {
        $scope.header = 'ADD';
        $scope.submitTxt = 'Confirm';
        $scope.isEditable = false;
        $scope.reset();
        $scope.masters();
    };

    $scope.edit = function (inventoryId) {

        $scope.header = 'EDIT';
        $scope.submitTxt = 'Update';
        $scope.isEditable = true;
        $scope.reset();
        existance(inventoryId);

        coreService.showInd();
        inventoryService.get(inventoryId)
        .then(function (response) {
            coreService.hideInd();
            var result = response.data;
            if (result.WarrantyDate != null) {
                result.WarrantyDate = new Date(result.WarrantyDate);
                //result.WarrantyDate = new Date(result.WarrantyDate.match(/\d+/)[0] * 1);
            }
                 

            $scope.oInventory = response.data;

            if (result.DeviceID != null) {
                deviceId = result.DeviceID;
                $('#hardware').val(1);
                ddlDeviceSpare('D');
            }
            else if (result.SpareID != null) {
                deviceId = result.SpareID;
                $('#hardware').val(2);
                ddlDeviceSpare('S');
            }
            $scope.form.hardware.$setDirty();

            $('.standardSelect').trigger("chosen:updated");

        }, function (err) {
            coreService.hideInd();
            coreService.error(coreService.message.error);
        });
    };

    $scope.submit = function () {
        $scope.oInventory.DeviceID = $('#device').find(":selected").val();
        var selectedHardware = $('#hardware').find(":selected").text();
       
        if (selectedHardware == 'Device')
            $scope.oInventory.DeviceID = $('#devicespare').find(":selected").val();
        else
            $scope.oInventory.SpareID = $('#devicespare').find(":selected").val();

        if ($scope.oInventory.InventoryID == 0) {
            coreService.showInd();
            inventoryService.add($scope.oInventory)
            .then(function (response) {
                if (response.data == 1) {
                    coreService.success(coreService.message.added);
                }
                else if(response.data== -1){
                    coreService.error(coreService.message.outOfStock);
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
            inventoryService.update($scope.oInventory)
            .then(function (response) {
                coreService.hideInd();
                if (response.data == true) {
                    coreService.success(coreService.message.updated);
                }
                else if (response.data == -1) {
                    coreService.error(coreService.message.outOfStock);
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
            inventoryService.delete($scope.oInventory)
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
    var existance = function (inventoryId) {

        coreService.showInd();
        inventoryService.existance(inventoryId)
        .then(function (response) {
            coreService.hideInd();
            $scope.isDelete = response.data ? false : true;
        }, function (err) {
            coreService.hideInd();
        });
    };

    var ddlDeviceSpare = function (hardwareType) {
        inventoryService.getDeviceSpareList()
       .then(function (response) {

           var result = response.data;

           result = $.grep(result, function (n, i) { return (n.HardwareType == hardwareType); });

           $('#devicespare').empty();
           $('#devicespare').append('<option value=""></option>');
           $.each(result, function (k, v) {
               $('#devicespare').append('<option value="' + v.DeviceID + '">' + v.DeviceName + '</option>');
           });

           if ($scope.isEditable) {
               $('#devicespare').val(deviceId);
               $scope.form.devicespare.$setDirty();
           }

           $('.standardSelect').trigger("chosen:updated");

       }, function (err) {
           coreService.hideInd();
           console.log(err.data);
       });
    };
    var ddlHardware = function () {
        inventoryService.getHardwareList()
       .then(function (response) {

           var result = response.data;
           $('#hardware').empty();
           $('#hardware').append('<option value=""></option>');
           $.each(result, function (k, v) {
               $('#hardware').append('<option value="' + v.HardwareID + '">' + v.HardwareName + '</option>');
           });

           $('.standardSelect').trigger("chosen:updated");

       }, function (err) {
           coreService.hideInd();
           console.log(err.data);
       });
    };

    $("#devicespare").change(function () {
        $scope.form.devicespare.$setDirty();
    });

    $("#hardware").change(function () {
        $scope.isEditable = false;
        $('#devicespare').empty();
        $scope.form.hardware.$setDirty();
        $scope.form.devicespare.$setPristine();
        var selectedHardware = $('#hardware').find(":selected").text();
        if (selectedHardware == 'Device') 
            ddlDeviceSpare('D');
        else
            ddlDeviceSpare('S');
        
    });
});