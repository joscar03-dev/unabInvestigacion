var InitApp = function () {
    var datatable = {
        reports: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: `/reporte/convocatoria-investigacion`.proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        data.handler = "Datatable";
                        data.startDate = $("#startDate").val();
                        data.endDate = $("#endDate").val();
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
                        title: "Código de Convocatoria",
                        data: "code"
                    },
                    {
                        title: "Nombre de Convocatoria",
                        data: "name"
                    },
                    {
                        title: "Total de Postulaciones",
                        data: "totalPostulations"
                    },
                    {
                        title: "Total de Proyectos Aprobados",
                        data: "totalProjectApproveds"
                    },
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

    var datePickers = {
        init: function () {
            $("#startDate").datepicker({
                clearBtn: true,
                orientation: "bottom",
                format: _app.constants.formats.datepicker
            });
            $("#endDate").datepicker({
                clearBtn: true,
                orientation: "bottom",
                format: _app.constants.formats.datepicker
            });
        }
    }

    var filters = {
        init: function () {
            $("#startDate").on("change", function () {
                datatable.reports.reload();
            });

            $("#endDate").on("change", function () {
                datatable.reports.reload();
            });
        },
    };

    return {
        init: function () {
            datatable.init();
            datePickers.init();
            filters.init();
        }
    }


}();

$(function () {
    InitApp.init();
})