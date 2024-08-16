var InitApp = function () {
    var datatable = {
        incubatorPostulant: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: "/admin/postulantes-convocatorias-incubadoras".proto().parseURL(),
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
                        title: "Título del Proyecto",
                        data: "title"
                    },
                    {
                        title: "Usuario",
                        data: "userName"
                    },
                    {
                        title: "Nombre Completo",
                        data: "fullName"
                    },
                    {
                        title: "Estado",
                        data: "reviewState"
                    },
                    {
                        title: "Opciones",
                        data: null,
                        orderable: false,
                        render: function (data) {
                            var template = "";
                            var detailUrl = `/admin/postulantes-convocatorias-incubadoras/${data.id}/detalle`;
                            //Archivo     
                            template += `<a class='btn btn-success m-btn btn-sm m-btn--icon-only' href='${detailUrl}' `;
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
            },
        },
        init: function () {
            this.incubatorPostulant.init();
        }


    };
    var search = {
        init: function () {
            $("#search").doneTyping(function () {
                datatable.incubatorPostulant.reload();
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