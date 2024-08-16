var InitApp = function () {
    var datatable = {
        incubatorPostulation: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: "/alumno/postulacion-convocatoria-emprendimiento".proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        data.handler = "Datatable";
                        data.searchValue = $("#search").val();
                    }
                },
                pageLength: 50,
                orderable: [],
                columns: [
                    {
                        title: "Código",
                        data: "incubatorConvocationCode"
                    },
                    {
                        title: "Convocatoria",
                        data: "incubatorConvocationName"
                    },
                    {
                        title: "Nombre del Proyecto",
                        data: "incubatorPostulationTitle"
                    },                   
                    {
                        title: "Estado de revisión",
                        data: "reviewStateText"
                    },
                    {
                        title: "Opciones",
                        data: null,
                        orderable: false,
                        render: function (data) {
                            var template = "";
                            var detailUrl = `/alumno/postulacion-convocatoria-emprendimiento/${data.id}/detalle`;
                            var inscriptionUrl = `/alumno/postulacion-convocatoria-emprendimiento/${data.id}/inscripcion`;
                            //Archivo     
                            template += `<a class='btn btn-success m-btn btn-sm m-btn--icon-only' href='${detailUrl}' `;
                            template += `<span><i class="la la-edit"></i><span>Detalle</span></span></a> `;
                            //Inscripción     
                            template += `<a class='btn btn-success m-btn btn-sm m-btn--icon-only' href='${inscriptionUrl}' `;
                            template += `<span><i class="la la-edit"></i><span>Formulario</span></span></a> `;
                            return template;
                        }
                    }
                ],
            },
            reload: function () {
                this.object.ajax.reload();
            },
            init: function () {
                this.object = $("#data-table").DataTable(this.options);
            }
        },
        init: function () {
            this.incubatorPostulation.init();
        }


    };
    var search = {
        init: function () {
            $("#search").doneTyping(function () {
                datatable.incubatorPostulation.reload();
            });
        }
    };
    return {
        init: function () {
            datatable.init();
            search.init();
        }
    }
}();

$(function () {
    InitApp.init();
})