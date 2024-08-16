var InitApp = function () {
    var datatable = {
        publication: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: "/investigador/publicaciones".proto().parseURL(),
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
                        title: "Fecha de Publicación",
                        data: "publishDate"
                    },
                    {
                        title: "Opciones",
                        data: null,
                        orderable: false,
                        render: function (data) {
                            var template = "";
                            var editUrl = `/investigador/publicaciones/${data.id}/editar`;
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
                        text: "La publicación sera eliminado",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonText: "Sí, eliminarlo",
                        confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                        cancelButtonText: "Cancelar",
                        showLoaderOnConfirm: true,
                        preConfirm: () => {
                            return new Promise((resolve) => {
                                $.ajax({
                                    url: ("/investigador/publicaciones?handler=PublicationDelete").proto().parseURL(),
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
                                            text: "La publicación ha sido eliminada con exito",
                                            confirmButtonText: "Excelente"
                                        }).then(datatable.publication.reload());
                                    },
                                    error: function (errormessage) {
                                        swal({
                                            type: "error",
                                            title: "Error",
                                            confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                            confirmButtonText: "Entendido",
                                            text: "La publicación presenta información relacionada"
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