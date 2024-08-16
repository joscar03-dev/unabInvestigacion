var InitApp = function () {
    var publishedChapterBookId = $("#Input_Id").val();

    var datatable = {
        author: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: `/investigador/capitulos-libros-publicados/${publishedChapterBookId}/editar`.proto().parseURL(),
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
                    modal.author.edit.show(id);
                });
                $("#author-datatable").on('click', '.btn-delete', function () {
                    var id = $(this).data("id");
                    modal.author.delete(id);
                });
            }
        },
        file: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: `/investigador/capitulos-libros-publicados/${publishedChapterBookId}/editar`.proto().parseURL(),
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
                    modal.file.edit.show(id);
                });
                $("#file-datatable").on('click', '.btn-delete', function () {
                    var id = $(this).data("id");
                    modal.file.delete(id);
                });
            }
        },
        init: function () {
            this.author.init();
            this.file.init();
        }
    };

    var modal = {
        author: {
            create: {
                object: $("#author-form").validate({
                    submitHandler: function (formElement, e) {
                        $("#btnSaveAuthor").addLoader();
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
                            datatable.author.reload();
                            modal.author.create.clear();
                            $("#btnSaveAuthor").removeLoader();
                        }).fail(function (error) {
                            toastr.error(error.responseText, _app.constants.toastr.title.error);
                            $("#btnSaveAuthor").removeLoader();
                            //if (error.responseText !== null && error.responseText !== "") $("#add_form_msg_txt").html(error.responseText);
                            //else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                            //$("#add_form_msg").removeClass("m--hide").show();
                        }).always(function () {

                        });
                    }
                }),
                show: function () {
                    $("#btnSaveAuthor").removeLoader();
                    $("#authorModal").modal("toggle");
                },
                clear: function () {
                    modal.author.create.object.resetForm();
                },
                events: function () {
                    $("#authorModal").on("hidden.bs.modal", function () {
                        modal.author.create.clear();
                    });
                }
            },
            edit: {
                object: $("#author-editform").validate({
                    submitHandler: function (formElement, e) {
                        $("#btnEditAuthor").addLoader();
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

                            datatable.author.reload();
                            modal.author.edit.clear();
                            $("#btnEditAuthor").removeLoader();
                        }).fail(function (error) {
                            toastr.error(error.responseText, _app.constants.toastr.title.error);
                            $("#btnEditAuthor").removeLoader();
                            //if (error.responseText !== null && error.responseText !== "") $("#add_form_msg_txt").html(error.responseText);
                            //else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                            //$("#add_form_msg").removeClass("m--hide").show();
                        }).always(function () {

                        });
                    }
                }),
                load: function (id) {
                    $.ajax({
                        url: `/investigador/capitulos-libros-publicados/${publishedChapterBookId}/editar`.proto().parseURL(),
                        type: "GET",
                        data: {
                            handler: "DetailAuthor",
                            Id: id
                        },
                    }).done(function (result) {
                        $("#author-editform input[name=PaternalSurname]").val(result.paternalSurname);
                        $("#author-editform input[name=MaternalSurname]").val(result.maternalSurname);
                        $("#author-editform input[name=Name]").val(result.name);
                        $("#author-editform input[name=Email]").val(result.email);
                        $("#author-editform input[name=Dni]").val(result.dni);
                        $("#author-editform input[name=Id]").val(result.id);
                        $("#btnEditAuthor").removeLoader();
                    }).fail(function (error) {
                        toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                        $("#btnEditAuthor").removeLoader();
                    }).always(function () {

                    });
                },
                show: function (id) {
                    $("#btnEditAuthor").removeLoader();
                    modal.author.edit.load(id);
                    $("#authorEditModal").modal("toggle");
                },
                clear: function () {
                    modal.author.edit.object.resetForm();
                },
                events: function () {
                    $("#authorEditModal").on("hidden.bs.modal", function () {
                        modal.author.edit.clear();
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
                                url: (`/investigador/capitulos-libros-publicados/${publishedChapterBookId}/editar?handler=DeleteAuthor`).proto().parseURL(),
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
                                    }).then(datatable.author.reload());
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
        file: {
            create: {
                object: $("#file-form").validate({
                    submitHandler: function (formElement, e) {
                        $("#btnSaveFile").addLoader();
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
                            datatable.file.reload();
                            modal.file.create.clear
                            $("#btnSaveFile").removeLoader();
                        }).fail(function (error) {
                            toastr.error(error.responseText, _app.constants.toastr.title.error);
                            $("#btnSaveFile").removeLoader();
                            //if (error.responseText !== null && error.responseText !== "") $("#add_form_msg_txt").html(error.responseText);
                            //else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                            //$("#add_form_msg").removeClass("m--hide").show();
                        }).always(function () {

                        });
                    }
                }),
                show: function () {
                    $("#btnSaveFile").removeLoader();
                    $("#fileModal").modal("toggle");
                },
                clear: function () {
                    modal.file.create.object.resetForm();
                    $("#file-form").find(".custom-file-label").text("Seleccionar archivo");
                },
                events: function () {
                    $("#fileModal").on("hidden.bs.modal", function () {
                        modal.file.create.clear();
                    });
                }
            },
            edit: {
                object: $("#file-editform").validate({
                    submitHandler: function (formElement, e) {
                        $("#btnEditFile").addLoader();
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
                            datatable.file.reload();
                            modal.file.edit.clear();
                        }).fail(function (error) {
                            toastr.error(error.responseText, _app.constants.toastr.title.error);
                            $("#btnEditFile").removeLoader();
                            //if (error.responseText !== null && error.responseText !== "") $("#add_form_msg_txt").html(error.responseText);
                            //else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                            //$("#add_form_msg").removeClass("m--hide").show();
                        }).always(function () {

                        });
                    }
                }),
                load: function (id) {
                    $.ajax({
                        url: `/investigador/capitulos-libros-publicados/${publishedChapterBookId}/editar`.proto().parseURL(),
                        type: "GET",
                        data: {
                            handler: "DetailFile",
                            id: id
                        },
                    }).done(function (result) {
                        $("#file-editform input[name=Id]").val(result.id);
                        $("#file-editform input[name=Name]").val(result.name);
                        $("#file-editform input[name=PublishedChapterBookId]").val(result.publishedChapterBookId);
                        $("#file-editform").find(".custom-file-label").text("Seleccionar archivo");
                        $("#file-editform").find(".custom-file").append('<div class="form-control-feedback">*Seleccionar en caso sea necesario reemplazar el documento vigente.</div>');

                    }).fail(function (error) {
                        toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                        if (error.responseText !== null && error.responseText !== "") $("#add_form_msg_txt").html(error.responseText);
                        else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                        $("#add_form_msg").removeClass("m--hide").show();
                    }).always(function () {

                    });

                },
                show: function (id) {
                    $("#btnEditFile").removeLoader();
                    modal.file.edit.load(id);
                    $("#fileEditModal").modal("toggle");
                },
                clear: function () {
                    modal.file.edit.object.resetForm();
                },
                events: function () {
                    $("#fileEditModal").on("hidden.bs.modal", function () {
                        modal.file.edit.clear();
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
                                url: (`/investigador/capitulos-libros-publicados/${publishedChapterBookId}/editar?handler=DeleteFile`).proto().parseURL(),
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
                                    }).then(datatable.file.reload());
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
            this.author.init();
            this.file.init();

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
                        location.href = `/investigador/capitulos-libros-publicados`.proto().parseURL();
                    }).fail(function (error) {
                        toastr.error(error.responseText, _app.constants.toastr.title.error);
                        $("#btnSave").removeLoader();
                    }).always(function () {

                    });
                }
            });
        }
    };



    return {
        init: function () {
            datatable.init();
            form.init();
        }
    }
}();

$(function () {
    InitApp.init();
})