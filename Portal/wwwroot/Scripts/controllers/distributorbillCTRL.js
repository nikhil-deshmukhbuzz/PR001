app.controller('distributorbillCTRL', function ($scope, $timeout, coreService, distributorbillService, distributorService, clientService, productService, invoiceService) {

    document.getElementById("template").style.visibility = "visible";
    var profileName = coreService.getUser().ProfileMaster.ProfileName;

    if (profileName == 'Distributor') {
        $scope.isAdmin = false;
    }
    else {
        $scope.isAdmin = true;
    }

    var distributorID = coreService.getUser().DistributorID;
    var userId = coreService.getUser().UserID;
    var clientId = 0;
    var productId = 0;
    var orderId = 0;
    var paymentStatusId = 0;
    var paymentModeId = 0;
    var distributorId = 0;
    var currentYear = new Date().getFullYear();
    var currentMonth = new Date().getMonth();

    $scope.initialize = function () {
        $('#bootstrap-data-table').DataTable().clear().destroy();
        coreService.showInd();
        distributorbillService.getList()
        .then(function (response) {
            var result = response.data;

            for (var i = 0; i < result.length; i++) {
                result[i].CreatedOn = new Date(result[i].CreatedOn);
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
        $('#paymentMode').empty();
        $('#paymentStatus').empty();
        $('#distributor').empty();

        if (profileName != 'Distributor') {
            ddlClient();
            ddlPaymentMode();
            ddlPaymentStatus();
            ddlDistributor();
        }
    };

    var clearValidation = function () {
        $scope.form.$setPristine();
    };

    $scope.reset = function () {
        $scope.oDistributorBill = {};
        $scope.oDistributorBill.InvoiceID = 0;
        $scope.PaymentDate = null;
        clearValidation();
    };

    $scope.edit = function (distributorbillId) {

        $scope.header = 'EDIT';
        $scope.submitTxt = 'Update';
        $scope.isEditable = true;
        $scope.reset();

        coreService.showInd();
        distributorbillService.get(distributorbillId)
        .then(function (response) {
            coreService.hideInd();
            var result = response.data;

            $scope.oDistributorBill.DistributorBillID = result.DistributorBillID;
            clientId = result.ClientID.toString();
            productId = result.ProductID.toString();
            orderId = result.OrderID.toString();
            paymentStatusId = result.PaymentStatusID.toString();
            paymentModeId = result.PaymentModeID;
            distributorId = result.DistributorID;

            if (result.PaymentDate != null) {
                $scope.PaymentDate = new Date(result.PaymentDate);
            }

            $scope.PaybleAmount = result.PaybleAmount;
            $scope.masters();

        }, function (err) {
            coreService.hideInd();
            coreService.error(coreService.message.error);
        });
    };

    $scope.submit = function () {

        var oDistributorBill = {};
        oDistributorBill.DistributorBillID = $scope.oDistributorBill.DistributorBillID;
        oDistributorBill.PaymentStatusID = $('#paymentStatus').find(":selected").val();
        oDistributorBill.PaymentModeID = $('#paymentMode').find(":selected").val();
        
        oDistributorBill.ModifiedOn = new Date();
       
        coreService.showInd();
        distributorbillService.update(oDistributorBill)
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
        
    };

    $scope.show = function () {
        $('#bootstrap-data-table').DataTable().clear().destroy();
        var distributorId = $('#distributorlist').find(":selected").val();
        var yearId = $('#year').find(":selected").val();
        var monthId = Number($('#month').find(":selected").val()) + 1;

        coreService.showInd();
        distributorbillService.filterByDate(distributorId, monthId, yearId)
      .then(function (response) {
          var result = response.data;

          for (var i = 0; i < result.length; i++) {
              result[i].CreatedOn = new Date(result[i].CreatedOn);
          }

          $scope.data = result;
          isTableLoaded = true;
          totalCal($scope.data);
          coreService.hideInd();
      }, function (err) {
          coreService.hideInd();
          console.log(err.data);
      });
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
    };
    var ddlOrder = function (clientId, productId) {
        invoiceService.getOrderList(clientId, productId,true)
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
    };
    var ddlDistributorList = function () {
        distributorService.getList()
       .then(function (response) {
           var result = response.data;

           $('#distributorlist').empty();
           $('#distributorlist').append('<option value=""></option>');
           $('#distributorlist').append('<option value="0">ALL</option>');
           $.each(result, function (k, v) {
               $('#distributorlist').append('<option value="' + v.DistributorID + '">' + v.DistributorName + '</option>');
           });

           if (profileName == 'Distributor') {
               $scope.isAdmin = false;
               $('#distributorlist').val(distributorID);
           }
           else {
               $('#distributorlist').val('0');
           }

          

           $('.standardSelect').trigger("chosen:updated");

           $scope.show();

       }, function (err) {
           coreService.hideInd();
           console.log(err.data);
       });
    };
    var ddlYear = function () {

        $('#year').empty();
        $('#year').append('<option value=""></option>');

        for (var i = 2018; i <= currentYear; i++) {
            $('#year').append('<option value="' + i + '">' + i + '</option>');
        }

        $('#year').val(currentYear);

        $('.standardSelect').trigger("chosen:updated");
    };
    var ddlMonth = function () {
        var month = [
            { MonthID: 0, MonthName: 'January' },
            { MonthID: 1, MonthName: 'February' },
            { MonthID: 2, MonthName: 'March' },
            { MonthID: 3, MonthName: 'April' },
            { MonthID: 4, MonthName: 'May' },
            { MonthID: 5, MonthName: 'June' },
            { MonthID: 6, MonthName: 'July' },
            { MonthID: 7, MonthName: 'August' },
            { MonthID: 8, MonthName: 'September' },
            { MonthID: 9,MonthName: 'October' },
            { MonthID: 10,MonthName: 'November' },
            { MonthID: 11,MonthName: 'December' },
        ];

        $('#month').empty();
        $('#month').append('<option value=""></option>');
        $.each(month, function (k, v) {
            $('#month').append('<option value="' + v.MonthID + '">' + v.MonthName + '</option>');
        });

        $('#month').val(currentMonth);

        $('.standardSelect').trigger("chosen:updated");
    };

    coreService.showInd();
    $timeout(function () {
        ddlYear();
        ddlMonth();
        ddlDistributorList();
        coreService.hideInd();
    }, 2000);
   
    var totalCal = function (data) {
        $scope.totalAmount = 0;

        for (var i = 0; i < data.length; i++) {
            $scope.totalAmount = $scope.totalAmount + data[i].PaybleAmount;
        }
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