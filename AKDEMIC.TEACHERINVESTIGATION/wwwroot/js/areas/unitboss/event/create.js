var InitApp = function () {
    var form = {
        init: function () {
            $("#create-form").validate({
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
                        location.href = `/jefe-unidad/eventos/`.proto().parseURL();
                    }).fail(function (error) {
                        toastr.error(error.responseText, _app.constants.toastr.title.error);
                        $("#btnSave").removeLoader();
                    }).always(function () {

                    });
                }
            });
        }
    };

    var select = {
        init: function () {
            this.unit.init();
        },
        unit: {
            init: function () {
                this.load();
            },
            load: function () {
                $.ajax({
                    url: (`/api/unidad/select/get`).proto().parseURL(),
                    type: "GET"
                }).done(function (result) {
                    $('#Input_UnitId').select2({
                        data: result,
                    });
                });
            },

        },
    }

    var datePickers = {
        init: function () {
            this.eventDate.init();
        },
        eventDate: {
            init: function () {
                $("#Input_EventDate").datepicker({
                    clearBtn: true,
                    orientation: "bottom",
                    format: _app.constants.formats.datepicker
                })
            }
        }
    }

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
            select.init();
            datePickers.init();
            summernote.init();
        }
    }

}();

$(function () {
    InitApp.init();
})