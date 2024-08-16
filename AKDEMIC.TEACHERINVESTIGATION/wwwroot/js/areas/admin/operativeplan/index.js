var InitApp = function () {
    var datatable = {
        operativeplan: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: "/admin/plan-operativo".proto().parseURL(),
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
                        title: "Nombre",
                        data: "name"
                    },
                    {
                        title: "Año",
                        data: "year"
                    },
                    {
                        title: "Estado",
                        data: null,
                        render: function (data) {
                            switch (data.state) {
                                case 0:
                                    return `<span class="m--font-warning">${data.stateText}</span>`;
                                case 1:
                                    return `<span class="m--font-info">${data.stateText}</span>`;
                                case 2:
                                    return `<span class="m--font-danger">${data.stateText}</span>`;
                            }
                        }
                    },
                    {
                        title: "Archivo",
                        data: null,
                        orderable: false,
                        render: function (data) {
                            var template = "";
                            var fileUrl = `/documentos/${data.filePath}`.proto().parseURL();
                            //FileURL

                            template += `<a href="${fileUrl}"  `;
                            template += "class='btn btn-success ";
                            template += "m-btn btn-sm m-btn--icon' download>";
                            template += "<span><i class='flaticon-file'></i></a> ";

                            return template;
                        }
                    },
                    {
                        title: "Unidad",
                        data: "unitText"
                    },
                    {
                        title: "Jefe de Unidad",
                        data: "unitBoss"
                    },
                    {
                        title: "Opciones",
                        data: null,
                        orderable: false,
                        render: function (data) {
                            var template = "";
                            //Edit
                            if (data.state != 1) {
                                template += "<button ";
                                template += "class='btn btn-info ";
                                template += "m-btn btn-sm m-btn--icon btn-edit' ";
                                template += " data-id='" + data.id + "'>";
                                template += "<span><i class='la la-edit'></i><span>Observar</span></span></button> ";

                                //Delete
                                template += "<button type='button' ";
                                template += "class='btn btn-success btn-delete ";
                                template += "m-btn btn-sm  m-btn--icon-only' ";
                                template += " data-id='" + data.id + "'>";
                                template += "<i class='la la-check'></i></button> ";
                            }
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
                $("#data-table").on('click', '.btn-edit', function () {
                    var id = $(this).data("id");
                    modal.operativeplan.edit.show(id);
                });

                $("#data-table").on('click', '.btn-delete', function () {
                    let id = $(this).data("id");
                    swal({
                        title: "¿Está seguro?",
                        text: "El Plan Operativo sera aprovado",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonText: "Sí, aprovarlo",
                        confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                        cancelButtonText: "Cancelar",
                        showLoaderOnConfirm: true,
                        preConfirm: () => {
                            return new Promise((resolve) => {
                                $.ajax({
                                    url: ("/admin/plan-operativo?handler=Approve").proto().parseURL(),
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
                                            text: "El Plan Operativo ha sido aprovado con exito",
                                            confirmButtonText: "Excelente"
                                        }).then(datatable.operativeplan.reload());
                                    },
                                    error: function (error) {

                                        let errorMessage = "El Plan operativo presenta errores";
                                        if (error != null) {
                                            errorMessage = error.responseText;
                                        }
                                        swal({
                                            type: "error",
                                            title: "Error",
                                            confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                            confirmButtonText: "Entendido",
                                            text: errorMessage
                                        }).then(datatable.operativeplan.reload());
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
            this.operativeplan.init();
        }
    };



    var search = {
        init: function () {
            $("#search").doneTyping(function () {
                datatable.operativeplan.reload();
            });
        }
    };

    var modal = {
        operativeplan: {
            edit: {
                object: $("#edit-form").validate({
                    submitHandler: function (formElement, e) {
                        $("#btnEdit").addLoader();
                        e.preventDefault();
                        let formData = new FormData(formElement);
                        $.ajax({
                            url: $(formElement).attr("action"),
                            type: "POST",
                            data: formData,
                            contentType: false,
                            processData: false
                        }).done(function () {
                            $(".modal").modal("hide");
                            $(".m-alert").addClass("m--hide");
                            toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);

                            datatable.operativeplan.reload();
                            modal.operativeplan.edit.clear();
                            $("#btnEdit").removeLoader();
                        }).fail(function (error) {
                            toastr.error(error.responseText, _app.constants.toastr.title.error);
                            $("#btnEdit").removeLoader();
                            //if (error.responseText !== null && error.responseText !== "") $("#add_form_msg_txt").html(error.responseText);
                            //else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                            //$("#add_form_msg").removeClass("m--hide").show();
                        }).always(function () {

                        });
                    }
                }),
                load: function (id) {
                    $.ajax({
                        url: `/admin/plan-operativo`.proto().parseURL(),
                        type: "GET",
                        data: {
                            handler: "DetailOperativePlan",
                            Id: id
                        },
                    }).done(function (result) {
                        $("#edit-form input[name=Id]").val(result.id);
                        $("#edit-form input[name=Observation]").val(result.observation);
                        $("#btnEdit").removeLoader();
                    }).fail(function (error) {
                        toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                        $("#btnEdit").removeLoader();
                    }).always(function () {

                    });
                },
                show: function (id) {
                    $("#btnEdit").removeLoader();
                    modal.operativeplan.edit.load(id);
                    $("#editModal").modal("toggle");
                },
                clear: function () {
                    modal.operativeplan.edit.object.resetForm();
                },
                events: function () {
                    $("#editModal").on("hidden.bs.modal", function () {
                        modal.operativeplan.edit.clear();
                    });
                }
            },
            init: function () {
                this.edit.events();
            }
        },
        init: function () {
            this.operativeplan.init();

        }
    };


    return {
        init: function () {
            datatable.init();
            search.init();
            modal.init();
        }
    }
}();

$(function () {
    InitApp.init();
})