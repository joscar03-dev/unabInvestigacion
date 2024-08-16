var InitApp = function () {
    var investigationConvocationPostulantId = $("#Input_InvestigationConvocationPostulantId").val();
    var pageForms = {
        init: function () {
            $("#monitorDocument-form").validate({
                submitHandler: function (form, e) {
                    $("#btnSaveMonitorDocument").addLoader();
                    e.preventDefault();
                    let formData = new FormData(form);
                    $.ajax({
                        url: $(form).attr("action"),
                        type: "POST",
                        data: formData,
                        contentType: false,
                        processData: false
                    }).done(function () {
                        $(".modal").modal("hide");
                        $(".m-alert").addClass("m--hide");
                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                        $("#btnSaveMonitorDocument").removeLoader();
                        window.location.href = window.location.href;
                    }).fail(function (error) {
                        toastr.error(error.responseText, _app.constants.toastr.title.error);
                        $("#btnSaveMonitorDocument").removeLoader();
                    }).always(function () {

                    });
                }
            });
        }
    };
    var datatable = {
        advance: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: `/monitor/postulaciones-investigacion/${investigationConvocationPostulantId}/detalle`.proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        data.handler = "AdvanceDatatable";
                        data.searchValue = $("#search").val();
                    }
                },
                pageLength: 50,
                orderable: [],
                columns: [
                    {
                        title: "Nombre de Avance",
                        data: "name"
                    },
                    {
                        title: "Fecha de Avance",
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
                this.object = $("#progressfileconvocationpostulant-datatable").DataTable(this.options);
            },
        },
        annexedFiles: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: `/monitor/postulaciones-investigacion/${investigationConvocationPostulantId}/detalle`.proto().parseURL(),
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
                    url: `/monitor/postulaciones-investigacion/${investigationConvocationPostulantId}/detalle`.proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        data.handler = "TechnicalFileDatatable";
                        data.searchValue = $("#search").val();
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
                    }
                ],
            },
            reload: function () {
                this.object.ajax.reload();
            },
            init: function () {
                this.object = $("#postulanttechnicalfile-datatable").DataTable(this.options);
            },
        },
        financialFiles: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: `/monitor/postulaciones-investigacion/${investigationConvocationPostulantId}/detalle`.proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        data.handler = "FinancialFileDatatable";
                        data.searchValue = $("#search").val();
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
                    },
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
            this.advance.init();
            this.annexedFiles.init();
            this.techFiles.init();
            this.financialFiles.init();
        }
    };
    
    return {
        init: function () {
            datatable.init();
            pageForms.init();
        }
    };
}();

$(function () {
    InitApp.init();
})