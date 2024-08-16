var InitApp = function () {
    var datatable = {
        incubatorConvocation: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: "/unidad-emprendimiento/convocatorias-emprendimiento".proto().parseURL(),
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
                            var editUrl = `/unidad-emprendimiento/convocatorias-emprendimiento/editar/${data.id}`; 
                            var fileUrl = `/unidad-emprendimiento/convocatorias-emprendimiento/${data.id}/archivos`;
                            //Archivo
                            
                            template += `<a class='btn btn-success m-btn btn-sm m-btn--icon-only' href='${fileUrl}' `;
                            template += `<span><i class="la la-edit"></i><span>Documentos</span></span></a> `;
                            //Edit                       
                            template += `<a class='btn btn-info m-btn btn-sm m-btn--icon-only' href='${editUrl}' `;
                            template += `<span><i class="la la-edit"></i><span>Editar</span></span></a> `;
                            //Delete
                            template += "<button ";
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
                    var id = $(this).data("id");
                    swal({
                        title: "¿Está seguro?",
                        text: "La convocatoria será eliminada.",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonText: "Sí, eliminarlo",
                        confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                        cancelButtonText: "Cancelar",
                        showLoaderOnConfirm: true,
                        preConfirm: () => {
                            return new Promise((resolve) => {
                                $.ajax({
                                    url: ("/unidad-emprendimiento/convocatorias-emprendimiento").proto().parseURL(),
                                    type: "GET",
                                    data: {
                                        handler: "Delete",
                                        id: id
                                    },
                                    success: function (result) {
                                        swal({
                                            type: "success",
                                            title: "Completado",
                                            text: "La convocatoria ha sido eliminada con exito",
                                            confirmButtonText: "Excelente"
                                        }).then(datatable.incubatorConvocation.reload());
                                    },
                                    error: function (errormessage) {
                                        swal({
                                            type: "error",
                                            title: "Error",
                                            confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                            confirmButtonText: "Entendido",
                                            text: "La convocatoria presenta información relacionada"
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