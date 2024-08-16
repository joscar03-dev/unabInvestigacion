var InitApp = function () {
    var datatable = {
        publication: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: "/admin/publicaciones".proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        data.handler = "Datatable";
                        data.searchValue = $("#search").val();
                        data.workCategory = $("#workCategory").val();
                        data.opusTypeId = $("#opusTypeId").val();
                    }
                },
                pageLength: 50,
                orderable: [],
                columns: [
                    {
                        title: "Categoría",
                        data: "workCategory"
                    },
                    {
                        title: "Obra",
                        data: "opusType"
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
                        title: "Fecha de Publicación",
                        data: "publishDate"
                    },
                                        {
                        title: "Opciones",
                        data: null,
                        orderable: false,
                        render: function (data) {
                            var template = "";
                            var detailUrl = `/admin/publicaciones/${data.id}/detalle`;
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
            this.publication.init();
        }
    };
    var search = {
        init: function () {
            $("#search").doneTyping(function () {
                datatable.publication.reload();
            });
        }
    };

    var select = {
        init: function () {
            this.opusType.init();
            this.workerCategory.init();
        },
        workerCategory: {
            init: function () {
                $('#workCategory').select2();
                this.events();
            },
            events: function () {
                $("#workCategory").on("change", function () {
                    datatable.publication.reload();
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
                    datatable.publication.reload();
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