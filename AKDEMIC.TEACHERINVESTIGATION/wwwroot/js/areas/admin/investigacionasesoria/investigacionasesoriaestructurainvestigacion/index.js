var InitApp = function () {
    var select = {
        init: function () {
            this.researcherInvestigacionasesoriaTipotrabajoinvestigacion.init();
        },
        researcherInvestigacionasesoriaTipotrabajoinvestigacion: {
            init: function () {
                this.load();
            },
            load: function () {
                $.ajax({
                    url: (`/api/investigacionasesoriatipotrabajoinvestigacion/select/get`).proto().parseURL(),
                    type: "GET"
                }).done(function (result) {
                    $('.input_IdTipotrabajoinvestigacion').select2({
                        data: result,
                        maximumSelectionLength: 1,
                    });
                });
            }
        },
    };
    var datatable = {
        investigacionasesoriaEstructurainvestigacion: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: "/investigacionasesoria/estructurainvestigacion".proto().parseURL(),
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
                        title: "Tipo de Investigación",
                        data: "nombretipotrabajoinvestigacion",
                        width:200
                    },
                    {
                        title: "Nombre",
                        data: "nombre"
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
                        width: 180,
                        orderable: false,
                        render: function (data) {
                            var template = "";
                            //anexo
                            template += "<button title='Subir anexo' class='btn btn-success m-btn btn-sm m-btn--icon-only btn-actividades '  data-id='" + data.id + "'>";
                            template += "<span><i class='la la-edit'></i><span>Requisitos</span></span></button>";


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
            this.investigacionasesoriaEstructurainvestigacion.init();
        }
    };
    var search = {
        init: function () {
            $("#search").doneTyping(function () {
                datatable.investigacionasesoriaEstructurainvestigacion.reload();
            });
        }
    };
    var modal = {
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
                        datatable.investigacionasesoriaEstructurainvestigacion.reload();
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
                $("#add-form select[name=IdTipotrabajoinvestigacion]").val("");
                $("#add-form select[name=IdTipotrabajoinvestigacion]").trigger("change");
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
                        datatable.investigacionasesoriaEstructurainvestigacion.reload();
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
                    url: "/investigacionasesoria/estructurainvestigacion".proto().parseURL(),
                    type: "GET",
                    data: {
                        handler: "Detail",
                        id: id
                    },
                }).done(function (result) {
                    $("#edit-form input[name=Id]").val(result.id);
                    $("#edit-form input[name=codigo]").val(result.codigo);
                    $("#edit-form input[name=nombre]").val(result.nombre);
                    $("#edit-form select[name=IdTipotrabajoinvestigacion]").val(result.idTipotrabajoinvestigacion);
                    $("#edit-form select[name=IdTipotrabajoinvestigacion]").trigger("change");
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
                $("#edit-form select[name=IdTipotrabajoinvestigacion]").val("");
                $("#edit-form select[name=IdTipotrabajoinvestigacion]").trigger("change");
                modal.edit.object.resetForm();
            },
            events: function () {
                $("#editModal").on("hidden.bs.modal", function () {
                    modal.edit.clear();
                });
            }
        },
        addactividades: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: "/investigacionasesoria/estructurainvestigacion".proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        data.handler = "Datatableactividades";
                        data.id = $("#addactividades-form input[name=Id]").val();
                    }
                },
                pageLength: 50,
                orderable: [],
                columns: [
                    {
                        title: "Orden",
                        data: "orden",
                        width: 100
                    },
                    {
                        title: "Requisito",
                        data: "descripcion",
                    },

                    {
                        title: "Opciones",
                        data: null,
                        width: 100,
                        orderable: false,
                        render: function (data) {
                            var template = "";


                          
                            template += "<button title='Editar'  type='button'  ";
                            template += "class='btn btn-info btn-editactividad ";
                            template += "m-btn btn-sm  m-btn--icon-only' ";
                            template += " data-id='" + data.id + "'>";
                            template += "<i class='la la-edit'></i></button>";
                                

                            return template;
                        }
                    }
                ],
                rowCallback: function (row, data) {
                   
                    contaractividad = contaractividad + 1;

                },
            },
            load: function (id) {
                $("#addactividades-form input[name=Id]").val(id);
                if (this.object == null) {
                    this.object = $("#data-tableactividades").DataTable(this.options);
                } else {
                    this.reload();
                }
                modal.addactividades.events();
                arrayactividad = new Array();
                contaractividad = 0;
                contaraprobado = 0;

            },
            reload: function () {
                this.object.ajax.reload();
                arrayactividad = new Array();
                contaractividad = 0;
            },
            show: function (id) {
                //$("#btnEdit").removeLoader();

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
                    modal.editactividades.show(id);
                });
                $("#data-tableactividades").on('click', '.btn-editactividadfinal', function () {

                    var id = $(this).data("id");
                    modal.editactividadesfinal.show(id);
                });
                $("#data-tableactividades").on('click', '.btn-editobservacionactividad', function () {

                    var id = $(this).data("id");
                    modal.editobservacionactividad.show(id);
                });

                $("#data-tableactividades").on('click', '.btn-enviaractividad', function () {
                    var id = $(this).data("id");
                    modal.enviaractividad(id);

                });
                $("#data-tableactividades").on('click', '.btn-observacionactividad', function () {
                    var id = $(this).data("id");
                    modal.observacionactividad(id);

                });


            }
        },

        delete: function (id) {
            swal({
                title: "¿Está seguro?",
                text: "La carrera será eliminado.",
                type: "warning",
                showCancelButton: true,
                confirmButtonText: "Sí, eliminarlo",
                confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                cancelButtonText: "Cancelar",
                showLoaderOnConfirm: true,
                preConfirm: () => {
                    return new Promise((resolve) => {                        
                        $.ajax({
                            url: ("/investigacionasesoria/estructurainvestigacion?handler=Delete").proto().parseURL(),
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
                                    text: "La Estructura ha sido eliminado con exito",
                                    confirmButtonText: "Excelente"
                                }).then(datatable.investigacionasesoriaEstructurainvestigacion.reload());
                            },
                            error: function (errormessage) {
                                swal({
                                    type: "error",
                                    title: "Error",
                                    confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                    confirmButtonText: "Entendido",
                                    text: "La Estructura presenta información relacionada"
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
   
    return {
        init: function () {
            datatable.init();
            search.init();
            modal.init();
            select.init();
        }
    }
}();

$(function () {
    InitApp.init();
})
