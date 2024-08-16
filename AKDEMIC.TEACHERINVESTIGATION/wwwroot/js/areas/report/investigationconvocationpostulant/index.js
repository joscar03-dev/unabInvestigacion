var InitApp = function () {
    var datatable = {
        reports: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: `/reporte/postulaciones`.proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        data.handler = "Datatable";
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
                        title: "Título del Proyecto",
                        data: "projectTitle"
                    },
                    {
                        title: "Nombre del Investigador",
                        data: "fullName"
                    },
                    {
                        title: "Estado de Revisión",
                        data: "reviewStateText"
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
    var highchart = {
        lang: function () {
            Highcharts.setOptions({
                lang: {
                    contextButtonTitle: "Opciones",
                    viewFullscreen: "Ver en pantalla completa",
                    printChart: "Imprimir",
                    downloadPNG: "Descargar PNG",
                    downloadJPEG: "Descargar JPEG",
                    downloadPDF: "Descargar PDF",
                    downloadSVG: "Descargar SVG",
                    downloadCSV: "Descargar CSV",
                    downloadXLS: "Descargar XLS",
                    openInCloud: "Abrir editor online"
                }
            })
        },
        credits: {
            text: 'Fuente: AKDEMIC',
            href: ''
        },
        buttons: {
            contextButton: {
                menuItems: ["viewFullscreen", "printChart", "separator", "downloadPNG", "downloadJPEG", "downloadPDF", "downloadSVG", "separator", "downloadCSV", "downloadXLS"]
            }
        },
        loadReport: function () {
            $("#chart-report-container").html("");
            $("#chart-report-container").append(`<div id="chart-report" class="chart"></div>`);
            $.ajax({
                url: `/reporte/postulaciones?handler=Chart`,
                type: "GET",
            }).done(function (result) {
                var categoriesCount = result.categories.length;
                var heightChart = 350;
                if (categoriesCount > 15) {
                    heightChart = categoriesCount * 20;
                }
                Highcharts.chart('chart-report', {
                    chart: {
                        height: heightChart,
                        events: {
                            exportData: function () {
                                var total = $("body").find(".highcharts-data-table");
                                var current = $(".m-portlet").find(".highcharts-data-table");
                                var currentLength = current.length;
                                for (currentLength; currentLength < total.length; currentLength++) {
                                    $(total[currentLength]).remove();
                                }
                            }
                        }
                    },
                    title: {
                        text: 'Cantidad de postulaciones por estado de revisión'
                    },
                    subtitle: {
                        text: ""
                    },
                    credits: highchart.credits,
                    xAxis: {
                        categories: result.categories,
                        title: {
                            text: 'Estado de Revisión'
                        }
                    },
                    yAxis: {
                        min: 0,
                        title: {
                            text: 'Cantidad de Postulaciones'
                        }
                    },
                    series: [{
                        type: 'column',
                        name: 'Cantidad',
                        data: result.data,
                        colorByPoint: true,
                        showInLegend: false
                    }],
                    exporting: {
                        showTable: true,
                        buttons: highchart.buttons
                    }
                })
            }).fail(function (error) {
                toastr.error(error.responseText, _app.constants.toastr.title.error);
            })
        },
        init: function () {
            highchart.lang();
            highchart.loadReport();
        }
    };

    return {
        init: function () {
            highchart.init();
            datatable.init();
        }
    }
}();

$(function () {
    InitApp.init();
})