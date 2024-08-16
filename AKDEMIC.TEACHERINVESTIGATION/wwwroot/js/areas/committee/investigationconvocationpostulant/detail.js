var InitApp = function () {
    var investigationConvocationPostulantId = $('#Input_InvestigationConvocationPostulantId').val();
    var currentReviewState = $("#ReviewState").data("reviewstate");

    var postulantReviewState = {
        update: function () {
            $.ajax({
                url: `/comite/postulaciones/detalle/${investigationConvocationPostulantId}?handler=ReviewState`,
                type: "GET",
            }).done(function (result) {
                currentReviewState = result.reviewState;
                $("#ReviewState").text(result.reviewStateText);
                if (currentReviewState == 0) {
                    $("#form-footer").css("display" , "block");
                } else {
                    $("#form-footer").css("display", "none");
                }
            }).fail(function (error) {

            }).always(function () {
            
            });
        }
    };


    var events = {
        init: function () {
            $("#Accept").on('click', function () {
                swal({
                    title: "¿Está seguro?",
                    text: "El estado de revisión cambiará a Admitido.",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonText: "Sí, cambiarlo",
                    confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                    cancelButtonText: "Cancelar",
                    showLoaderOnConfirm: true,
                    preConfirm: () => {
                        return new Promise((resolve) => {
                            $.ajax({
                                url: `/comite/postulaciones/detalle/${investigationConvocationPostulantId}?handler=Accept`.proto().parseURL(),
                                type: "POST",
                                beforeSend: function (xhr) {
                                    xhr.setRequestHeader("XSRF-TOKEN",
                                        $('#observationModal input:hidden[name="__RequestVerificationToken"]').val());
                                },
                                success: function (result) {
                                    swal({
                                        type: "success",
                                        title: "Completado",
                                        text: "El estado de revisión ha sido cambiado con exito",
                                        confirmButtonText: "Excelente"
                                    }).then(window.location.href = window.location.href);
                                },
                                error: function (errormessage) {
                                    let defaulterror = "No se ha podido cambiar el estado de revisión";
                                    if (errormessage.responseText != "") {
                                        defaulterror = errormessage.responseText;
                                    }
                                    swal({
                                        type: "error",
                                        title: "Error",
                                        confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                        confirmButtonText: "Entendido",
                                        text: defaulterror
                                    });
                                }
                            });
                        });
                    },
                    allowOutsideClick: () => !swal.isLoading()
                });
            });
            $("#Reject").on('click', function () {
                swal({
                    title: "¿Está seguro?",
                    text: "El estado de revisión cambiará a Rechazado.",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonText: "Sí, cambiarlo",
                    confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                    cancelButtonText: "Cancelar",
                    showLoaderOnConfirm: true,
                    preConfirm: () => {
                        return new Promise((resolve) => {
                            $.ajax({
                                url: `/comite/postulaciones/detalle/${investigationConvocationPostulantId}?handler=Reject`.proto().parseURL(),
                                type: "POST",
                                beforeSend: function (xhr) {
                                    xhr.setRequestHeader("XSRF-TOKEN",
                                        $('#observationModal input:hidden[name="__RequestVerificationToken"]').val());
                                },
                                success: function (result) {
                                    swal({
                                        type: "success",
                                        title: "Completado",
                                        text: "El estado de revisión ha sido cambiado con exito",
                                        confirmButtonText: "Excelente"
                                    }).then(window.location.href = window.location.href);

                                },
                                error: function (errormessage) {
                                    let defaulterror = "No se ha podido cambiar el estado de revisión";
                                    if (errormessage.responseText != "") {
                                        defaulterror = errormessage.responseText;
                                    }
                                    swal({
                                        type: "error",
                                        title: "Error",
                                        confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                        confirmButtonText: "Entendido",
                                        text: defaulterror
                                    });
                                }
                            });
                        });
                    },
                    allowOutsideClick: () => !swal.isLoading()
                });
            });
        }
    };
    var datatable = {
        observation: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: `/comite/postulaciones/detalle/${investigationConvocationPostulantId}`.proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        data.handler = "ObservationsDatatable";
                    }
                },
                pageLength: 50,
                orderable: [],
                columns: [
                    {
                        title: "Fecha de Observación",
                        data: "createdAt"
                    },
                    {
                        title: "Descripción",
                        data: "description"
                    },
                    {
                        title: "Estado",
                        data: null,
                        render: function (data) {
                            switch (data.state) {
                                case 0:
                                    return `<span class="m--font-warning">${data.stateText}</span>`;
                                case 1:
                                    return `<span class="m--font-info">${data.stateText}</span>`;
                                case 2:
                                    return `<span class="m--font-danger">${data.stateText}</span>`;
                                case 3:
                                    return `<span class="m--font-success">${data.stateText}</span>`;
                                default:
                                    return `<span class="m--font-primary">${data.stateText}</span>`;
                            }
                        }
                    },
                    {
                        title: "Opciones",
                        data: null,
                        orderable: false,
                        render: function (data) {
                            var template = "";
                            if (data.state == 1) {
                                template += "<button ";
                                template += "class='btn btn-danger btn-rejected ";
                                template += "m-btn btn-sm  m-btn--icon-only' ";
                                template += " data-id='" + data.id + "'>";
                                template += "<i class='la la-close'></i></button> ";

                                template += "<button ";
                                template += "class='btn btn-success btn-solved ";
                                template += "m-btn btn-sm  m-btn--icon-only' ";
                                template += " data-id='" + data.id + "'>";
                                template += "<i class='la la-check'></i></button> ";
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
                this.object = $("#observation-table").DataTable(this.options);
                this.events();
            },
            events: function () {
                $("#observation-table").on('click', '.btn-rejected', function () {
                    var id = $(this).data("id");
                    swal({
                        title: "¿Está seguro?",
                        text: "El estado de la observación cambiará a pendiente de correción.",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonText: "Sí, cambiarlo",
                        confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                        cancelButtonText: "Cancelar",
                        showLoaderOnConfirm: true,
                        preConfirm: () => {
                            return new Promise((resolve) => {
                                $.ajax({
                                    url: `/comite/postulaciones/detalle/${investigationConvocationPostulantId}?handler=RejectObservation`.proto().parseURL(),
                                    type: "POST",
                                    data: {
                                        postulantObservationId: id
                                    },
                                    beforeSend: function (xhr) {
                                        xhr.setRequestHeader("XSRF-TOKEN",
                                            $('#observationModal input:hidden[name="__RequestVerificationToken"]').val());
                                    },
                                    success: function (result) {
                                        postulantReviewState.update();
                                        swal({
                                            type: "success",
                                            title: "Completado",
                                            text: "El estado de la observación ha sido cambiada con exito",
                                            confirmButtonText: "Excelente"
                                        }).then(datatable.observation.reload());
                                    },
                                    error: function (errormessage) {
                                        let defaulterror = "No se ha podido cambiar el estado de la observación";
                                        if (errormessage.responseText != "") {
                                            defaulterror = errormessage.responseText;
                                        }
                                        swal({
                                            type: "error",
                                            title: "Error",
                                            confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                            confirmButtonText: "Entendido",
                                            text: defaulterror
                                        });
                                    }
                                });
                            });
                        },
                        allowOutsideClick: () => !swal.isLoading()
                    });
                });
                $("#observation-table").on('click', '.btn-solved', function () {
                    var id = $(this).data("id");
                    swal({
                        title: "¿Está seguro?",
                        text: "El estado de la observación cambiará a Subsanado.",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonText: "Sí, cambiarlo",
                        confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                        cancelButtonText: "Cancelar",
                        showLoaderOnConfirm: true,
                        preConfirm: () => {
                            return new Promise((resolve) => {
                                $.ajax({
                                    url: `/comite/postulaciones/detalle/${investigationConvocationPostulantId}?handler=AbsolvedObservation`.proto().parseURL(),
                                    type: "POST",
                                    data: {
                                        postulantObservationId: id
                                    },
                                    beforeSend: function (xhr) {
                                        xhr.setRequestHeader("XSRF-TOKEN",
                                            $('#observationModal input:hidden[name="__RequestVerificationToken"]').val());
                                    },
                                    success: function (result) {
                                        postulantReviewState.update();
                                        swal({
                                            type: "success",
                                            title: "Completado",
                                            text: "El estado de la observación ha sido cambiada con exito",
                                            confirmButtonText: "Excelente"
                                        }).then(datatable.observation.reload());
                                    },
                                    error: function (errormessage) {
                                        let defaulterror = "No se ha podido cambiar el estado de la observación";
                                        if (errormessage.responseText != "") {
                                            defaulterror = errormessage.responseText;
                                        }
                                        swal({
                                            type: "error",
                                            title: "Error",
                                            confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                            confirmButtonText: "Entendido",
                                            text: defaulterror
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
        annexedFiles: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: `/comite/postulaciones/detalle/${investigationConvocationPostulantId}`.proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        data.handler = "AnnexedFileDatatable";
                        data.searchValue = $("#search").val();
                    }
                },
                pageLength: 50,
                orderable: [],
                columns: [
                    {
                        title: "Nombre de Anexo",
                        data: "name"
                    },
                    {
                        title: "Fecha de Subida",
                        data: "createdAt"
                    },
                    {
                        title: "Archivo",
                        data: null,
                        orderable: false,
                        render: function (data) {
                            var template = "";
                            var fileUrl = `/documentos/${data.documentPath}`.proto().parseURL();
                            //FileURL
                            template += `<a href="${fileUrl}"  `;
                            template += "class='btn btn-success ";
                            template += "m-btn btn-sm m-btn--icon' download>";
                            template += "<span><i class='flaticon-file'></i></span></a> ";

                            return template;
                        }
                    }
                ],
            },
            reload: function () {
                this.object.ajax.reload();
            },
            init: function () {
                this.object = $("#annexedFile-datatable").DataTable(this.options);
                this.events();
            },
            events: function () {
                $("#annexedFile-datatable").on('click', '.btn-delete', function () {
                    var id = $(this).data("id");
                    modal.annexedFiles.delete(id);
                });
            }
        },
        init: function () {
            this.observation.init();
            this.annexedFiles.init();
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
                        //$(".modal").modal("hide");
                        $(".m-alert").addClass("m--hide");
                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);

                        datatable.observation.reload();
                        postulantReviewState.update();
                        $("#btnSave").removeLoader();
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
            },
            clear: function () {
                modal.create.object.resetForm();
            },
            events: function () {
                $("#observationModal").on("hidden.bs.modal", function () {
                    modal.create.clear();
                });
            }
        },
        init: function () {
            this.create.events();
        }

    };
    return {
        init: function () {
            datatable.init();
            modal.init();
            events.init();
        }
    }
}();

$(function () {
    InitApp.init();
})