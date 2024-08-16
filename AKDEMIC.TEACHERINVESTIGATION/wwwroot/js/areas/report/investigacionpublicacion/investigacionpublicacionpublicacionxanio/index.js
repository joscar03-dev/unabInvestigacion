var InitApp = function () {
    var datatable = {
        reports: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: `/reporte/investigacionpublicacion/publicacionxanio`.proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        data.handler = "Datatable";
                        data.anio = $("#lst_anio").val();
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
                        title: "FECHA DE PUBLICACIÓN",
                        data: "publishDate"
                    },
                    {
                        title: "TIPO DE OBRA",
                        data: "opusTypeName"
                    },
                    
                    {
                        title: "TÍTULO",
                        data: "title"
                    },
                    {
                        title: "DESCRIPCIÓN",
                        data: "description"
                    },
                    {
                        title: "VOLUMEN",
                        data: "volume"
                    }  ,
                    {
                        title: "AUTOR PRINCIPAL",
                        data: "mainAuthor"
                    },
                    {
                        title: "DOI",
                        data: "doi",
                        className: "text-right",

                        
                    },
                    {
                        title: "QUARTIL",
                        data: "quartil",
                        className: "text-right",

                    },
                    {
                        title: "AUTORES",
                        data: "autores",
                        className: "text-right",

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
            $("#lst_anio").on("change", function () {
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