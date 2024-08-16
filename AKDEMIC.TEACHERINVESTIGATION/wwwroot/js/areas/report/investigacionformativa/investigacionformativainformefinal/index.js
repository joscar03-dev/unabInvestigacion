var InitApp = function () {
    var datatable = {
        reports: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: `/reporte/investigacionformativa/informefinal`.proto().parseURL(),
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
                        title: "Año",
                        data: "anio"
                    },
                    {
                        title: "Carrera",
                        data: "nombrecarrera"
                    },
                    
                    {
                        title: "Título",
                        data: "titulo"
                    },
                    {
                        title: "Docente",
                        data: "fullname"
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
                        title: "Informe",
                        data: null,
                        width: 50,
                        render: function (data) {
                            var template = "";
                            var fileUrl = `/documentos/${data.archivourl}`.proto().parseURL();
                            //FileURL
                            if (data.archivourlcarta != '') {
                                template += `<a href="${fileUrl}"  `;
                                template += "class='btn btn-success ";
                                template += "m-btn btn-sm m-btn--icon' download>";
                                template += "<span><i class='flaticon-file'></i></a> ";
                            }
                            return template;
                        }
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