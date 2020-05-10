app.controller('orderCTRL', function ($scope, $timeout, coreService, clientService, productService, orderService) {

    document.getElementById("template").style.visibility = "visible";
    var userId = coreService.getUser().UserID;
    var profileName = coreService.getUser().ProfileMaster.ProfileName;

    if (profileName == 'Distributor') {
        $scope.isAdmin = false;
    }
    else {
        $scope.isAdmin = true;
    }

    var distributorID = coreService.getUser().DistributorID;
    var clientId = 0;
    var productId = 0;
    var clientTypeId = 0;
    var statusId = 0;

    $scope.initialize = function () {
        $('#bootstrap-data-table').DataTable().clear().destroy();
        coreService.showInd();
        if (profileName != 'Distributor') {
            orderService.getList()
            .then(function (response) {
                var result = response.data;
                for (var i = 0; i < result.length; i++) {
                    if (result[i].OrderDate != null)
                        result[i].OrderDate = new Date(result[i].OrderDate);

                    if (result[i].DeadlineDate != null)
                        result[i].DeadlineDate = new Date(result[i].DeadlineDate);
                }
                $scope.data = result;
                isTableLoaded = true;
                coreService.hideInd();
            }, function (err) {
                coreService.hideInd();
                coreService.error(coreService.message.error);
            });
        }
        else {
            orderService.getListByDistributor(distributorID)
           .then(function (response) {
               var result = response.data;
               for (var i = 0; i < result.length; i++) {
                   if (result[i].OrderDate != null)
                       result[i].OrderDate = new Date(result[i].OrderDate);

                   if (result[i].DeadlineDate != null)
                       result[i].DeadlineDate = new Date(result[i].DeadlineDate);
               }
               $scope.data = result;
               isTableLoaded = true;
               coreService.hideInd();
           }, function (err) {
               coreService.hideInd();
               coreService.error(coreService.message.error);
           });
        }
    };

    $scope.masters = function () {
        $('#client').empty();
        $('#product').empty();
        $('#clientType').empty();
        ddlClient();
        ddlStatus();
    };

    var clearValidation = function () {
       $scope.form.$setPristine();
    };

    $scope.reset = function () {
        $scope.oOrder = {};
        $scope.oOrder.OrderID = 0;
        $scope.Comment = '';
        $scope.DeadlineDate = null;
        $scope.isDelete = false;
        clearValidation();
    };

    $scope.add = function () {
        $scope.header = 'ADD';
        $scope.submitTxt = 'Confirm';
        $scope.isEditable = false;
        $scope.masters();
        $scope.reset()
    };

    $scope.edit = function (orderId) {

        $scope.header = 'EDIT';
        $scope.submitTxt = 'Update';
        $scope.isEditable = true;
      
        $scope.reset();

        coreService.showInd();
        orderService.get(orderId)
        .then(function (response) {
            coreService.hideInd();
            var result = response.data;

            $scope.oOrder = result;
            $scope.Comment = result.Comment;

            clientId = result.ClientID.toString();
            productId = result.ProductID.toString();
            clientTypeId = result.ClientTypeID.toString();
            statusId = result.OrderStatusID.toString();

            if (statusId == '1' || profileName == 'Superadmin') {
                $scope.isCancel = true;
            }
            else {
                $scope.isCancel = false;
            }

            $scope.masters();
            if (result.DeadlineDate != null) {
                $scope.DeadlineDate = new Date(result.DeadlineDate);
            }

        }, function (err) {
            coreService.hideInd();
            coreService.error(coreService.message.error);
        });
    };

    $scope.orderStatus = function (orderId) {

        orderService.get(orderId)
        .then(function (response) {
            coreService.hideInd();
            var status = response.data.OrderStatus.Status;
            $scope.trackStatus = [];

            var currentStatus = 'complete';

            var j = 1;
            for (var i = 0; i < $scope.status.length; i++) {
                if (status == 'Cancel') {
                    if ($scope.status[i].Status == 'Pending' || $scope.status[i].Status == 'Cancel') {

                        $scope.trackStatus.push(
                        {
                            OrderStatusID: $scope.status[i].OrderStatusID,
                            Status: $scope.status[i].Status,
                            CurrentStatus: currentStatus
                        }
                    );

                    }
                }
                else {
                    if (status == $scope.status[i].Status) {
                        currentStatus = 'active';
                    }

                    if ($scope.status[i].Status != 'Cancel') {
                        $scope.trackStatus.push(
                            {
                                OrderStatusID: $scope.status[i].OrderStatusID,
                                Status: $scope.status[i].Status,
                                CurrentStatus: currentStatus
                            }

                        );
                    }

                    if (currentStatus == 'active') {
                        currentStatus = 'disabled';
                    }
                }
            }
           

        }, function (err) {
            coreService.hideInd();
            coreService.error(coreService.message.error);
        });
    };

    $scope.submit = function () {

        var oOrder = {};
        oOrder.OrderID = $scope.oOrder.OrderID;
        oOrder.ClientID = $('#client').find(":selected").val();
        oOrder.ProductID = $('#product').find(":selected").val();
        oOrder.ClientTypeID = $('#clientType').find(":selected").val();
        oOrder.HardwareQauntity = $scope.oOrder.HardwareQauntity;
        oOrder.DeadLineDate = $scope.DeadlineDate;
        oOrder.Comment = $scope.Comment;
        
        oOrder.CreatedOn = new Date();
        oOrder.CreatedBy = userId;
        oOrder.ModifiedOn = new Date();
        oOrder.ModifiedBy = userId;

        if ($scope.oOrder.OrderID == 0) {
            oOrder.OrderStatusID = 1;
            coreService.showInd();
            orderService.add(oOrder)
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
            oOrder.OrderStatusID = $('#status').find(":selected").val() == '' ? 0 : $('#status').find(":selected").val();

            coreService.showInd();
            orderService.update(oOrder)
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

    $scope.delete = function () {

        if (confirm("Are you sure to delete this record?")) {
            coreService.showInd();
            deviceService.delete($scope.oDevice)
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
           console.log(result);
           $('#product').empty();
           $('#product').append('<option value=""></option>');
           $.each(result, function (k, v) {
               $('#product').append('<option value="' + v.ProductMaster.ProductID + '">' + v.ProductMaster.ProductName + '</option>');
           });

           if ($scope.isEditable) {
               $('#product').val(productId);
               $scope.form.product.$setDirty();
               ddlClientType(productId);
           }

           $('.standardSelect').trigger("chosen:updated");

       }, function (err) {
           coreService.hideInd();
           console.log(err.data);
       });
    }
    var ddlClientType = function (productId) {
        productService.getClientTypeList(productId)
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
               console.log(err.data);
           });
    }
    var ddlStatus = function () {
        orderService.getOrderStatusList()
       .then(function (response) {

           var result = response.data;
           $scope.status = result;
           result = $.grep(result, function (a) { return  a.Status == 'Cancel'; });
           $('#status').empty();
           $('#status').append('<option value=""></option>');
           $.each(result, function (k, v) {
               $('#status').append('<option value="' + v.OrderStatusID + '">' + v.Status + '</option>');
           });

           if ($scope.isEditable) {
               $('#status').val(statusId);
           }

           $('.standardSelect').trigger("chosen:updated");

       }, function (err) {
           coreService.hideInd();
           console.log(err.data);
       });
    };

    $("#client").change(function () {
        $scope.form.client.$setDirty();

        var clientId = $('#client').find(":selected").val();
        ddlProduct(clientId);
    });

    $("#product").change(function () {
        $scope.form.product.$setDirty();
      //  $scope.form.clientType.$setPristine();
        var productId = $('#product').find(":selected").val();
        ddlClientType(productId);
    });

    $("#clientType").change(function () {
        $scope.form.clientType.$setDirty();
    });

    $("#status").change(function () {
        $scope.form.status.$setDirty();
    });

});