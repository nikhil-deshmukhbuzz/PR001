app.service('dispatchService', function ($http) {

    this.add = function (oDispatch) {
        return $http.post('/Dispatch/Add', oDispatch);

    };

    this.update = function (oDispatch) {
        return $http.post('/Dispatch/Update', oDispatch);

    };

    this.getList = function () {
        return $http.get('/Dispatch/GetList');
    };

    this.get = function (dispatchId) {
        return $http.get('/Dispatch/Get?dispatchId=' + dispatchId);

    };

    this.getInventoryList = function (isEditable) {
        return $http.get('/Dispatch/GetInventoryList?isEditable=' + isEditable);

    };

    this.getOrderList = function (clientId, productId) {
        return $http.get('/Dispatch/GetOrderList?clientId=' + clientId + '&productId=' + productId);

    };

    this.delete = function (oDispatch) {
        return $http.post('/Dispatch/Delete', oDispatch);

    };

    this.makeInvoice = function (oDispatch) {
        return $http.post('/Dispatch/MakeInvoice', oDispatch);

    };

    this.invoiceAvailable = function (orderId) {
        return $http.get('/Dispatch/InvoiceAvailable?orderId=' + orderId);
    };

    this.draftAvailable = function (orderId) {
        return $http.get('/Dispatch/DraftAvailable?orderId=' + orderId);
    };

    this.getShippingAddress = function (clientId) {
        return $http.get('/Dispatch/GetShippingAddress?clientId=' + clientId);
    };

});