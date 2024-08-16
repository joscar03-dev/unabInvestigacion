var InitApp = function () {
    var investigationConvocationPostulantId = $('#Input_InvestigationConvocationPostulantId').val();
    var events = {
        init: function () {
            $("#Accept").on('click', function () {
                swal({
                    title: "¿Está seguro?",
                    text: "El estado de la postulación cambiará a Aceptado.",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonText: "Sí, cambiarlo",
                    confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                    cancelButtonText: "Cancelar",
                    showLoaderOnConfirm: true,
                    preConfirm: () => {
                        return new Promise((resolve) => {
                            $.ajax({
                                url: `/admin/postulaciones/detalle/${investigationConvocationPostulantId}?handler=Accept`.proto().parseURL(),
                                type: "POST",
                                beforeSend: function (xhr) {
                                    xhr.setRequestHeader("XSRF-TOKEN",
                                        $('#observationModal input:hidden[name="__RequestVerificationToken"]').val());
                                },
                                success: function (result) {
                                    swal({
                                        type: "success",
                                        title: "Completado",
                                        text: "El estado de la postulación ha sido cambiado con exito",
                                        confirmButtonText: "Excelente"
                                    });
                                },
                                error: function (errormessage) {
                                    swal({
                                        type: "error",
                                        title: "Error",
                                        confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                        confirmButtonText: "Entendido",
                                        text: "No se ha podido cambiar el estado de la postulación"
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
                    text: "El estado de la postulación cambiará a Rechazado.",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonText: "Sí, cambiarlo",
                    confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                    cancelButtonText: "Cancelar",
                    showLoaderOnConfirm: true,
                    preConfirm: () => {
                        return new Promise((resolve) => {
                            $.ajax({
                                url: `/admin/postulaciones/detalle/${investigationConvocationPostulantId}?handler=Reject`.proto().parseURL(),
                                type: "POST",
                                beforeSend: function (xhr) {
                                    xhr.setRequestHeader("XSRF-TOKEN",
                                        $('#observationModal input:hidden[name="__RequestVerificationToken"]').val());
                                },
                                success: function (result) {
                                    swal({
                                        type: "success",
                                        title: "Completado",
                                        text: "El estado de la postulación ha sido cambiado con exito",
                                        confirmButtonText: "Excelente"
                                    });
                                },
                                error: function (errormessage) {
                                    swal({
                                        type: "error",
                                        title: "Error",
                                        confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                        confirmButtonText: "Entendido",
                                        text: "No se ha podido cambiar el estado de la postulación"
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
                    url: `/admin/postulaciones/detalle/${investigationConvocationPostulantId}`.proto().parseURL(),
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
                                    url: `/admin/postulaciones/detalle/${investigationConvocationPostulantId}?handler=RejectObservation`.proto().parseURL(),
                                    type: "POST",
                                    data: {
                                        postulantObservationId: id
                                    },
                                    beforeSend: function (xhr) {
                                        xhr.setRequestHeader("XSRF-TOKEN",
                                            $('#observationModal input:hidden[name="__RequestVerificationToken"]').val());
                                    },
                                    success: function (result) {
                                        swal({
                                            type: "success",
                                            title: "Completado",
                                            text: "El estado de la observación ha sido cambiada con exito",
                                            confirmButtonText: "Excelente"
                                        }).then(datatable.observation.reload());
                                    },
                                    error: function (errormessage) {
                                        swal({
                                            type: "error",
                                            title: "Error",
                                            confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                            confirmButtonText: "Entendido",
                                            text: "No se ha podido cambiar el estado de la observación"
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
                        text: "El estado de cambiará a Subsanado.",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonText: "Sí, cambiarlo",
                        confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                        cancelButtonText: "Cancelar",
                        showLoaderOnConfirm: true,
                        preConfirm: () => {
                            return new Promise((resolve) => {
                                $.ajax({
                                    url: `/admin/postulaciones/detalle/${investigationConvocationPostulantId}?handler=AbsolvedObservation`.proto().parseURL(),
                                    type: "POST",
                                    data: {
                                        postulantObservationId: id
                                    },
                                    beforeSend: function (xhr) {
                                        xhr.setRequestHeader("XSRF-TOKEN",
                                            $('#observationModal input:hidden[name="__RequestVerificationToken"]').val());
                                    },
                                    success: function (result) {
                                        swal({
                                            type: "success",
                                            title: "Completado",
                                            text: "El estado de la observación ha sido cambiada con exito",
                                            confirmButtonText: "Excelente"
                                        }).then(datatable.observation.reload());
                                    },
                                    error: function (errormessage) {
                                        swal({
                                            type: "error",
                                            title: "Error",
                                            confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                            confirmButtonText: "Entendido",
                                            text: "No se ha podido cambiar el estado de la observación"
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
                    url: `/admin/postulaciones/detalle/${investigationConvocationPostulantId}`.proto().parseURL(),
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
    var progress = {
        init: function () {
            this.load();
        },
        load: function () {
            $.ajax({
                url: `/investigador/convocatoria/${investigationConvocationPostulantId}/inscripcion`.proto().parseURL(),
                type: "GET",
                data: {
                    handler: "ProgressPercentage",
                    InvestigationConvocationPostulantId: investigationConvocationPostulantId
                },
            }).done(function (result) {
                $('#progressBarComplete').text(`${result.progressBarPercentage} %`);
                $('#progressBarComplete').css("width", `${result.progressBarPercentage}%`);
                $("#generalInformation-percentage").text(`${result.generalInformationPercentage} %`);
                $('#problemDescription-percentage').text(`${result.problemDescriptionPercentage} %`);
                $('#markReferences-percentage').text(`${result.markReferencePercentage} %`);
                $('#methodology-percentage').text(`${result.methodologyPercentage} %`);
                $('#expectedResult-percentage').text(`${result.expectedResultPercentage} %`);
                $("#team-member-percentage").text(`${result.teamMemberPercentage} %`);
                $("#annexFiles-percentage").text(`${result.annexFilesPercentage} %`);

            });
        }
    }
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