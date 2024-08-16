var InitApp = function () {
    var datatable = {
        publishedBook: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: "/investigador/libros-publicados".proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        delete data.columns;
                        data.handler = "Datatable";
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
                        title: "Autor Principal",
                        data: "mainAuthor"
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
                        title: "Depósito legal",
                        data: "legalDeposit"
                    },
                    {
                        title: "Opciones",
                        data: null,
                        orderable: false,
                        render: function (data) {
                            var template = "";
                            var editUrl = `/investigador/libros-publicados/${data.id}/editar`;
                            //Edit                  
                            template += `<a class='btn btn-info m-btn btn-sm m-btn--icon-only' href='${editUrl}' `;
                            template += `<span><i class="la la-edit"></i><span>Editar</span></span></a> `;

                            //Delete
                            template += "<button type='button' ";
                            template += "class='btn btn-danger btn-delete ";
                            template += "m-btn btn-sm  m-btn--icon-only' ";
                            template += " data-id='" + data.id + "'>";
                            template += "<i class='la la-trash'></i></button> ";

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
                this.events();
            },
            events: function () {

                $("#data-table").on('click', '.btn-delete', function () {
                    let id = $(this).data("id");
                    swal({
                        title: "¿Está seguro?",
                        text: "El libro publicado sera eliminado",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonText: "Sí, eliminarlo",
                        confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                        cancelButtonText: "Cancelar",
                        showLoaderOnConfirm: true,
                        preConfirm: () => {
                            return new Promise((resolve) => {
                                $.ajax({
                                    url: ("/investigador/libros-publicados?handler=PublishedBookDelete").proto().parseURL(),
                                    type: "POST",
                                    data: {
                                        id: id
                                    },
                                    beforeSend: function (xhr) {
                                        xhr.setRequestHeader("XSRF-TOKEN",
                                            $('input:hidden[name="__RequestVerificationToken"]').val());
                                    },
                                    success: function (result) {
                                        swal({
                                            type: "success",
                                            title: "Completado",
                                            text: "El libro publicado ha sido eliminado con exito",
                                            confirmButtonText: "Excelente"
                                        }).then(datatable.publishedBook.reload());
                                    },
                                    error: function (errormessage) {
                                        swal({
                                            type: "error",
                                            title: "Error",
                                            confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                            confirmButtonText: "Entendido",
                                            text: "El libro publicado presenta información relacionada"
                                        });
                                    }
                                });
                            });
                        },
                        allowOutsideClick: () => !swal.isLoading()
                    });


                });
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