var InitApp = function () {
    var datatable = {
        conference: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: "/admin/congresos".proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        delete data.columns;
                        data.handler = "Datatable";
                        data.searchValue = $("#search").val();
                        data.type = $("#type").val();
                        data.opusTypeId = $("#opusTypeId").val();
                    }
                },
                pageLength: 50,
                orderable: [],
                columns: [
                    {
                        title: "Tipo de Obra",
                        data: "opusType"
                    },
                    {
                        title: "Tipo de Congreso",
                        data: "type"
                    },
                    {
                        title: "Título",
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
                            var detailUrl = `/admin/congresos/${data.id}/detalle`;
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
            }
        },
        init: function () {
            this.conference.init();
        }
    };
    var search = {
        init: function () {
            $("#search").doneTyping(function () {
                datatable.conference.reload();
            });
        }
    };

    var select = {
        init: function () {
            this.opusType.init();
            this.type.init();
        },
        type: {
            init: function () {
                $('#type').select2();
                this.events();
            },
            events: function () {
                $("#type").on("change", function () {
                    datatable.conference.reload();
                });
            }
        },
        opusType: {
            init: function () {
                this.load();
                this.events();
            },
            load: function () {
                $.ajax({
                    url: (`/api/tipo-de-obra/select/get`).proto().parseURL(),
                    type: "GET"
                }).done(function (result) {
                    $('#opusTypeId').select2({
                        data: result,
                    });
                });
            },
            events: function () {
                $("#opusTypeId").on("change", function () {
                    datatable.conference.reload();
                });
            }

        },
    };

    return {
        init: function () {
            datatable.init();
            search.init();
            select.init();
        }
    }
}();

$(function () {
    InitApp.init();
})