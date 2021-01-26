﻿// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

toastr.options = {
    "closeButton": false,
    "debug": false,
    "newestOnTop": false,
    "progressBar": false,
    "positionClass": "toast-top-right",
    "preventDuplicates": false,
    "onclick": null,
    "showDuration": "300",
    "hideDuration": "1000",
    "timeOut": "5000",
    "extendedTimeOut": "1000",
    "showEasing": "swing",
    "hideEasing": "linear",
    "showMethod": "fadeIn",
    "hideMethod": "fadeOut"
}
function ShowMessage(msg, color, title) {

    if (msg instanceof Array) {
        var errorsHtml = "<ul>";
        $.each(msg, function (key, value) {
            errorsHtml += '<li>' + value + '</li>';
        });
        errorsHtml += "</ul>";

        msg = errorsHtml;
    }
    Command: toastr["success"]("ww", "w")

    //Command: toastr[color](msg, title);
}
function isNotNullAndUndef(variable) {
    return (variable != null && variable != undefined && $.trim(variable) != "");
}

$(document).on("click", ".CloseModle", function (e) {
    $("#OpenModal").modal('hide');
});

$(document).on("click", ".openModal", function (e) {
    e.preventDefault();
    var currentBtn = $(this);

    var href = $(this).attr("href");
    var ajaxType = $(this).attr("ajaxType");
    var modelsize = $(this).attr("modelsize");
    var modalid = $(this).attr("modalid");
    var title = $(this).attr("title");
    if (!isNotNullAndUndef(modalid))
        modalid = "OpenModal";

    var outsidedisabled = $(this).attr("outsidedisabled");

    if (modelsize == "modal-xl") { $(".modal-dialog").addClass("modal-xl"); } else { $(".modal-dialog").removeClass("modal-xl") };
    if (modelsize == "modal-xxl") { $(".modal-dialog").addClass("modal-xxl"); } else { $(".modal-dialog").removeClass("modal-xxl") };
    getModalContent(href, ajaxType, "#" + modalid + "", title, outsidedisabled, function () {
        //loadAjaxFormForPartialView();
    });
});
const ScrollOptions = {
    scrollbars: {
        autoHide: 'scroll',
        autoHideDelay: 1500
    }
};
function getModalContent(sourceUrl, ajaxType, modalWrapper, title, outSideDisabled, param1) {
    if (title) {
        $(modalWrapper + ' .modal-title').html(title);
    }
    if (sourceUrl !== '#') {
        $.ajax({
            method: ajaxType,
            url: sourceUrl,
            contentType: 'application/json'
        }).done(function (result) {

            if (result.status == -1) {
                ShowMessage(result.msg, result.color, result.management);
            }
            else {
                /* Success Ajax */
                $(modalWrapper + " .modal-content").html(result);
              //  $('.modal-scroll-content > .modal-body').overlayScrollbars(ScrollOptions);

                $('.formSubmitOutSideBtn').click(function () {
                    var formId = $(this).attr('formId');
                    $("#" + formId).submit();
                });

                if (outSideDisabled == "true") {
                    $(modalWrapper).modal({
                        backdrop: 'static',
                        keyboard: false
                    });
                }
                else {
                    $(modalWrapper).modal('show');
                }
                if (typeof param1 === "function") {
                    param1();
                }

                //var collection = $(modalWrapper + " .modal-content").find('.collectionItem');
                //if (collection) {
                //    if (collection.length == 1) {
                //        $(modalWrapper + " .modal-content").find('.removeCollectionItem').addClass('disabled');
                //    }
                //}
            }
        })
            .fail(function (res) {
                $(modalWrapper).modal('hide');
                if (res.status == 403) {
                    ShowMessage('You do not have permission to access this module!', 'error', "Authentication Management");
                }
                else {
                    if (sourceUrl.includes('_CustomerAddresses')) {
                        swal('Error!', 'Could not connect to NAV service', 'error');
                    } else {
                        swal('Sorry!', 'Something went wrong! Try again please', 'error');
                    }
                }
            });
    }
    else {
        $(modalWrapper).modal('show');
    }
}
/*
function loadAjaxFormForPartialView() {

    $(".ajaxForm").ajaxForm(
        {
            success: function (json) {
                $(".ajaxForm :submit").prop("disabled", false);
                if (json.status == 1) {
                    ShowMessage(json.msg, json.color, json.management);
                    if (json.link != "") {
                        setTimeout(function () { window.location.replace(json.link); }, 800);
                    } else {
                        $('.ajaxForm').resetForm();
                    }
                    $("#OpenModal").modal('hide');

                } else if (json.status == 0) {
                    ShowMessage(json.msg, json.color, json.management);
                } else if (json.status == -1) {
                    ShowMessage(errorsHtml, json.color, json.management);
                }
            }, beforeSubmit: function () {
                $(".ajaxForm :submit").prop("disabled", true);
            }
            , error: function (json) {
            }
        });

    $(".select-2-drodpdown").select2({
        closeOnSelect: false,
    });
}
*/