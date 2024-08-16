var InitApp = function () {
    var incubatorConvocation = $("#create-form input[name='IncubatorConvocationId']").val();
    var documentArr = [];

    var getFormattedFileSize = function (bytes) {
        if (bytes == 0) {
            return '0 Bytes';
        }

        var sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB'];
        var i = parseInt(Math.floor(Math.log(bytes) / Math.log(1024)));
        var fileSize = Math.round(bytes / Math.pow(1024, i), 2) + ' ' + sizes[i];

        return fileSize;
    };

    var annexData = {
        init: function () {
            $.ajax({
                url: (`/alumno/convocatoria-emprendimiento/${incubatorConvocation}/detalle?handler=IncubatorConvocationAnnex`).proto().parseURL(),
                type: "GET",
            }).done(function (result) {
                for (var i = 0; i < result.length; i++) {
                    documentArr.push({
                        incubatorConvocationAnnexId: result[i].id,
                        code: result[i].code,
                        name: result[i].name,
                        file: null,
                        size: '0'
                    });
                }

                datatable.document.reload();
            });
        }
    }
    var datatable = {

        document: {
            object: null,
            options: {
                serverSide: false,
                filter: false,
                lengthChange: false,
                data: documentArr,
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
                        orderable: false,
                        render: function (data) {
                            var template = "";

                            //Delete
                            template += "<button type='button' ";
                            template += "class='btn btn-success btn-upload-document ";
                            template += "m-btn btn-sm  m-btn--icon-only' ";
                            template += ` data-name='${data.name}' data-id='${data.incubatorConvocationAnnexId}' >`
                            template += "<i class='la la-file'></i></button> ";

                            return template;
                        }
                    }
                ],
            },
            reload: function () {
                this.object.clear();
                this.object.rows.add(documentArr);
                this.object.draw();
            },
            events: function () {
                $("#document-table").on('click', '.btn-upload-document', function () {
                    //Call Modal
                    var name = $(this).data("name");
                    var id = $(this).data("id");
                    $("#add-document-form input[name='Name']").val(name);
                    $("#add-document-form input[name='Id']").val(id);
                    modal.document.show();
                });
            },
            init: function () {
                this.object = $("#document-table").DataTable(this.options);
                this.events();
            }
        },
        init: function () {
            this.document.init();
        }
    };
    var select = {
        init: function () {
            this.department.init();
            this.province.init();
            this.district.init();
        },
        department: {
            init: function () {
                this.load();
                this.events();
            },
            load: function () {
                $.ajax({
                    url: (`/api/departamentos/get`).proto().parseURL(),
                    type: "GET"
                }).done(function (result) {
                    $("#DepartmentId").select2({
                        data: result,
                    });
                });
            },
            events: function () {
                $("#DepartmentId").on("change", function () {
                    if ($(this).val() == 0) {
                        select.province.clear();
                        select.district.clear();
                    } else {
                        select.province.load();
                    }
                });
            }
        },
        province: {
            init: function () {
                $("#ProvinceId").select2();
                this.events();
            },
            clear: function () {
                $("#ProvinceId").empty();
                $("#ProvinceId").html('<option value="0" selected disabled>Selecciona Provincia</option>')
            },
            load: function () {
                select.province.clear();
                select.district.clear();
                $.ajax({
                    url: (`/api/provincias/get`).proto().parseURL(),
                    type: "GET",
                    data: {
                        departmentId: $("#DepartmentId").val()
                    }
                }).done(function (result) {
                    $("#ProvinceId").select2({
                        data: result,
                    });
                });
            },
            events: function () {
                $("#ProvinceId").on("change", function () {
                    if ($(this).val() == 0) {
                        select.district.clear();
                    } else {
                        select.district.load();
                    }
                });
            }
        },
        district: {
            init: function () {
                $("#DistrictId").select2();
            },
            clear: function () {
                $("#DistrictId").empty();
                $("#DistrictId").html('<option value="0" selected disabled>Selecciona Distrito</option>')
            },
            load: function () {
                select.district.clear();
                $.ajax({
                    url: (`/api/distritos/get`).proto().parseURL(),
                    type: "GET",
                    data: {
                        provinceId: $("#ProvinceId").val()
                    }
                }).done(function (result) {
                    $("#DistrictId").select2({
                        data: result,
                    });
                });
            }
        },
    };

    var modal = {
        document: {
            object: $("#add-document-form").validate({
                submitHandler: function (formElement, e) {
                    $("#btnSaveDocument").addLoader();
                    e.preventDefault();
                    let annexId = $("#add-document-form input[name='Id']").val();
                    let realfile = $("#add-document-form input[name='File']")["0"].files[0];

                    let index = documentArr.findIndex((x => x.incubatorConvocationAnnexId == annexId));

                    documentArr[index].file = realfile;
                    documentArr[index].size = realfile.size;

                    datatable.document.reload();
                    $("#btnSaveDocument").removeLoader();
                    $("#add-document-modal").modal("hide");
                }
            }),
            show: function () {
                $("#btnSaveDocument").removeLoader();
                $("#add-document-modal").modal("toggle");
            },
            clear: function () {
                $("#add-file-form input[name='File']").next().html("Seleccione archivo");
                modal.document.object.resetForm();
            },
            events: function () {
                $("#add-document-modal").on("hidden.bs.modal", function () {
                    modal.document.clear();
                });
            },
            init: function () {
                this.events();
            }
        },
        init: function () {
            this.author.init();
            this.document.init();
        }
    };

    var form = {
        incubatorPostulation: {
            init: function () {
                $("#create-form").validate({
                    submitHandler: function (formElement, e) {
                        var anyFileNull = documentArr.filter(x => x.file == null);
                        if (anyFileNull.length > 0) {
                            toastr.error("Todos los documentos de anexo son necesarios", _app.constants.toastr.title.error);
                            return;
                        }
                        $("#btnSave").addLoader();
                        e.preventDefault();
                        let formData = new FormData(formElement);
                        for (var i = 0; i < documentArr.length; i++) {
                            formData.append(`IncubatorPostulationAnnexes[${i}].AnnexFile`, documentArr[i].file);
                            formData.append(`IncubatorPostulationAnnexes[${i}].IncubatorConvocationAnnexId`, documentArr[i].incubatorConvocationAnnexId);
                        }

                        formData.append("DepartmentText", $("#DepartmentId option:selected").text());
                        formData.append("ProvinceText", $("#ProvinceId option:selected").text());
                        formData.append("DistrictText", $("#DistrictId option:selected").text());
                        $.ajax({
                            url: $(formElement).attr("action"),
                            type: "POST",
                            data: formData,
                            contentType: false,
                            processData: false
                        }).done(function () {
                            toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                            location.href = `/alumno/postulacion-convocatoria-emprendimiento`.proto().parseURL();
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
            this.incubatorPostulation.init();
        }


    };

    return {
        init: function () {
            datatable.init();
            select.init();
            annexData.init();
            form.init();
        },
        print: function () {
            console.log(documentArr);
        }
    }
}();

$(function () {
    InitApp.init();
});