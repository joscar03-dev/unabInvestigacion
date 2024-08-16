var InitApp = function () { 
    var investigationConvocationPostulantId = $("#Input_InvestigationConvocationPostulantId").val();
    var datatable = {
        observation: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: `/investigador/convocatoria/${investigationConvocationPostulantId}/inscripcion`.proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        delete data.columns;
                        data.handler = "ObservationsDatatable";
                    }
                },
                pageLength: 50,
                orderable: [],
                columns: [
                    {
                        title: "Fecha de Observación",
                        data: "createdAt"
                    },
                    {
                        title: "Descripción",
                        data: "description"
                    },
                    {
                        title: "Estado",
                        data: null,
                        render: function (data) {
                            switch (data.state)
                            {
                                case 0:
                                    return `<span class="m--font-warning">${data.stateText}</span>`;
                                case 1:
                                    return `<span class="m--font-info">${data.stateText}</span>`;
                                case 2:
                                    return `<span class="m--font-danger">${data.stateText}</span>`;
                                case 3:
                                    return `<span class="m--font-success">${data.stateText}</span>`;
                                default:
                                    return `<span class="m--font-primary">${data.stateText}</span>`;
                            }
                        }
                    },
                    {
                        title: "Opciones",
                        data: null,
                        orderable: false,
                        render: function (data) {
                            var template = "";

                            if (data.state == 0) {
                                template += "<button ";
                                template += "class='btn btn-success btn-solved ";
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
                this.object = $("#observation-table").DataTable(this.options);
                this.events();
            },
            events: function () {
                $("#observation-table").on('click', '.btn-solved', function () {
                    var id = $(this).data("id");
                    swal({
                        title: "¿Está seguro?",
                        text: "El estado de la observación cambiará a pendiente de revisión.",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonText: "Sí, cambiarlo",
                        confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                        cancelButtonText: "Cancelar",
                        showLoaderOnConfirm: true,
                        preConfirm: () => {
                            return new Promise((resolve) => {
                                $.ajax({
                                    url: `/investigador/convocatoria/${investigationConvocationPostulantId}/inscripcion?handler=SolveObservation`.proto().parseURL(),
                                    type: "POST",
                                    data: {
                                        postulantObservationId: id
                                    },
                                    beforeSend: function (xhr) {
                                        xhr.setRequestHeader("XSRF-TOKEN",
                                            $('#observationModal input:hidden[name="__RequestVerificationToken"]').val());
                                    },
                                    success: function (result) {
                                        swal({
                                            type: "success",
                                            title: "Completado",
                                            text: "El estado de la observación ha sido cambiada con exito",
                                            confirmButtonText: "Excelente"
                                        }).then(datatable.observation.reload());
                                    },
                                    error: function (errormessage) {
                                        swal({
                                            type: "error",
                                            title: "Error",
                                            confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                            confirmButtonText: "Entendido",
                                            text: "No se ha podido cambiar el estado de la observación"
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
        advance: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: `/investigador/convocatoria/${investigationConvocationPostulantId}/inscripcion`.proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        delete data.columns;
                        data.handler = "AdvanceDatatable";
                        data.searchValue = $("#search").val();
                    }
                },
                pageLength: 50,
                orderable: [],
                columns: [
                    {
                        title: "Nombre de Avance",
                        data: "name"
                    },
                    {
                        title: "Fecha de Avance",
                        data: "createdAt"
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
                this.object = $("#progressfileconvocationpostulant-datatable").DataTable(this.options);
                this.events();
            },
            events: function () {
                $("#progressfileconvocationpostulant-datatable").on('click', '.btn-edit', function () {
                    var id = $(this).data("id");
                    modal.progressFile.edit.show(id);
                });
                $("#progressfileconvocationpostulant-datatable").on('click', '.btn-delete', function () {
                    var id = $(this).data("id");
                    modal.progressFile.delete(id);
                });
            }
        },
        techFiles: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: `/investigador/convocatoria/${investigationConvocationPostulantId}/inscripcion`.proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        delete data.columns;
                        data.handler = "UploadTechDocumentFileDatatable";
                        data.searchValue = $("#search").val();
                    }
                },
                pageLength: 50,
                orderable: [],
                columns: [
                    {
                        title: "Nombre de Documento Tecnico",
                        data: "name"
                    },
                    {
                        title: "Fecha de Subida",
                        data: "createdAt"
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
                this.object = $("#postulanttechnicalfile-datatable").DataTable(this.options);
                this.events();
            },
            events: function () {
                $("#postulanttechnicalfile-datatable").on('click', '.btn-edit', function () {
                    var id = $(this).data("id");
                    modal.techFiles.edit.show(id);
                });
                $("#postulanttechnicalfile-datatable").on('click', '.btn-delete', function () {
                    var id = $(this).data("id");
                    modal.techFiles.delete(id);
                });
            }
        },
        financialFiles: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: `/investigador/convocatoria/${investigationConvocationPostulantId}/inscripcion`.proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        delete data.columns;
                        data.handler = "UploadFinancialDocumentFileDatatable";
                        data.searchValue = $("#search").val();
                    }
                },
                pageLength: 50,
                orderable: [],
                columns: [
                    {
                        title: "Nombre de Documento Financiero",
                        data: "name"
                    },
                    {
                        title: "Fecha de Subida",
                        data: "createdAt"
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
                this.object = $("#postulantfinancialfile-datatable").DataTable(this.options);
                this.events();
            },
            events: function () {
                $("#postulantfinancialfile-datatable").on('click', '.btn-edit', function () {
                    var id = $(this).data("id");
                    modal.financialFiles.edit.show(id);
                });
                $("#postulantfinancialfile-datatable").on('click', '.btn-delete', function () {
                    var id = $(this).data("id");
                    modal.financialFiles.delete(id);
                });
            }
        },
        annexedFiles: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: `/investigador/convocatoria/${investigationConvocationPostulantId}/inscripcion`.proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        delete data.columns;
                        data.handler = "AnnexedFileDatatable";
                        data.searchValue = $("#search").val();
                    }
                },
                pageLength: 50,
                orderable: [],
                columns: [
                    {
                        title: "Nombre de Anexo",
                        data: "name"
                    },
                    {
                        title: "Fecha de Subida",
                        data: "createdAt"
                    },
                    {
                        title: "Archivo",
                        data: null,
                        orderable: false,
                        render: function (data) {
                            var template = "";
                            var fileUrl = `/documentos/${data.documentPath}`.proto().parseURL();
                            //FileURL
                            template += `<a href="${fileUrl}"  `;
                            template += "class='btn btn-success ";
                            template += "m-btn btn-sm m-btn--icon' download>";
                            template += "<span><i class='flaticon-file'></i></span></a> ";

                            return template;
                        }
                    },
                    {
                        title: "Opciones",
                        data: null,
                        orderable: false,
                        render: function (data) {
                            var template = "";
                            //Delete
                            template += `<button type="button" `;
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
                this.object = $("#annexedFile-datatable").DataTable(this.options);
                this.events();
            },
            events: function () {
                $("#annexedFile-datatable").on('click', '.btn-delete', function () {
                    var id = $(this).data("id");
                    modal.annexedFiles.delete(id);
                });
            }
        },
        teamMember: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: `/investigador/convocatoria/${investigationConvocationPostulantId}/inscripcion`.proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        delete data.columns;
                        data.handler = "TeamMemberDatatable";
                        data.searchValue = $("#search").val();
                    }
                },
                pageLength: 50,
                orderable: [],
                columns: [
                    {
                        title: "Investigador",
                        data: "fullName"
                    },
                    {
                        title: "Rol",
                        data: "roleName"
                    },
                    {
                        title: "Concytec",
                        data: null,
                        orderable: false,
                        
                        render: function (data) {
                            var template = "";
                            var fileUrl = `${data.cvUrlCTE}`;
                            //FileURL
                            if (data.cvUrlCTE != null) {
                                template += `<a href="${fileUrl}"  Target="_blank" `;
                                template += "class='btn btn-success ";
                                template += "m-btn btn-sm m-btn--icon'>";
                                template += "<span><i class='flaticon-file'></i></span></a> ";
                            } else {
                                template += "--"
                            }

                            return template;
                        }
                    },
                    {
                        title: "Opciones",
                        data: null,
                        orderable: false,
                        render: function (data) {
                            var template = "";
                            //Delete
                            template += `<button type="button" `;
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
                this.object = $("#team-member-datatable").DataTable(this.options);
                this.events();
            },
            events: function () {
                $("#team-member-datatable").on('click', '.btn-delete', function () {
                    var id = $(this).data("id");
                    swal({
                        title: "¿Está seguro?",
                        text: "El miembro del equipo será eliminada.",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonText: "Sí, eliminarlo",
                        confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                        cancelButtonText: "Cancelar",
                        showLoaderOnConfirm: true,
                        preConfirm: () => {
                            return new Promise((resolve) => {
                                $.ajax({
                                    url: (`/investigador/convocatoria/${investigationConvocationPostulantId}/inscripcion`).proto().parseURL(),
                                    type: "GET",
                                    data: {
                                        handler: "TeamMemberDelete",
                                        id: id
                                    },
                                    success: function (result) {
                                        swal({
                                            type: "success",
                                            title: "Completado",
                                            text: "El miembro del equipo ha sido eliminada con exito",
                                            confirmButtonText: "Excelente"
                                        }).then(convocationForms.teamMember.load()).then(convocationForms.teamMember.reset());
                                        progress.load();
                                    },
                                    error: function (errormessage) {
                                        swal({
                                            type: "error",
                                            title: "Error",
                                            confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                            confirmButtonText: "Entendido",
                                            text: "El miembro del equipo presenta información relacionada"
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
        externalMember: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: `/investigador/convocatoria/${investigationConvocationPostulantId}/inscripcion`.proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        delete data.columns;
                        data.handler = "ExternalMemberDatatable";
                        data.searchValue = $("#search").val();
                    }
                },
                pageLength: 50,
                orderable: [],
                columns: [
                    {
                        title: "Nombres",
                        data: "name"
                    },
                    {
                        title: "Paterno",
                        data: "paternalSurname"
                    },
                    {
                        title: "Materno",
                        data: "maternalSurname"
                    },
                    {
                        title: "Dni",
                        data: "dni"
                    },
                    {
                        title: "Profesión",
                        data: "profession"
                    },
                    {
                        title: "Cv",
                        data: null,
                        orderable: false,

                        render: function (data) {
                            var template = "";
                            var fileUrl = `${data.cvFilePath}`;
                            //FileURL
                            if (data.cvFilePath != null) {
                                template += `<a href="${fileUrl}"  Target="_blank" `;
                                template += "class='btn btn-success ";
                                template += "m-btn btn-sm m-btn--icon'>";
                                template += "<span><i class='flaticon-file'></i></span></a> ";
                            } else {
                                template += "--"
                            }

                            return template;
                        }
                    },
                    {
                        title: "Opciones",
                        data: null,
                        orderable: false,
                        render: function (data) {
                            var template = "";
                            //Delete
                            template += `<button type="button" `;
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
                this.object = $("#external-member-datatable").DataTable(this.options);
                this.events();
            },
            events: function () {
                $("#external-member-datatable").on('click', '.btn-delete', function () {
                    var id = $(this).data("id");
                    modal.externalMembers.delete(id);
                });
            }
        },
        init: function () {
            this.observation.init();
            this.advance.init();
            this.techFiles.init();
            this.financialFiles.init();
            this.annexedFiles.init();
            this.teamMember.init();
            this.externalMember.init();
        }
    };
    var events = {
        init: function () {
            $("input[type=checkbox]").change(function () {
                console.log("change");
                if ($(this).prop("checked")) {
                    $(this).parent().attr("class", "option bg-selected");
                }
                else {
                    $(this).parent().attr("class", "option");
                }
            });

            $("input[type=radio]").click(function () {
                $(this).parent().parent().children().attr("class", "option");
                if ($(this).prop("checked"))
                    $(this).parent().attr("class", "option bg-selected");
                else
                    $(this).parent().attr("class", "option");
            });

            $("#AddBtn").on("click", function () {
                modal.create.show();
            });
        }
    };
    var select = {
        init: function () {            
            this.researcherUser.init();
            this.researcherUserRole.init();
        },        
        researcherUser: {
            init: function () {
                $("#team-member-form select[name=ResearcherUserId]").select2({
                    width: "100%",
                    placeholder: "Buscar...",
                    ajax: {
                        url: "/api/usuarios/select-search".proto().parseURL(),
                        dataType: "json",
                        data: function (params) {
                            return {
                                term: params.term,
                                page: params.page
                            };
                        },
                        processResults: function (data, params) {
                            return {
                                results: data
                            };
                        },
                        cache: true
                    },
                    escapeMarkup: function (markup) {
                        return markup;
                    },
                    minimumInputLength: 3
                });
            }
        },
        researcherUserRole: {
            init: function () {
                this.load();
            },
            load: function () {
                $.ajax({
                    url: (`/api/rolesinvestigacion/select/get`).proto().parseURL(),
                    type: "GET"
                }).done(function (result) {
                    $('#team-member-form select[name="ResearcherUserRoleId"]').select2({
                        data: result,
                    });
                });
            }
        },
    };
    var modal = {
        progressFile: {
            create: {
                object: $("#add-form").validate({
                    submitHandler: function (form, e) {
                        $("#btnSaveCreate").addLoader();
                        e.preventDefault();
                        let formData = new FormData(form);
                        $.ajax({
                            url: $(form).attr("action"),
                            type: "POST",
                            data: formData,
                            contentType: false,
                            processData: false
                        }).done(function () {
                            $(".modal").modal("hide");
                            $(".m-alert").addClass("m--hide");
                            toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                            datatable.advance.reload();
                            modal.progressFile.create.clear();
                            $("#btnSaveCreate").removeLoader();
                        }).fail(function (error) {
                            toastr.error(error.responseText, _app.constants.toastr.title.error);
                            $("#btnSaveCreate").removeLoader();
                            //if (error.responseText !== null && error.responseText !== "") $("#add_form_msg_txt").html(error.responseText);
                            //else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                            //$("#add_form_msg").removeClass("m--hide").show();
                        }).always(function () {

                        });
                    }
                }),
                show: function () {
                    $("#btnSaveCreate").removeLoader();
                    $("#addModal").modal("toggle");
                },
                clear: function () {
                    $("#add-form").find(".custom-file-label").text("Seleccionar archivo");
                    modal.progressFile.create.object.resetForm();
                },
                events: function () {
                    $("#addModal").on("hidden.bs.modal", function () {
                        modal.progressFile.create.clear();
                    });
                }
            },
            edit: {
                object: $("#edit-form").validate({
                    submitHandler: function (form, e) {
                        $("#btnEdit").addLoader();
                        e.preventDefault();
                        let formData = new FormData(form);
                        $.ajax({
                            url: $(form).attr("action"),
                            type: "POST",
                            data: formData,
                            contentType: false,
                            processData: false
                        }).done(function () {
                            $(".modal").modal("hide");
                            $(".m-alert").addClass("m--hide");
                            toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);

                            datatable.advance.reload();
                            modal.progressFile.edit.clear();
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
                        url: `/investigador/convocatoria/${investigationConvocationPostulantId}/inscripcion`.proto().parseURL(),
                        type: "GET",
                        data: {
                            handler: "DetailAdvance",
                            Id: id
                        },
                    }).done(function (result) {
                        $("#edit-form input[name=Name]").val(result.name);
                        $("#edit-form input[name=Id]").val(result.id);
                        $("#edit-form").find(".custom-file-label").text("Seleccionar archivo");
                        $("#edit-form").find(".custom-file").append('<div class="form-control-feedback">*Seleccionar en caso sea necesario reemplazar el documento vigente.</div>');
                    }).fail(function (error) {
                        toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                        if (error.responseText !== null && error.responseText !== "") $("#add_form_msg_txt").html(error.responseText);
                        else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                        $("#add_form_msg").removeClass("m--hide").show();
                    }).always(function () {

                    });
                },
                show: function (id) {
                    $("#btnEdit").removeLoader();
                    modal.progressFile.edit.load(id);
                    $("#editModal").modal("toggle");
                },
                clear: function () {
                    $("#edit-form").find(".custom-file-label").text("Seleccionar archivo");
                    modal.progressFile.edit.object.resetForm();
                },
                events: function () {
                    $("#editModal").on("hidden.bs.modal", function () {
                        modal.progressFile.edit.clear();
                    });
                }
            },
            delete: function (id) {
                swal({
                    title: "¿Está seguro?",
                    text: "El Avance será eliminada.",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonText: "Sí, eliminarlo",
                    confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                    cancelButtonText: "Cancelar",
                    showLoaderOnConfirm: true,
                    preConfirm: () => {
                        return new Promise((resolve) => {
                            $.ajax({
                                url: (`/investigador/convocatoria/${investigationConvocationPostulantId}/inscripcion`).proto().parseURL(),
                                type: "GET",
                                data: {
                                    handler: "AdvanceDelete",
                                    id: id
                                },
                                success: function (result) {
                                    swal({
                                        type: "success",
                                        title: "Completado",
                                        text: "El Avance ha sido eliminada con exito",
                                        confirmButtonText: "Excelente"
                                    }).then(datatable.advance.reload());
                                },
                                error: function (errormessage) {
                                    swal({
                                        type: "error",
                                        title: "Error",
                                        confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                        confirmButtonText: "Entendido",
                                        text: "El Avance presenta información relacionada"
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
        techFiles: {
            create: {
                object: $("#tech-document-form").validate({
                    submitHandler: function (form, e) {
                        $("#btnSaveTechDocument").addLoader();
                        e.preventDefault();
                        let formData = new FormData(form);
                        $.ajax({
                            url: $(form).attr("action"),
                            type: "POST",
                            data: formData,
                            contentType: false,
                            processData: false
                        }).done(function () {
                            $(".modal").modal("hide");
                            $(".m-alert").addClass("m--hide");
                            toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                            datatable.techFiles.reload();
                            modal.techFiles.create.clear();
                            $("#btnSaveTechDocument").removeLoader();
                        }).fail(function (error) {
                            toastr.error(error.responseText, _app.constants.toastr.title.error);
                            $("#btnSaveTechDocument").removeLoader();
                            //if (error.responseText !== null && error.responseText !== "") $("#add_form_msg_txt").html(error.responseText);
                            //else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                            //$("#add_form_msg").removeClass("m--hide").show();
                        }).always(function () {

                        });
                    }
                }),
                show: function () {
                    $("#btnSaveTechDocument").removeLoader();
                    $("#techDocumentModal").modal("toggle");
                },
                clear: function () {
                    $("#tech-document-form").find(".custom-file-label").text("Seleccionar archivo");
                    modal.techFiles.create.object.resetForm();
                },
                events: function () {
                    $("#techDocumentModal").on("hidden.bs.modal", function () {
                        modal.techFiles.create.clear();
                    });
                }
            },
            edit: {
                object: $("#edit-form-techfile").validate({
                    submitHandler: function (form, e) {
                        $("#btnEditTechFile").addLoader();
                        e.preventDefault();
                        let formData = new FormData(form);
                        $.ajax({
                            url: $(form).attr("action"),
                            type: "POST",
                            data: formData,
                            contentType: false,
                            processData: false
                        }).done(function () {
                            $(".modal").modal("hide");
                            $(".m-alert").addClass("m--hide");
                            toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);

                            datatable.techFiles.reload();
                            modal.techFiles.edit.clear();
                            $("#btnEditTechFile").removeLoader();
                        }).fail(function (error) {
                            toastr.error(error.responseText, _app.constants.toastr.title.error);
                            $("#btnEditTechFile").removeLoader();
                            //if (error.responseText !== null && error.responseText !== "") $("#add_form_msg_txt").html(error.responseText);
                            //else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                            //$("#add_form_msg").removeClass("m--hide").show();
                        }).always(function () {

                        });
                    }
                }),
                load: function (id) {
                    $.ajax({
                        url: `/investigador/convocatoria/${investigationConvocationPostulantId}/inscripcion`.proto().parseURL(),
                        type: "GET",
                        data: {
                            handler: "DetailUploadTechDocumentFile",
                            Id: id
                        },
                    }).done(function (result) {
                        $("#edit-form-techfile input[name=Name]").val(result.name);
                        $("#edit-form-techfile input[name=Id]").val(result.id);
                        $("#edit-form-techfile").find(".custom-file-label").text("Seleccionar archivo");
                        $("#edit-form-techfile").find(".custom-file").append('<div class="form-control-feedback">*Seleccionar en caso sea necesario reemplazar el documento vigente.</div>');
                        $("#btnEditTechFile").removeLoader();
                    }).fail(function (error) {
                        toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                        if (error.responseText !== null && error.responseText !== "") $("#add_form_msg_txt").html(error.responseText);
                        else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                        $("#btnEditTechFile").removeLoader();
                        $("#add_form_msg").removeClass("m--hide").show();
                    }).always(function () {

                    });
                },
                show: function (id) {
                    $("#btnEditTechFile").removeLoader();
                    modal.techFiles.edit.load(id);
                    $("#editModalTech").modal("toggle");
                },
                clear: function () {
                    $("#edit-form-techfile").find(".custom-file-label").text("Seleccionar archivo");
                    modal.techFiles.edit.object.resetForm();
                },
                events: function () {
                    $("#editModalTech").on("hidden.bs.modal", function () {
                        modal.techFiles.edit.clear();
                    });
                }
            },
            delete: function (id) {
                swal({
                    title: "¿Está seguro?",
                    text: "El documento técnico será eliminado.",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonText: "Sí, eliminarlo",
                    confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                    cancelButtonText: "Cancelar",
                    showLoaderOnConfirm: true,
                    preConfirm: () => {
                        return new Promise((resolve) => {
                            $.ajax({
                                url: (`/investigador/convocatoria/${investigationConvocationPostulantId}/inscripcion`).proto().parseURL(),
                                type: "GET",
                                data: {
                                    handler: "UploadTechDocumentFileDelete",
                                    id: id
                                },
                                success: function (result) {
                                    swal({
                                        type: "success",
                                        title: "Completado",
                                        text: "El documento técnico  ha sido eliminada con exito",
                                        confirmButtonText: "Excelente"
                                    }).then(datatable.techFiles.reload());
                                },
                                error: function (errormessage) {
                                    swal({
                                        type: "error",
                                        title: "Error",
                                        confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                        confirmButtonText: "Entendido",
                                        text: "El documento técnico  presenta información relacionada"
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
        financialFiles: {
            create: {
                object: $("#financial-document-form").validate({
                    submitHandler: function (form, e) {
                        $("#btnSaveFinancialDocument").addLoader();
                        e.preventDefault();
                        let formData = new FormData(form);
                        $.ajax({
                            url: $(form).attr("action"),
                            type: "POST",
                            data: formData,
                            contentType: false,
                            processData: false
                        }).done(function () {
                            $(".modal").modal("hide");
                            $(".m-alert").addClass("m--hide");
                            toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                            datatable.financialFiles.reload();
                            modal.financialFiles.create.clear();
                            $("#btnSaveFinancialDocument").removeLoader();
                        }).fail(function (error) {
                            toastr.error(error.responseText, _app.constants.toastr.title.error);
                            $("#btnSaveFinancialDocument").removeLoader();
                            //if (error.responseText !== null && error.responseText !== "") $("#add_form_msg_txt").html(error.responseText);
                            //else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                            //$("#add_form_msg").removeClass("m--hide").show();
                        }).always(function () {

                        });
                    }
                }),
                show: function () {
                    $("#btnSaveFinancialDocument").removeLoader();
                    $("#financialDocumentModal").modal("toggle");
                },
                clear: function () {
                    $("#financial-document-form").find(".custom-file-label").text("Seleccionar archivo");
                    modal.financialFiles.create.object.resetForm();
                },
                events: function () {
                    $("#financialDocumentModal").on("hidden.bs.modal", function () {
                        modal.financialFiles.create.clear();
                    });
                }
            },
            edit: {
                object: $("#edit-form-financialfile").validate({
                    submitHandler: function (form, e) {
                        $("#btnEditFinancialFile").addLoader();
                        e.preventDefault();
                        let formData = new FormData(form);
                        $.ajax({
                            url: $(form).attr("action"),
                            type: "POST",
                            data: formData,
                            contentType: false,
                            processData: false
                        }).done(function () {
                            $(".modal").modal("hide");
                            $(".m-alert").addClass("m--hide");
                            toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);

                            datatable.financialFiles.reload();
                            modal.financialFiles.edit.clear();
                            $("#btnEditFinancialFile").removeLoader();
                        }).fail(function (error) {
                            toastr.error(error.responseText, _app.constants.toastr.title.error);
                            $("#btnEditFinancialFile").removeLoader();
                            //if (error.responseText !== null && error.responseText !== "") $("#add_form_msg_txt").html(error.responseText);
                            //else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                            //$("#add_form_msg").removeClass("m--hide").show();
                        }).always(function () {

                        });
                    }
                }),
                load: function (id) {
                    $.ajax({
                        url: `/investigador/convocatoria/${investigationConvocationPostulantId}/inscripcion`.proto().parseURL(),
                        type: "GET",
                        data: {
                            handler: "DetailUploadFinancialDocumentFile",
                            Id: id
                        },
                    }).done(function (result) {
                        $("#edit-form-financialfile input[name=Name]").val(result.name);
                        $("#edit-form-financialfile input[name=Id]").val(result.id);
                        $("#edit-form-financialfile").find(".custom-file-label").text("Seleccionar archivo");
                        $("#edit-form-financialfile").find(".custom-file").append('<div class="form-control-feedback">*Seleccionar en caso sea necesario reemplazar el documento vigente.</div>');
                    }).fail(function (error) {
                        toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                        if (error.responseText !== null && error.responseText !== "") $("#add_form_msg_txt").html(error.responseText);
                        else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                        $("#add_form_msg").removeClass("m--hide").show();
                    }).always(function () {

                    });
                },
                show: function (id) {
                    $("#btnEditFinancialFile").removeLoader();
                    modal.financialFiles.edit.load(id);
                    $("#editModalFinancial").modal("toggle");
                },
                clear: function () {
                    $("#edit-form-financialfile").find(".custom-file-label").text("Seleccionar archivo");
                    modal.financialFiles.edit.object.resetForm();
                },
                events: function () {
                    $("#editModalFinancial").on("hidden.bs.modal", function () {
                        modal.financialFiles.edit.clear();
                    });
                }
            },
            delete: function (id) {
                swal({
                    title: "¿Está seguro?",
                    text: "El documento de financiero será eliminado.",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonText: "Sí, eliminarlo",
                    confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                    cancelButtonText: "Cancelar",
                    showLoaderOnConfirm: true,
                    preConfirm: () => {
                        return new Promise((resolve) => {
                            $.ajax({
                                url: (`/investigador/convocatoria/${investigationConvocationPostulantId}/inscripcion`).proto().parseURL(),
                                type: "GET",
                                data: {
                                    handler: "UploadFinancialDocumentFileDelete",
                                    id: id
                                },
                                success: function (result) {
                                    swal({
                                        type: "success",
                                        title: "Completado",
                                        text: "El documento de financiero ha sido eliminado con exito",
                                        confirmButtonText: "Excelente"
                                    }).then(datatable.financialFiles.reload());
                                },
                                error: function (errormessage) {
                                    swal({
                                        type: "error",
                                        title: "Error",
                                        confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                        confirmButtonText: "Entendido",
                                        text: "El documento de financiero presenta información relacionada"
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
        annexedFiles: {
            create: {
                object: $("#annexed-file-modal-form").validate({
                    submitHandler: function (form, e) {
                        $("#btnSaveAnnexedFile").addLoader();
                        e.preventDefault();
                        let formData = new FormData(form);
                        $.ajax({
                            url: $(form).attr("action"),
                            type: "POST",
                            data: formData,
                            contentType: false,
                            processData: false
                        }).done(function () {
                            $(".modal").modal("hide");
                            $(".m-alert").addClass("m--hide");
                            toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                            datatable.annexedFiles.reload();
                            convocationForms.annexedFile.load()
                            modal.annexedFiles.create.clear();
                            $("#btnSaveAnnexedFile").removeLoader();
                        }).fail(function (error) {
                            toastr.error(error.responseText, _app.constants.toastr.title.error);
                            $("#btnSaveAnnexedFile").removeLoader();
                            //if (error.responseText !== null && error.responseText !== "") $("#add_form_msg_txt").html(error.responseText);
                            //else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                            //$("#add_form_msg").removeClass("m--hide").show();
                        }).always(function () {

                        });
                    }
                }),
                show: function () {
                    $("#btnSaveAnnexedFile").removeLoader();
                    $("#annexedFileModal").modal("toggle");
                },
                clear: function () {
                    $("#annexed-file-modal-form").find(".custom-file-label").text("Seleccionar archivo");
                    modal.annexedFiles.create.object.resetForm();
                },
                events: function () {
                    $("#annexedFileModal").on("hidden.bs.modal", function () {
                        modal.annexedFiles.create.clear();
                    });
                }
            },
            delete: function (id) {
                swal({
                    title: "¿Está seguro?",
                    text: "El anexo de postulación será eliminado.",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonText: "Sí, eliminarlo",
                    confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                    cancelButtonText: "Cancelar",
                    showLoaderOnConfirm: true,
                    preConfirm: () => {
                        return new Promise((resolve) => {
                            $.ajax({
                                url: (`/investigador/convocatoria/${investigationConvocationPostulantId}/inscripcion`).proto().parseURL(),
                                type: "GET",
                                data: {
                                    handler: "AnnexedFileDelete",
                                    id: id
                                },
                                success: function (result) {
                                    swal({
                                        type: "success",
                                        title: "Completado",
                                        text: "El anexo de postulación ha sido eliminada con exito",
                                        confirmButtonText: "Excelente"
                                    }).then(datatable.annexedFiles.reload()).then(convocationForms.annexedFile.load());
                                },
                                error: function (errormessage) {
                                    swal({
                                        type: "error",
                                        title: "Error",
                                        confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                        confirmButtonText: "Entendido",
                                        text: "El anexo de postulación presenta información relacionada"
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
            }
        },
        externalMembers: {
            create: {
                object: $("#external-member-modal-form").validate({
                    submitHandler: function (form, e) {
                        $("#btnSaveExternalMember").addLoader();
                        e.preventDefault();
                        let formData = new FormData(form);
                        $.ajax({
                            url: $(form).attr("action"),
                            type: "POST",
                            data: formData,
                            contentType: false,
                            processData: false
                        }).done(function () {
                            $(".modal").modal("hide");
                            $(".m-alert").addClass("m--hide");
                            toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                            convocationForms.externalMember.load();
                            modal.externalMembers.create.clear();
                            progress.load();
                            $("#btnSaveExternalMember").removeLoader();
                        }).fail(function (error) {
                            toastr.error(error.responseText, _app.constants.toastr.title.error);
                            $("#btnSaveExternalMember").removeLoader();
                            //if (error.responseText !== null && error.responseText !== "") $("#add_form_msg_txt").html(error.responseText);
                            //else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                            //$("#add_form_msg").removeClass("m--hide").show();
                        }).always(function () {

                        });
                    }
                }),
                show: function () {
                    $("#btnSaveExternalMember").removeLoader();
                    $("#externalMemberModal").modal("toggle");
                },
                clear: function () {
                    $("#external-member-modal-form").find(".custom-file-label").text("Seleccionar archivo");
                    modal.externalMembers.create.object.resetForm();
                },
                events: function () {
                    $("#externalMemberModal").on("hidden.bs.modal", function () {
                        modal.externalMembers.create.clear();
                    });
                }
            },
            delete: function (id) {
                swal({
                    title: "¿Está seguro?",
                    text: "El colaborador externo será eliminado.",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonText: "Sí, eliminarlo",
                    confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                    cancelButtonText: "Cancelar",
                    showLoaderOnConfirm: true,
                    preConfirm: () => {
                        return new Promise((resolve) => {
                            $.ajax({
                                url: (`/investigador/convocatoria/${investigationConvocationPostulantId}/inscripcion`).proto().parseURL(),
                                type: "GET",
                                data: {
                                    handler: "ExternalMemberDelete",
                                    id: id
                                },
                                success: function (result) {
                                    swal({
                                        type: "success",
                                        title: "Completado",
                                        text: "El colaborador externo ha sido eliminada con exito",
                                        confirmButtonText: "Excelente"
                                    }).then(convocationForms.externalMember.load());
                                    progress.load();
                                },
                                error: function (errormessage) {
                                    swal({
                                        type: "error",
                                        title: "Error",
                                        confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                        confirmButtonText: "Entendido",
                                        text: "El colaborador externo presenta información relacionada"
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
            }
        },

        init: function () {
            this.progressFile.init();
            this.techFiles.init();
            this.financialFiles.init();
            this.annexedFiles.init();
            this.externalMembers.init();
        }
    };

    var convocationForms = {
        //1
        generalInformation: {
            load: function () {
                $.ajax({
                    url: `/investigador/convocatoria/${investigationConvocationPostulantId}/inscripcion?handler=GeneralInformationLoad`,
                    type: "GET"
                }).done(function (data) {
                    var sum = 0;
                    $('.data-weight').each(function () {
                        sum += parseFloat($(this).val());
                    });
                    $("#generalInformation-container").html(data);
                });
            },
            events: function () {
                $("#generalInformation-form").validate({
                    submitHandler: function (form, e) {
                        $("#btnGeneralInformationSave").addLoader();
                        e.preventDefault();
                        let formData = new FormData(form);
                        formData.append("FacultyText", $("#Faculty option:selected").text());
                        formData.append("CareerText", $("#Career option:selected").text());
                        $.ajax({
                            url: $(form).attr("action"),
                            type: "POST",
                            data: formData,
                            contentType: false,
                            processData: false,
                            
                        }).done(function () {
                            toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                            convocationForms.generalInformation.load();
                            progress.load();
                            $("#btnGeneralInformationSave").removeLoader();
                        }).fail(function (error) {
                            toastr.error(error.responseText, _app.constants.toastr.title.error);
                            $("#btnGeneralInformationSave").removeLoader();
                        }).always(function () {

                        });
                    }

                });
            },
            init: function () {
                this.load();
                this.events();
            }
        },
        //2
        problemDescription: {
            
            load: function () {
                $.ajax({
                    url: `/investigador/convocatoria/${investigationConvocationPostulantId}/inscripcion?handler=ProblemDescriptionLoad`,
                    type: "GET"
                }).done(function (data) {
                    $("#problemDescription-container").html(data);

                });
            },
            events: function () {
                $("#problemDescription-form").validate({
                    ignore: ".note-editor *",
                    submitHandler: function (form, e) {
                        $("#btnProblemDescriptionSave").addLoader();
                        e.preventDefault();
                        let formData = new FormData(form);
                        $.ajax({
                            url: $(form).attr("action"),
                            type: "POST",
                            data: formData,
                            contentType: false,
                            processData: false,
                        }).done(function () {
                            toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                            convocationForms.problemDescription.load();
                            progress.load();
                            $("#btnProblemDescriptionSave").removeLoader();
                        }).fail(function (error) {
                            toastr.error(error.responseText, _app.constants.toastr.title.error);
                            $("#btnProblemDescriptionSave").removeLoader();
                        }).always(function () {

                        });
                    }

                });
            },

            init: function () {
                this.events();
                
            }
        },
        //3
        markReference: {
            load: function () {
                $.ajax({
                    url: `/investigador/convocatoria/${investigationConvocationPostulantId}/inscripcion?handler=MarkReferenceLoad`,
                    type: "GET"
                }).done(function (data) {
                    $("#markReference-container").html(data);
                });
            },
            events: function () {
                $("#markReference-form").validate({
                    submitHandler: function (form, e) {
                        $("#btnMarkReferenceSave").addLoader();
                        e.preventDefault();
                        let formData = new FormData(form);
                        $.ajax({
                            url: $(form).attr("action"),
                            type: "POST",
                            data: formData,
                            contentType: false,
                            processData: false,
                        }).done(function () {
                            toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                            $("#btnMarkReferenceSave").removeLoader();
                            progress.load();
                        }).fail(function (error) {
                            toastr.error(error.responseText, _app.constants.toastr.title.error);
                            $("#btnMarkReferenceSave").removeLoader();
                        }).always(function () {

                        });
                    }

                });
            },
            init: function () {
                this.events();
            }
        },
        //4
        methodology: {
            load: function () {
                $.ajax({
                    url: `/investigador/convocatoria/${investigationConvocationPostulantId}/inscripcion?handler=MethodologyLoad`,
                    type: "GET"
                }).done(function (data) {
                    $("#methodology-container").html(data);
                });
            },
            events: function () {
                $("#methodology-form").validate({
                    submitHandler: function (form, e) {
                        $("#btnMethodologySave").addLoader();
                        e.preventDefault();
                        let formData = new FormData(form);
                        $.ajax({
                            url: $(form).attr("action"),
                            type: "POST",
                            data: formData,
                            contentType: false,
                            processData: false,
                        }).done(function () {
                            toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                            convocationForms.methodology.load();
                            progress.load();
                            $("#btnMethodologySave").removeLoader();
                        }).fail(function (error) {
                            toastr.error(error.responseText, _app.constants.toastr.title.error);
                            $("#btnMethodologySave").removeLoader();
                        }).always(function () {

                        });
                    }

                });
            },
            init: function () {
                this.events();
            }
        },
        //5
        expectedResult: {
            load: function () {
                $.ajax({
                    url: `/investigador/convocatoria/${investigationConvocationPostulantId}/inscripcion?handler=ExpectedResultLoad`,
                    type: "GET"
                }).done(function (data) {
                    $("#expected-result-container").html(data);
                });
            },
            events: function () {
                $("#expected-result-form").validate({
                    submitHandler: function (form, e) {
                        $("#btnExpectedResultSave").addLoader();
                        e.preventDefault();
                        let formData = new FormData(form);
                        $.ajax({
                            url: $(form).attr("action"),
                            type: "POST",
                            data: formData,
                            contentType: false,
                            processData: false,
                        }).done(function () {
                            toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                            $("#btnExpectedResultSave").removeLoader();
                            progress.load();
                        }).fail(function (error) {
                            toastr.error(error.responseText, _app.constants.toastr.title.error);
                            $("#btnExpectedResultSave").removeLoader();
                        }).always(function () {

                        });
                    }

                });
            },
            init: function () {
                this.events();
            }
        },
        //6
        teamMember: {
            calculateWeight: function() {
                $.ajax({
                    url: `/investigador/convocatoria/${investigationConvocationPostulantId}/inscripcion`.proto().parseURL(),
                    type: "GET",
                    data: {
                        handler: "TeamMemberWeight",
                        InvestigationConvocationPostulantId: investigationConvocationPostulantId
                    },
                }).done(function (result) {
                    $("#team-member-weight").text(`(peso: ${result} %)`);
                    
                });
            },
            load: function () {
                datatable.teamMember.reload();
                convocationForms.teamMember.calculateWeight();
            },
            reset: function () {
                $("#team-member-form select[name=ResearcherUserId]").empty();
                $("#team-member-form select[name=ResearcherUserRoleId]").val(0).trigger("change");                
                $("#team-member-form").validate().resetForm();
                $("#team-member-form input[name=CvFile]").next('.custom-file-label').html("Seleccione archivo");
            },
            events: function () {
                $("#team-member-form").validate({
                    submitHandler: function (form, e) {
                        $("#btnTeamMemberSave").addLoader();
                        e.preventDefault();
                        let formData = new FormData(form);
                        $.ajax({
                            url: $(form).attr("action"),
                            type: "POST",
                            data: formData,
                            contentType: false,
                            processData: false,
                        }).done(function () {
                            toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                            convocationForms.teamMember.reset();
                            convocationForms.teamMember.load();
                            progress.load();
                            $("#btnTeamMemberSave").removeLoader();
                        }).fail(function (error) {
                            toastr.error(error.responseText, _app.constants.toastr.title.error);
                            $("#btnTeamMemberSave").removeLoader();
                        }).always(function () {

                        });
                    }

                });
            },
            init: function () {
                this.events();
            }
        },
        externalMember: {
            calculateWeight: function () {
                $.ajax({
                    url: `/investigador/convocatoria/${investigationConvocationPostulantId}/inscripcion`.proto().parseURL(),
                    type: "GET",
                    data: {
                        handler: "ExternalMemberWeight",
                        InvestigationConvocationPostulantId: investigationConvocationPostulantId
                    },
                }).done(function (result) {
                    $("#external-member-weight").text(`(peso: ${result} %)`);

                });
            },
            load: function () {
                datatable.externalMember.reload();
                convocationForms.externalMember.calculateWeight();
            },
        },
        //7
        annexedFile: {
            load: function () {
                $.ajax({
                    url: `/investigador/convocatoria/${investigationConvocationPostulantId}/inscripcion?handler=AnnexedFileLoad`,
                    type: "GET"
                }).done(function (data) {
                    $('#annexed-file-form input[name=ProjectDuration]').val(data.projectDuration);
                    $('#projectDurationWeight').text(`${data.projectDurationWeight} %`);
                    $('#postulationAttachmentFilesWeight').text(`(peso: ${data.postulationAttachmentFilesWeight} %)`);                    
                });
            },
            reset: function () {

            },
            events: function () {
                $("#annexed-file-form").validate({
                    submitHandler: function (form, e) {
                        $("#btnAnnexedFileSave").addLoader();
                        e.preventDefault();
                        let formData = new FormData(form);
                        $.ajax({
                            url: $(form).attr("action"),
                            type: "POST",
                            data: formData,
                            contentType: false,
                            processData: false,
                        }).done(function () {
                            toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                            convocationForms.annexedFile.load();
                            $("#btnAnnexedFileSave").removeLoader();
                            progress.load();
                        }).fail(function (error) {
                            toastr.error(error.responseText, _app.constants.toastr.title.error);
                            $("#btnAnnexedFileSave").removeLoader();
                        }).always(function () {

                        });
                    }

                });
            },
            init: function () {
                this.events();
            }
        },
        //8
        additionalQuestion: {
            load: function () {

            },
            reset: function () {

            },
            events: function () {
                $("#additional-question-form").validate({
                    submitHandler: function (form, e) {
                        $("#btnAdditionalQuestionSave").addLoader();
                        e.preventDefault();
                        let formData = new FormData(form);
                        $.ajax({
                            url: $(form).attr("action"),
                            type: "POST",
                            data: formData,
                            contentType: false,
                            processData: false,
                        }).done(function () {
                            toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                            $("#btnAdditionalQuestionSave").removeLoader();
                            progress.load();
                        }).fail(function (error) {
                            toastr.error(error.responseText, _app.constants.toastr.title.error);
                            $("#btnAdditionalQuestionSave").removeLoader();
                        }).always(function () {

                        });
                    }

                });
            },
            init: function () {
                this.events();
            }
        },
        init: function () {
            this.markReference.init();
            this.problemDescription.init();
            this.generalInformation.init();
            this.methodology.init();
            this.expectedResult.init();
            this.teamMember.init();
            this.annexedFile.init();
            this.additionalQuestion.init();
        }
    };

    var wizard = {
        object: null,
        load: function () {
            this.object = new mWizard('m_wizard', {
                startStep: 1
            });
        },
        events: {
            onChange: function () {
                $(".m-wizard__step-number").on("click", function () {
                    var step = $(this).data("step");
                    switch (step) {
                        case 1:

                            convocationForms.generalInformation.load();
                            
                            break;
                        case 2:
                            convocationForms.problemDescription.load();
                            break;
                        case 3:
                            convocationForms.markReference.load();
                            break;
                        case 4:
                            convocationForms.methodology.load();
                            break;
                        case 5:
                            convocationForms.expectedResult.load();
                            break;
                        case 6:
                            convocationForms.teamMember.reset();
                            convocationForms.teamMember.calculateWeight();
                            convocationForms.externalMember.calculateWeight();
                            break;
                        case 7:
                            convocationForms.annexedFile.load();
                            break;
                        default:
                    }
                    wizard.object.goTo(step);

                });
            },
            init: function () {
                this.onChange();
            }
        },
        init: function () {
            this.events.init();
            this.load();
        }
    };

    var progress = {
        init: function () {
            this.load();
        },
        load: function () {
            $.ajax({
                url: `/investigador/convocatoria/${investigationConvocationPostulantId}/inscripcion`.proto().parseURL(),
                type: "GET",
                data: {
                    handler: "ProgressPercentage",
                    InvestigationConvocationPostulantId: investigationConvocationPostulantId
                },
            }).done(function (result) {
                $('#progressBarComplete').text(`${result.progressBarPercentage} %`);
                $('#progressBarComplete').css("width",`${result.progressBarPercentage}%`);
                $("#generalInformation-percentage").text(`${result.generalInformationPercentage} %`);
                $('#problemDescription-percentage').text(`${result.problemDescriptionPercentage} %`);
                $('#markReferences-percentage').text(`${result.markReferencePercentage} %`);
                $('#methodology-percentage').text(`${result.methodologyPercentage} %`);
                $('#expectedResult-percentage').text(`${result.expectedResultPercentage} %`);
                $("#team-member-percentage").text(`${result.teamMemberPercentage} %`);
                $("#annexFiles-percentage").text(`${result.annexFilesPercentage} %`);
                $("#additionalQuestions-percentage").text(`${result.additionalQuestionsPercentage} %`);
                
            });
        }
    }

    var summernote = {
        init: function () {
            $(".mv_summernote").summernote({
                lang: "es-ES",
                height: 250,
                toolbar: [
                    ['style', ['style']],
                    ['font', ['bold', 'italic', 'underline', 'strikethrough', 'superscript', 'subscript', 'clear']],
                    ['fontname', ['fontname']],
                    ['fontsize', ['fontsize']],
                    ['color', ['color']],
                    ['para', ['ol', 'ul', 'paragraph', 'height']],
                    ['table', ['table']],
                    ['insert', ['link', 'video', 'hr']],
                    ['view', ['undo', 'redo', 'fullscreen', 'codeview']]
                ]
            });
        }
    };

    return {
        // public functions
        init: function () {
            progress.init();
            modal.init();
            select.init();
            events.init();
            datatable.init();
            wizard.init();
            convocationForms.init();
            summernote.init();
        },
        progressBarCalculate: function () {
            progress.load();
        }
    };

}();

$(function () {
    InitApp.init();
    if ($("#Input_ProjectState").val() == 3 || $("#Input_ProjectState").val() == 2) {
        $(":input").prop("disabled", true);
        $(":button").prop("disabled", true);
    }
})
