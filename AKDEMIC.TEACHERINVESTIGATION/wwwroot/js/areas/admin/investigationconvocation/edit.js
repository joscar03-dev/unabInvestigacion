var InitApp = function () {
    var form = {
        init: function () {
            $("#edit-form").validate({
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
                        $("#btnSave").removeLoader();
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
                $("#Input_InvestigationConvocationStartDate").datepicker({
                    clearBtn: true,
                    orientation: "bottom",
                    format: _app.constants.formats.datepicker
                })
                $("#Input_InvestigationConvocationEndDate").datepicker({
                    clearBtn: true,
                    orientation: "bottom",
                    format: _app.constants.formats.datepicker
                })
                $("#Input_InvestigationConvocationInscriptionStartDate").datetimepicker({
                    clearBtn: true,
                    orientation: "bottom",
                    format: _app.constants.formats.datetimepicker
                })
                $("#Input_InvestigationConvocationInscriptionEndDate").datetimepicker({
                    clearBtn: true,
                    orientation: "bottom",
                    format: _app.constants.formats.datetimepicker
                })
                $("#Input_InvestigationConvocationInquiryStartDate").datepicker({
                    clearBtn: true,
                    orientation: "bottom",
                    format: _app.constants.formats.datepicker
                })
                $("#Input_InvestigationConvocationInquiryEndDate").datepicker({
                    clearBtn: true,
                    orientation: "bottom",
                    format: _app.constants.formats.datepicker
                })
                $("#extension-form input[name='NewEndDate']").datepicker({
                    clearBtn: true,
                    orientation: "bottom",
                    format: _app.constants.formats.datepicker
                })
            }
        }
    }


    var switchInput = {
        init: function () {
            this.allowInquiries.init();

        },
        allowInquiries: {
            init: function () {
                $("#Input_InvestigationConvocationAllowInquiries").bootstrapSwitch({
                    onText: "SÍ",
                    offText: "NO",
                    onColor: "success",
                    offColor: "danger"
                });

                this.events();
                if ($('#Input_InvestigationConvocationAllowInquiries').is(':checked')) {
                    $("#InquiryContainer").css("display", "flex");
                } else {
                    $("#InquiryContainer").css("display", "none");
                }
            },
            events: function () {
                $('#Input_InvestigationConvocationAllowInquiries').on('switchChange.bootstrapSwitch', function (e) {
                    if ($(this).is(':checked')) {
                        $("#InquiryContainer").css("display", "flex");
                    } else {
                        $("#InquiryContainer").css("display", "none");
                    }
                });
            }
        },


    };
    var modal = {
        extensionstime: {
            create: {
                object: $("#extension-form").validate({
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
                            $(".modal").modal("hide");
                            $(".m-alert").addClass("m--hide");
                            toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                            modal.extensionstime.create.clear();
                            $("#btnSave").removeLoader();
                            window.location.href = window.location.href;
                        }).fail(function (error) {
                            toastr.error(error.responseText, _app.constants.toastr.title.error);
                            $("#btnSave").removeLoader();
                            //if (error.responseText !== null && error.responseText !== "") $("#add_form_msg_txt").html(error.responseText);
                            //else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                            //$("#add_form_msg").removeClass("m--hide").show();
                        }).always(function () {

                        });
                    }
                }),
                show: function () {
                    $("#btnSave").removeLoader();
                    $("#extensionModal").modal("toggle");
                },
                clear: function () {
                    $("#extension-form").find(".custom-file-label").text("Seleccionar archivo");
                    modal.extensionstime.create.object.resetForm();
                },
                events: function () {
                    $("#extensionModal").on("hidden.bs.modal", function () {
                        modal.extensionstime.create.clear();
                    });
                }
            },
            init: function () {
                this.create.events();
            }
        },
        init: function () {
            this.extensionstime.init();
        }
    };
    var loadPicture = {
        init: function () {
            $("#Input_InvestigationConvocationPictureFile").on("change",
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
            modal.init();
            loadPicture.init();
        }
    }

}();

$(function () {
    InitApp.init();
})