var InitApp = function () {
    var datatable = {
        incubatorConvocation: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: "/admin/convocatorias-incubadoras".proto().parseURL(),
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
                    }
                ],
            },
            reload: function () {
                this.object.ajax.reload();
            },
            init: function () {
                this.object = $("#data-table").DataTable(this.options);
            },
        },
        init: function () {
            this.incubatorConvocation.init();
        }


    };
    var search = {
        init: function () {
            $("#search").doneTyping(function () {
                datatable.incubatorConvocation.reload();
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