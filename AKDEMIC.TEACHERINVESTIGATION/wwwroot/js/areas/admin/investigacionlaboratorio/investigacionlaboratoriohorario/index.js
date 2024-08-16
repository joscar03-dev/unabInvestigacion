var InitApp = function () {
    var selectdocente = {
        init: function () {
            this.researcherMaestroDocente.init();
        },
        researcherMaestroDocente: {
            init: function () {
                this.load();
            },
            load: function () {
                $.ajax({
                    url: (`/api/maestrodocente/select/get`).proto().parseURL(),
                    type: "GET"
                }).done(function (result) {
                    $('.input_IdDocente').select2({
                        data: result,
                        maximumSelectionLength: 1,
                    });
                });
            }
        },
    };
    var selectproyecto = {
        init: function () {
            this.researcherMaestroProyecto.init();
        },
        researcherMaestroProyecto: {
            init: function () {
                this.load();
            },
            load: function () {
                $.ajax({
                    url: (`/api/investigacionlaboratorioproyecto/select/get`).proto().parseURL(),
                    type: "GET"
                }).done(function (result) {
                    $('.input_IdProyecto').select2({
                        data: result,
                        maximumSelectionLength: 1,
                    });
                });
            }
        },
    };
    var selectequipo = {
        init: function () {
            this.researcherMaestroEquipo.init();
        },
        researcherMaestroEquipo: {
            init: function () {
                this.load();
            },
            load: function () {
                $.ajax({
                    url: (`/api/maestroequipo/select/get`).proto().parseURL(),
                    type: "GET"
                }).done(function (result) {
                    $('.input_IdEquipo').select2({
                        data: result,
                        maximumSelectionLength: 1,
                    });
                });
            }
        },
    };
    var selectlaboratorio = {
        init: function () {
            this.researcherMaestroLaboratorio.init();
        },
        researcherMaestroLaboratorio: {
            init: function () {
                this.load();
            },
            load: function () {
                $.ajax({
                    url: (`/api/maestrolaboratorio/select/get`).proto().parseURL(),
                    type: "GET"
                }).done(function (result) {
                    $('.input_IdLaboratorio').select2({
                        data: result,
                        maximumSelectionLength: 1,
                    });
                });
            }
        },
    };
    var datatable = {
        maestroAreaacademica: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: "/investigacionlaboratorio/horario".proto().parseURL(),
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
                        title: "Proyecto",
                        data: "nombreproyecto",
                        width: 100,
                    },
                    {
                        title: "Laboratorio",
                        data: "nombrelaboratorio",
                        width: 100,
                    },
                    {
                        title: "Docente",
                        data: "nombredocente",
                        width: 200,
                    }, {
                        title: "Fecha y Hora Inicial",
                        data: "fechaini",
                        width:100,
                    },
                    {
                        title: "Fecha y Hora Final",
                        data: "fechafin",
                        width: 100,
                    },
                    {
                        title: "Actividad",
                        data: "actividad"
                    },
                   
                   
                    {
                        title: "Opciones",
                        data: null,
                        width:100,
                        orderable: false,
                        render: function (data) {
                            var template = "";

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
            }
        },
        init: function () {
            this.maestroAreaacademica.init();
        }
    };
    var search = {
        init: function () {
            $("#search").doneTyping(function () {
                datatable.maestroAreaacademica.reload();
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
                        datatable.maestroAreaacademica.reload();
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
                        datatable.maestroAreaacademica.reload();
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
                    url: "/investigacionlaboratorio/horario".proto().parseURL(),
                    type: "GET",
                    data: {
                        handler: "Detail",
                        id: id
                    },
                }).done(function (result) {
                    $("#edit-form input[name=Id]").val(result.id);
                    $("#edit-form input[name=codigo]").val(result.codigo);
                    $("#edit-form input[name=actividad]").val(result.actividad);
                    $("#edit-form select[name=IdLaboratorio]").val(result.idLaboratorio);
                    $("#edit-form select[name=IdLaboratorio]").trigger("change");
                    $("#edit-form select[name=IdDocente]").val(result.idDocente);
                    $("#edit-form select[name=IdDocente]").trigger("change");
                    $("#edit-form select[name=IdProyecto]").val(result.idProyecto);
                    $("#edit-form select[name=IdProyecto]").trigger("change");
                    $("#edit-form select[name=IdEquipo]").val(result.idEquipo);
                    $("#edit-form select[name=IdEquipo]").trigger("change");
                    $("#edit-form input[name=fechaini]").val(result.fechaini);
                    $("#edit-form input[name=fechafin]").val(result.fechafin);


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
        delete: function (id) {
            swal({
                title: "¿Está seguro?",
                text: "El Horario será eliminado.",
                type: "warning",
                showCancelButton: true,
                confirmButtonText: "Sí, eliminarlo",
                confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                cancelButtonText: "Cancelar",
                showLoaderOnConfirm: true,
                preConfirm: () => {
                    return new Promise((resolve) => {                        
                        $.ajax({
                            url: ("/investigacionlaboratorio/horario?handler=Delete").proto().parseURL(),
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
                                    text: "El Horario ha sido eliminado con exito",
                                    confirmButtonText: "Excelente"
                                }).then(datatable.maestroAreaacademica.reload());
                            },
                            error: function (errormessage) {
                                swal({
                                    type: "error",
                                    title: "Error",
                                    confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                    confirmButtonText: "Entendido",
                                    text: "El Horario presenta información relacionada"
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
    var datePickers = {
        init: function () {

          

            $("#add-form input[name=fechaini]").datetimepicker({
                clearBtn: true,
                orientation: "bottom",
                format: _app.constants.formats.datetimepicker
            });

            $("#add-form input[name=fechafin]").datetimepicker({
                clearBtn: true,
                orientation: "bottom",
                format: _app.constants.formats.datetimepicker
            });


            $("#edit-form input[name=fechaini]").datetimepicker({
                clearBtn: true,
                orientation: "bottom",
                format: _app.constants.formats.datetimepicker
            });

            $("#edit-form input[name=fechafin]").datetimepicker({
                clearBtn: true,
                orientation: "bottom",
                format: _app.constants.formats.datetimepicker
            });
        }
    }

    return {
        init: function () {
            datatable.init();
            search.init();
            modal.init();
            datePickers.init();
            selectdocente.init();
            selectequipo.init();
            selectproyecto.init();
            selectlaboratorio.init();
        }
    }
}();

$(function () {
    InitApp.init();
})
