//var $loading = $('#loadingIcon').hide();

$(document)
  .ajaxStart(function () {
      $("div#loadingIcon").addClass('show');
      //$loading.show();
  })
  .ajaxStop(function () {
      $("div#loadingIcon").removeClass('show');
      //$loading.hide();
  });