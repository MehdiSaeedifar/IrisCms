/// <reference path="_references.js" />
/// <reference path="jquery-1.8.3.intellisense.js" />
/// <reference path="jquery-1.8.3.js" />
/// <reference path="jquery-ui-1.9.2.js" />


function loadAjaxComponents() {


    checkboxChecked();
    ajaxBind();
    slideButton();
    autoComplete();
    radioButtonSearch();
    $('select').chosen();
}
function showLoading() {

    var top = ($(window).height() - $('div#loading').height()) / 2;

    $('div#loading').css({ 'z-index': '2000', 'top': top + $(document).scrollTop() }).fadeIn('slow');
}
function hideLoading() {
    $('div#loading').fadeOut('slow').css('z-index', '10');
}

function checkboxChecked() {
    $('#user-table input[type=checkbox]').on('mousedown', function () {
        //$this = $(this);
        if (!$(this).is(':checked')) {
            $(this).parents('tr').addClass('selected-row');
        } else {
            $(this).parents('tr').removeClass('selected-row');
        }

    });
}
function ajaxBind() {
    $('[data-i-ajax=true]').each(function () {
        //$this = $(this);
        $(this).off('click');
        $(this).on('click', function () {
            doAjax($(this), $(this).attr('data-i-src'), $(this).attr('data-i-ajax-method'), $(this).attr('data-i-data'), $(this).attr('data-i-target-id'));
        });
    });

}

function doAjax(caller, src, method, data, targetId) {
    showLoading();
    $.ajax({
        type: method,
        url: src,
        data: data,
        dataType: "json",
        complete: function (response, status) {

            hideLoading();
            $(targetId).html(response.responseText);
            if (caller.attr('data-i-show-dialog') == "true") {
                showDialog(targetId, caller);
                loadAjaxComponents();
                run(function () {
                    $(targetId).removeData("validator");
                    $(targetId).removeData("unobtrusiveValidation");
                    $.validator.unobtrusive.parse(targetId); $('#date-picker').datepicker({ changeMonth: true, changeYear: true });
                });
            }

        }

    });
}
var $dialog;
function showDialog(targetId, caller) {
    var width = caller.attr('data-i-dialog-width');
    var height = caller.attr('data-i-dialog-height');
    var showEffect = caller.attr('data-i-dialog-show-effect');
    var showDir = caller.attr('data-i-dialog-show-dir');
    var hideEffect = caller.attr('data-i-dialog-hide-effect');
    var hideDir = caller.attr('data-i-dialog-hide-dir');
    var isModal = caller.attr('data-i-ismodal');
    var dialogTitle = caller.attr('data-i-dialog-title');
    $dialog = $(targetId).dialog({
        width: width, height: height, show: { effect: showEffect, direction: showDir }, hide: { effect: hideEffect, direction: hideDir }, modal: isModal, title: dialogTitle,/* buttons: { "بستن": function () { $(this).dialog("close"); } },*/ open: function () { }, close: function () { $(this).children('div').remove(); $('.redactor_dropdown').remove(); }
    });


};


function slideButton() {
    $('[data-slide-toggle-id]').each(function () {
        var $this = $(this);
        $this.off('click');
        $this.on('click', function (e) {
            e.preventDefault();
            $this.text(($this.text() === "پیشرفته") ? "معمولی" : "پیشرفته");
            $($this.attr('data-slide-toggle-id')).stop(true, false).slideToggle('blind');
        });
    });

}

function autoComplete() {
    $('input[data-autocomplete]').each(function () {
        $(this).autocomplete({ source: $(this).attr('data-autocomplete') + '?searchBy=' + $(this).attr('data-filter'), minLength: $(this).attr('data-min-length') });
    });
}
function radioButtonSearch() {
    $('div#search-types input[type=radio]').on('click', function () {
        $('input[data-autocomplete]').attr('data-filter', $(this).attr('value'));
        autoComplete();
    });
}

function loadRedactor(selector, option) {
    var options = { direction: 'rtl', autoresize: false };
    $.extend(options, option);
    $(selector).redactor(options);
}

function bindChosen(selector, option) {
    var options = { no_results_text: "موردی یافت نشد" };
    $.extend(options, option);
    $(selector).chosen(options);
    $('select').chosen();
}

function initializeTooltip(option) {
    var options = {};
    $.extend(options, option);
    $('[title]').each(function () {
        var $this = $(this);
        var $option = { content: $this.attr('title') };
        $.extend(options, $option);
        $this.tooltip(options);
    });
}
function loadImagePreview(selector, url) {




    showLoading();
    //$.ajax({
    //    type: "Get",
    //    url: url,
    //    complete: function (response,status) {
    //        $(selector).html('<img src="' + url + '" />');
    //    }
    //}).fail(function () { $(selector).html('عکسی یافت نشد'); });

    var img = $("<img />").attr('src', url).css({ width: "300px", height: "200px" })
    .load(function () {
        if (!this.complete || typeof this.naturalWidth === "undefined" || this.naturalWidth === 0) {
            $(selector).html('عکسی یافت نشد');
            hideLoading();
        } else {
            $(selector).html(img);
            hideLoading();
        }
    }).error(function () { $(selector).html('موردی یافت نشد'); hideLoading(); });

    if ($(selector).val() === "") {
        hideLoading();
    }
}

function showDownloadLinks() {
    $('.btn-i-link').first().next('div').show().end().end().on('click', function (event) {
        event.preventDefault();
        $(this).next('div').slideToggle('slow');
    });
}
$('div#iris-sidebar ul').sortable({});
$('div#user-table table tbody tr').resizable({});

function reloadPage() {
    window.location.reload();
}