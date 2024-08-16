var InitApp = function () {
    var datatable = {
        investigationConvocation: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: "/investigador/convocatoria".proto().parseURL(),
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
                        title: "Código",
                        data: "code"
                    },
                    {
                        title: "Convocatoria",
                        data: "name"
                    },
                    {
                        title: "Título del Proyecto",
                        data: "projectTitle"
                    },
                    {
                        title: "Facultad",
                        data: "facultyText"
                    },
                    {
                        title: "Fecha de Inscripción",
                        data: "createdAt"
                    },
                    {
                        title: "Porcentaje de Avance",
                        data: "progressPercentage"
                    },
                    {
                        title: "Estado de revisión",
                        data: "reviewState"
                    },
                    {
                        title: "Estado de avance",
                        data: "progressState"
                    },
                    {
                        title: "Opciones",
                        data: null,
                        orderable: false,
                        render: function (data) {
                            var template = "";
                            var detailUrl = `/investigador/convocatoria/${data.id}/detalle`;
                            var pdfUrl = `/investigador/convocatoria?handler=Pdf&id=${data.id}`;
                            //Archivo     
                            template += `<a class='btn btn-success m-btn btn-sm m-btn--icon-only' href='${detailUrl}' `;
                            template += `<span><i class="la la-edit"></i><span>Detalle</span></span></a> `;

                            template += `<a class='btn btn-success m-btn btn-sm m-btn--icon-only btn-pdf' href='${pdfUrl}'`;
                            template += `<span><i class="la la-edit"></i><span>PDF</span></span></a> `;
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
/*                this.events();*/
            },
            //events: function () {
            //    $("#data-table").on('click', '.btn-pdf', function () {
            //        var id = $(this).data('id');

            //        $.ajax({
            //            url: "/investigador/convocatoria".proto().parseURL(),
            //            type: "GET",
            //            data: {
            //                handler: "Pdf",
            //                id: id
            //            },
            //        }).done(function (result) {

            //        });

            //    });

            //    //$("#data-table").on('click', '.btn-delete', function () {
            //    //    var id = $(this).data("id");
            //    //    modal.delete(id);
            //    //});
            //}
        },
        init: function () {
            this.investigationConvocation.init();
        }


    };
    var search = {
        init: function () {
            $("#search").doneTyping(function () {
                datatable.investigationConvocation.reload();
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