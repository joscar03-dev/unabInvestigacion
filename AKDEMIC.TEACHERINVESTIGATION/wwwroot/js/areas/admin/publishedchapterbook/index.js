var InitApp = function () {
    var datatable = {
        publishedChapterBook: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: "/admin/capitulos-libros-publicados".proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        delete data.columns;
                        data.handler = "Datatable";
                        data.searchValue = $("#search").val();
                    }
                },
                pageLength: 50,
                orderable: [],
                columns: [
                    {
                        title: "Título del libro",
                        data: "bookTitle"
                    },
                    {
                        title: "Título del capítulo",
                        data: "chapterTitle"
                    },
                    {
                        title: "Editorial",
                        data: "publishingHouse"
                    },
                    {
                        title: "Ciudad de Edición",
                        data: "publishingCity"
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
                        title: "Opciones",
                        data: null,
                        orderable: false,
                        render: function (data) {
                            var template = "";
                            var detailUrl = `/admin/capitulos-libros-publicados/${data.id}/detalle`;
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
            this.publishedChapterBook.init();
        }
    };
    var search = {
        init: function () {
            $("#search").doneTyping(function () {
                datatable.publishedChapterBook.reload();
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