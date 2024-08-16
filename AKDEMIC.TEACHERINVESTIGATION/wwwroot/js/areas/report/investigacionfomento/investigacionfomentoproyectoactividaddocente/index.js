var InitApp = function () {
    var selectconvocatoria = {
        init: function () {
            this.researcherInvestigacionfomentoConvocatoria.init();
        },
        researcherInvestigacionfomentoConvocatoria: {
            init: function () {
                this.load();
            },
            load: function () {
                $.ajax({
                    url: (`/api/investigacionfomentoconvocatoria/select/get`).proto().parseURL(),
                    type: "GET"
                }).done(function (result) {
                    $('.input_lst_convocatoria').select2({
                        data: result,
                        maximumSelectionLength: 1,

                    });
                    $(".input_lst_convocatoria").on('change', function () {
                        $("#searchconvocatoria").val($("#lst_convocatoria").val())
                        datatable.reports.reload();
                    });
                });

            }
        },
    };
    var selectdocente = {
        init: function () {
            this.researcherMaestroDocente.init();
        },
        researcherMaestroDocente: {
            init: function () {
                this.load();
            },
            load: function () {
                $.ajax({
                    url: (`/api/maestrodocente/select/get`).proto().parseURL(),
                    type: "GET"
                }).done(function (result) {
                    $('.input_lst_docente').select2({
                        data: result,
                        maximumSelectionLength: 1,
                    });
                    $(".input_lst_docente").on('change', function () {
                      
                        $("#searchdocente").val($("#lst_docente").val())

                        $(".input_lst_proyecto").empty();
                        selectproyectodocente.researcherInvetigacionfomentoProyectodocente.load($("#lst_docente").val(), $("#lst_convocatoria").val())
                        $(".input_lst_proyecto").trigger("change");
                        datatable.reports.reload();
                        
                    });
                });
            }
        },
    };
    var selectproyectodocente = {
        init: function () {
            this.researcherInvetigacionfomentoProyectodocente.init();
        },
        researcherInvetigacionfomentoProyectodocente: {
            init: function () {
                this.load();
            },
            load: function (iddocente,idconvocatoria) {
                $.ajax({
                    url: (`/api/investigacionfomentoConovocatoriaproyectodocente/select/get`).proto().parseURL(),
                    type: "GET",
                    data: "iddocente=" + (!iddocente ? '' : iddocente) + "&idconvocatoria=" + (!idconvocatoria ? '' : idconvocatoria) 
                }).done(function (result) {
                    $('.input_lst_proyecto').select2({
                        data: result,
                        maximumSelectionLength: 1,
                    });
                    $(".input_lst_proyecto").on('change', function () {
                        $("#searchproyecto").val($("#lst_proyecto").val())
                        datatable.reports.reload();
                    });
                });
            }
        },
    };
    var datatable = {
        reports: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: `/reporte/investigacionfomento/proyectoactividaddocente`.proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        data.handler = "Datatable";
                        data.searchconvocatoria = $("#searchconvocatoria").val();
                        data.searchdocente = $("#searchdocente").val();
                        data.searchproyecto = $("#searchproyecto").val();
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
                        title: "NOMBRE DE PROYECTO",
                        data: "nombre"
                    },
                    {
                        title: "ACTIVIDAD DEL PROYECTO",
                        data: "nombreactividad"
                    },
                    {
                        title: "MES 1",
                        data: "valormes1",
                    },
                    {
                        title: "MES 2",
                        data: "valormes2"
                    },
                    {
                        title: "MES 3",
                        data: "valormes1"
                    },
                    {
                        title: "MES 4",
                        data: "valormes4"
                    },
                    {
                        title: "MES 5",
                        data: "valormes5"
                    },
                    {
                        title: "MES 6",
                        data: "valormes6"
                    },
                    {
                        title: "MES 7",
                        data: "valormes7"
                    },
                    {
                        title: "MES 9",
                        data: "valormes9"
                    },
                    {
                        title: "MES 10",
                        data: "valormes10"
                    },
                    {
                        title: "MES 11",
                        data: "valormes11"
                    },
                    {
                        title: "MES 12",
                        data: "valormes12"
                    },
                    {
                        title: "MES 13",
                        data: "valormes13"
                    },
                    {
                        title: "MES 14",
                        data: "valormes14"
                    },
                    {
                        title: "MES 15",
                        data: "valormes15"
                    },
                    {
                        title: "MES 16",
                        data: "valormes16"
                    },
                    {
                        title: "MES 17",
                        data: "valormes17"
                    },
                    {
                        title: "MES 18",
                        data: "valormes18"
                    },
                    {
                        title: "MES 19",
                        data: "valormes19"
                    },
                    {
                        title: "MES 20",
                        data: "valormes20"
                    },
                    {
                        title: "Cumplio SI",
                        data: null,
                        render: function (data) {
                            template = "";
                            template += (data.cumplio=="1"?"X":"")
                            return template;
                        }
                    },
                    {
                        title: "Cumplio NO",
                        data: null,
                        render: function (data) {
                            template = "";
                            template += (data.nocumplio == "1" ? "X" : "")
                            return template;
                        }
                    },
                    {
                        title: "OBSERVACIONES",
                        data: "observaciones"
                    }
                    
                ],
                rowCallback: function (row, data) {

                    contador++;
                    if (contador == 1) {
                        var table = $('#data-table').DataTable();
                        n = row.cells.length;
                        for (var i = 0; i < n; i++) {
                            table.columns([i, i]).visible(true);
                        }
                        table.columns.adjust().draw(true);
                        for (var i = data.totalmes ; i < n - 3; i++) {
                            //table.rows[0].cells[i].style = "display:none";
                           // row.cells[i].style = "display:none";
                            //stable.rows[0].cells[i].style = "display:none";

                            table.columns([i, i]).visible(false);
                        }
                    
                        table.columns.adjust().draw(false);
                    }
                   /* table = document.getElementById("data-table");
                   
                     n = row.cells.length;
                    for (i = data.totalmes + 1; i <= n - 2; i++) {
                        table.rows[0].cells[i].style = "display:none";
                         row.cells[i].style = "display:none";
                         //stable.rows[0].cells[i].style = "display:none";
                     }*/

                  
                   
                },
            },
            
            init: function () {
                datatable.reports.object = $("#data-table").DataTable(datatable.reports.options);
                
            },
            reload: function () {
                contador = 0;
                $("#data-table").dataTable().fnDestroy();
                datatable.reports.object = $("#data-table").DataTable(datatable.reports.options);
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
            selectconvocatoria.init();
            selectdocente.init();
           selectproyectodocente.init();
        }
    }


}();

$(function () {
    InitApp.init();
})