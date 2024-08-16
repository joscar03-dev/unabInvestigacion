var InitApp = function () {
    var selectcarrera = {
        init: function () {
            this.researcherMaestroCarrera.init();
        },
        researcherMaestroCarrera: {
            init: function () {
                this.load();
            },
            load: function () {
                $.ajax({
                    url: (`/api/maestrocarrera/select/get`).proto().parseURL(),
                    type: "GET"
                }).done(function (result) {
                    $('.input_IdCarrera').select2({
                        data: result,
                        maximumSelectionLength: 1,
                    });
                });
            }
        },
    };
  
    var selectusuario = {
        init: function () {
            this.researcherMaestroUsuario.init();
        },
        researcherMaestroUsuario: {
            init: function () {
                this.load();
            },
            load: function () {
                $.ajax({
                    url: (`/api/maestrousuario/select/get`).proto().parseURL(),
                    type: "GET"
                }).done(function (result) {
                    $('.input_IdUser').select2({
                        data: result,
                        maximumSelectionLength: 1,
                    });
                });
            }
        },
    };
  
    var datatable = {
        maestroDocente: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: "/investigacionasesoria/asesor".proto().parseURL(),
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
                        title: "Docente",
                        data: "nombreuser"
                    },
                    {
                        title: "Carrera",
                        data: "nombrecarrera",
                    },
                    {
                        title: "Opciones",
                        data: null,
                        width: 80,
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
            this.maestroDocente.init();
        }
    };
    var search = {
        init: function () {
            $("#search").doneTyping(function () {
                datatable.maestroDocente.reload();
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
                        datatable.maestroDocente.reload();
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
                $("#edit-form select[name=IdCarrera]").val('');
                $("#edit-form select[name=IdCarrera]").trigger("change");

                $("#edit-form select[name=IdUser]").val('');
                $("#edit-form select[name=IdUser]").trigger("change");
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
                        datatable.maestroDocente.reload();
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
                    url: "/investigacionasesoria/asesor".proto().parseURL(),
                    type: "GET",
                    data: {
                        handler: "Detail",
                        id: id
                    },
                }).done(function (result) {
                    $("#edit-form input[name=Id]").val(result.id);
                    $("#edit-form select[name=IdCarrera]").val(result.idCarrera);
                    $("#edit-form select[name=IdCarrera]").trigger("change");
                  
                    $("#edit-form select[name=IdUser]").val(result.idUser);
                    $("#edit-form select[name=IdUser]").trigger("change");
                 
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
                $("#edit-form select[name=IdCarrera]").val('');
                $("#edit-form select[name=IdCarrera]").trigger("change");

                $("#edit-form select[name=IdUser]").val('');
                $("#edit-form select[name=IdUser]").trigger("change");
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
                text: "El docente será eliminado.",
                type: "warning",
                showCancelButton: true,
                confirmButtonText: "Sí, eliminarlo",
                confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                cancelButtonText: "Cancelar",
                showLoaderOnConfirm: true,
                preConfirm: () => {
                    return new Promise((resolve) => {                        
                        $.ajax({
                            url: ("/investigacionasesoria/asesor?handler=Delete").proto().parseURL(),
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
                                    text: "El Docente ha sido eliminado con exito",
                                    confirmButtonText: "Excelente"
                                }).then(datatable.maestroDocente.reload());
                            },
                            error: function (errormessage) {
                                swal({
                                    type: "error",
                                    title: "Error",
                                    confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                    confirmButtonText: "Entendido",
                                    text: "El Docente presenta información relacionada"
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
            selectcarrera.init();           
            selectusuario.init();
           
        }
    }
}();

$(function () {
    InitApp.init();
})
