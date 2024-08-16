var InitApp = function () {
    var incubatorConvocationId = $("#IncubatorConvocationId").val();

    var datatable = {
        files: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: `/unidad-emprendimiento/convocatorias-emprendimiento/${incubatorConvocationId}/archivos`.proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        delete data.columns;
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
                this.object = $("#data-table").DataTable(this.options);
                this.events();
            },
            events: function () {
                $("#data-table").on('click', '.btn-edit', function () {
                    var id = $(this).data("id");
                    modal.edit.show(id);
                });

                $("#data-table").on('click', '.btn-delete', function () {
                    var id = $(this).data("id");
                    modal.delete(id);
                });
            }
        },
        annexes: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: `/unidad-emprendimiento/convocatorias-emprendimiento/${incubatorConvocationId}/archivos`.proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        delete data.columns;
                        data.handler = "DatatableAnnex";
                        data.searchValue = $("#search").val();
                    }
                },
                pageLength: 50,
                orderable: [],
                columns: [
                    {
                        title: "Nombre",
                        data: "name"
                    },
                    {
                        title: "Código",
                        data: "code"
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
                this.object = $("#data-table-annex").DataTable(this.options);
                this.events();
            },
            events: function () {
                $("#data-table-annex").on('click', '.btn-edit', function () {
                    var id = $(this).data("id");
                    modal.editAnnex.show(id);
                });

                $("#data-table-annex").on('click', '.btn-delete', function () {
                    var id = $(this).data("id");
                    modal.deleteAnnex(id);
                });
            }
        },
        external_evaluator: {
            object: null,
            options: {
                ajax: {
                    url: `/unidad-emprendimiento/convocatorias-emprendimiento/${incubatorConvocationId}/archivos`,
                    type: "GET",
                    data: function (data) {
                        delete data.columns;
                        data.handler = "ExternalEvaluatorDatatable"
                    }
                },
                columns: [
                    {
                        data: "userName",
                        title: "Usuario"
                    },
                    {
                        data: "fullName",
                        title: "Nombre Completo"
                    },
                    {
                        data: "dni",
                        title: "DNI"
                    },
                    {
                        data: "email",
                        title: "Correo Electrónico"
                    },
                    {
                        data: null,
                        title: "Opciones",
                        render: function (row) {
                            var tpm = "";
                            tpm += `<button title='Eliminar' data-userid='${row.userId}' type='button' class="btn-delete btn btn-danger m-btn m-btn--icon btn-sm m-btn--icon-only"><i class="la la-trash"></i></button>`;
                            return tpm;
                        }
                    }
                ]
            },
            reload: function () {
                this.object.ajax.reload();
            },
            events: function () {
                $("#external_evaluator_datatable").on("click", ".btn-delete", function () {
                    var userid = $(this).data("userid");
                    var formData = new FormData();
                    formData.append("IncubatorConvocationId", incubatorConvocationId);
                    formData.append("UserId", userid);
                    swal({
                        type: "warning",
                        title: "Eliminará al evaluador seleccionado.",
                        text: "¿Seguro que desea eliminarlo?.",
                        confirmButtonText: "Sí",
                        showCancelButton: true,
                        showLoaderOnConfirm: true,
                        allowOutsideClick: () => !swal.isLoading(),
                        preConfirm: () => {
                            return new Promise(() => {
                                $.ajax({
                                    type: "POST",
                                    data: formData,
                                    contentType: false,
                                    processData: false,
                                    url: `/unidad-emprendimiento/convocatorias-emprendimiento/${incubatorConvocationId}/archivos?handler=DeleteExternalEvaluator`,
                                    beforeSend: function (xhr) {
                                        xhr.setRequestHeader("XSRF-TOKEN",
                                            $('input:hidden[name="__RequestVerificationToken"]').val());
                                    }
                                })
                                    .done(function () {
                                        datatable.external_evaluator.reload();
                                        swal({
                                            type: "success",
                                            title: "Completado",
                                            text: "Evaluador eliminado satisfactoriamente.",
                                            confirmButtonText: "Aceptar"
                                        });
                                    })
                                    .fail(function (e) {
                                        swal({
                                            type: "error",
                                            title: "Error",
                                            confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                            confirmButtonText: "Aceptar",
                                            text: e.status === 502 ? "No hay respuesta del servidor" : e.responseText
                                        });
                                    });

                            });
                        }
                    });

                });
            },
            init: function () {
                this.object = $("#external_evaluator_datatable").DataTable(this.options);
                this.events();
            }
        },
        init: function () {
            this.files.init();
            this.annexes.init();
            this.external_evaluator.init();
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
                        datatable.files.reload();
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

                        datatable.files.reload();
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
                    url: `/unidad-emprendimiento/convocatorias-emprendimiento/${incubatorConvocationId}/archivos`.proto().parseURL(),
                    type: "GET",
                    data: {
                        handler: "DetailFile",
                        incubatorConvocationFileId: id
                    },
                }).done(function (result) {
                    $("#edit-form input[name=Number]").val(result.number);
                    $("#edit-form input[name=Name]").val(result.name);
                    $("#edit-form input[name=IncubatorConvocationFileId]").val(result.id);
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
                text: "El Archivo será eliminado.",
                type: "warning",
                showCancelButton: true,
                confirmButtonText: "Sí, eliminarlo",
                confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                cancelButtonText: "Cancelar",
                showLoaderOnConfirm: true,
                preConfirm: () => {
                    return new Promise((resolve) => {
                        $.ajax({
                            url: (`/unidad-emprendimiento/convocatorias-emprendimiento/${incubatorConvocationId}/archivos?handler=DeleteIncubatorConvocationFile`).proto().parseURL(),
                            type: "POST",
                            data: {
                                id: id
                            },
                            beforeSend: function (xhr) {
                                xhr.setRequestHeader("XSRF-TOKEN",
                                    $('input:hidden[name="__RequestVerificationToken"]').val());
                            },
                            success: function (result) {
                                swal({
                                    type: "success",
                                    title: "Completado",
                                    text: "El Archivo ha sido eliminado con exito",
                                    confirmButtonText: "Excelente"
                                }).then(datatable.files.reload());
                            },
                            error: function (errormessage) {
                                swal({
                                    type: "error",
                                    title: "Error",
                                    confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                    confirmButtonText: "Entendido",
                                    text: "El Archivo presenta información relacionada"
                                });
                            }
                        });
                    });
                },
                allowOutsideClick: () => !swal.isLoading()
            });

        },
        createAnnex: {
            object: $("#add-form-annex").validate({
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
                        datatable.annexes.reload();
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
                $("#addModalAnnex").modal("toggle");
            },
            clear: function () {

                modal.createAnnex.object.resetForm();
            },
            events: function () {
                $("#addModalAnnex").on("hidden.bs.modal", function () {
                    modal.createAnnex.clear();
                });
            }
        },
        editAnnex: {
            object: $("#edit-form-annex").validate({
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

                        datatable.annexes.reload();
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
                    url: `/unidad-emprendimiento/convocatorias-emprendimiento/${incubatorConvocationId}/archivos`.proto().parseURL(),
                    type: "GET",
                    data: {
                        handler: "DetailAnnex",
                        incubatorConvocationAnnexId: id
                    },
                }).done(function (result) {
                    $("#edit-form-annex input[name=Code]").val(result.code);
                    $("#edit-form-annex input[name=Name]").val(result.name);
                    $("#edit-form-annex input[name=IncubatorConvocationAnnexId]").val(result.id);

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
                modal.editAnnex.load(id);
                $("#editModalAnnex").modal("toggle");
            },
            clear: function () {

                modal.editAnnex.object.resetForm();
            },
            events: function () {
                $("#editModalAnnex").on("hidden.bs.modal", function () {
                    modal.editAnnex.clear();
                });
            }
        },
        deleteAnnex: function (id) {
            swal({
                title: "¿Está seguro?",
                text: "El Anexo será eliminado.",
                type: "warning",
                showCancelButton: true,
                confirmButtonText: "Sí, eliminarlo",
                confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                cancelButtonText: "Cancelar",
                showLoaderOnConfirm: true,
                preConfirm: () => {
                    return new Promise((resolve) => {
                        $.ajax({
                            url: (`/unidad-emprendimiento/convocatorias-emprendimiento/${incubatorConvocationId}/archivos?handler=DeleteIncubatorConvocationAnnex`).proto().parseURL(),
                            type: "POST",
                            data: {
                                id: id
                            },
                            beforeSend: function (xhr) {
                                xhr.setRequestHeader("XSRF-TOKEN",
                                    $('input:hidden[name="__RequestVerificationToken"]').val());
                            },
                            success: function (result) {
                                swal({
                                    type: "success",
                                    title: "Completado",
                                    text: "El Anexo ha sido eliminado con exito",
                                    confirmButtonText: "Excelente"
                                }).then(datatable.annexes.reload());
                            },
                            error: function (errormessage) {
                                swal({
                                    type: "error",
                                    title: "Error",
                                    confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                    confirmButtonText: "Entendido",
                                    text: "El Anexo presenta información relacionada"
                                });
                            }
                        });
                    });
                },
                allowOutsideClick: () => !swal.isLoading()
            });

        },
        external_evaluator: {
            object: $("#external_evaluator_form"),
            form: {
                object: $("#external_evaluator_form").validate({
                    submitHandler: function (formElement, e) {
                        e.preventDefault();
                        mApp.block(modal.external_evaluator.object, {
                            message: "Agregando evaluador..."
                        });

                        $.ajax({
                            url: `/unidad-emprendimiento/convocatorias-emprendimiento/${incubatorConvocationId}/archivos?handler=AddExternalEvaluator`,
                            method: "POST",
                            data: $(formElement).serialize()
                        })
                            .done(function (e) {
                                toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                                datatable.external_evaluator.reload();
                            })
                            .fail(function (e) {
                                var msg = e.status === 502 ? "No hay respuesta del servidor" : e.responseText;
                                toastr.error(msg, _app.constants.toastr.title.error);
                            })
                            .always(function () {
                                modal.external_evaluator.object.find(":input").attr("disabled", false);
                                mApp.unblock(modal.external_evaluator.object);
                            });
                    }
                })
            }
        },


        init: function () {
            this.create.events();
            this.edit.events();
            this.createAnnex.events();
            this.editAnnex.events();
        }

    };



    var search = {
        init: function () {
            $("#search").doneTyping(function () {
                datatable.files.reload();
            });
        }
    };

    var events = {
        init: function () {
            $("#AddBtn").on("click", function () {
                modal.create.show();
            });

            $("#AddBtnAnnex").on("click", function () {
                modal.createAnnex.show();
            });
        }
    };

    var select = {
        init: function () {
            this.external_evaluator.init();
        },
        external_evaluator: {
            init: function () {
                this.load();
            },
            load: function () {
                modal.external_evaluator.object.find("[name='UserId']").select2({
                    ajax: {
                        url: "/api/usuarios/select/evaluadores-externos",
                        delay: 300,
                    },
                    minimumInputLength: 2,
                    dropdownParent: modal.external_evaluator.object,
                    placeholder: 'Seleccione evaluador externo...',
                    allowClear: true
                });
            }
        }
    }

    return {
        init: function () {
            datatable.init();
            modal.init();
            search.init();
            events.init();
            select.init();
        }
    };

}();

$(function () {
    InitApp.init();
});
