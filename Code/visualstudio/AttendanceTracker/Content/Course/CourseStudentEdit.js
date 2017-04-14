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

        if ($('#Form').parsley().validate() && typeof($('.student-typeahead').attr("value")) !== "undefined") {
            $.ajax({
                url: "AddStudentPost",
                type: "POST",
                data: ko.toJSON(self),
                contentType: "application/json",
                success: function (data) {
                    if (data.length == 36) {
                        console.log(data);
                        var objectData = data;

                        window.location.href = "View?id=" + objectData;
                    } else {
                        alert("Error adding student.");
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
      .addValidator('validstudent', {
          requirementType: 'string',
          validateString: function (value, requirement) {
              if (typeof ($('.student-typeahead').attr("value")) === 'undefined') {
                  console.log("bad student");
                  return false;
              } else {
                  console.log("good student");
                  return true;
              }
          },
          messages: {
              en: 'Please select a student from the list.'
          }
      });
    $('#Form').parsley();
});

$(document).ready(function () {
    var map = {};
    var states = [];

    var getData = $.get("../Shared/PossibleUsers", function (data) {

        var box = $('.student-typeahead').val();

        $.each(data.Users, function (i, item) {
            if (item.Id == box) {
                $('.student-typeahead').val(item.Name);
            }
        });

        $('.student-typeahead').bind('typeahead:change', function (ev, suggestion) {
            $.each(data.Users, function (i, item) {
                if (item.Name == suggestion) {
                    $('.student-typeahead').attr("value", item.Id);
                    window.viewModel.StudentId(item.Id);
                }
            });
        });

        $('.student-typeahead').typeahead({
            hint: true,
            highlight: true
        },
        {
            name: 'states',
            source: function (query, process) {
                map = {};
                states = [];

                var substrRegex = new RegExp(query, 'i');

                $.each(data.Users, function (i, state) {
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