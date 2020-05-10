app.controller('invoiceCTRL', function ($scope, $timeout, coreService, clientService, productService, invoiceService) {

    document.getElementById("template").style.visibility = "visible";
    var userId = coreService.getUser().UserID;
    var clientId = 0;
    var productId = 0;
    var orderId = 0;
    var paymentStatusId = 0;
    var paymentModeId = 0;

    $scope.dispatchDetails = [];
    $scope.billingDetails = [];

    $scope.initialize = function () {
        $('#bootstrap-data-table').DataTable().clear().destroy();
        coreService.showInd();
        invoiceService.getList()
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
        $('#client').empty();
        $('#product').empty();
        $('#order').empty();
        $('#paymentMode').empty();
        $('#paymentStatus').empty();

        ddlClient();
        ddlPaymentMode();
        ddlPaymentStatus();
    };

    var clearValidation = function () {
        $scope.form.$setPristine();
    };

    $scope.reset = function () {
        $scope.oInvoice = {};
        $scope.oInvoice.InvoiceID = 0;
        $scope.PaymentDate = null;
        $scope.dispatchDetails = [];
        $scope.billingDetails = [];
        clearValidation();
    };

    $scope.add = function () {
        $scope.header = 'ADD';
        $scope.submitTxt = 'Confirm';
        $scope.isEditable = false;
        $scope.IsDraft = true;
        $scope.reset();
        $scope.masters();
    };

    $scope.edit = function (invoiceId) {

        $scope.header = 'EDIT';
        $scope.submitTxt = 'Update';
        $scope.isEditable = true;
        $scope.reset();

        coreService.showInd();
        invoiceService.get(invoiceId)
        .then(function (response) {
            coreService.hideInd();
            var result = response.data;

            $scope.oInvoice.InvoiceID = result.InvoiceID;
            clientId = result.ClientID.toString();
            productId = result.ProductID.toString();
            orderId = result.OrderID.toString();
            paymentStatusId = result.PaymentStatusID.toString();
            paymentModeId = result.PaymentModeID;

            if (result.PaymentDate != null) {
                $scope.PaymentDate = new Date(result.PaymentDate);
            }

            $scope.TotalAmount = result.TotalAmount;
            $scope.IsDraft = result.IsDraft;
            $scope.dispatchDetails = [];
            $scope.billingDetails = [];

            $scope.masters();

            var invoiceDetails = result.InvoiceDetails;
            for (var i = 0; i < invoiceDetails.length; i++) {
                if (invoiceDetails[i].InventoryID != null) {

                    if (invoiceDetails[i].InventoryMaster.DeviceMaster != null) {
                        $scope.dispatchDetails.push({ InventoryID: invoiceDetails[i].InventoryID, InventoryName: invoiceDetails[i].InventoryMaster.DeviceMaster.DeviceName, Amount: invoiceDetails[i].Amount })
                    }

                    if (invoiceDetails[i].InventoryMaster.SpareMaster != null) {
                        $scope.dispatchDetails.push({ InventoryID: invoiceDetails[i].InventoryID, InventoryName: invoiceDetails[i].InventoryMaster.SpareMaster.SpareName, Amount: invoiceDetails[i].Amount })
                    }
                }
                else if (invoiceDetails[i].InvoiceHeaderID != 0) {
                    $scope.billingDetails.push({ InvoiceHeaderID: invoiceDetails[i].InvoiceHeaderID, Header: invoiceDetails[i].InvoiceHeader.Header, Amount: invoiceDetails[i].Amount,IsSoftware: invoiceDetails[i].InvoiceHeader.IsSoftware });
                }
            }

        }, function (err) {
            coreService.hideInd();
            coreService.error(coreService.message.error);
        });
    };

    $scope.submit = function (draft) {

        var oInvoice = {};
        oInvoice.InvoiceID = $scope.oInvoice.InvoiceID;
        oInvoice.ClientID = $('#client').find(":selected").val();
        oInvoice.ProductID = $('#product').find(":selected").val();
        oInvoice.OrderID = $('#order').find(":selected").val();
        oInvoice.PaymentStatusID = $('#paymentStatus').find(":selected").val();
        oInvoice.PaymentModeID = $('#paymentMode').find(":selected").val();
        oInvoice.PaymentDate = $scope.PaymentDate;
        oInvoice.IsDraft = draft;
        oInvoice.Name = $scope.Client.ClientName;
        oInvoice.ContactPerson = $scope.Client.ContactPerson;
        oInvoice.TotalAmount = $scope.TotalAmount;

        oInvoice.CreatedOn = new Date();
        oInvoice.CreatedBy = userId;
        oInvoice.ModifiedOn = new Date();
        oInvoice.ModifiedBy = userId;

        var oInvoiceDetails = [];

        for (var i = 0; i < $scope.dispatchDetails.length; i++) {
            oInvoiceDetails.push({ InventoryID: $scope.dispatchDetails[i].InventoryID, Amount: $scope.dispatchDetails[i].Amount });
        }

        for (var j = 0; j < $scope.billingDetails.length; j++) {
            oInvoiceDetails.push({ InvoiceHeaderID: $scope.billingDetails[j].InvoiceHeaderID, Amount: $scope.billingDetails[j].Amount });
        }

        oInvoice.InvoiceDetails = oInvoiceDetails;

        if ($scope.oInvoice.InvoiceID == 0) {
            coreService.showInd();
            invoiceService.add(oInvoice)
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
            invoiceService.update(oInvoice)
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
            invoiceService.delete($scope.oInvoice)
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

    $scope.makeInvoice = function (invoiceId) {
        var oInvoice = {};
        oInvoice.InvoiceID = invoiceId;

        coreService.showInd();
        invoiceService.makeInvoice(oInvoice)
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

    function saveByteArray(invoiceName, byte) {
        var blob = new Blob([byte], { type: "application/pdf" });
        var link = document.createElement('a');
        link.href = window.URL.createObjectURL(blob);
        link.download = invoiceName;
        link.click();
    };

    $scope.removeHeader = function (index) {
        $scope.billingDetails.splice(index, 1);
        $scope.total();
    };

    $scope.total = function () {
        var total = 0;

        for (var i = 0; i < $scope.billingDetails.length; i++) {
            total = total + parseFloat($scope.billingDetails[i].Amount);
        }

        for (var j = 0; j < $scope.dispatchDetails.length; j++) {
            total = total + parseFloat($scope.dispatchDetails[j].Amount);
        }

        $scope.TotalAmount = total;
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
               getClientDetails(clientId);
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
               ddlOrder(clientId, productId);
           }

           $('.standardSelect').trigger("chosen:updated");

       }, function (err) {
           coreService.hideInd();
           console.log(err.data);
       });
    }
    var ddlOrder = function (clientId, productId) {
        invoiceService.getOrderList(clientId, productId, $scope.isEditable)
       .then(function (response) {

           var result = response.data;
           console.log(result);
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
                   if (v.OrderStatus.Status == 'Pending' || v.OrderStatus.Status == 'Device Ready')
                       $('#order').append('<option value="' + v.OrderID + '">' + v.OrderNumber + '</option>');
               });
           }

           $('.standardSelect').trigger("chosen:updated");

       }, function (err) {
           coreService.hideInd();
           console.log(err.data);
       });
    };
    var ddlPaymentMode = function () {
        invoiceService.getPaymentMode()
       .then(function (response) {

           var result = response.data;
           $('#paymentMode').empty();
           $('#paymentMode').append('<option value=""></option>');
           $.each(result, function (k, v) {
               $('#paymentMode').append('<option value="' + v.PaymentModeID + '">' + v.Mode + '</option>');
           });

           if ($scope.isEditable) {
               if (paymentModeId != null) {
                   $('#paymentMode').val(paymentModeId);
                   $scope.form.paymentMode.$setDirty();
               }
           }

           $('.standardSelect').trigger("chosen:updated");

       }, function (err) {
           coreService.hideInd();
           console.log(err.data);
       });
    };
    var ddlPaymentStatus = function () {
        invoiceService.getPaymentStatus()
       .then(function (response) {
           $scope.IsPaid = false;

           var result = response.data;
           $('#paymentStatus').empty();
           $('#paymentStatus').append('<option value=""></option>');


           if ($scope.isEditable) {

               if (paymentStatusId == 3) {
                    result = $.grep(result, function (a) { return a.Status == 'Pending'; });
               }
               else {
                    result = $.grep(result, function (a) { return a.Status != 'Pending'; });
               }

               $.each(result, function (k, v) {

                   $('#paymentStatus').append('<option value="' + v.PaymentStatusID + '">' + v.Status + '</option>');

               });

               $('#paymentStatus').val(paymentStatusId);
               $scope.form.paymentStatus.$setDirty();

               var status = $('#paymentStatus').find(":selected").text();

               if (status == 'Paid')
                   $scope.IsPaid = true;

           }
           else {
               $.each(result, function (k, v) {
                   if (v.Status == 'Pending')
                       $('#paymentStatus').append('<option value="' + v.PaymentStatusID + '">' + v.Status + '</option>');
               });
           }

           $('.standardSelect').trigger("chosen:updated");

       }, function (err) {
           coreService.hideInd();
           console.log(err.data);
       });
    };

    var getInventoryList = function (orderId) {

        invoiceService.getInventoryList(orderId)
         .then(function (response) {
             var result = response.data;
             $scope.dispatchDetails = [];

             for (var i = 0; i < result.length; i++) {

                 if (result[i].InventoryMaster.DeviceMaster != null) {
                     $scope.dispatchDetails.push({ InventoryID: result[i].InventoryMaster.InventoryID, InventoryName: result[i].InventoryMaster.DeviceMaster.DeviceName, Amount: result[i].InventoryMaster.DeviceMaster.Amount })

                 }

                 if (result[i].InventoryMaster.SpareMaster != null) {
                     $scope.dispatchDetails.push({ InventoryID: result[i].InventoryMaster.InventoryID, InventoryName: result[i].InventoryMaster.SpareMaster.SpareName, Amount: result[i].InventoryMaster.SpareMaster.Amount })

                 }
             }

             $scope.total();
         }, function (err) {
             coreService.hideInd();
             console.log(err.data);
         });
    };
    var getInvoiceHeaders = function () {
        $scope.billingDetails = [];

        invoiceService.getInvoiceHeaders()
         .then(function (response) {
             var result = response.data;

             for (var i = 0; i < result.length; i++) {
                 if (result[i].IsSoftware) {
                     var productId = Number($('#product').find(":selected").val());
                     if (result[i].ProductID == productId) {
                         $scope.billingDetails.push({ InvoiceHeaderID: result[i].InvoiceHeaderID, Header: result[i].Header, Amount: result[i].Amount, IsSoftware: true });
                     }
                 }
                 else {
                     $scope.billingDetails.push({ InvoiceHeaderID: result[i].InvoiceHeaderID, Header: result[i].Header, Amount: result[i].Amount, IsSoftware: false });
                 }
             }

             $scope.total();
         }, function (err) {
             coreService.hideInd();
             console.log(err.data);
         });
    };
    var getClientDetails = function (clientId) {
        clientService.get(clientId)
         .then(function (response) {
             $scope.Client = response.data;
         }, function (err) {
             coreService.error(coreService.message.error);
         });
    };

    $("#client").change(function () {
        $scope.form.client.$setDirty();

        var clientId = $('#client').find(":selected").val();
        ddlProduct(clientId);
        getClientDetails(clientId);
    });

    $("#product").change(function () {
        $scope.form.product.$setDirty();
        var clientId = $('#client').find(":selected").val();
        var productId = $('#product').find(":selected").val();
        ddlOrder(clientId, productId);
    });

    $("#order").change(function () {
        $scope.form.order.$setDirty();
        var orderId = $('#order').find(":selected").val();
        getInventoryList(orderId);
        getInvoiceHeaders();
    });

    $("#paymentStatus").change(function () {
        $scope.form.paymentStatus.$setDirty();
    });
});