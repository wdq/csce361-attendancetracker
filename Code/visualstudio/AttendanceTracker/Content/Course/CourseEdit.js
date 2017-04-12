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
        if ($('#Form').parsley().validate() && typeof ($('.location-typeahead').attr("value")) != 'undefined') {
            $.ajax({
                url: "EditPost",
                type: "POST",
                data: ko.toJSON(self),
                contentType: "application/json",
                success: function (data) {
                    if (data.length == 36) {
                        console.log(data);
                        var objectData = data;

                        window.location.href = "View?id=" + objectData;
                    } else {
                        alert("Error saving course.");
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

$(document).ready(function () {
    window.Parsley
      .addValidator('validlocation', {
          requirementType: 'string',
          validateString: function (value, requirement) {
              if (typeof ($('.location-typeahead').attr("value")) === 'undefined') {
                  console.log("bad location");
                  return false;
              } else {
                  console.log("good location");
                  return true;
              }
          },
          messages: {
              en: 'Please select a location from the list.'
         } 
      });
    $('#Form').parsley();
});

$(document).ready(function () {
    var map = {};
    var states = [];

    var getData = $.get("../Shared/PossibleLocations", function (data) {

        var box = $('.location-typeahead').val();

        $.each(data.Locations, function (i, item) {
            if (item.Id == box) {
                $('.location-typeahead').val(item.Name);
            }
        });

        $('.location-typeahead').bind('typeahead:change', function (ev, suggestion) {
            $.each(data.Locations, function (i, item) {
                if (item.Name == suggestion) {
                    $('.location-typeahead').attr("value", item.Id);
                    window.viewModel.LocationRoomId(item.Id);
                }
            });
        });

        $('.location-typeahead').typeahead({
            hint: true,
            highlight: true
        },
        {
            name: 'states',
            source: function (query, process) {
                map = {};
                states = [];

                var substrRegex = new RegExp(query, 'i');

                $.each(data.Locations, function (i, state) {
                    if (substrRegex.test(state.Name)) {
                        map[state.Name] = state;
                        states.push(state.Name);
                    }
                })
                process(states);
            }
        }).focus();

    });
});