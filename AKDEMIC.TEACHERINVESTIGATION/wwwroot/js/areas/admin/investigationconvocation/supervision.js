var InitApp = function () {
    var investigationConvocationId = $('#InvestigationConvocationId').val();
    var datatable = {
        supervisor: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: `/admin/convocatoria-de-investigacion/${investigationConvocationId}/comite-tecnico`.proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        data.handler = "SupervisorDatatable";
                        data.searchValue = $("#search").val();
                    }
                },
                pageLength: 50,
                orderable: [],
                columns: [
                    {
                        title: "Código de Usuario",
                        data: "userName"
                    },
                    {
                        title: "Nombre ",
                        data: "name"
                    },
                    {
                        title: "Apellido Paterno",
                        data: "paternalSurname"
                    },
                    {
                        title: "Apellido Materno",
                        data: "maternalSurname"
                    },
                    {
                        title: "DNI",
                        data: "dni"
                    },
                    {
                        title: "Opciones",
                        data: null,
                        orderable: false,
                        render: function (data) {
                            var template = "";
                            //Delete
                            template += "<button ";
                            template += "class='btn btn-danger btn-delete ";
                            template += "m-btn btn-sm  m-btn--icon-only' ";
                            template += " data-id='" + data.userId + "'>";
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
                $("#data-table").on('click', '.btn-delete', function () {
                    var id = $(this).data("id");
                    modal.delete(id);
                });
            }
        },
        init: function () {
            this.supervisor.init();
        }
    };
    var search = {
        init: function () {
            $("#search").doneTyping(function () {
                datatable.supervisor.reload();
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

                        datatable.supervisor.reload();
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
                $("#add-form select[name=UserId]").empty();
                modal.create.object.resetForm();
            },
            events: function () {
                $("#addModal").on("hidden.bs.modal", function () {
                    modal.create.clear();
                });
            }
        },
        delete: function (id) {
            swal({
                title: "¿Está seguro?",
                text: "El miembro del comité técnico será eliminado.",
                type: "warning",
                showCancelButton: true,
                confirmButtonText: "Sí, eliminarlo",
                confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                cancelButtonText: "Cancelar",
                showLoaderOnConfirm: true,
                preConfirm: () => {
                    return new Promise((resolve) => {
                        $.ajax({
                            url: (`/admin/convocatoria-de-investigacion/${investigationConvocationId}/comite-tecnico`).proto().parseURL(),
                            type: "GET",
                            data: {
                                handler: "SupervisorDelete",
                                userId: id
                            },
                            success: function (result) {
                                swal({
                                    type: "success",
                                    title: "Completado",
                                    text: "El miembro del comité técnico ha sido eliminado con exito",
                                    confirmButtonText: "Excelente"
                                }).then(datatable.supervisor.reload());
                            },
                            error: function (errormessage) {
                                swal({
                                    type: "error",
                                    title: "Error",
                                    confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                    confirmButtonText: "Entendido",
                                    text: "El miembro del comité técnico presenta información relacionada"
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
        }

    };
    var events = {
        init: function () {
            $("#AddBtn").on('click', function () {
                modal.create.show();
            });
        }
    };
    var select = {
        init: function () {
            this.supervisor.init();
            this.userType.init();
        },
        userType: {
            init: function () {
                this.load();
                this.events();
            },
            load: function () {
                $('#add-form select[name="UserType"]').select2({ dropdownParent: $('#addModal')});
            },
            events: function () {
                $('#add-form select[name="UserType"]').on("change", function () {
                    $("#add-form select[name=UserId]").empty();
                });
            }
        },
        supervisor: {
            init: function () {
                $("#add-form select[name=UserId]").select2({
                    width: "100%",
                    dropdownParent: $('#addModal'),
                    placeholder: "Buscar...",
                    ajax: {
                        url: "/api/usuarios/select-search".proto().parseURL(),
                        dataType: "json",
                        data: function (params) {
                            return {
                                userType: $('#add-form select[name="UserType"]').val(),
                                term: params.term,
                                page: params.page
                            };
                        },
                        processResults: function (data, params) {
                            return {
                                results: data
                            };
                        },
                        cache: true
                    },
                    escapeMarkup: function (markup) {
                        return markup;
                    },
                    minimumInputLength: 3
                });
            }            
        }
    };
    
    return {
        init: function () {
            select.init();
            datatable.init();
            modal.init();
            events.init();
            search.init();
        }
    }
}();

$(function () {
    InitApp.init();
})
