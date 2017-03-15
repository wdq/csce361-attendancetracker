var ViewModel = function (data) {
    var self = this;

    ko.mapping.fromJS(data, null, self);

    self.serializeForm = function (form) {
        var array = form.serializeArray();
        var json = {};

        $.each(array, function () {
            json[this.name] = this.value || '';
        });

        return json;
    };

    self.save = function () {
        if ($('#Form').parsley().validate()) {
            $.ajax({
                url: "EditBluetoothPost",
                type: "POST",
                data: ko.toJSON(self),
                contentType: "application/json",
                success: function (data) {
                    if (data.length == 36) {
                        console.log(data);
                        var objectData = data;

                        window.location.href = "View?id=" + objectData;
                    } else {
                        alert("Error saving user Bluetooth.");
                    }
                }
            });
        } else {
            alert("Error: invalid form input.");
        }
    }
}

$(function () {
    window.viewModel = new ViewModel(initialModel);
    ko.applyBindings(window.viewModel);
})