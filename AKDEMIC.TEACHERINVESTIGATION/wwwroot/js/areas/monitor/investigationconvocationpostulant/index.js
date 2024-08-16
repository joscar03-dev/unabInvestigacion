var InitApp = function () {
    var datatable = {
        postulations: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: "/monitor/postulaciones-investigacion".proto().parseURL(),
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
                        title: "Convocatoria",
                        data: "name"
                    },
                    {
                        title: "Nombre del Proyecto",
                        data: "projectTitle"
                    },
                    {
                        title: "Nombre de Investigador",
                        data: "fullName"
                    },
                    {
                        title: "Opciones",
                        data: null,
                        orderable: false,
                        render: function (data) {
                            var detailUrl = `/monitor/postulaciones-investigacion/${data.id}/detalle`;
                            var template = "";

                            //Edit                       
                            template += `<a class='btn btn-info m-btn btn-sm m-btn--icon-only' href='${detailUrl}' `;
                            template += `<span><i class="la la-edit"></i><span>Detalle</span></span></a> `;


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
            this.postulations.init();
        }
    };
    var search = {
        init: function () {
            $("#search").doneTyping(function () {
                datatable.postulations.reload();
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
