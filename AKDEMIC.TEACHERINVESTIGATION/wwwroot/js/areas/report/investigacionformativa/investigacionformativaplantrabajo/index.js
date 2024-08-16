var InitApp = function () {
    var datatable = {
        reports: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: `/reporte/investigacionformativa/plantrabajo`.proto().parseURL(),
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
                        title: "Código",
                        data: "codigoplantrabajo"
                    },
                    {
                        title: "Area",
                        data: "nombreareaacademica"
                    },
                    {
                        title: "Linea de Investigación",
                        data: "titulolinea"
                    },
                    {
                        title: "Título",
                        data: "titulo"
                    },
                    {
                        title: "Docente",
                        data: "nombredocente"
                    },
                    {
                        title: "Alumnos",
                        data: null,
                        orderable: false,
                        render: function (data) {
                            var template =  (data.coautor1 == null ? '' : data.coautor1) ;
                            if (data.coautor2 != null) {
                                template += ' / ' + (data.coautor2 == null ? '' : data.coautor2) ;
                            }
                            return template;
                        }
                    },
                    {
                        title: "Evento UNAB",
                        data: "nombretipoevento",
                        
                    },
                    {
                        title: "Resultado",
                        data: "nombretiporesultado"
                    },
                    {
                        title: "Estado",
                        data: "nombreestado"
                    },
                    {
                        title: "Actividad 1",
                        data: "act1"
                    },
                    {
                        title: "Actividad 2",
                        data: "act2"
                    },
                    {
                        title: "Actividad 3",
                        data: "act3"
                    },
                    {
                        title: "Actividad 4",
                        data: "act4"
                    },
                    {
                        title: "Actividad 5",
                        data: "act5"
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