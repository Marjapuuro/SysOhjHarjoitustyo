var ViewModel = function () {
    var self = this;
    self.models = ko.observableArray();
    self.error = ko.observable();

    var modelsUri = '/api/Models/';

    function ajaxHelper(uri, method, data) {
        self.error(''); // Clear error message
        return $.ajax({
            type: method,
            url: uri,
            dataType: 'json',
            contentType: 'application/json',
            data: data ? JSON.stringify(data) : null
        }).fail(function (jqXHR, textStatus, errorThrown) {
            self.error(errorThrown);
        });
    }

    function getAllModels() {
        ajaxHelper(modelsUri, 'GET').done(function (data) {
            self.models(data);
        });
    }

    // Fetch the initial data.
    getAllModels();

    self.detail = ko.observable();

    self.getModelDetail = function (item) {
        ajaxHelper(modelsUri + item.Id, 'GET').done(function (data) {
            self.detail(data);
        });
    }

    self.manufacturers = ko.observableArray();
    self.newModel = {
        Manufacturer: ko.observable(),
        FuelType: ko.observable(),
        Price: ko.observable(),
        Name: ko.observable(),
        Year: ko.observable()
    }

    var manufacturersUri = '/api/Manufacturers/';

    function getManufacturers() {
        ajaxHelper(manufacturersUri, 'GET').done(function (data) {
            self.manufacturers(data);
        });
    }

    self.addModel = function (formElement) {
        var model = {
            ManufacturerId: self.newModel.Manufacturer().Id,
            FuelType: self.newModel.FuelType(),
            Price: self.newModel.Price(),
            Name: self.newModel.Name(),
            Year: self.newModel.Year()
        };

        ajaxHelper(modelsUri, 'POST', model).done(function (item) {
            self.models.push(item);
        });
    }

    getManufacturers();
};

ko.applyBindings(new ViewModel());