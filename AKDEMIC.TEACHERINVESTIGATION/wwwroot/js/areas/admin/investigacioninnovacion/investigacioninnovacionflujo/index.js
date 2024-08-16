var InitApp = function () {
    var selectarea = {
        init: function () {
            this.researcherMaestroArea.init();
        },
        researcherMaestroArea: {
            init: function () {
                this.load();
            },
            load: function () {
                $.ajax({
                    url: (`/api/maestroarea/select/get`).proto().parseURL(),
                    type: "GET",
                    data:"idoficina=edcd01ea-6404-11ee-b7b1-16d13ee00159",
                }).done(function (result) {
                    $('.input_IdArea').select2({
                        data: result,
                        maximumSelectionLength: 1
                    });
                });
            }
        },
    };

    var datatable = {
        maestroFlujo: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: "/investigacioninnovacion/flujo".proto().parseURL(),
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
                        title: "Código",
                        data: "codigo"
                    },
                    {
                        title: "Nombre",
                        data: "nombre"
                    },
                    {
                        title: "Descripción",
                        data: "descripcion"
                    },
                    {
                        title: "Estado",
                        data: null,
                        render: function (data) {
                            var estado = data.activo == 1 ? "ACTIVO" : "INACTIVO";
                            var label = data.activo == 1 ? "label-success" : "label-danger";
                            var template = "";
                            template += "<span class='btn m-btn btn-sm m-btn--"+ label +"'>";
                            template += estado;
                            template += "</span> ";
                            return template;
                        }
                    },
                    {
                        title: "Opciones",
                        data: null,
                        width: 80,
                        orderable: false,
                        render: function (data) {
                            var template = "";
                            //persona
                            template += "<button title='Registrar Areas'  style='background-color:#aaeee8' ";
                            template += " class='btn  btn-actividades ";
                            template += " m-btn btn-sm  m-btn--icon-only' ";
                            template += " data-id='" + data.id + "' > ";
                            template += "<span><i class='la la-user'></i><span>Areas</span></span></button>";

                            //Edit
                            template += "<button ";
                            template += "class='btn btn-info btn-edit ";
                            template += "m-btn btn-sm  m-btn--icon-only' ";
                            template += " data-id='" + data.id + "'>";
                            template += "<i class='la la-edit'></i></button> ";

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


                $("#data-table").on('click', '.btn-actividades', function () {
                    var id = $(this).data("id");
                    modal.addactividades.show(id);
                });
            }
        },
        init: function () {
            this.maestroFlujo.init();
        }
    };
    var search = {
        init: function () {
            $("#search").doneTyping(function () {
                datatable.maestroFlujo.reload();
            });
        }
    };
    var modal = {
        addactividades: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: "/investigacioninnovacion/flujo".proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        data.handler = "DatatableArea";
                        data.id = $("#addactividades-form input[name=Id]").val();
                    }
                },
                pageLength: 50,
                orderable: [],
                columns: [

                    {
                        title: "Area",
                        data: "nombre"
                    },
                    {
                        title: "Orden",
                        data: "orden"
                    },
                    {
                        title: "Retorna Docente",
                        data: null,
                        width: 100,
                        render: function (data) {
                            var estado = data.retornadocente == 1 ? "SI" : "NO";
                            var label = data.retornadocente == 1 ? "label-success" : "label-danger";
                            var template = "";
                            template += "<span class='btn m-btn btn-sm m-btn--" + label + "'>";
                            template += estado;
                            template += "</span> ";
                            return template;
                        }
                    },
                    {
                        title: "Estado",
                        data: null,
                        width: 100,
                        render: function (data) {
                            var estado = data.activo == 1 ? "ACTIVO" : "INACTIVO";
                            var label = data.activo == 1 ? "label-success" : "label-danger";
                            var template = "";
                            template += "<span class='btn m-btn btn-sm m-btn--" + label + "'>";
                            template += estado;
                            template += "</span> ";
                            return template;
                        }
                    },
                    {
                        title: "Opciones",
                        data: null,
                        width: 100,
                        orderable: false,
                        render: function (data) {
                            var template = "";



                            //Edit
                            template += "<button title='Editar'  type='button'  ";
                            template += "class='btn btn-info btn-editactividad ";
                            template += "m-btn btn-sm  m-btn--icon-only' ";
                            template += " data-id='" + data.id + "'>";
                            template += "<i class='la la-edit'></i></button>";

                            //Delete
                            template += "<button type='button' ";
                            template += "class='btn btn-danger btn-deleteactividad ";
                            template += "m-btn btn-sm  m-btn--icon-only' ";
                            template += " data-id='" + data.id + "'>";
                            template += "<i class='la la-trash'></i></button> ";




                            return template;
                        }
                    }
                ],

            },
            load: function (id) {
                $("#addactividades-form input[name=Id]").val(id);
                $("#nuevoactividades-form input[name=IdFlujo]").val(id);
                if (this.object == null) {
                    this.object = $("#data-tableactividades").DataTable(this.options);
                } else {
                    this.reload();
                }
                modal.addactividades.events();
                arrayactividad = new Array();
                contaractividad = 0;

            },
            reload: function () {
                this.object.ajax.reload();
                arrayactividad = new Array();
                contaractividad = 0;
            },
            show: function (id) {
                modal.addactividades.load(id);
                $("#addModalActividades").modal("toggle");
            },
            clear: function () {
                // modal.addactividades.object.resetForm();
            },
            events: function () {

                $("#addModalActividades").on("hidden.bs.modal", function () {
                    modal.addactividades.clear();
                });
                $("#addModalActividades").on("hidden.bs.modal", function () {
                    modal.addactividades.clear();
                });


                $("#data-tableactividades").on('click', '.btn-editactividad', function () {
                    var id = $(this).data("id");
                    modal.editarea.show(id);
                });

                $("#data-tableactividades").on('click', '.btn-deleteactividad', function () {
                    var id = $(this).data("id");
                    modal.deletearea(id);

                });



            }
        },
        createarea: {
            object: $("#nuevoactividades-form").validate({
                submitHandler: function (form, e) {
                    $("#btnNuevoctividades").addLoader();
                    id = $("#addactividades-form input[name=Id]").val();
                    $("#nuevoactividades-form input[name=IdFlujo]").val(id);
                    e.preventDefault();
                    let formData = new FormData(form);
                    $.ajax({
                        url: $(form).attr("action"),
                        type: "POST",
                        data: formData,
                        contentType: false,
                        processData: false
                    }).done(function () {
                        $("#nuevoactividades-form select[name=IdArea]").val("");
                        $("#nuevoactividades-form select[name=IdArea]").trigger("change");

                        $("#nuevoModalActividades").modal("hide");
                        $(".m-alert").addClass("m--hide");
                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);

                        modal.addactividades.reload();
                        modal.createarea.clear();
                        $("#btnNuevoctividades").removeLoader();

                        //location.reload();
                    }).fail(function (error) {
                        toastr.error(error.responseText, _app.constants.toastr.title.error);
                        $("#btnNuevoctividades").removeLoader();
                        //if (error.responseText !== null && error.responseText !== "") $("#add_form_msg_txt").html(error.responseText);
                        //else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                        //$("#add_form_msg").removeClass("m--hide").show();
                    }).always(function () {

                    });
                }
            }),
            show: function () {
                $("#btnNuevoctividades").removeLoader();
                $("#nuevoModalActividades").modal("toggle");
            },
            clear: function () {
                modal.createarea.object.resetForm();
            },
            events: function () {
                $("#nuevoModalActividades").on("hidden.bs.modal", function () {
                    modal.createarea.clear();
                });

            }
        },
        create: {
            object: $("#add-form").validate({
                submitHandler: function (form, e) {
                    $("#btnSave").addLoader();
                    e.preventDefault();
                    $.ajax({
                        url: $(form).attr("action"),
                        type: "POST",
                        data: $(form).serialize()
                    }).done(function () {
                        $(".modal").modal("hide");
                        $(".m-alert").addClass("m--hide");
                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                        datatable.maestroFlujo.reload();
                        modal.create.clear();
                        $("#btnSave").removeLoader();
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
                    $.ajax({
                        url: $(form).attr("action"),
                        type: "POST",
                        data: $(form).serialize()
                    }).done(function () {
                        $(".modal").modal("hide");
                        $(".m-alert").addClass("m--hide");
                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                        datatable.maestroFlujo.reload();
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
                    url: "/investigacioninnovacion/flujo".proto().parseURL(),
                    type: "GET",
                    data: {
                        handler: "Detail",
                        id: id
                    },
                }).done(function (result) {
                    $("#edit-form input[name=Id]").val(result.id);
                    $("#edit-form input[name=codigo]").val(result.codigo);
                    $("#edit-form input[name=nombre]").val(result.nombre);
                    $("#edit-form select[name=IdFacultad]").val(result.idFacultad);
                    $("#edit-form select[name=IdFacultad]").trigger("change");
                    $("#edit-form textarea[name=descripcion]").val(result.descripcion);
                    $("#edit-form input[name=activo]").prop("checked", result.activo == 1 ? true : false);

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
                modal.edit.object.resetForm();
            },
            events: function () {
                $("#editModal").on("hidden.bs.modal", function () {
                    modal.edit.clear();
                });
            }
        },
        editarea: {
            object: $("#editactividades-form").validate({
                submitHandler: function (form, e) {
                    $("#btnEditactividades").addLoader();
                    e.preventDefault();
                    let formData = new FormData(form);
                    $.ajax({
                        url: $(form).attr("action"),
                        type: "POST",
                        data: formData,
                        contentType: false,
                        processData: false
                    }).done(function () {
                        $("#editModalActividades").modal("hide");
                        $(".m-alert").addClass("m--hide");
                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                        modal.addactividades.reload();
                        modal.editactividades.clear();
                    }).fail(function (error) {
                        toastr.error(error.responseText, _app.constants.toastr.title.error);
                        $("#btnEditactividades").removeLoader();
                        //if (error.responseText !== null && error.responseText !== "") $("#add_form_msg_txt").html(error.responseText);
                        //else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                        //$("#add_form_msg").removeClass("m--hide").show();
                    }).always(function () {

                    });
                }
            }),
            load: function (id) {

                $("#editactividades-form input[name=Id]").val(id);
                $.ajax({
                    url: "/investigacioninnovacion/flujo".proto().parseURL(),
                    type: "GET",
                    data: {
                        handler: "DetailArea",
                        id: id
                    },
                }).done(function (result) {
                    $("#editactividades-form input[name=Id]").val(result.id);
                    $("#editactividades-form input[name=orden]").val(result.orden);

                    $("#editactividades-form input[name=IdFlujo]").val(result.idFlujo);
                    $("#editactividades-form select[name=IdArea]").val(result.idArea);
                    $("#editactividades-form select[name=IdArea]").trigger("change");
                    $("#editactividades-form input[name=activo]").prop("checked", result.activo == 1 ? true : false);
                    $("#editactividades-form input[name=retornadocente]").prop("checked", result.retornadocente == 1 ? true : false);

                }).fail(function (error) {
                    toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                    if (error.responseText !== null && error.responseText !== "") $("#add_form_msg_txt").html(error.responseText);
                    else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                    $("#add_form_msg").removeClass("m--hide").show();
                }).always(function () {

                });
            },
            show: function (id) {
                $("#btnEditactividades").removeLoader();
                modal.editarea.load(id);
                $("#editModalActividades").modal("toggle");
            },
            clear: function () {
                $("#editactividades-form").find(".custom-file-label").text("Seleccionar archivo");
                modal.editarea.object.resetForm();
            },
            events: function () {
                $("#editModalActividades").on("hidden.bs.modal", function () {
                    modal.editarea.clear();
                });
            }
        },
        delete: function (id) {
            swal({
                title: "¿Está seguro?",
                text: "La flujo será eliminado.",
                type: "warning",
                showCancelButton: true,
                confirmButtonText: "Sí, eliminarlo",
                confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                cancelButtonText: "Cancelar",
                showLoaderOnConfirm: true,
                preConfirm: () => {
                    return new Promise((resolve) => {                        
                        $.ajax({
                            url: ("/investigacioninnovacion/flujo?handler=Delete").proto().parseURL(),
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
                                    text: "La Carrera ha sido eliminado con exito",
                                    confirmButtonText: "Excelente"
                                }).then(datatable.maestroFlujo.reload());
                            },
                            error: function (errormessage) {
                                swal({
                                    type: "error",
                                    title: "Error",
                                    confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                    confirmButtonText: "Entendido",
                                    text: "La Carrera presenta información relacionada"
                                });
                            }
                        });
                    });
                },
                allowOutsideClick: () => !swal.isLoading()
            });
        },
        deletearea: function (id) {
            swal({
                title: "¿Está seguro?",
                text: "El usuario será eliminado.",
                type: "warning",
                showCancelButton: true,
                confirmButtonText: "Sí, eliminarlo",
                confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                cancelButtonText: "Cancelar",
                showLoaderOnConfirm: true,
                preConfirm: () => {
                    return new Promise((resolve) => {
                        $.ajax({
                            url: ("/investigacioninnovacion/flujo?handler=Deletearea").proto().parseURL(),
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
                                    text: "El Usuaio ha sido eliminado con exito",
                                    confirmButtonText: "Excelente"
                                }).then(modal.addactividades.reload());
                            },
                            error: function (errormessage) {
                                swal({
                                    type: "error",
                                    title: "Error",
                                    confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                    confirmButtonText: "Entendido",
                                    text: "El Usuario presenta información relacionada"
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
            this.createarea.events();
            this.edit.events();
            this.addactividades.events();
            this.editarea.events();
        }

    };
   
    return {
        init: function () {
            datatable.init();
            search.init();
            modal.init();
            selectarea.init();
        }
    }
}();

$(function () {
    InitApp.init();
})
