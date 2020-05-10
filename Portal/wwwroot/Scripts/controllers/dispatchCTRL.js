app.controller('dispatchCTRL', function ($scope, $timeout, coreService, dispatchService, clientService, productService, stateService, districtService, cityService) {
  
    document.getElementById("template").style.visibility = "visible";

    var listOfInventoryId = [];
    var inventory = [];
    var districtId = 0;
    var cityId = 0;
    var orderId = 0;
    $scope.isEditable = false;

    $scope.initialize = function () {
        $('#bootstrap-data-table').DataTable().clear().destroy();
        coreService.showInd();
        dispatchService.getList()
        .then(function (response) {
            var result = response.data;
            //ddlInventory();

            for (var i = 0; i < result.length; i++) {
                if (result[i].DispatchDate != null)
                    result[i].DispatchDate = new Date(result[i].DispatchDate);
            }

            $scope.data = result;
            isTableLoaded = true;
            coreService.hideInd();
        }, function (err) {
            coreService.hideInd();
            coreService.error(coreService.message.error);
        });
    };

    $scope.masters = function () {
        $('#client').empty();
        $('#product').empty();
        $('#order').empty();
       // $('#inventory').empty();
        $('#state').empty();
        $('#district').empty();
        $('#city').empty();

        ddlClient();
        ddlInventory();
        ddlState();
    };

    var clearValidation = function () {
        $scope.form.$setPristine();
    };

    $scope.reset = function () {
        listOfInventoryId = [];
        $scope.ShippingAddress = '';
        $scope.Dispatched = false;
        $scope.DispatchDate = null;
        $scope.inventory_invalid = false;
        $scope.isInvoiceAvailable = false;
        $scope.isDraftAvailable = false;
        clearValidation();
    };

    $scope.add = function () {
        $scope.header = 'ADD';
        $scope.submitTxt = 'Confirm';
        $scope.isEditable = false;
        $scope.DispatchID = 0;
        $scope.reset();
        $scope.masters();
    };

    $scope.edit = function (dispatchId) {

        $scope.header = 'EDIT';
        $scope.submitTxt = 'Update';
        $scope.isEditable = true;
        $scope.reset();
        $scope.DispatchID = dispatchId;

        coreService.showInd();
        dispatchService.get(dispatchId)
        .then(function (response) {
            coreService.hideInd();
            var result = response.data;
            inventory = [];
            $scope.DispatchNumber = result.DispatchNumber;
            $scope.Dispatched = result.IsDispatched;
            if (result.DispatchDate != null)
            {
                $scope.DispatchDate = new Date(result.DispatchDate);
            }
            $scope.ShippingAddress = result.ShippingAddress;

            for (var i = 0; i < result.DispatchDetails.length; i++) {
                inventory.push(result.DispatchDetails[i].InventoryID);
            }

            inventory = inventory.filter(function (x, i, a) {
                return a.indexOf(x) == i;
            });

            $("#inventory").val(inventory).trigger("chosen:updated");
            $scope.inventory_invalid = false;


            clientId = result.ClientID.toString();
            productId = result.ProductID.toString();
            orderId = result.OrderID.toString();
            stateId = result.StateID.toString();
            districtId = result.DistrictID.toString();
            cityId = result.CityID.toString();

            getInvoiceAvailable(orderId);
            getdraftAvailable(orderId);
            $scope.masters();
            $scope.IsDispatched = result.IsDispatched;

        }, function (err) {
            coreService.hideInd();
            coreService.error(coreService.message.error);
        });
    };

    $scope.submit = function () {
        var dispatch = {};
        dispatch.DispatchID = $scope.DispatchID;
        dispatch.DispatchDate = $scope.DispatchDate;
        dispatch.ShippingAddress = $scope.ShippingAddress;
        dispatch.ClientID = $('#client').find(":selected").val();
        dispatch.ProductID = $('#product').find(":selected").val();
        dispatch.StateID = $('#state').find(":selected").val();
        dispatch.DistrictID = $('#district').find(":selected").val();
        dispatch.CityID = $('#city').find(":selected").val();
        listOfInventoryId = $('#inventory').chosen().val();
        dispatch.IsDispatched = $scope.Dispatched;
        dispatch.OrderID = $('#order').chosen().val();
        
        var DispatchDetail = [];
        for (var i = 0; i < listOfInventoryId.length; i++) {
            DispatchDetail.push({ InventoryID: listOfInventoryId[i]});
        }
        dispatch.DispatchDetails = DispatchDetail;
        
        if ($scope.DispatchID == 0) {
            coreService.showInd();
            dispatchService.add(dispatch)
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
            dispatchService.update(dispatch)
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
            var oDispatch = {};
            oDispatch.DispatchID = $scope.DispatchID;
            oDispatch.OrderID = $('#order').chosen().val();
            coreService.showInd();
            dispatchService.delete(oDispatch)
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

    $scope.makeInvoice = function (dispatchId) {
        var oDispatch = {};
        oDispatch.DispatchID = dispatchId;
        coreService.showInd();
        dispatchService.makeInvoice(oDispatch)
        .then(function (response) {
            coreService.hideInd();
            if (response.data != null) {
                console.log(response.data);
                var file = base64ToArrayBuffer(response.data.FileContents);
                saveByteArray(response.data.FileDownloadName, file);
            }

        }, function (err) {
            coreService.hideInd();
            coreService.error(coreService.message.error);
        });
    };

    function base64ToArrayBuffer(base64) {
        var binaryString = window.atob(base64);
        var binaryLen = binaryString.length;
        var bytes = new Uint8Array(binaryLen);
        for (var i = 0; i < binaryLen; i++) {
            var ascii = binaryString.charCodeAt(i);
            bytes[i] = ascii;
        }
        return bytes;
    };

    function saveByteArray(shippingName, byte) {
        var blob = new Blob([byte], { type: "application/pdf" });
        var link = document.createElement('a');
        link.href = window.URL.createObjectURL(blob);
        link.download = shippingName;
        link.click();
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

           if ($scope.isEditable) {
               $('#client').val(clientId);
               $scope.form.client.$setDirty();
               ddlProduct(clientId);
           }

           $('.standardSelect').trigger("chosen:updated");

       }, function (err) {
           coreService.hideInd();
           console.log(err.data);
       });
    };
    var ddlProduct = function (clientId) {
        productService.getList(clientId)
       .then(function (response) {

           var result = response.data;

           $('#product').empty();
           $('#product').append('<option value=""></option>');
           $.each(result, function (k, v) {
               $('#product').append('<option value="' + v.ProductMaster.ProductID + '">' + v.ProductMaster.ProductName + '</option>');
           });
           if ($scope.isEditable) {
               $('#product').val(productId);
               $scope.form.product.$setDirty();
               ddlOrder(clientId, productId);
           }

           $('.standardSelect').trigger("chosen:updated");

       }, function (err) {
           coreService.hideInd();
           console.log(err.data);
       });
    }
    var ddlOrder = function (clientId, productId) {
        dispatchService.getOrderList(clientId,productId)
       .then(function (response) {

           var result = response.data;
           $('#order').empty();

           if ($scope.isEditable) {
               $('#order').append('<option value=""></option>');
               $.each(result, function (k, v) {
                   $('#order').append('<option value="' + v.OrderID + '">' + v.OrderNumber + '</option>');
               });

               $('#order').val(orderId);
               $scope.form.order.$setDirty();
           }
           else {
               $('#order').append('<option value=""></option>');
               $.each(result, function (k, v) {
                   if(v.OrderStatus.Status == 'Pending')
                   $('#order').append('<option value="' + v.OrderID + '">' + v.OrderNumber + '</option>');
               });
           }

           $('.standardSelect').trigger("chosen:updated");

       }, function (err) {
           coreService.hideInd();
           console.log(err.data);
       });
    };
    var ddlInventory = function () {
        dispatchService.getInventoryList($scope.isEditable)
       .then(function (response) {
           var result = response.data;

           $('#optDevice').empty();
           $('#optSpare').empty();
           

           $.each(result, function (k, v) {
              // var ref = '#' + v.ReferenceNumber + '-' + v.InventoryName;
               if (v.DeviceMaster != null) {
                   var ref = '#' + v.ReferenceNumber + '-' + v.DeviceMaster.DeviceName;
                   $('#optDevice').append('<option value="' + v.InventoryID + '">' + ref + '</option>');
               }
               if (v.SpareMaster != null) {
                   var ref = '#' + v.ReferenceNumber + '-' + v.SpareMaster.SpareName;
                   $('#optSpare').append('<option value="' + v.InventoryID + '">' + ref + '</option>');
               }
           });

           if ($scope.isEditable) {

               inventory = inventory.filter(function (x, i, a) {
                   return a.indexOf(x) == i;
               });

               $("#inventory").val(inventory).trigger("chosen:updated");
           }

           $('.standardSelect').trigger("chosen:updated");

       }, function (err) {
           coreService.hideInd();
           console.log(err.data);
       });
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

           if ($scope.isEditable) {
               $('#state').val(stateId);
               $scope.form.state.$setDirty();
               ddlDistrict(stateId);
           }

           $('.standardSelect').trigger("chosen:updated");

       }, function (err) {
           coreService.hideInd();
           console.log(err.data);
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
           console.log(err.data);
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
           console.log(err.data);
       });
    }

    var getShippingAddress = function (clientId) {
        dispatchService.getShippingAddress(clientId)
         .then(function (response) {
             var result = response.data;

             var address = result.Address + ', ' + result.CityMaster.CityName + ', Dist: ' + result.DistrictMaster.DistrictName + ', ' + result.StateMaster.StateName + '-' + result.CityMaster.Picode;

             $scope.ShippingAddress = address;
         }, function (err) {
             coreService.error(coreService.message.error);
         });
    };
    var getInvoiceAvailable = function (orderId) {
        dispatchService.invoiceAvailable(orderId)
         .then(function (response) {
             $scope.isInvoiceAvailable = response.data;
         }, function (err) {
             coreService.error(coreService.message.error);
         });
    };

    var getdraftAvailable = function (orderId) {
        dispatchService.draftAvailable(orderId)
         .then(function (response) {
             $scope.isDraftAvailable = response.data;
         }, function (err) {
             coreService.error(coreService.message.error);
         });
    };

    $("#client").change(function () {
        $scope.form.client.$setDirty();
        $scope.ShippingAddress = '';
        var clientId = $('#client').find(":selected").val();
        ddlProduct(clientId);
        getShippingAddress(clientId);
    });

    $("#product").change(function () {
        $scope.form.product.$setDirty();
        var clientId = $('#client').find(":selected").val();
        var productId = $('#product').find(":selected").val();
        ddlOrder(clientId, productId);
    });

    $("#order").change(function () {
        $scope.form.order.$setDirty();
    });

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

    $('#inventory').chosen().change(function () {
        var listOfInventoryId = $('#inventory').chosen().val();
        if (listOfInventoryId == null) {
            $scope.inventory_invalid = true;
        } 
        else {
            $scope.inventory_invalid = false;
        }
           
    });

});