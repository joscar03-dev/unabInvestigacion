var InitApp = function () {
    var investigationConvocationPostulantId = $('#Input_InvestigationConvocationPostulantId').val();
    var requestDocument = {
        init: function () {
            $("#requestDocument").on("click", function () {
                $("#requestDocument").addLoader();
                $.ajax({
                    url: `/comite-evaluador/postulaciones/${investigationConvocationPostulantId}/detalle?handler=RequestDocument`.proto().parseURL(),
                    type: "GET",                    
                }).done(function () {
                    toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                    window.location.href = window.location.href
                }).fail(function (error) {
                    toastr.error(error.responseText, _app.constants.toastr.title.error);
                    $("#requestDocument").removeLoader();
                }).always(function () {

                });
            });
        }
    };
    var formResolution = {
        init: function () {
            $("#resolution-form").validate({
                submitHandler: function (form, e) {
                    let submitedButton = e.originalEvent.submitter;
                    e.preventDefault();
                    let formData = new FormData(form);
                    $(submitedButton).addLoader();
                    if ($(submitedButton).data("id") === "accept") {
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
                                        url: $(submitedButton).attr("formaction"),
                                        type: "POST",
                                        data: formData,
                                        contentType: false,
                                        processData: false,
                                        success: function (result) {
                                            swal({
                                                type: "success",
                                                title: "Completado",
                                                text: "El estado de la postulación ha sido cambiado con exito",
                                                confirmButtonText: "Excelente"
                                            });
                                            window.location.href = window.location.href;
                                        },
                                        error: function (errormessage) {
                                            swal({
                                                type: "error",
                                                title: "Error",
                                                confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                                confirmButtonText: "Entendido",
                                                text: errormessage.responseText
                                            });
                                        }
                                    });
                                });
                            },
                            allowOutsideClick: () => !swal.isLoading()
                        });
                    }
                    else if ($(submitedButton).data("id") === "reject") {
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
                                        url: $(submitedButton).attr("formaction"),
                                        type: "POST",
                                        data: formData,
                                        contentType: false,
                                        processData: false,
                                        success: function (result) {
                                            swal({
                                                type: "success",
                                                title: "Completado",
                                                text: "El estado de la postulación ha sido cambiado con exito",
                                                confirmButtonText: "Excelente"
                                            });
                                            window.location.href = window.location.href;
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
                    }
                    $(submitedButton).removeLoader();
                }
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
                    url: `/comite-evaluador/postulaciones/${investigationConvocationPostulantId}/detalle`.proto().parseURL(),
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
                ],
            },
            reload: function () {
                this.object.ajax.reload();
            },
            init: function () {
                this.object = $("#observation-table").DataTable(this.options);
            }
        },
        annexedFiles: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: `/comite-evaluador/postulaciones/${investigationConvocationPostulantId}/detalle`.proto().parseURL(),
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
        techFiles: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: `/comite-evaluador/postulaciones/${investigationConvocationPostulantId}/detalle`.proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        data.handler = "TechnicalFileDatatable";
                    }
                },
                pageLength: 50,
                orderable: [],
                columns: [
                    {
                        title: "Nombre de Documento Tecnico",
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
                            var fileUrl = `/documentos/${data.filePath}`.proto().parseURL();
                            //FileURL
                            template += `<a href="${fileUrl}"  `;
                            template += "class='btn btn-success ";
                            template += "m-btn btn-sm m-btn--icon' download>";
                            template += "<span><i class='flaticon-file'></i></a> ";

                            return template;
                        }
                    },
                ],
            },
            reload: function () {
                this.object.ajax.reload();
            },
            init: function () {
                this.object = $("#postulanttechnicalfile-datatable").DataTable(this.options);
            }
        },
        financialFiles: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: `/comite-evaluador/postulaciones/${investigationConvocationPostulantId}/detalle`.proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        data.handler = "FinancialFileDatatable";
                    }
                },
                pageLength: 50,
                orderable: [],
                columns: [
                    {
                        title: "Nombre de Documento Financiero",
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
                            var fileUrl = `/documentos/${data.filePath}`.proto().parseURL();
                            //FileURL
                            template += `<a href="${fileUrl}"  `;
                            template += "class='btn btn-success ";
                            template += "m-btn btn-sm m-btn--icon' download>";
                            template += "<span><i class='flaticon-file'></i></a> ";

                            return template;
                        }
                    }
                ],
            },
            reload: function () {
                this.object.ajax.reload();
            },
            init: function () {
                this.object = $("#postulantfinancialfile-datatable").DataTable(this.options);
            },
        },
        init: function () {
            this.observation.init();
            this.annexedFiles.init();
            this.techFiles.init();
            this.financialFiles.init();
        }
    };
    return {
        init: function () {
            requestDocument.init();
            formResolution.init();
            datatable.init();
        }
    }
}();

$(function () {
    InitApp.init();
})