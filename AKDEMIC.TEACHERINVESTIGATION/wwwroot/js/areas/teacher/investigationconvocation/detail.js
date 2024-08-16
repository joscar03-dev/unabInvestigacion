var InitApp = function () {
    var investigationConvocationPostulantId = $("#InvestigationConvocationPostulantId").val();

    var datatable = {
        init: function () {
            this.files.init();
            this.inquiries.init();
        },

        files: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: `/investigador/convocatoria/${investigationConvocationPostulantId}/detalle`.proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        data.handler = "Datatable";
                        data.searchValue = $("#search").val();
                    }
                },
                pageLength: 50,
                orderable: [],
                columns: [
                    {
                        title: "Número",
                        data: "number"
                    },
                    {
                        title: "Nombre",
                        data: "name"
                    },
                    {
                        title: "Archivo",
                        data: null,
                        orderable: false,
                        render: function (data) {
                            var template = "";
                            var fileUrl = `/documentos/${data.filePath}`.proto().parseURL();
                            //FileURL
                            template += `<a href="${fileUrl}"  `;
                            template += "class='btn btn-success ";
                            template += "m-btn btn-sm m-btn--icon' download>";
                            template += "<span><i class='flaticon-file'></i></a> ";
                            return template;
                        }
                    },
                ],
            },
            reload: function () {
                this.object.ajax.reload();
            },
            init: function () {
                this.object = $("#data-table").DataTable(this.options);
            }
        },
        inquiries: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: `/investigador/convocatoria/${investigationConvocationPostulantId}/detalle`.proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        data.handler = "InquiriesDatatable";
                        data.searchValue = $("#search").val();
                    }
                },
                pageLength: 50,
                orderable: [],
                columns: [
                    {
                        title: "Consulta",
                        data: "inquiry"
                    },
                    {
                        title: "Usuario",
                        data: "userName"
                    },
                    {
                        title: "Nombre Completo",
                        data: "fullName"
                    },
                    {
                        title: "Archivo",
                        data: null,
                        orderable: false,
                        render: function (data) {
                            var template = "";
                            if (data.filePath != null) {
                                var fileUrl = `/documentos/${data.filePath}`.proto().parseURL();
                                //FileURL
                                template += `<a href="${fileUrl}"  `;
                                template += "class='btn btn-success ";
                                template += "m-btn btn-sm m-btn--icon' download>";
                                template += "<span><i class='flaticon-file'></i></a> ";
                            }
                            return template;
                        }
                    },
                    {
                        title: "Opciones",
                        data: null,
                        orderable: false,
                        render: function (data) {
                            var template = "";
                            //Edit
                            template += "<button ";
                            template += "class='btn btn-info ";
                            template += "m-btn btn-sm m-btn--icon btn-edit' ";
                            template += " data-id='" + data.id + "'>";
                            template += "<span><i class='la la-edit'></i><span>Editar</span></span></button> ";
                            //Delete
                            template += "<button ";
                            template += "class='btn btn-danger btn-delete ";
                            template += "m-btn btn-sm  m-btn--icon-only' ";
                            template += " data-id='" + data.id + "'>";
                            template += "<i class='la la-trash'></i></button> ";
                            return template;
                        }
                    }
                ],
            },
            reload: function () {
                this.object.ajax.reload();
            },
            init: function () {
                this.object = $("#data-table-inquiries").DataTable(this.options);
                this.events();
            },
            events: function () {
                $("#data-table-inquiries").on('click', '.btn-edit', function () {
                    var id = $(this).data("id");
                    modal.edit.show(id);
                });
                $("#data-table-inquiries").on('click', '.btn-delete', function () {
                    var id = $(this).data("id");
                    modal.delete(id);
                });
            }
        }
        
    };

    var modal = {
        create: {
            object: $("#add-form").validate({
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
                        datatable.inquiries.reload();
                        modal.create.clear();
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
                $("#addModal").modal("toggle");
            },
            clear: function () {
                $("#add-form").find(".custom-file-label").text("Seleccionar archivo");
                modal.create.object.resetForm();
            },
            events: function () {
                $("#addModal").on("hidden.bs.modal", function () {
                    modal.create.clear();
                });
            }
        },
        edit: {
            object: $("#edit-form").validate({
                submitHandler: function (form, e) {
                    $("#btnEdit").addLoader();
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

                        datatable.inquiries.reload();
                        modal.edit.clear();
                    }).fail(function (error) {
                        toastr.error(error.responseText, _app.constants.toastr.title.error);
                        $("#btnEdit").removeLoader();
                        //if (error.responseText !== null && error.responseText !== "") $("#add_form_msg_txt").html(error.responseText);
                        //else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                        //$("#add_form_msg").removeClass("m--hide").show();
                    }).always(function () {

                    });
                }
            }),
            load: function (id) {
                $.ajax({
                    url: `/investigador/convocatoria/${investigationConvocationPostulantId}/detalle`.proto().parseURL(),
                    type: "GET",
                    data: {
                        handler: "DetailFileInquiry",
                        investigationConvocationFileInquiryId: id
                    },
                }).done(function (result) {
                    $("#edit-form textarea[name=Inquiry]").val(result.inquiry);
                    $("#edit-form input[name=InvestigationConvocationFileInquiryId]").val(result.id);
                    $("#edit-form").find(".custom-file-label").text("Seleccionar archivo");
                    $("#edit-form").find(".custom-file").append('<div class="form-control-feedback">*Seleccionar en caso sea necesario reemplazar el documento vigente.</div>');
                }).fail(function (error) {
                    toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                    if (error.responseText !== null && error.responseText !== "") $("#add_form_msg_txt").html(error.responseText);
                    else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                    $("#add_form_msg").removeClass("m--hide").show();
                }).always(function () {

                });
            },
            show: function (id) {
                $("#btnEdit").removeLoader();
                modal.edit.load(id);
                $("#editModal").modal("toggle");
            },
            clear: function () {
                $("#edit-form").find(".custom-file-label").text("Seleccionar archivo");
                modal.edit.object.resetForm();
            },
            events: function () {
                $("#editModal").on("hidden.bs.modal", function () {
                    modal.edit.clear();
                });
            }
        },
        delete: function (id) {
            swal({
                title: "¿Está seguro?",
                text: "La Consulta será eliminada.",
                type: "warning",
                showCancelButton: true,
                confirmButtonText: "Sí, eliminarlo",
                confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                cancelButtonText: "Cancelar",
                showLoaderOnConfirm: true,
                preConfirm: () => {
                    return new Promise((resolve) => {
                        $.ajax({
                            url: (`/investigador/convocatoria/${investigationConvocationPostulantId}/detalle`).proto().parseURL(),
                            type: "GET",
                            data: {
                                handler: "InquiryDelete",
                                id: id
                            },
                            success: function (result) {
                                swal({
                                    type: "success",
                                    title: "Completado",
                                    text: "La Consulta ha sido eliminada con exito",
                                    confirmButtonText: "Excelente"
                                }).then(datatable.inquiries.reload());
                            },
                            error: function (errormessage) {
                                swal({
                                    type: "error",
                                    title: "Error",
                                    confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                    confirmButtonText: "Entendido",
                                    text: "La Consulta presenta información relacionada"
                                });
                            }
                        });
                    });
                },
                allowOutsideClick: () => !swal.isLoading()
            });
        },

        init: function () {
            this.create.events();
            this.edit.events();
        }

    };

    var search = {
        init: function () {
            $("#search").doneTyping(function () {
                datatable.files.reload();
            });
        }
    };

    var datepicker = {
        init: function () {
            this.investigationConvocationDate.init();
        },
        investigationConvocationDate: {
            init: function () {
                $("#InvestigationConvocationStartDate").datepicker({
                    clearBtn: true,
                    orientation: "bottom",
                    format: _app.constants.formats.datepicker
                })
                $("#InvestigationConvocationEndDate").datepicker({
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
                $("#InvestigationConvocationAllowInquiries").bootstrapSwitch({
                    onText: "SÍ",
                    offText: "NO",
                    onColor: "success",
                    offColor: "danger"
                });
            }
        },
    };

    var events = {
        init: function () {
            $("#AddBtn").on("click", function () {
                modal.create.show();
            });
        }
    }

    return {
        init: function () {
            datatable.init();
            modal.init();
            search.init();
            datepicker.init();
            switchInput.init();
            events.init();
        }
    };

}();

$(function () {
    InitApp.init();
});
