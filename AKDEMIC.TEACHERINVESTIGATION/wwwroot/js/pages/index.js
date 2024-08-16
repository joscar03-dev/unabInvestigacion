var InitApp = function () {
    var datatable = {
        projectReportNotExpired: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: "/".proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        data.handler = "DatatableNotExpired";
                    }
                },
                pageLength: 50,
                orderable: [],
                columns: [
                    {
                        title: "Investigador Principal",
                        data: "fullName"
                    },
                    {
                        title: "Correo",
                        data: "email"
                    },
                    {
                        title: "Entregable",
                        data: "reportName"
                    },
                    {
                        title: "Fecha de Expiración",
                        data: "expirationDate"
                    },
                    {
                        title: "Dias  Restantes",
                        data: "timeRest"
                    },
                    {
                        title: "Último correo enviado",
                        data: "lastEmailSendedDate"
                        
                    },
                    {
                        title: "Opciones",
                        data: null,
                        orderable: false,
                        render: function (data) {
                            var template = "";

                            
                            template += "<button ";
                            template += "class='btn btn-brand m-btn  btn-delete ";
                            template += "m-btn btn-sm  m-btn--icon-only' ";
                            template += " data-id='" + data.id + "'>";
                            template += "<i class='la la-envelope-o'></i> Enviar Correo</button> ";

                            return template;
                        }
                    }
                ],
            },
            reload: function () {
                this.object.ajax.reload();
            },
            init: function () {
                this.object = $("#projectReportNotExpired-table").DataTable(this.options);
                this.events();
            },
            events: function () {
                $("#projectReportNotExpired-table").on('click', '.btn-delete', function () {
                    var id = $(this).data("id");
                    swal({
                        title: "¿Está seguro?",
                        text: "Se enviara un correo al usuario que esta pronto de vencer su entregable.",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonText: "Sí, enviar",
                        confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                        cancelButtonText: "Cancelar",
                        showLoaderOnConfirm: true,
                        preConfirm: () => {
                            return new Promise((resolve) => {
                                $.ajax({
                                    url: ("/?handler=SendProjectReportEmail").proto().parseURL(),
                                    type: "POST",
                                    data: {
                                        investigationProjectReportId: id
                                    },
                                    beforeSend: function (xhr) {
                                        xhr.setRequestHeader("XSRF-TOKEN",
                                            $('input:hidden[name="__RequestVerificationToken"]').val());
                                    },
                                    success: function (result) {
                                        swal({
                                            type: "success",
                                            title: "Completado",
                                            text: "Se envió el correo con exito",
                                            confirmButtonText: "Excelente"
                                        }).then(datatable.projectReportNotExpired.reload());
                                    },
                                    error: function (errormessage) {
                                        swal({
                                            type: "error",
                                            title: "Error",
                                            confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                            confirmButtonText: "Entendido",
                                            text: "No se pudo enviar el Correo"
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
        projectReportExpired: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: "/".proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        data.handler = "DatatableExpired";
                    }
                },
                pageLength: 50,
                orderable: [],
                columns: [

                    {
                        title: "Investigador Principal",
                        data: "fullName"
                    },
                    {
                        title: "Correo",
                        data: "email"
                    },
                    {
                        title: "Entregable",
                        data: "reportName"
                    },
                    {
                        title: "Fecha de Expiración",
                        data: "expirationDate"
                    },
                ],
            },
            reload: function () {
                this.object.ajax.reload();
            },
            init: function () {
                this.object = $("#projectReportExpired-table").DataTable(this.options);
            },
        },
        init: function () {
            this.projectReportNotExpired.init();
            this.projectReportExpired.init();
        }
    };
    return {
        init: function () {
            datatable.init();
        }
    }
}();

$(function () {
    InitApp.init();
})