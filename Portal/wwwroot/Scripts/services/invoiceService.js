app.service('invoiceService', function ($http) {

    this.add = function (oInvoice) {
        return $http.post('/Invoice/Add', oInvoice);

    };

    this.update = function (oInvoice) {
        return $http.post('/Invoice/Update', oInvoice);

    };

    this.getList = function () {
        return $http.get('/Invoice/GetList');

    };

    this.get = function (invoiceId) {
        return $http.get('/Invoice/Get?invoiceId=' + invoiceId);

    };

    this.getOrderList = function (clientId, productId, isEditable) {
        return $http.get('/Invoice/GetOrderList?clientId=' + clientId + '&productId=' + productId + '&isEditable=' + isEditable);

    };

    this.getPaymentMode = function () {
        return $http.get('/Invoice/GetPaymentMode');

    };

    this.getPaymentStatus = function () {
        return $http.get('/Invoice/GetPaymentStatus');

    };

    this.getInvoiceHeaders = function () {
        return $http.get('/Invoice/GetInvoiceHeaders');

    };

    this.getInventoryList = function (orderId) {
        return $http.get('/Invoice/GetInventoryList?orderId=' + orderId);

    };

    this.delete = function (oInvoice) {
        return $http.post('/Invoice/Delete', oInvoice);

    };

    this.makeInvoice = function (oInvoice) {
        return $http.post('/Invoice/MakeInvoice', oInvoice);

    };
});