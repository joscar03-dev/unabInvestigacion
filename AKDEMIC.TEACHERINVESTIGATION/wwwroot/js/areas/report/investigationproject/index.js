var InitApp = function () {
    var datatable = {
        reports: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: `/reporte/proyecto-investigacion`.proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        data.handler = "Datatable";
                        data.investigationProjectTypeId = $("#projectTypeId").val();
                        data.financingInvestigationId = $("#financingInvestigationId").val();
                    },
                },
                dom: 'Bfrtip',
                buttons: [
                    'excel', 'pdf'
                ],
                pageLength: 30,
                orderable: [],
                columns: [
                    {
                        title: "Investigador",
                        data: "userFullName"
                    },
                    {
                        title: "Título del Proyecto",
                        data: "projectTitle"
                    },
                    {
                        title: "Opciones",
                        data: null,
                        orderable: false,
                        render: function (data) {
                            var template = "";
                            var detailUrl = `/reporte/proyecto-investigacion/${data.id}/detalle`;
                            //Detalle                       
                            template += `<a class='btn btn-info m-btn btn-sm m-btn--icon-only' href='${detailUrl}' `;
                            template += `<span><i class="la la-edit"></i><span>Detalle</span></span></a> `;

                            return template;
                        }
                    }

                ]
            },
            init: function () {
                datatable.reports.object = $("#data-table").DataTable(datatable.reports.options);
            },
            reload: function () {
                datatable.reports.object.ajax.reload();
            }
        },
        init: function () {
            datatable.reports.init();
        }
    };

    var select = {
        init: function () {
            this.projectType.init();
            this.financingInvestigation.init();
        },
        projectType: {
            init: function () {
                this.load();
                this.events();
            },
            load: function () {
                $.ajax({
                    url: (`/api/tiposdeproyecto/select/get`).proto().parseURL(),
                    type: "GET"
                }).done(function (result) {
                    $('#projectTypeId').select2({
                        data: result,
                    });
                });
            },
            events: function () {
                $("#projectTypeId").on("change", function () {
                    datatable.reports.reload();
                });
            }

        },
        financingInvestigation: {
            init: function () {
                this.load();
                this.events();
            },
            load: function () {
                $.ajax({
                    url: (`/api/financiamiento/select/get`).proto().parseURL(),
                    type: "GET"
                }).done(function (result) {
                    $('#financingInvestigationId').select2({
                        data: result,
                    });
                });
            },
            events: function () {
                $("#financingInvestigationId").on("change", function () {
                    datatable.reports.reload();
                });
            }

        },
    };


    return {
        init: function () {
            datatable.init();
            select.init();
        }
    }


}();

$(function () {
    InitApp.init();
})