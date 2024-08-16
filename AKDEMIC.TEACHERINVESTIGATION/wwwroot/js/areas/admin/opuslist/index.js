var InitApp = function () {
    var selectValor = {
        init: function () {
            this.researcherOpusValor.init();
        },
        researcherOpusValor: {
            init: function () {
                this.load();
            },
            load: function () {
                $.ajax({
                    url: (`/api/valor-de-obra/select/get`).proto().parseURL(),
                    type: "GET"
                }).done(function (result) {
                    $('.input_IdValor').select2({
                        data: result,
                        maximumSelectionLength: 1,

                    });
                });
            }
        },
    };
    var datatable = {
        opusType: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: "/admin/list".proto().parseURL(),
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
                        data: "code",
                        width: 50,
                    },
                    {
                        title: "Nombre",
                        data: "name"
                    },
                    {
                        title: "Opciones",
                        data: null,
                        width:180,

                        orderable: false,
                        render: function (data) {
                            var template = "";
                            //valor
                            template += "<button title='Valors de la lista' ";
                            template += " style = 'background-color:#aaeee8' ";
                            template += " class='btn  btn-valor ";
                            template += " m-btn btn-sm  m-btn--icon-only' ";
                            template += " data-id='" + data.id + "' > ";
                            template += "<span><i class='la la-bar-chart-o'></i><span> ";
                            template += "Valores</span ></span ></button > ";


                            //Edit
                            template += "<button type='button' ";
                            template += "class='btn btn-info btn-edit ";
                            template += "m-btn btn-sm  m-btn--icon-only' ";
                            template += " data-id='" + data.id + "'>";
                            template += "<i class='la la-edit'></i></button> ";

                            //Delete
                            template += "<button type='button' ";
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
                $("#data-table").on('click', '.btn-valor', function () {

                    var id = $(this).data("id");
                    modal.addvalor.show(id);
                });
            }
        },
        init: function () {
            this.opusType.init();
        }
    };
    var search = {
        init: function () {
            $("#search").doneTyping(function () {
                datatable.opusType.reload();
            });
        }
    };
    var modal = {
        addvalor: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: "/admin/list".proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        data.handler = "DatatableValor";
                        data.id = $("#addvalor-form input[name=Id]").val();
                    }
                },
                pageLength: 50,
                orderable: [],
                columns: [

                    {
                        title: "Valor",
                        data: "nombrevalor"
                    },
                    {
                        title: "Estado",
                        data: null,
                        width: 50,
                        render: function (data) {
                            var estado = data.activo == 1 ? "ACTIVO" : "ACTIVO";
                            var label = data.activo == 1 ? "label-success" : "label-success";
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
                        width: 40,
                        orderable: false,
                        render: function (data) {
                            var template = "";



                            //Edit
                            template += "<button style='display:none' title='Editar'  type='button'  ";
                            template += "class='btn btn-info btn-editvalor ";
                            template += "m-btn btn-sm  m-btn--icon-only' ";
                            template += " data-id='" + data.id + "'>";
                            template += "<i class='la la-edit'></i></button>";

                            //Delete
                            template += "<button type='button' ";
                            template += "class='btn btn-danger btn-deletevalor ";
                            template += "m-btn btn-sm  m-btn--icon-only' ";
                            template += " data-id='" + data.id + "'>";
                            template += "<i class='la la-trash'></i></button> ";




                            return template;
                        }
                    }
                ],

            },
            load: function (id) {
                $("#addvalor-form input[name=Id]").val(id);
                $("#nuevovalor-form input[name=IdList]").val(id);
                if (this.object == null) {
                    this.object = $("#data-tablevalor").DataTable(this.options);
                } else {
                    this.reload();
                }
                modal.addvalor.events();


            },
            reload: function () {
                this.object.ajax.reload();

            },
            show: function (id) {
                modal.addvalor.load(id);
                $("#addModalValor").modal("toggle");
            },
            clear: function () {
                // modal.addarearequisito.object.resetForm();
            },
            events: function () {

                $("#addModalValor").on("hidden.bs.modal", function () {
                    modal.addvalor.clear();
                });
                $("#addModalValor").on("hidden.bs.modal", function () {
                    modal.addvalor.clear();
                });


                $("#data-tablevalor").on('click', '.btn-editvalor', function () {
                    var id = $(this).data("id");
                    modal.editvalor.show(id);
                });

                $("#data-tablevalor").on('click', '.btn-deletevalor', function () {
                    var id = $(this).data("id");
                    modal.deletevalor(id);

                });



            }
        },
        createvalor: {
            object: $("#nuevovalor-form").validate({
                submitHandler: function (form, e) {
                    $("#btnNuevovalor").addLoader();
                    id = $("#addvalor-form input[name=Id]").val();
                    $("#nuevovalor-form input[name=IdList]").val(id);
                    e.preventDefault();
                    let formData = new FormData(form);
                    $.ajax({
                        url: $(form).attr("action"),
                        type: "POST",
                        data: formData,
                        contentType: false,
                        processData: false
                    }).done(function () {
                        $("#nuevovalor-form select[name=IdValor]").val("");
                        $("#nuevovalor-form select[name=IdValor]").trigger("change");
                        $("#nuevoModalValor").modal("hide");
                        $(".m-alert").addClass("m--hide");
                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);

                        modal.addvalor.reload();
                        modal.createvalor.clear();
                        $("#btnNuevovalor").removeLoader();

                        //location.reload();
                    }).fail(function (error) {
                        toastr.error(error.responseText, _app.constants.toastr.title.error);
                        $("#btnNuevovalor").removeLoader();
                        //if (error.responseText !== null && error.responseText !== "") $("#add_form_msg_txt").html(error.responseText);
                        //else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                        //$("#add_form_msg").removeClass("m--hide").show();
                    }).always(function () {

                    });
                }
            }),
            show: function () {
                $("#btnNuevovalor").removeLoader();
                $("#nuevoModalValor").modal("toggle");
            },
            clear: function () {
                modal.createvalor.object.resetForm();
            },
            events: function () {
                $("#nuevoModalAreaIdListaverificaciones").on("hidden.bs.modal", function () {
                    modal.createvalor.clear();
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
                        datatable.opusType.reload();
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
                        datatable.opusType.reload();
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
                    url: `/admin/list`.proto().parseURL(),
                    type: "GET",
                    data: {
                        handler: "Detail",
                        id: id
                    },
                }).done(function (result) {
                    $("#edit-form input[name=Id]").val(result.id);
                    $("#edit-form input[name=Code]").val(result.code);
                    $("#edit-form input[name=Name]").val(result.name);
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
                text: "La lista será eliminado.",
                type: "warning",
                showCancelButton: true,
                confirmButtonText: "Sí, eliminarlo",
                confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                cancelButtonText: "Cancelar",
                showLoaderOnConfirm: true,
                preConfirm: () => {
                    return new Promise((resolve) => {
                        $.ajax({
                            url: ("/admin/list?handler=Delete").proto().parseURL(),
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
                                    text: "El tipo de obra ha sido eliminado con exito",
                                    confirmButtonText: "Excelente"
                                }).then(datatable.opusType.reload());
                            },
                            error: function (errormessage) {
                                swal({
                                    type: "error",
                                    title: "Error",
                                    confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                    confirmButtonText: "Entendido",
                                    text: "El tipo de obra presenta información relacionada"
                                });
                            }
                        });
                    });
                },
                allowOutsideClick: () => !swal.isLoading()
            });
        },
        deletevalor: function (id) {
            swal({
                title: "¿Está seguro?",
                text: "El valor será eliminada.",
                type: "warning",
                showCancelButton: true,
                confirmButtonText: "Sí, eliminarlo",
                confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                cancelButtonText: "Cancelar",
                showLoaderOnConfirm: true,
                preConfirm: () => {
                    return new Promise((resolve) => {
                        $.ajax({
                            url: ("/admin/list?handler=Deletevalor").proto().parseURL(),
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
                                    text: "El valor ha sido eliminado con exito",
                                    confirmButtonText: "Excelente"
                                }).then(modal.addvalor.reload());
                            },
                            error: function (errormessage) {
                                swal({
                                    type: "error",
                                    title: "Error",
                                    confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                    confirmButtonText: "Entendido",
                                    text: "El valor presenta información relacionada"
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
            this.addvalor.events();
            this.createvalor.events();
        }

    };
    return {
        init: function () {
            datatable.init();
            search.init();
            modal.init();
            selectValor.init();
            
        }
    }
}();

$(function () {
    InitApp.init();
})