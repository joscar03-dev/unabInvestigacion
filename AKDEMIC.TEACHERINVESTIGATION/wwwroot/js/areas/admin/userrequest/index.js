var InitApp = function () {
    var datatable = {
        userrequest: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: "/admin/solicitudes-de-registro-externo".proto().parseURL(),
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
                        title: "Apellido Paterno",
                        data: "paternalSurname"
                    },
                    {
                        title: "Apellido Materno",
                        data: "maternalSurname"
                    },
                    {
                        title: "Nombre",
                        data: "name"
                    },
                    {
                        title: "Dni",
                        data: "dni"
                    },
                    {
                        title: "Correo",
                        data: "email"
                    },
                    {
                        title: "Estado",
                        data: "stateText"
                    },
                    {
                        title: "Tipo",
                        data: "typeText"
                    },
                    {
                        title: "Opciones",
                        data: null,
                        orderable: false,
                        render: function (data) {
                            var template = "";

                            if (data.state == 1 && !data.hasLogged) {
                                //Accept                  
                                template += "<button type='button' ";
                                template += "class='btn btn-primary btn-send-email ";
                                template += "m-btn btn-sm  m-btn--icon-only' ";
                                template += " data-id='" + data.id + "'>";
                                template += "<i class='flaticon-multimedia-3'></i></button> ";
                            }
                            
                            if (data.state == 0) {

                                //Accept                  
                                template += "<button type='button' ";
                                template += "class='btn btn-primary btn-accept ";
                                template += "m-btn btn-sm  m-btn--icon-only' ";
                                template += " data-id='" + data.id + "'>";
                                template += "<i class='la la-check'></i></button> ";

                                //Reject
                                template += "<button type='button' ";
                                template += "class='btn btn-brand btn-reject ";
                                template += "m-btn btn-sm  m-btn--icon-only' ";
                                template += " data-id='" + data.id + "'>";
                                template += "<i class='la la-remove'></i></button> ";
                            }


                            if (data.state == 0 || data.state == 2) {
                                //Delete
                                template += "<button type='button' ";
                                template += "class='btn btn-danger btn-delete ";
                                template += "m-btn btn-sm  m-btn--icon-only' ";
                                template += " data-id='" + data.id + "'>";
                                template += "<i class='la la-trash'></i></button> ";
                            }


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
                $("#data-table").on('click', '.btn-send-email', function () {
                    var btn = $(this);
                    let id = $(this).data("id");
                    btn.addLoader();
                    $.ajax({
                        url: ("/admin/solicitudes-de-registro-externo?handler=UserRequestReSendEmail").proto().parseURL(),
                        type: "POST",
                        data: {
                            id: id
                        },
                        beforeSend: function (xhr) {
                            xhr.setRequestHeader("XSRF-TOKEN",
                                $('input:hidden[name="__RequestVerificationToken"]').val());
                        },
                    }).done(function (result) {
                        let defaultText = _app.constants.toastr.message.success.task;
                        if (!(result == null || result == '')) {
                            defaultText = result;
                        }
                        toastr.success(defaultText, _app.constants.toastr.title.success);
                        datatable.userrequest.reload();
                        btn.removeLoader();
                    }).fail(function (error) {
                        toastr.error(error.responseText, _app.constants.toastr.title.error);
                        btn.removeLoader();
                    }).always(function () {

                    });

                });
                $("#data-table").on('click', '.btn-accept', function () {
                    var btn = $(this);
                    let id = $(this).data("id");
                    btn.addLoader();
                    $.ajax({
                        url: ("/admin/solicitudes-de-registro-externo?handler=UserRequestAccept").proto().parseURL(),
                        type: "POST",
                        data: {
                            id: id
                        },
                        beforeSend: function (xhr) {
                            xhr.setRequestHeader("XSRF-TOKEN",
                                $('input:hidden[name="__RequestVerificationToken"]').val());
                        },
                    }).done(function (result) {
                        let defaultText = _app.constants.toastr.message.success.task;
                        if (!(result == null || result == '')) {
                            defaultText = result;
                        }
                        toastr.success(defaultText, _app.constants.toastr.title.success);
                        datatable.userrequest.reload();
                        btn.removeLoader();
                    }).fail(function (error) {
                        toastr.error(error.responseText, _app.constants.toastr.title.error);
                        btn.removeLoader();
                    }).always(function () {

                    });

                });
                $("#data-table").on('click', '.btn-delete', function () {
                    let id = $(this).data("id");
                    swal({
                        title: "¿Está seguro?",
                        text: "La solicitud será eliminada",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonText: "Sí, eliminarla",
                        confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                        cancelButtonText: "Cancelar",
                        showLoaderOnConfirm: true,
                        preConfirm: () => {
                            return new Promise((resolve) => {
                                $.ajax({
                                    url: ("/admin/solicitudes-de-registro-externo?handler=UserRequestDelete").proto().parseURL(),
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
                                            text: "La solicitud ha sido eliminada con exito",
                                            confirmButtonText: "Excelente"
                                        }).then(datatable.userrequest.reload());
                                    },
                                    error: function (errormessage) {

                                        let defaultText = "La solicitud presenta información relacionada";

                                        if (!(errormessage.responseText == null || errormessage.responseText == ''))
                                            defaultText = errormessage.responseText;
                                        swal({
                                            type: "error",
                                            title: "Error",
                                            confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                            confirmButtonText: "Entendido",
                                            text: defaultText
                                        });
                                    }
                                });
                            });
                        },
                        allowOutsideClick: () => !swal.isLoading()
                    });
                });
                $("#data-table").on('click', '.btn-reject', function () {
                    let id = $(this).data("id");
                    swal({
                        title: "¿Está seguro?",
                        text: "La solicitud sera rechazada",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonText: "Sí, rechazarla",
                        confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                        cancelButtonText: "Cancelar",
                        showLoaderOnConfirm: true,
                        preConfirm: () => {
                            return new Promise((resolve) => {
                                $.ajax({
                                    url: ("/admin/solicitudes-de-registro-externo?handler=UserRequestReject").proto().parseURL(),
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
                                            text: "La solicitud ha sido rechazada con exito",
                                            confirmButtonText: "Excelente"
                                        }).then(datatable.userrequest.reload());
                                    },
                                    error: function (errormessage) {
                                        let defaultText = "La solicitud presenta información relacionada";

                                        if (!(errormessage.responseText == null || errormessage.responseText == ''))
                                            defaultText = errormessage.responseText;

                                        swal({
                                            type: "error",
                                            title: "Error",
                                            confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                            confirmButtonText: "Entendido",
                                            text: defaultText
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
            this.userrequest.init();
        }
    };

    var modal = {
        sendInvitation: {
            object: $("#send-invitation-form").validate({
                submitHandler: function (formElement, e) {
                    $("#btnSend").addLoader();
                    e.preventDefault();
                    $.ajax({
                        url: $(formElement).attr("action"),
                        type: "POST",
                        data: $(formElement).serialize()
                    }).done(function () {
                        $(".modal").modal("hide");
                        $(".m-alert").addClass("m--hide");
                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                        datatable.userrequest.reload();
                        modal.sendInvitation.clear();
                        $("#btnSend").removeLoader();
                    }).fail(function (error) {
                        toastr.error(error.responseText, _app.constants.toastr.title.error);
                        $("#btnSend").removeLoader();
                        //if (error.responseText !== null && error.responseText !== "") $("#add_form_msg_txt").html(error.responseText);
                        //else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                        //$("#add_form_msg").removeClass("m--hide").show();
                    }).always(function () {

                    });
                }
            }),
            show: function () {
                $("#btnSend").removeLoader();
                $("#sendInvitationModal").modal("toggle");
            },
            clear: function () {
                modal.sendInvitation.object.resetForm();
            },
            events: function () {
                $("#sendInvitationModal").on("hidden.bs.modal", function () {
                    modal.sendInvitation.clear();
                });
            }
        },
        init: function () {
            this.sendInvitation.events();
        }

    };

    var search = {
        init: function () {
            $("#search").doneTyping(function () {
                datatable.userrequest.reload();
            });
        }
    };
    return {
        init: function () {
            datatable.init();
            search.init();
        }
    }
}();

$(function () {
    InitApp.init();
})