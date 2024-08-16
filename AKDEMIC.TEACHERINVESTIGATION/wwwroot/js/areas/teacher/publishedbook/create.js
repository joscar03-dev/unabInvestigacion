var InitApp = function () {
    var getFormattedFileSize = function (bytes) {
        if (bytes == 0) {
            return '0 Bytes';
        }

        var sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB'];
        var i = parseInt(Math.floor(Math.log(bytes) / Math.log(1024)));
        var fileSize = Math.round(bytes / Math.pow(1024, i), 2) + ' ' + sizes[i];

        return fileSize;
    };

    var authorArr = [];
    var fileArr = [];
    var datatable = {
        author: {
            object: null,
            options: {
                serverSide: false,
                filter: false,
                lengthChange: false,
                data: authorArr,
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
                        title: "Correo electrónico",
                        data: "email"
                    },
                    {
                        title: "Dni",
                        data: "dni"
                    },
                    {
                        title: "Opciones",
                        render: function (row, index) {
                            var tmp = `<button data-index="${index}" type="button" class="btn btn-danger btn-sm m-btn m-btn--icon btn-delete-author"<span><i class="la la-trash"> </i> </span> Eliminar </span></span></button>`
                            return tmp;

                        }

                    }
                ],
            },
            reload: function () {
                this.object.clear();
                this.object.rows.add(authorArr);
                this.object.draw();
            },
            events: function () {
                $("#authors-table").on('click', '.btn-delete-author', function () {
                    let index = $(this).data("index");
                    authorArr.splice(index, 1);
                    datatable.author.reload();
                });
            },
            init: function () {
                this.object = $("#authors-table").DataTable(this.options);
                this.events();
            }
        },
        file: {
            object: null,
            options: {
                serverSide: false,
                filter: false,
                lengthChange: false,
                data: fileArr,
                columns: [

                    {
                        title: "Nombre",
                        data: "name"
                    },
                    {
                        title: "Tamaño",
                        data: null,
                        render: function (result) {
                            var bytes = result.size;
                            return getFormattedFileSize(bytes);
                        }
                    },
                    {
                        title: "Opciones",
                        data: null,
                        render: function (row, index) {
                            var tmp = `<button data-index="${index}" type="button" class="btn btn-danger btn-sm m-btn m-btn--icon btn-delete-file"<span><i class="la la-trash"> </i> </span> Eliminar </span></span></button>`
                            return tmp;

                        }

                    }
                ],
            },
            reload: function () {
                this.object.clear();
                this.object.rows.add(fileArr);
                this.object.draw();
            },
            events: function () {
                $("#files-table").on('click', '.btn-delete-file', function () {
                    let index = $(this).data("index");
                    fileArr.splice(index, 1);
                    datatable.file.reload();
                });
            },
            init: function () {
                this.object = $("#files-table").DataTable(this.options);
                this.events();
            }
        },
        init: function () {
            this.author.init();
            this.file.init();
        }
    };
    var modal = {
        author: {
            object: $("#add-author-form").validate({
                submitHandler: function (formElement, e) {
                    $("#btnSaveAuthor").addLoader();
                    e.preventDefault();
                    let paternalSurname = $("#add-author-form input[name='PaternalSurname']").val();
                    let maternalSurname = $("#add-author-form input[name='MaternalSurname']").val();
                    let name = $("#add-author-form input[name='Name']").val();
                    let email = $("#add-author-form input[name='Email']").val();
                    let dni = $("#add-author-form input[name='Dni']").val();

                    if (authorArr.some(x => x.dni === dni)) {
                        toastr.error("Este Dni ya se encuentra registrado en la tabla", _app.constants.toastr.title.error);
                        return false;
                    }

                    authorArr.push({
                        paternalSurname: paternalSurname,
                        maternalSurname: maternalSurname,
                        name: name,
                        email: email,
                        dni: dni
                    });
                    datatable.author.reload();

                    $("#btnSaveAuthor").removeLoader();
                    $("#add-authors-modal").modal("hide");
                }
            }),
            show: function () {
                $("#btnSaveAuthor").removeLoader();
                $("#add-authors-modal").modal("toggle");
            },
            clear: function () {
                modal.author.object.resetForm();
            },
            events: function () {
                $("#add-authors-modal").on("hidden.bs.modal", function () {
                    modal.author.clear();
                });
            },
            init: function () {
                this.events();
            }
        },
        file: {
            object: $("#add-file-form").validate({
                submitHandler: function (formElement, e) {
                    $("#btnSavefile").addLoader();
                    e.preventDefault();
                    let fileName = $("#add-file-form input[name='Name']").val();
                    let realfile = $("#add-file-form input[name='File']")["0"].files[0];

                    fileArr.push({
                        name: fileName,
                        file: realfile,
                        size: realfile.size
                    });
                    datatable.file.reload();

                    $("#btnSavefile").removeLoader();
                    $("#add-files-modal").modal("hide");
                }
            }),
            show: function () {
                $("#btnSavefile").removeLoader();
                $("#add-files-modal").modal("toggle");
            },
            clear: function () {
                $("#add-file-form input[name='File']").next().html("Seleccione archivo");
                modal.file.object.resetForm();
            },
            events: function () {
                $("#add-files-modal").on("hidden.bs.modal", function () {
                    modal.file.clear();
                });
            },
            init: function () {
                this.events();
            }
        },
        init: function () {
            this.author.init();
            this.file.init();
        }
    };

    var form = {
        publishedBook: {
            init: function () {
                $("#create-form").validate({
                    submitHandler: function (formElement, e) {
                        $("#btnSave").addLoader();
                        e.preventDefault();
                        let formData = new FormData(formElement);
                        for (var i = 0; i < authorArr.length; i++) {
                            formData.append(`Input.PublishedBookAuthors[${i}].PaternalSurname`, authorArr[i].paternalSurname);
                            formData.append(`Input.PublishedBookAuthors[${i}].MaternalSurname`, authorArr[i].maternalSurname);
                            formData.append(`Input.PublishedBookAuthors[${i}].Name`, authorArr[i].name);
                            formData.append(`Input.PublishedBookAuthors[${i}].Email`, authorArr[i].email);
                            formData.append(`Input.PublishedBookAuthors[${i}].Dni`, authorArr[i].dni);
                        }
                        for (var i = 0; i < fileArr.length; i++) {
                            formData.append(`Input.PublishedBookFiles[${i}].File`, fileArr[i].file);
                            formData.append(`Input.PublishedBookFiles[${i}].Name`, fileArr[i].name);
                        }
                        $.ajax({
                            url: $(formElement).attr("action"),
                            type: "POST",
                            data: formData,
                            contentType: false,
                            processData: false
                        }).done(function () {
                            toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                            location.href = `/investigador/libros-publicados/crear`.proto().parseURL();
                        }).fail(function (error) {
                            toastr.error(error.responseText, _app.constants.toastr.title.error);
                            $("#btnSave").removeLoader();
                        }).always(function () {

                        });
                    }
                });
            }
        },
        init: function () {
            this.publishedBook.init();
        }


    };


    var checkevents = {
        init: function () {
            $("#Input_TermnConditions").on("click", function () {
                if ($("#Input_TermnConditions").is(':checked')) {
                    $(".to_hide").attr("hidden", false);
                } else {
                    $(".to_hide").attr("hidden", true);
                }
            });
        }
    }


    return {
        init: function () {
            datatable.init();
            form.init();
            modal.init();
            checkevents.init();
        }
    }
}();

$(function () {
    InitApp.init();
})

