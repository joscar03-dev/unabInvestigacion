var InitApp = function () {
    var publicationId = $("#Input_Id").val();

    var datatable = {
        publicationAuthor: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: `/investigador/publicaciones/${publicationId}/editar`.proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        data.handler = "AuthorDatatable";
                        data.searchValue = $("#search").val();
                    }
                },
                pageLength: 50,
                orderable: [],
                columns: [
                    {
                        title: "Apellido Paterno",
                        data: "paternalSurname"
                    },
                    {
                        title: "Apellido Materno",
                        data: "maternalSurname"
                    },
                    {
                        title: "Nombres",
                        data: "name"
                    },
                    {
                        title: "Correo",
                        data: "email"
                    },
                    {
                        title: "Dni",
                        data: "dni"
                    },
                    {
                        title: "Opciones",
                        data: null,
                        orderable: false,
                        render: function (data) {
                            var template = "";
                            //Edit
                            template += "<button ";
                            template += "class='btn btn-info ";
                            template += "m-btn btn-sm m-btn--icon btn-edit' ";
                            template += " data-id='" + data.id + "'>";
                            template += "<span><i class='la la-edit'></i><span>Editar</span></span></button> ";
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
                this.object = $("#author-datatable").DataTable(this.options);
                this.events();
            },
            events: function () {
                $("#author-datatable").on('click', '.btn-edit', function () {
                    var id = $(this).data("id");
                    modal.publicationAuthor.edit.show(id);
                });
                $("#author-datatable").on('click', '.btn-delete', function () {
                    var id = $(this).data("id");
                    modal.publicationAuthor.delete(id);
                });
            }
        },
        publicationFile: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: `/investigador/publicaciones/${publicationId}/editar`.proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        data.handler = "FileDatatable";
                        data.searchValue = $("#search").val();
                    }
                },
                pageLength: 50,
                orderable: [],
                columns: [
                    {
                        title: "Name",
                        data: "name"
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
                        title: "Opciones",
                        data: null,
                        orderable: false,
                        render: function (data) {
                            var template = "";
                            //Edit
                            template += "<button ";
                            template += "class='btn btn-info ";
                            template += "m-btn btn-sm m-btn--icon btn-edit' ";
                            template += " data-id='" + data.id + "'>";
                            template += "<span><i class='la la-edit'></i><span>Editar</span></span></button> ";
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
                this.object = $("#file-datatable").DataTable(this.options);
                this.events();
            },
            events: function () {
                $("#file-datatable").on('click', '.btn-edit', function () {
                    var id = $(this).data("id");
                    modal.publicationFile.edit.show(id);
                });
                $("#file-datatable").on('click', '.btn-delete', function () {
                    var id = $(this).data("id");
                    modal.publicationFile.delete(id);
                });
            }
        },
        init: function () {
            this.publicationAuthor.init();
            this.publicationFile.init();
        }
    };

    var modal = {
        publicationAuthor: {
            create: {
                object: $("#publicationAuthor-form").validate({
                    submitHandler: function (formElement, e) {
                        $("#btnSavePublicationAuthor").addLoader();
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
                            datatable.publicationAuthor.reload();
                            modal.publicationAuthor.create.clear();
                            $("#btnSavePublicationAuthor").removeLoader();
                        }).fail(function (error) {
                            toastr.error(error.responseText, _app.constants.toastr.title.error);
                            $("#btnSavePublicationAuthor").removeLoader();
                            //if (error.responseText !== null && error.responseText !== "") $("#add_form_msg_txt").html(error.responseText);
                            //else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                            //$("#add_form_msg").removeClass("m--hide").show();
                        }).always(function () {

                        });
                    }
                }),
                show: function () {
                    $("#btnSavePublicationAuthor").removeLoader();
                    $("#publicationAuthorModal").modal("toggle");
                },
                clear: function () {
                    modal.publicationAuthor.create.object.resetForm();
                },
                events: function () {
                    $("#publicationAuthorModal").on("hidden.bs.modal", function () {
                        modal.publicationAuthor.create.clear();
                    });
                }
            },
            edit: {
                object: $("#publicationAuthor-editform").validate({
                    submitHandler: function (formElement, e) {
                        $("#btnEditPublicationAuthor").addLoader();
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

                            datatable.publicationAuthor.reload();
                            modal.publicationAuthor.edit.clear();
                            $("#btnEditPublicationAuthor").removeLoader();
                        }).fail(function (error) {
                            toastr.error(error.responseText, _app.constants.toastr.title.error);
                            $("#btnEditPublicationAuthor").removeLoader();
                            //if (error.responseText !== null && error.responseText !== "") $("#add_form_msg_txt").html(error.responseText);
                            //else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                            //$("#add_form_msg").removeClass("m--hide").show();
                        }).always(function () {

                        });
                    }
                }),
                load: function (id) {
                    $.ajax({
                        url: `/investigador/publicaciones/${publicationId}/editar`.proto().parseURL(),
                        type: "GET",
                        data: {
                            handler: "DetailPublicationAuthor",
                            Id: id
                        },
                    }).done(function (result) {
                        $("#publicationAuthor-editform input[name=PaternalSurname]").val(result.paternalSurname);
                        $("#publicationAuthor-editform input[name=MaternalSurname]").val(result.maternalSurname);
                        $("#publicationAuthor-editform input[name=Name]").val(result.name);
                        $("#publicationAuthor-editform input[name=Email]").val(result.email);
                        $("#publicationAuthor-editform input[name=Dni]").val(result.dni);
                        $("#publicationAuthor-editform input[name=Id]").val(result.id);
                        $("#btnEditPublicationAuthor").removeLoader();
                    }).fail(function (error) {
                        toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                        $("#btnEditPublicationAuthor").removeLoader();
                    }).always(function () {

                    });
                },
                show: function (id) {
                    $("#btnEditPublicationAuthor").removeLoader();
                    modal.publicationAuthor.edit.load(id);
                    $("#publicationAuthorEditModal").modal("toggle");
                },
                clear: function () {
                    modal.publicationAuthor.edit.object.resetForm();
                },
                events: function () {
                    $("#publicationAuthorEditModal").on("hidden.bs.modal", function () {
                        modal.publicationAuthor.edit.clear();
                    });
                }
            },
            delete: function (id) {
                swal({
                    title: "¿Está seguro?",
                    text: "El Autor será eliminado.",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonText: "Sí, eliminarlo",
                    confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                    cancelButtonText: "Cancelar",
                    showLoaderOnConfirm: true,
                    preConfirm: () => {
                        return new Promise((resolve) => {
                            $.ajax({
                                url: (`/investigador/publicaciones/${publicationId}/editar?handler=DeletePublicationAuthor`).proto().parseURL(),
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
                                        text: "El Autor ha sido eliminada con exito",
                                        confirmButtonText: "Excelente"
                                    }).then(datatable.publicationAuthor.reload());
                                },
                                error: function (errormessage) {
                                    swal({
                                        type: "error",
                                        title: "Error",
                                        confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                        confirmButtonText: "Entendido",
                                        text: "El Autor presenta información relacionada"
                                    });
                                }
                            });
                        });
                    },
                    allowOutsideClick: () => !swal.isLoading()
                });
            },
            init: function () {
                this.create.events();
                this.edit.events();
            }
        },
        publicationFile: {
            create: {
                object: $("#publicationFile-form").validate({
                    submitHandler: function (formElement, e) {
                        $("#btnSavePublicationFile").addLoader();
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
                            datatable.publicationFile.reload();
                            modal.publicationFile.create.clear
                            $("#btnSavePublicationFile").removeLoader();
                        }).fail(function (error) {
                            toastr.error(error.responseText, _app.constants.toastr.title.error);
                            $("#btnSavePublicationFile").removeLoader();
                            //if (error.responseText !== null && error.responseText !== "") $("#add_form_msg_txt").html(error.responseText);
                            //else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                            //$("#add_form_msg").removeClass("m--hide").show();
                        }).always(function () {

                        });
                    }
                }),
                show: function () {
                    $("#btnSavePublicationFile").removeLoader();
                    $("#publicationFileModal").modal("toggle");
                },
                clear: function () {
                    modal.publicationFile.create.object.resetForm();
                    $("#publicationFile-form").find(".custom-file-label").text("Seleccionar archivo");
                },
                events: function () {
                    $("#publicationFileModal").on("hidden.bs.modal", function () {
                        modal.publicationFile.create.clear();
                    });
                }
            },
            edit: {
                object: $("#publicationFile-editform").validate({
                    submitHandler: function (formElement, e) {
                        $("#btnEditPublicationFile").addLoader();
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
                            datatable.publicationFile.reload();
                            modal.edit.clear();
                        }).fail(function (error) {
                            toastr.error(error.responseText, _app.constants.toastr.title.error);
                            $("#btnEditPublicationFile").removeLoader();
                            //if (error.responseText !== null && error.responseText !== "") $("#add_form_msg_txt").html(error.responseText);
                            //else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                            //$("#add_form_msg").removeClass("m--hide").show();
                        }).always(function () {

                        });
                    }
                }),
                load: function (id) {
                    $.ajax({
                        url: `/investigador/publicaciones/${publicationId}/editar`.proto().parseURL(),
                        type: "GET",
                        data: {
                            handler: "DetailPublicationFile",
                            id: id
                        },
                    }).done(function (result) {
                        $("#publicationFile-editform input[name=Id]").val(result.id);
                        $("#publicationFile-editform input[name=Name]").val(result.name);
                        $("#publicationFile-editform input[name=PublicationId]").val(result.publicationId);
                        $("#publicationFile-editform").find(".custom-file-label").text("Seleccionar archivo");
                        $("#publicationFile-editform").find(".custom-file").append('<div class="form-control-feedback">*Seleccionar en caso sea necesario reemplazar el documento vigente.</div>');

                    }).fail(function (error) {
                        toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                        if (error.responseText !== null && error.responseText !== "") $("#add_form_msg_txt").html(error.responseText);
                        else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                        $("#add_form_msg").removeClass("m--hide").show();
                    }).always(function () {

                    });

                },
                show: function (id) {
                    $("#btnEditPublicationFile").removeLoader();
                    modal.publicationFile.edit.load(id);
                    $("#publicationFileEditModal").modal("toggle");
                },
                clear: function () {
                    modal.publicationFile.edit.object.resetForm();
                },
                events: function () {
                    $("#publicationFileEditModal").on("hidden.bs.modal", function () {
                        modal.publicationFile.edit.clear();
                    });
                }
            },
            delete: function (id) {
                swal({
                    title: "¿Está seguro?",
                    text: "El Archivo será eliminado.",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonText: "Sí, eliminarlo",
                    confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                    cancelButtonText: "Cancelar",
                    showLoaderOnConfirm: true,
                    preConfirm: () => {
                        return new Promise((resolve) => {
                            $.ajax({
                                url: (`/investigador/publicaciones/${publicationId}/editar?handler=DeletePublicationFile`).proto().parseURL(),
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
                                        text: "El Archivo ha sido eliminado con exito",
                                        confirmButtonText: "Excelente"
                                    }).then(datatable.publicationFile.reload());
                                },
                                error: function (errormessage) {
                                    swal({
                                        type: "error",
                                        title: "Error",
                                        confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                        confirmButtonText: "Entendido",
                                        text: "El Archivo presenta información relacionada"
                                    });
                                }
                            });
                        });
                    },
                    allowOutsideClick: () => !swal.isLoading()
                });
            },
            init: function () {
                this.create.events();
                this.edit.events();
            }
        },
        init: function () {
            this.publicationAuthor.init();
            this.publicationFile.init();

        }
    };

    var form = {
        init: function () {
            $("#edit-form").validate({
                submitHandler: function (form, e) {
                    $("#btnSave").addLoader();
                    e.preventDefault();
                    let formData = new FormData(form);
                    $.ajax({
                        url: $(form).attr("action"),
                        type: "POST",
                        data: formData,
                        contentType: false,
                        processData: false
                    }).done(function () {
                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                        location.href = `/investigador/publicaciones`.proto().parseURL();
                    }).fail(function (error) {
                        toastr.error(error.responseText, _app.constants.toastr.title.error);
                        $("#btnSave").removeLoader();
                    }).always(function () {

                    });
                }
            });
        }
    };

    var select = {
        init: function () {
            this.authorShipOrder.init();
            this.publicationFunction.init();
            this.opusType.init();
            this.indexPlace.init();
            this.identificationType.init();
            this.workerCategory.init();
        },
        workerCategory: {
            init: function () {
                $('#Input_WorkCategory').select2();
            }
        },
        authorShipOrder: {
            init: function () {
                this.load();
            },
            load: function () {
                $.ajax({
                    url: (`/api/orden-autoria/select/get`).proto().parseURL(),
                    type: "GET"
                }).done(function (result) {
                    $('#Input_AuthorShipOrderId').select2({
                        data: result,
                    });
                    var authorShipOrderId = $("#AuthorShipOrderId").val();
                    $("#Input_AuthorShipOrderId").val(authorShipOrderId).trigger("change");
                });
            },
        },
        publicationFunction: {
            init: function () {
                this.load();
            },
            load: function () {
                $.ajax({
                    url: (`/api/funciones/select/get`).proto().parseURL(),
                    type: "GET"
                }).done(function (result) {
                    $('#Input_PublicationFunctionId').select2({
                        data: result,
                    });
                    var publicationFunctionId = $("#PublicationFunctionId").val();
                    $("#Input_PublicationFunctionId").val(publicationFunctionId).trigger("change");
                });
            },

        },
        opusType: {
            init: function () {
                this.load();
            },
            load: function () {
                $.ajax({
                    url: (`/api/tipo-de-obra/select/get`).proto().parseURL(),
                    type: "GET"
                }).done(function (result) {
                    $('#Input_OpusTypeId').select2({
                        data: result,
                    });
                    var opusTypeId = $("#OpusTypeId").val();
                    $("#Input_OpusTypeId").val(opusTypeId).trigger("change");
                });
            },

        },
        indexPlace: {
            init: function () {
                this.load();
            },
            load: function () {
                $.ajax({
                    url: (`/api/indexada/select/get`).proto().parseURL(),
                    type: "GET"
                }).done(function (result) {
                    $('#Input_IndexPlaceId').select2({
                        data: result,
                    });
                    var indexPlaceId = $("#IndexPlaceId").val();
                    $('#Input_IndexPlaceId').val(indexPlaceId).trigger("change");
                });
            },

        },
        identificationType: {
            init: function () {
                this.load();
            },
            load: function () {
                $.ajax({
                    url: (`/api/tipo-de-identificacion/select/get`).proto().parseURL(),
                    type: "GET"
                }).done(function (result) {
                    $('#Input_IdentificationTypeId').select2({
                        data: result,
                    });
                    var identificationTypeId = $("#IdentificationTypeId").val();
                    $('#Input_IdentificationTypeId').val(identificationTypeId).trigger("change");
                });
            },

        },

    };
    var datePickers = {
        init: function () {
            $("#Input_PublishDate").datepicker({
                clearBtn: true,
                orientation: "bottom",
                format: _app.constants.formats.datepicker
            });
        }
    }


    return {
        init: function () {
            datatable.init();
            form.init();
            datePickers.init();
            select.init();
        }
    }
}();

$(function () {
    InitApp.init();
})