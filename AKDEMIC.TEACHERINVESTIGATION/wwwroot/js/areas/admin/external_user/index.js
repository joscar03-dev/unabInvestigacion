var index = function () {

    var BaseRoute = `/admin/usuarios-externos`;

    var datatable = {
        external_user: {
            object: null,
            options: {
                ajax: {
                    url: `${BaseRoute}`,
                    type: "GET",
                    data: function (data) {
                        data.handler = "ExternalUserDatatable";
                        data.search = $("#search").val();
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
                        data: null,
                        title: "Opciones",
                        render: function (row) {
                            var tpm = "";
                            tpm += `<a href="/admin/usuarios-externos/editar/${row.id}" class="btn btn-brand m-btn btn-sm m-btn m-btn--icon"><span><i class="la la-edit"></i><span>Editar</span></span></a> `;
                            tpm += `<button data-id='${row.id}' class="btn btn-delete btn-danger m-btn m-btn--icon btn-sm m-btn--icon-only"><i class="la la-trash"></i></button>`;
                            return tpm;
                        }
                    }
                ]
            },
            reload: function () {
                datatable.external_user.object.ajax.reload();
            },
            events: {
                onDelete: function () {
                    $("#data-table").on("click", ".btn-delete", function () {

                        var id = $(this).data("id");

                        swal({
                            type: "warning",
                            title: "Eliminará al usuario seleccionado.",
                            text: "¿Seguro que desea eliminarlo?.",
                            confirmButtonText: "Aceptar",
                            showCancelButton: true,
                            showLoaderOnConfirm: true,
                            allowOutsideClick: () => !swal.isLoading(),
                            preConfirm: () => {
                                return new Promise(() => {
                                    $.ajax({
                                        type: "POST",
                                        url: `${BaseRoute}?handler=DeleteExternalUser&id=${id}`,
                                        beforeSend: function (xhr) {
                                            xhr.setRequestHeader("XSRF-TOKEN",
                                                $('input:hidden[name="__RequestVerificationToken"]').val());
                                        }
                                    })
                                        .done(function () {
                                            datatable.external_user.reload();
                                            swal({
                                                type: "success",
                                                title: "Completado",
                                                text: "Usuario eliminado satisfactoriamente.",
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
                    })
                },
                init: function () {
                    this.onDelete();
                }
            },
            init: function () {
                datatable.external_user.object = $("#data-table").DataTable(datatable.external_user.options);
                datatable.external_user.events.init();
            }
        },
        init: function () {
            datatable.external_user.init();
        }
    }


    var events = {
        onSearch: function () {
            $("#search").doneTyping(function () {
                datatable.external_user.reload();
            })
        },
        init: function () {
            this.onSearch();
        }
    }

    return {
        init: function () {
            datatable.init();
            events.init();
        }
    }
}();

$(() => {
    index.init();
});