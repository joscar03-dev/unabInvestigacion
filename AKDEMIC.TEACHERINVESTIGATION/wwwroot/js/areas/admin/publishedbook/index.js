var InitApp = function () {
    var datatable = {
        publishedBook: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: "/admin/libros-publicados".proto().parseURL(),
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
                        title: "Título",
                        data: "title"
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
                        title: "Deposito legal",
                        data: "legalDeposit"
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
                            var detailUrl = `/admin/libros-publicados/${data.id}/detalle`;
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
            this.publishedBook.init();
        }
    };
    var search = {
        init: function () {
            $("#search").doneTyping(function () {
                datatable.publishedBook.reload();
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