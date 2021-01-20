// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


$(document).on("click", ".CloseModle", function (e) {
    $("#OpenModal").modal('hide');
});

$(document).on("click", ".openModal", function (e) {
    e.preventDefault();
    var currentBtn = $(this);
    var currentIcon = addButtonLoader(currentBtn);

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
        loadAjaxFormForPartialView();
        removeButtonLoader(currentBtn, currentIcon);
    });
});