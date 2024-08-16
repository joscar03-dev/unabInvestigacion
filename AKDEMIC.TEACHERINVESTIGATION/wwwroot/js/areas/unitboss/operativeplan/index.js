var InitApp = function () {
    var datatable = {
        operativeplan: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: "/jefe-unidad/plan-operativo".proto().parseURL(),
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
                        title: "Nombre",
                        data: "name"
                    },
                    {
                        title: "Año",
                        data: "year"
                    },
                    {
                        title: "Estado",
                        data: "stateText"
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
                    modal.operativeplan.edit.show(id);
                });

                $("#data-table").on('click', '.btn-delete', function () {
                    let id = $(this).data("id");
                    swal({
                        title: "¿Está seguro?",
                        text: "El Plan Operativo sera eliminado",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonText: "Sí, eliminarlo",
                        confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                        cancelButtonText: "Cancelar",
                        showLoaderOnConfirm: true,
                        preConfirm: () => {
                            return new Promise((resolve) => {
                                $.ajax({
                                    url: ("/jefe-unidad/plan-operativo?handler=Delete").proto().parseURL(),
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
                                            text: "El Plan Operativo ha sido eliminado con exito",
                                            confirmButtonText: "Excelente"
                                        }).then(datatable.operativeplan.reload());
                                    },
                                    error: function (error) {

                                        let errorMessage = "El Plan operativo presenta información relacionada";
                                        if (error != null) {
                                            errorMessage = error.responseText;
                                        }
                                        swal({
                                            type: "error",
                                            title: "Error",
                                            confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                            confirmButtonText: "Entendido",
                                            text: errorMessage
                                        });
                                    }
                                });
                            });
                        },
                        allowOutsideClick: () => !swal.isLoading()
                    });


                });
            }
        },
        init: function () {
            this.operativeplan.init();
        }
    };



    var search = {
        init: function () {
            $("#search").doneTyping(function () {
                datatable.operativeplan.reload();
            });
        }
    };

    var modal = {
        operativeplan: {
            create: {
                object: $("#add-form").validate({
                    submitHandler: function (formElement, e) {
                        $("#btnSave").addLoader();
                        e.preventDefault();
                        let formData = new FormData(formElement);
                        $.ajax({
                            url: $(formElement).attr("action"),
                            type: "POST",
                            data: formData,
                            contentType: false,
                            processData: false
                        }).done(function () {
                            $(".modal").modal("hide");
                            $(".m-alert").addClass("m--hide");
                            toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                            datatable.operativeplan.reload();
                            modal.operativeplan.create.clear
                            $("#btnSave").removeLoader();
                        }).fail(function (error) {
                            toastr.error(error.responseText, _app.constants.toastr.title.error);
                            $("#btnSave").removeLoader();
                        }).always(function () {

                        });
                    }
                }),
                show: function () {
                    $("#btnSave").removeLoader();
                    $("#addModal").modal("toggle");
                },
                clear: function () {
                    modal.operativeplan.create.object.resetForm();
                    $("#add-form").find(".custom-file-label").text("Seleccionar archivo");
                },
                events: function () {
                    $("#addModal").on("hidden.bs.modal", function () {
                        modal.operativeplan.create.clear();
                    });
                }
            },
            edit: {
                object: $("#edit-form").validate({
                    submitHandler: function (formElement, e) {
                        $("#btnEdit").addLoader();
                        e.preventDefault();
                        let formData = new FormData(formElement);
                        $.ajax({
                            url: $(formElement).attr("action"),
                            type: "POST",
                            data: formData,
                            contentType: false,
                            processData: false
                        }).done(function () {
                            $(".modal").modal("hide");
                            $(".m-alert").addClass("m--hide");
                            toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);

                            datatable.operativeplan.reload();
                            modal.operativeplan.edit.clear();
                            $("#btnEdit").removeLoader();
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
                        url: `/jefe-unidad/plan-operativo`.proto().parseURL(),
                        type: "GET",
                        data: {
                            handler: "DetailOperativePlan",
                            Id: id
                        },
                    }).done(function (result) {
                        $("#edit-form input[name=Id]").val(result.id);
                        $("#edit-form input[name=Name]").val(result.name);
                        $("#edit-form").find(".custom-file-label").text("Seleccionar archivo");
                        $("#edit-form").find(".custom-file").append('<div class="form-control-feedback">*Seleccionar en caso sea necesario reemplazar el documento vigente.</div>');
                        $("#btnEdit").removeLoader();
                    }).fail(function (error) {
                        toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                        $("#btnEdit").removeLoader();
                    }).always(function () {

                    });
                },
                show: function (id) {
                    $("#btnEdit").removeLoader();
                    modal.operativeplan.edit.load(id);
                    $("#editModal").modal("toggle");
                },
                clear: function () {
                    modal.operativeplan.edit.object.resetForm();
                },
                events: function () {
                    $("#editModal").on("hidden.bs.modal", function () {
                        modal.operativeplan.edit.clear();
                    });
                }
            },
            init: function () {
                this.create.events();
                this.edit.events();
            }
        },
        init: function () {
            this.operativeplan.init();

        }
    };


    return {
        init: function () {
            datatable.init();
            search.init();
            modal.init();
        }
    }
}();

$(function () {
    InitApp.init();
})