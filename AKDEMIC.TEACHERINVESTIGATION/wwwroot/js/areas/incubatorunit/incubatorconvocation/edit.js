var InitApp = function () {
    var form = {
        init: function () {
            $("#edit-form").validate({
                ignore: ".note-editor *",
                submitHandler: function (form, e) {
                    $("#btnSave").addLoader();
                    e.preventDefault();
                    let formData = new FormData(form);
                    let selectedFaculties = $('#faculties option:selected');
                    for (var i = 0; i < selectedFaculties.length; i++) {
                        formData.append(`Input.Faculties[${i}].id`, selectedFaculties[i].value);
                        formData.append(`Input.Faculties[${i}].name`, selectedFaculties[i].text);
                    }
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
            this.incubatorConvocationDate.init();
        },
        incubatorConvocationDate: {
            init: function () {
                $("#Input_StartDate").datepicker({
                    clearBtn: true,
                    orientation: "bottom",
                    format: _app.constants.formats.datepicker
                });

                $("#Input_EndDate").datepicker({
                    clearBtn: true,
                    orientation: "bottom",
                    format: _app.constants.formats.datepicker
                });

                $("#Input_InscriptionStartDate").datetimepicker({
                    clearBtn: true,
                    orientation: "bottom",
                    format: _app.constants.formats.datetimepicker
                });

                $("#Input_InscriptionEndDate").datetimepicker({
                    clearBtn: true,
                    orientation: "bottom",
                    format: _app.constants.formats.datetimepicker
                });
                
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

    var select = {
        init: function () {
            this.faculties.init();
        },
        faculties: {
            init: function () {
                this.load();
                this.events();
            },
            load: function () {
                $.ajax({
                    url: `/api/facultades/get`.proto().parseURL()
                }).done(function (result) {
                    $('#faculties').selectpicker({
                        actionsBox: true,
                        selectAllText: 'Marcar todos',
                        deselectAllText: 'Desmarcar todos',
                        noneSelectedText: 'Seleccionar',
                        size: 10,
                    });
                    let dataHtml = '';
                    for (var i = 0; i < result.length; i++) {
                        dataHtml += `<option value="${result[i].id}">${result[i].text}</option>`
                    }
                    $('#faculties').html(dataHtml).selectpicker("refresh");

                    $('#faculties').selectpicker('val', incubatorFaculties);
                    $('#faculties').selectpicker("refresh");
                });

            },
            events: function () {
                $('#faculties').on('changed.bs.select', function (e, clickedIndex, isSelected, previousValue) {
                    $('#faculties').selectpicker("refresh");
                });
                $('#faculties').on('shown.bs.select', function (e, clickedIndex, isSelected, previousValue) {
                    $('#faculties').selectpicker("refresh");
                });
            }
        },
    }

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

    return {
        init: function () {
            form.init();
            datePickers.init();
            loadPicture.init();
            summernote.init();
            select.init();

        }
    }

}();

$(function () {
    InitApp.init();
})