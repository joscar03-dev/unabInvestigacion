var InitApp = function () {
    var datatable = {
        investigationConvocation: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: "/coordinador-monitor/convocatoria-incubadora".proto().parseURL(),
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
                        data: "code"
                    },
                    {
                        title: "Nombre",
                        data: "name"
                    },
                    {
                        title: "Fecha de Inicio de Postulación",
                        data: "inscriptionStartDate"
                    },
                    {
                        title: "Fecha de Fin de Postulación",
                        data: "inscriptionEndDate"
                    },
                    {
                        title: "Fecha de Inicio",
                        data: "startDate"
                    },
                    {
                        title: "Fecha de Fin",
                        data: "endDate"
                    },
                    {
                        title: "Opciones",
                        data: null,
                        orderable: false,
                        render: function (data) {
                            var template = "";
                            var monitorsUrl = `/coordinador-monitor/convocatoria-incubadora/${data.id}/monitores`;
                            //Archivo

                            template += `<a class='btn btn-success m-btn btn-sm m-btn--icon-only' href='${monitorsUrl}' `;
                            template += `<span><i class="la la-users"></i><span>Monitores</span></span></a> `;

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
            this.investigationConvocation.init();
        }


    };
    var search = {
        init: function () {
            $("#search").doneTyping(function () {
                datatable.investigationConvocation.reload();
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