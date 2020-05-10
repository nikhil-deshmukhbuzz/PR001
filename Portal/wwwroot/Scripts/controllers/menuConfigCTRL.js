var isLoad = false;
app.controller('menuConfigCTRL', function ($scope, $timeout, coreService, menuService) {
  
    document.getElementById("template").style.visibility = "visible";

    var clientId = 0;
    // var menuId = 0;
    var userId = 1;
   // var profileType = 'ConfigurationAdmin';
    var profileType = 'Customer';

    
    $scope.initialize = function () {
        $scope.isMenuCard = false;
        $('#client').empty();
        $('#menu').empty();
        ddlClient();
        $scope.masters();
    };

    $scope.masters = function () {
        $scope.isMenuCard = false;
        $scope.IsClientSelected = false;
        if (profileType == 'Customer') {
            $scope.IsCustomer = true;
            clientId = 1001;
            ddlMenu(clientId);
        }
        else {

            $scope.IsCustomer = false;
            ddlClient();
        }
    };

    var clearValidation = function () {
        $scope.form.$setPristine();
    };

    $scope.reset = function () {

        $scope.oMenu = {};
        $scope.oMenu.DeviceMenuID = 0;
        $scope.oMenu.ClientID = $('#client').find(":selected").val();
        $scope.oMenu.ClientName = $('#client').find(":selected").text();
        $scope.oMenu.Title = '';
        $scope.oMenu.IsActive = true;
        clearValidation();
    };

    $scope.add = function () {
        $scope.header = 'ADD';
        $scope.submitTxt = 'Confirm';
        $scope.isEditable = false;
        $scope.reset();
    };

    $scope.edit = function () {

        $scope.header = 'EDIT';
        $scope.submitTxt = 'Update';
        $scope.reset();
        menuId = $('#menu').find(":selected").val();

        coreService.showInd();
        menuService.getMenu(menuId)
        .then(function (response) {
            coreService.hideInd();
            $scope.oMenu = response.data;
            $scope.oMenu.ClientName = $('#client').find(":selected").text();
        }, function (err) {
            coreService.hideInd();
            coreService.error(coreService.message.error);
        });
    };

    $scope.submit = function () {

        if ($scope.oMenu.DeviceMenuID == 0) {
            coreService.showInd();
            menuService.add($scope.oMenu)
            .then(function (response) {
                coreService.hideInd();
                if (response.data == 1) {
                    coreService.success(coreService.message.added);
                }
                else if (response.data == 2) {
                    coreService.error(coreService.message.limitExceed);
                }
                else {
                    coreService.success(coreService.message.error);
                }

                $scope.initialize();
            }, function (err) {
                coreService.hideInd();
                coreService.error(coreService.message.error);
            });
        }
        else {
            coreService.showInd();
            menuService.update($scope.oMenu)
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

    $scope.refresh = function () {
        menuId = $('#menu').find(":selected").val();
        $scope.menuName = $('#menu').find(":selected").text();
        bindMenuDetails(menuId);
    };

    $scope.save = function () {
        var oDeviceMenu = {};
        oDeviceMenu.DeviceMenuID = menuId;
        oDeviceMenu.MenuHeader = $scope.data;
        coreService.showInd();
        menuService.save(oDeviceMenu)
        .then(function (response) {
            coreService.hideInd();
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
    };
    
    //configuration

    $scope.configuration = function () {
        $scope.header = 'ADD';
        $scope.submitTxt = 'Confirm';
        $scope.installation_invalid = true;
        $scope.configForm.$setPristine();

        if (profileType == 'Customer') {
            clientId = 1001;
            ddlConfigMenu(clientId);
            ddlInstallation(clientId);
        }
        else {
            $scope.oConfig = {};
            $scope.oConfig.ClientName = $('#client').find(":selected").text();
            $scope.menuName = $('#menu').find(":selected").text();
            ddlConfigMenu(clientId);
            ddlInstallation(clientId);
            }
    };

    $scope.configSave = function () {
        var oConfig = [];
        var menuId = $('#configmenu').find(":selected").val();
        var listOfInstallationId = $('#device').chosen().val();
        for (var i = 0; i < listOfInstallationId.length; i++) {
            oConfig.push({ DeviceMenuID: menuId, InstallationID: listOfInstallationId[i] });
        }

        console.log(oConfig);

        coreService.showInd();
        menuService.configSave(oConfig)
        .then(function (response) {
            coreService.hideInd();
            if (response.data == 1) {
                coreService.success(coreService.message.updated);
            }
            else {
                coreService.success(coreService.message.error);
            }

            $scope.initialize();
        }, function (err) {
            coreService.hideInd();
            coreService.error(coreService.message.error);
        });
    };

    var ddlInstallation = function (clientId) {
        menuService.getInstallationList(clientId)
       .then(function (response) {

           var result = response.data;
           $('#device').empty();
           $('#device').append('<option value=""></option>');
           $.each(result, function (k, v) {
               $('#device').append('<option value="' + v.InstallationID + '">' + v.DeviceName + '</option>');
           });

           $('.standardSelect').trigger("chosen:updated");

       }, function (err) {
           coreService.hideInd();
           console.log(error.data);
       });
    };
    var ddlConfigMenu = function (clientId) {
        menuService.getList(clientId)
       .then(function (response) {

           var result = response.data;
           $('#configmenu').empty();
           $('#configmenu').append('<option value=""></option>');
           $.each(result, function (k, v) {
               $('#configmenu').append('<option value="' + v.DeviceMenuID + '">' + v.Title + '</option>');
           });

           $('.standardSelect').trigger("chosen:updated");

       }, function (err) {
           coreService.hideInd();
           console.log(error.data);
       });
    };
    var bindInstallation = function (deviceMenuId) {
        menuService.getDeviceMenuLink(deviceMenuId)
        .then(function (response) {
            var result = response.data;
            var installation = [];

            if (result.length > 0) {
                for (var i = 0; i < result.length; i++) {
                    installation.push(result[i].InstallationID);
                }

                console.log(installation);
                $("#device").val(installation).trigger("chosen:updated");
                $scope.installation_invalid = false;
            }
            else {
                $("#device").val(installation).trigger("chosen:updated");
                $scope.installation_invalid = true;
            }

        }, function (err) {
            coreService.hideInd();
            console.log(error.data);
        });
       
    };

    $("#configmenu").change(function () {
        $scope.configForm.configmenu.$setDirty();
        var deviceMenuId = $('#configmenu').find(":selected").val();
        bindInstallation(deviceMenuId);
    });

    $('#device').chosen().change(function () {
        var listOfInstallationId = $('#device').chosen().val();
        if (listOfInstallationId == null) {
            $scope.installation_invalid = true;
        }
        else {
            $scope.installation_invalid = false;
        }

    });
    //


    var ddlClient = function () {
        menuService.getClientList()
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
    var ddlMenu = function (clientId) {
        menuService.getList(clientId)
       .then(function (response) {

           var result = response.data;
           $('#menu').empty();
           $('#menu').append('<option value=""></option>');
           $.each(result, function (k, v) {
               $('#menu').append('<option value="' + v.DeviceMenuID + '">' + v.Title + '</option>');
           });

           $('.standardSelect').trigger("chosen:updated");

       }, function (err) {
           coreService.hideInd();
           console.log(error.data);
       });
    };
    var bindMenuDetails = function (menuId) {
        className1 = "tf-child-true";
        style1 = "padding-left: 20px;";
        isNew = true;

        coreService.showInd();
        menuService.get(menuId)
       .then(function (response) {
           coreService.hideInd();
           var result = response.data;
           $scope.data = [];
           $scope.isMenuCard = true;
           for (var i = 0; i < result.length; i++) {
               var details = result[i].MenuDetails;
               var MenuDetails = [];
               for (var j = 0; j < details.length; j++) {
                   MenuDetails.push({ "DeviceMenuID": menuId, "MenuHeaderID": details[j].MenuHeaderID, "MenuName": details[j].MenuName, "Small": details[j].Small, "Large": details[j].Large, "Style": "padding-left:20px", "IsEdit": false })
               }
               $scope.data.push({ "DeviceMenuID": menuId, "UserID": userId, "MenuHeaderID": result[i].MenuHeaderID, "Header": result[i].Header, "Small": result[i].Small, "Large": result[i].Large, "IsEdit": false, "IsExtend": false, "Class": className1, "Style": style1, "isNew": isNew, "MenuDetails": MenuDetails });
           }
           
       }, function (err) {
           coreService.hideInd();
           console.log(error.data);
       });
    };

    $("#client").change(function () {
        clientId = $('#client').find(":selected").val();
        $scope.data = [];
        $scope.isMenuCard = false;
        $scope.IsClientSelected = true;
        ddlMenu(clientId);
    });

    $("#menu").change(function () {
        menuId = $('#menu').find(":selected").val();
        $scope.menuName = $('#menu').find(":selected").text();
        bindMenuDetails(menuId);
    });

    ////////////////////////////////////
    isLoad = true;
    var isNew = false;
    var className = "tf-child-true";
    var style = "padding-left: 20px;";
    var className1 = "";
    var style1 = "";
    //header
    $scope.headerClk = function (header, id) {
        header.IsEdit = false;
        if (header.IsExtend)
            header.IsExtend = false;
        else
            header.IsExtend = true;

        if (header.isNew)
            applyTreeFilter(id)
    };

    var applyTreeFilter = function (id) {
        var ID = "#headerID-" + id;
        if ($(ID).hasClass("tf-open")) {
            $(ID).removeClass("tf-open");
        } else {
            $(ID).addClass("tf-open");
        }
    };

    $scope.addHeader = function () {
        $scope.data.push({ "DeviceMenuID": menuId,"MenuHeaderID": 1, "Header": "Default Header", "Small": "", "Large": "", "IsEdit": false, "IsExtend": false, "Class": className, "Style": style, "isNew": true, "MenuDetails": [] });
    };

    $scope.editHeader = function (header) {
        if (header.IsEdit)
            header.IsEdit = false;
        else
            header.IsEdit = true;
    };

    $scope.closeHeader = function (detail) {
        detail.IsEdit = false;
    };

    $scope.removeHeader = function (index) {
        $scope.data.splice(index, 1);
    };
    ////

    //details
    $scope.addDetail = function (header) {
        header.IsEdit = false;
        header.MenuDetails.push({ "DeviceMenuID": menuId, "MenuHeaderID": header.MenuHeaderID, "MenuName": "Default Menu", "Small": "0", "Large": "0", "Style": "padding-left:20px", "IsEdit": false });
    };

    $scope.editDetail = function (detail) {
        if (detail.IsEdit)
            detail.IsEdit = false;
        else
            detail.IsEdit = true;
    };

    $scope.closeDetail = function (detail) {
        detail.IsEdit = false;
    };

    $scope.removeDetail = function (detail, index) {
        detail.splice(index, 1);
    };

    /////

});

$(function () {
    var x = setInterval(function () {
        if (isLoad) {
            console.log('load');
            var tree = new treefilter($("#my-tree"), {
                searcher: $("input#my-search"),
                multiselect: false
            });
            isLoad = false;
        }
    }, 1000);

});