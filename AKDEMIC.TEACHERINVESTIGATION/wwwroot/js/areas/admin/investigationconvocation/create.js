var InitApp = function () {
    var form = {
        init: function () {
            $("#create-form").validate({
                ignore: ".note-editor *",
                submitHandler: function (form, e) {
                    $("#btnSave").addLoader();
                    e.preventDefault();
                    let formData = new FormData(form);
                    $.ajax({
                        url: $(form).attr("action"),
                        type: "POST",
                        data: formData,
                        contentType: false,
                        processData: false
                    }).done(function () {
                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                        location.href = `/admin/convocatorias`.proto().parseURL();
                    }).fail(function (error) {
                        toastr.error(error.responseText, _app.constants.toastr.title.error);
                        $("#btnSave").removeLoader();
                    }).always(function () {

                    });
                }
            });
        }
    };

    var datePickers = {
        init: function () {
            this.investigationConvocationDate.init();
        },
        investigationConvocationDate: {
            init: function () {
                $("#Input_StartDate").datepicker({
                    clearBtn: true,
                    orientation: "bottom",
                    format: _app.constants.formats.datepicker
                })
                $("#Input_EndDate").datepicker({
                    clearBtn: true,
                    orientation: "bottom",
                    format: _app.constants.formats.datepicker
                })
                $("#Input_InscriptionStartDate").datetimepicker({
                    clearBtn: true,
                    orientation: "bottom",
                    format: _app.constants.formats.datetimepicker
                })
                $("#Input_InscriptionEndDate").datetimepicker({
                    clearBtn: true,
                    orientation: "bottom",
                    format: _app.constants.formats.datetimepicker
                })
                $("#Input_InquiryStartDate").datepicker({
                    clearBtn: true,
                    orientation: "bottom",
                    format: _app.constants.formats.datepicker
                })
                $("#Input_InquiryEndDate").datepicker({
                    clearBtn: true,
                    orientation: "bottom",
                    format: _app.constants.formats.datepicker
                })
            }
        }
    };


    var switchInput = {
        init: function () {
            this.allowInquiries.init();
        },
        allowInquiries: {
            init: function () {
                $("#Input_AllowInquiries").bootstrapSwitch({
                    onText: "SÍ",
                    offText: "NO",
                    onColor: "success",
                    offColor: "danger"
                });
                this.events();
            },
            events: function () {
                $('#Input_AllowInquiries').on('switchChange.bootstrapSwitch', function (e) {
                    if ($(this).is(':checked')) {
                        $("#InquiryContainer").css("display", "flex");
                    } else {
                        $("#InquiryContainer").css("display", "none");
                    }
                });
            }
        },


    };
    var loadPicture = {
        init: function () {
            $("#Input_PictureFile").on("change",
                function (e) {

                    var tgt = e.target || window.event.srcElement,
                        files = tgt.files;
                    // FileReader support
                    if (FileReader && files && files.length) {
                        var fr = new FileReader();
                        fr.onload = function () {
                            $("#current-picture").attr("src", fr.result);
                        }
                        fr.readAsDataURL(files[0]);
                    }
                    // Not supported
                    else {
                        console.log("File Reader not supported.");
                    }
                });
        }
    };

    var summernote = {
        init: function () {
            $(".mv_summernote").summernote({
                lang: "es-ES",
                height: 250,
                toolbar: [
                    ['style', ['style']],
                    ['font', ['bold', 'italic', 'underline', 'strikethrough', 'superscript', 'subscript', 'clear']],
                    ['fontname', ['fontname']],
                    ['fontsize', ['fontsize']],
                    ['color', ['color']],
                    ['para', ['ol', 'ul', 'paragraph', 'height']],
                    ['table', ['table']],
                    ['insert', ['link', 'video', 'hr']],
                    ['view', ['undo', 'redo', 'fullscreen', 'codeview']]
                ]
            });
        }
    };



    return {
        init: function () {
            form.init();
            datePickers.init();
            switchInput.init();
            summernote.init();
            loadPicture.init();
        }
    }

}();

$(function () {
    InitApp.init();
})