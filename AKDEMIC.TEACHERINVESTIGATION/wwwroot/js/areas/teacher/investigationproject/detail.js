var InitApp = function () {
    var investigationProjectId = $("#Input_InvestigationProjectId").val();
    var datatable = {
        investigationProjectTask: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: `/investigador/proyectos/${investigationProjectId}/detalle`.proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        data.handler = "TaskDatatable";
                        data.searchValue = $("#search").val();
                    }
                },
                pageLength: 50,
                orderable: [],
                columns: [
                    {
                        title: "Tarea",
                        data: "taskName"
                    },
                    {
                        title: "Usuario",
                        data: "fullName"
                    },
                    {
                        title: "Fecha",
                        data: "createdAt"
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
                this.object = $("#task-datatable").DataTable(this.options);
                this.events();
            },
            events: function () {
                $("#task-datatable").on('click', '.btn-edit', function () {
                    var id = $(this).data("id");
                    modal.investigationProjectTask.edit.show(id);
                });
                $("#task-datatable").on('click', '.btn-delete', function () {
                    var id = $(this).data("id");
                    modal.investigationProjectTask.delete(id);
                });
            }
        },
        investigationProjectExpense: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: `/investigador/proyectos/${investigationProjectId}/detalle`.proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        data.handler = "ExpenseDatatable";
                        data.searchValue = $("#search").val();
                    }
                },
                pageLength: 50,
                orderable: [],
                columns: [
                    {
                        title: "Descripción",
                        data: "description"
                    },
                    {
                        title: "Gasto",
                        data: "amount"
                    },
                    {
                        title: "Centro de Financiamiento",
                        data: "financingInvestigation"
                    },
                    {
                        title: "Tarea",
                        data: "taskDescription"
                    },
                    {
                        title: "Especifica de producto",
                        data: "expenseCode"
                    },
                    {
                        title: "Tipo de producto",
                        data: "productType"
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
                this.object = $("#expense-datatable").DataTable(this.options);
                this.events();
            },
            events: function () {
                $("#expense-datatable").on('click', '.btn-edit', function () {
                    var id = $(this).data("id");
                    modal.investigationProjectExpense.edit.show(id);
                });
                $("#expense-datatable").on('click', '.btn-delete', function () {
                    var id = $(this).data("id");
                    modal.investigationProjectExpense.delete(id);
                });
            }
        },
        investigationProjectReport: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: `/investigador/proyectos/${investigationProjectId}/detalle`.proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        data.handler = "ReportDatatable";
                        data.searchValue = $("#search").val();
                    }
                },
                pageLength: 50,
                orderable: [],
                columns: [
                    {
                        title: "Entregable",
                        data: "name"
                    },
                    {
                        title: "Fecha de Expiración",
                        data: "expirationDate"
                    },
                    {
                        title: "Archivo",
                        data: null,
                        orderable: false,
                        render: function (data) {
                            var template = "";
                            if (data.reportUrl == "" || data.reportUrl == null) {
                                template += "<span>Sube tu Entregable</span>";
                                return template;
                            }

                            var fileUrl = `/documentos/${data.reportUrl}`.proto().parseURL();
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
                            //template += "<button ";
                            //template += "class='btn btn-info btn-edit ";
                            //template += "m-btn btn-sm  m-btn--icon-only' ";
                            //template += " data-id='" + data.id + "'>";
                            //template += "<i class='la la-edit'></i></button> ";


                            template += "<button ";
                            template += "class='btn btn-accent btn-create ";
                            template += "m-btn btn-sm m-btn--icon' ";
                            template += " data-id='" + data.id + "'>";
                            template += "<span>";
                            template += "<i class='la la-arrow-up'></i> ";
                            template += "<span>Subir Archivo</span>";
                            template += "</span></button>";


                            return template;
                        }
                    }
                ],
            },
            reload: function () {
                this.object.ajax.reload();
            },
            init: function () {
                this.object = $("#projectReport-datatable").DataTable(this.options);
                this.events();
            },
            events: function () {
                $("#projectReport-datatable").on('click', '.btn-create', function () {
                    var id = $(this).data("id");
                    modal.investigationProjectReport.edit.show(id);
                });
            }
        },
        investigationProjectTeamMembers: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: `/investigador/proyectos/${investigationProjectId}/detalle`.proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        data.handler = "ProjectTeamMemberDatatable";
                        data.searchValue = $("#search").val();
                    }
                },
                pageLength: 50,
                orderable: [],
                columns: [
                    {
                        title: "Miembro",
                        data: "fullName"
                    },
                    {
                        title: "Rol",
                        data: "roleName"
                    },
                    {
                        title: "CV",
                        data: null,
                        orderable: false,
                        render: function (data) {
                            var template = "";
                            var fileUrl = `/documentos/${data.cvFilePath}`.proto().parseURL();
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
                this.object = $("#teamMember-datatable").DataTable(this.options);
                this.events();
            },
            events: function () {
                $("#teamMember-datatable").on('click', '.btn-edit', function () {
                    var id = $(this).data("id");
                    modal.investigationProjectTeamMembers.edit.show(id);
                });
                $("#teamMember-datatable").on('click', '.btn-delete', function () {
                    var id = $(this).data("id");
                    modal.investigationProjectTeamMembers.delete(id);
                });
            }
        },
        investigationProjectScientificArticle: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: `/investigador/proyectos/${investigationProjectId}/detalle`.proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        data.handler = "ProjectScientificArticleDatatable";
                        data.searchValue = $("#search").val();
                    }
                },
                pageLength: 50,
                orderable: [],
                columns: [
                    {
                        title: "Titulo del Artículo Científico",
                        data: "title"
                    },
                    {
                        title: "Artículo Científico",
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
                this.object = $("#scientificArticle-datatable").DataTable(this.options);
                this.events();
            },
            events: function () {
                $("#scientificArticle-datatable").on('click', '.btn-edit', function () {
                    var id = $(this).data("id");
                    modal.investigationProjectScientificArticle.edit.show(id);
                });
                $("#scientificArticle-datatable").on('click', '.btn-delete', function () {
                    var id = $(this).data("id");
                    modal.investigationProjectScientificArticle.delete(id);
                });
            }
        },
        init: function () {
            this.investigationProjectTask.init();
            this.investigationProjectExpense.init();
            this.investigationProjectReport.init();
            this.investigationProjectTeamMembers.init();
            this.investigationProjectScientificArticle.init();
        }
    };

    var form = {
        init: function () {
            $("#create-form").validate({
                submitHandler: function (formElement, e) {
                    $("#btnSaveProject").addLoader();
                    e.preventDefault();
                    let formData = new FormData(formElement);
                    $.ajax({
                        url: $(formElement).attr("action"),
                        type: "POST",
                        data: formData,
                        contentType: false,
                        processData: false
                    }).done(function (result) {
                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                        $("#btnSaveProject").removeLoader();
                        if (result.ganttDiagramUrl != null && result.ganttDiagramUrl != "") {
                            $("#create-form").find(".custom-file-label").text("Seleccionar archivo");
                            $("#create-form").find(".custom-file").append(`<div class="form-control-feedback">*Seleccionar en caso sea necesario reemplazar el documento vigente.</div>`);
                            $("#gantt_label").html(`Diagrama de Gant <a href="/documentos/${result.ganttDiagramUrl}" title="Descargar" class="m-widget4__icon custom--document-link" download><i class="la la-download custom--document-download"></i></a>`);
                        }
                        
                    }).fail(function (error) {
                        toastr.error(error.responseText, _app.constants.toastr.title.error);
                        $("#btnSaveProject").removeLoader();
                    }).always(function () {

                    });
                }
            });
        }
    };

    var summernote = {
        init: function () {
            $(".mv_summernote").summernote({
                lang: "es-ES",
                height: 150,
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

    var modal = {
        investigationProjectTask: {
            create: {
                object: $("#investigationProjectTask-form").validate({
                    submitHandler: function (formElement, e) {
                        $("#btnSaveInvestigationProjectTask").addLoader();
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
                            datatable.investigationProjectTask.reload();
                            select.projectTask.load();
                            modal.investigationProjectTask.create.clear();
                            $("#btnSaveInvestigationProjectTask").removeLoader();
                        }).fail(function (error) {
                            toastr.error(error.responseText, _app.constants.toastr.title.error);
                            $("#btnSaveInvestigationProjectTask").removeLoader();
                            //if (error.responseText !== null && error.responseText !== "") $("#add_form_msg_txt").html(error.responseText);
                            //else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                            //$("#add_form_msg").removeClass("m--hide").show();
                        }).always(function () {

                        });
                    }
                }),
                show: function () {
                    $("#btnSaveInvestigationProjectTask").removeLoader();
                    $("#investigationProjectTaskModal").modal("toggle");
                },
                clear: function () {
                    modal.investigationProjectTask.create.object.resetForm();
                },
                events: function () {
                    $("#investigationProjectTaskModal").on("hidden.bs.modal", function () {
                        modal.investigationProjectTask.create.clear();
                    });
                }
            },
            edit: {
                object: $("#investigationProjectTask-editform").validate({
                    submitHandler: function (formElement, e) {
                        $("#btnEditInvestigationProjectTask").addLoader();
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

                            datatable.investigationProjectTask.reload();
                            select.projectTask.load();
                            modal.investigationProjectTask.edit.clear();
                            $("#btnEditInvestigationProjectTask").removeLoader();
                        }).fail(function (error) {
                            toastr.error(error.responseText, _app.constants.toastr.title.error);
                            $("#btnEditInvestigationProjectTask").removeLoader();
                            //if (error.responseText !== null && error.responseText !== "") $("#add_form_msg_txt").html(error.responseText);
                            //else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                            //$("#add_form_msg").removeClass("m--hide").show();
                        }).always(function () {

                        });
                    }
                }),
                load: function (id) {
                    $.ajax({
                        url: `/investigador/proyectos/${investigationProjectId}/detalle`.proto().parseURL(),
                        type: "GET",
                        data: {
                            handler: "DetailTask",
                            Id: id
                        },
                    }).done(function (result) {
                        $("#investigationProjectTask-editform input[name=Description]").val(result.description);
                        $("#investigationProjectTask-editform input[name=Id]").val(result.id);
                        $("#btnEditInvestigationProjectTask").removeLoader();
                    }).fail(function (error) {
                        toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                        $("#btnEditInvestigationProjectTask").removeLoader();
                    }).always(function () {

                    });
                },
                show: function (id) {
                    $("#btnEditInvestigationProjectTask").removeLoader();
                    modal.investigationProjectTask.edit.load(id);
                    $("#investigationProjectTaskEditModal").modal("toggle");
                },
                clear: function () {
                    modal.investigationProjectTask.edit.object.resetForm();
                },
                events: function () {
                    $("#investigationProjectTaskEditModal").on("hidden.bs.modal", function () {
                        modal.investigationProjectTask.edit.clear();
                    });
                }
            },
            delete: function (id) {
                swal({
                    title: "¿Está seguro?",
                    text: "La tarea será eliminada.",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonText: "Sí, eliminarla",
                    confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                    cancelButtonText: "Cancelar",
                    showLoaderOnConfirm: true,
                    preConfirm: () => {
                        return new Promise((resolve) => {
                            $.ajax({
                                url: (`/investigador/proyectos/${investigationProjectId}/detalle?handler=DeleteTask`).proto().parseURL(),
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
                                        text: "La tarea ha sido eliminada con exito",
                                        confirmButtonText: "Excelente"
                                    }).then(datatable.investigationProjectTask.reload()).then(select.projectTask.reload());

                                },
                                error: function (errormessage) {
                                    swal({
                                        type: "error",
                                        title: "Error",
                                        confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                        confirmButtonText: "Entendido",
                                        text: "La tarea presenta información relacionada"
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
        investigationProjectExpense: {
            create: {
                object: $("#investigationProjectExpense-form").validate({
                    submitHandler: function (formElement, e) {
                        $("#btnSaveInvestigationProjectExpense").addLoader();
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
                            datatable.investigationProjectExpense.reload();
                            modal.investigationProjectExpense.create.clear();
                            $("#btnSaveInvestigationProjectExpense").removeLoader();
                        }).fail(function (error) {
                            toastr.error(error.responseText, _app.constants.toastr.title.error);
                            $("#btnSaveInvestigationProjectExpense").removeLoader();
                            //if (error.responseText !== null && error.responseText !== "") $("#add_form_msg_txt").html(error.responseText);
                            //else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                            //$("#add_form_msg").removeClass("m--hide").show();
                        }).always(function () {

                        });
                    }
                }),
                show: function () {
                    $("#btnSaveInvestigationProjectExpense").removeLoader();
                    $("#investigationProjectExpenseModal").modal("toggle");
                },
                clear: function () {
                    modal.investigationProjectExpense.create.object.resetForm();
                },
                events: function () {
                    $("#investigationProjectExpenseModal").on("hidden.bs.modal", function () {
                        modal.investigationProjectExpense.create.clear();
                    });
                }
            },
            edit: {
                object: $("#investigationProjectExpense-editform").validate({
                    submitHandler: function (formElement, e) {
                        $("#btnEditInvestigationProjectExpense").addLoader();
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

                            datatable.investigationProjectExpense.reload();
                            modal.investigationProjectExpense.edit.clear();
                            $("#btnEditInvestigationProjectExpense").removeLoader();
                        }).fail(function (error) {
                            toastr.error(error.responseText, _app.constants.toastr.title.error);
                            $("#btnEditInvestigationProjectExpense").removeLoader();
                            //if (error.responseText !== null && error.responseText !== "") $("#add_form_msg_txt").html(error.responseText);
                            //else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                            //$("#add_form_msg").removeClass("m--hide").show();
                        }).always(function () {

                        });
                    }
                }),
                load: function (id) {
                    $.ajax({
                        url: `/investigador/proyectos/${investigationProjectId}/detalle`.proto().parseURL(),
                        type: "GET",
                        data: {
                            handler: "DetailExpense",
                            Id: id
                        },
                    }).done(function (result) {
                        $("#investigationProjectExpense-editform input[name=Description]").val(result.description);
                        $("#investigationProjectExpense-editform input[name=Amount]").val(result.amount);
                        $("#investigationProjectExpense-editform input[name=Id]").val(result.id);
                        $("#investigationProjectExpense-editform input[name=ExpenseCode]").val(result.expenseCode);
                        $("#investigationProjectExpense-editform input[name=ProductType]").val(result.productType);
                        $("#investigationProjectExpense-editform select[name=InvestigationProjectTaskId]").val(result.investigationProjectTaskId).trigger("change");
                    }).fail(function (error) {
                        toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                    }).always(function () {

                    });
                },
                show: function (id) {
                    $("#btnEditInvestigationProjectExpense").removeLoader();
                    modal.investigationProjectExpense.edit.load(id);
                    $("#investigationProjectExpenseEditModal").modal("toggle");
                },
                clear: function () {
                    modal.investigationProjectExpense.edit.object.resetForm();
                },
                events: function () {
                    $("#investigationProjectExpenseEditModal").on("hidden.bs.modal", function () {
                        modal.investigationProjectExpense.edit.clear();
                    });
                }
            },
            delete: function (id) {
                swal({
                    title: "¿Está seguro?",
                    text: "El gasto será eliminado.",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonText: "Sí, eliminarlo",
                    confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                    cancelButtonText: "Cancelar",
                    showLoaderOnConfirm: true,
                    preConfirm: () => {
                        return new Promise((resolve) => {
                            $.ajax({
                                url: (`/investigador/proyectos/${investigationProjectId}/detalle?handler=DeleteExpense`).proto().parseURL(),
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
                                        text: "El gasto ha sido eliminado con exito",
                                        confirmButtonText: "Excelente"
                                    }).then(datatable.investigationProjectExpense.reload());
                                },
                                error: function (errormessage) {
                                    swal({
                                        type: "error",
                                        title: "Error",
                                        confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                        confirmButtonText: "Entendido",
                                        text: "El gasto presenta información relacionada"
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
        investigationProjectReport: {
            edit: {
                object: $("#investigationProjectReport-editform").validate({
                    submitHandler: function (formElement, e) {
                        $("#btnEditInvestigationProjectReport").addLoader();
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

                            datatable.investigationProjectReport.reload();
                            modal.investigationProjectReport.edit.clear();
                            $("#btnEditInvestigationProjectReport").removeLoader();
                        }).fail(function (error) {
                            toastr.error(error.responseText, _app.constants.toastr.title.error);
                            $("#btnEditInvestigationProjectReport").removeLoader();
                            //if (error.responseText !== null && error.responseText !== "") $("#add_form_msg_txt").html(error.responseText);
                            //else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                            //$("#add_form_msg").removeClass("m--hide").show();
                        }).always(function () {

                        });
                    }
                }),
                load: function (id) {
                    $.ajax({
                        url: `/investigador/proyectos/${investigationProjectId}/detalle`.proto().parseURL(),
                        type: "GET",
                        data: {
                            handler: "DetailUploadprojectreportDocumentFile",
                            Id: id
                        },
                    }).done(function (result) {
                        $("#investigationProjectReport-editform input[name=Id]").val(result.id);
                        $("#investigationProjectReport-editform input[name=Name]").val(result.name);
                        $("#investigationProjectReport-editform input[name=ExpirationDate]").val(result.expirationDate);
                        $("#investigationProjectReport-editform").find(".custom-file-label").text("Seleccionar archivo");
                        $("#investigationProjectReport-editform").find(".custom-file").append('<div class="form-control-feedback">*Seleccionar en caso sea necesario reemplazar el documento vigente.</div>');
                    }).fail(function (error) {
                        toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                        //if (error.responseText !== null && error.responseText !== "") $("#add_form_msg_txt").html(error.responseText);
                        //else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                        //$("#add_form_msg").removeClass("m--hide").show();
                    }).always(function () {

                    });
                },
                show: function (id) {
                    $("#btnEditInvestigationProjectReport").removeLoader();
                    modal.investigationProjectReport.edit.load(id);
                    $("#investigationProjectReportEditModal").modal("toggle");
                },
                clear: function () {
                    $("#investigationProjectReport-editform").find(".custom-file-label").text("Seleccionar archivo");
                    modal.investigationProjectReport.edit.object.resetForm();
                },
                events: function () {
                    $("#investigationProjectReportEditModal").on("hidden.bs.modal", function () {
                        modal.investigationProjectReport.edit.clear();
                    });
                }
            },
            init: function () {
                this.edit.events();
            }
        },
        investigationProjectTeamMembers: {
            create: {
                object: $("#investigationProjectTeamMember-form").validate({
                    submitHandler: function (formElement, e) {
                        $("#btnSaveInvestigationProjectTeamMember").addLoader();
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
                            datatable.investigationProjectTeamMembers.reload();
                            modal.investigationProjectTeamMembers.create.clear
                            $("#btnSaveInvestigationProjectTeamMember").removeLoader();
                        }).fail(function (error) {
                            toastr.error(error.responseText, _app.constants.toastr.title.error);
                            $("#btnSaveInvestigationProjectTeamMember").removeLoader();
                            //if (error.responseText !== null && error.responseText !== "") $("#add_form_msg_txt").html(error.responseText);
                            //else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                            //$("#add_form_msg").removeClass("m--hide").show();
                        }).always(function () {

                        });
                    }
                }),
                show: function () {
                    $("#btnSaveInvestigationProjectTeamMember").removeLoader();
                    $("#investigationProjectTeamMember").modal("toggle");
                },
                clear: function () {
                    modal.investigationProjectTeamMembers.create.object.resetForm();
                    $("#investigationProjectTeamMember-form").find(".custom-file-label").text("Seleccionar archivo");
                },
                events: function () {
                    $("#investigationProjectTeamMember").on("hidden.bs.modal", function () {
                        modal.investigationProjectTeamMembers.create.clear();
                    });
                }
            },
            edit: {
                object: $("#investigationProjectProjectTeamMember-editform").validate({
                    submitHandler: function (formElement, e) {
                        $("#btnEditInvestigationProjectTeamMember").addLoader();
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
                            datatable.investigationProjectTeamMembers.reload();
                            modal.edit.clear();
                        }).fail(function (error) {
                            toastr.error(error.responseText, _app.constants.toastr.title.error);
                            $("#btnEditInvestigationProjectTeamMember").removeLoader();
                            //if (error.responseText !== null && error.responseText !== "") $("#add_form_msg_txt").html(error.responseText);
                            //else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                            //$("#add_form_msg").removeClass("m--hide").show();
                        }).always(function () {

                        });
                    }
                }),
                load: function (id) {
                    $.ajax({
                        url: `/investigador/proyectos/${investigationProjectId}/detalle`.proto().parseURL(),
                        type: "GET",
                        data: {
                            handler: "DetailProjectTeamMembers",
                            id: id
                        },
                    }).done(function (result) {
                        $("#investigationProjectProjectTeamMember-editform input[name=Id]").val(result.id);
                        $("#investigationProjectProjectTeamMember-editform input[name=InvestigationProjectId]").val(result.investigationProjectId);
                        $("#investigationProjectProjectTeamMember-editform input[name=FullName]").val(result.fullName);
                        $("#investigationProjectProjectTeamMember-editform select[name=TeamMemberRoleId]").val(result.teamMemberRoleId).trigger("change");
                        $("#investigationProjectProjectTeamMember-editform").find(".custom-file-label").text("Seleccionar archivo");
                        $("#investigationProjectProjectTeamMember-editform").find(".custom-file").append('<div class="form-control-feedback">*Seleccionar en caso sea necesario reemplazar el documento vigente.</div>');
                        $("#investigationProjectProjectTeamMember-editform textarea[name=Objectives]").summernote('code',result.objectives);
                    }).fail(function (error) {
                        toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                        if (error.responseText !== null && error.responseText !== "") $("#add_form_msg_txt").html(error.responseText);
                        else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                        $("#add_form_msg").removeClass("m--hide").show();
                    }).always(function () {

                    });

                },
                show: function (id) {
                    $("#btnEditInvestigationProjectTeamMember").removeLoader();
                    modal.investigationProjectTeamMembers.edit.load(id);
                    $("#investigationProjectProjectEditTeamMemberModal").modal("toggle");
                },
                clear: function () {
                    modal.investigationProjectTeamMembers.edit.object.resetForm();
                },
                events: function () {
                    $("#investigationProjectProjectEditTeamMemberModal").on("hidden.bs.modal", function () {
                        modal.investigationProjectTeamMembers.edit.clear();
                    });
                }
            },
            delete: function (id) {
                swal({
                    title: "¿Está seguro?",
                    text: "El miembro de equipo será eliminado.",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonText: "Sí, eliminarlo",
                    confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                    cancelButtonText: "Cancelar",
                    showLoaderOnConfirm: true,
                    preConfirm: () => {
                        return new Promise((resolve) => {
                            $.ajax({
                                url: (`/investigador/proyectos/${investigationProjectId}/detalle?handler=DeleteProjectTeamMembers`).proto().parseURL(),
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
                                        text: "El miembro de equipo ha sido eliminado con exito",
                                        confirmButtonText: "Excelente"
                                    }).then(datatable.investigationProjectTeamMembers.reload());
                                },
                                error: function (errormessage) {
                                    swal({
                                        type: "error",
                                        title: "Error",
                                        confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                        confirmButtonText: "Entendido",
                                        text: "El miembro de equipo presenta información relacionada"
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
        investigationProjectFinalDocument: {
            create: {
                object: $("#finalDocument-form").validate({
                    submitHandler: function (formElement, e) {
                        $("#btnSaveFinalDocument").addLoader();
                        e.preventDefault();
                        let formData = new FormData(formElement);
                        $.ajax({
                            url: $(formElement).attr("action"),
                            type: "POST",
                            data: formData,
                            contentType: false,
                            processData: false
                        }).done(function (result) {
                            $(".modal").modal("hide");
                            $(".m-alert").addClass("m--hide");
                            toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                            $("#btnSaveFinalDocument").removeLoader();
                            window.location.href = window.location.href;
                        }).fail(function (error) {
                            toastr.error(error.responseText, _app.constants.toastr.title.error);
                            $("#btnSaveFinalDocument").removeLoader();
                            //if (error.responseText !== null && error.responseText !== "") $("#add_form_msg_txt").html(error.responseText);
                            //else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                            //$("#add_form_msg").removeClass("m--hide").show();
                        }).always(function () {

                        });
                    }
                }),
                show: function () {
                    $("#btnSaveFinalDocument").removeLoader();
                    $("#finalDocumentModal").modal("toggle");
                },
                clear: function () {
                    modal.investigationProjectFinalDocument.create.object.resetForm();
                },
                events: function () {
                    $("#finalDocumentModal").on("hidden.bs.modal", function () {
                        modal.investigationProjectFinalDocument.create.clear();
                    });
                }
            },
            init: function () {
                this.create.events();
               
            }
        },
        investigationProjectScientificArticle: {
            create: {
                object: $("#investigationProjectScientificArticle-form").validate({
                    submitHandler: function (formElement, e) {
                        $("#btnSaveInvestigationProjectScientificArticle").addLoader();
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
                            datatable.investigationProjectScientificArticle.reload();
                            modal.investigationProjectScientificArticle.create.clear
                            $("#btnSaveInvestigationProjectScientificArticle").removeLoader();
                        }).fail(function (error) {
                            toastr.error(error.responseText, _app.constants.toastr.title.error);
                            $("#btnSaveInvestigationProjectScientificArticle").removeLoader();
                            //if (error.responseText !== null && error.responseText !== "") $("#add_form_msg_txt").html(error.responseText);
                            //else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                            //$("#add_form_msg").removeClass("m--hide").show();
                        }).always(function () {

                        });
                    }
                }),
                show: function () {
                    $("#btnSaveInvestigationProjectScientificArticle").removeLoader();
                    $("#investigationProjectScientificArticle").modal("toggle");
                },
                clear: function () {
                    modal.investigationProjectScientificArticle.create.object.resetForm();
                    $("#investigationProjectScientificArticle-form").find(".custom-file-label").text("Seleccionar archivo");
                },
                events: function () {
                    $("#investigationProjectScientificArticle").on("hidden.bs.modal", function () {
                        modal.investigationProjectScientificArticle.create.clear();
                    });
                }
            },
            edit: {
                object: $("#investigationProjectProjectScientificArticle-editform").validate({
                    submitHandler: function (formElement, e) {
                        $("#btnEditInvestigationProjectScientificArticle").addLoader();
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
                            datatable.investigationProjectScientificArticle.reload();
                            modal.edit.clear();
                        }).fail(function (error) {
                            toastr.error(error.responseText, _app.constants.toastr.title.error);
                            $("#btnEditInvestigationProjectScientificArticle").removeLoader();
                            //if (error.responseText !== null && error.responseText !== "") $("#add_form_msg_txt").html(error.responseText);
                            //else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                            //$("#add_form_msg").removeClass("m--hide").show();
                        }).always(function () {

                        });
                    }
                }),
                load: function (id) {
                    $.ajax({
                        url: `/investigador/proyectos/${investigationProjectId}/detalle`.proto().parseURL(),
                        type: "GET",
                        data: {
                            handler: "DetailProjectScientificArticle",
                            id: id
                        },
                    }).done(function (result) {
                        $("#investigationProjectProjectScientificArticle-editform input[name=Id]").val(result.id);
                        $("#investigationProjectProjectScientificArticle-editform input[name=Title]").val(result.title);
                        $("#investigationProjectProjectScientificArticle-editform input[name=InvestigationProjectId]").val(result.investigationProjectId);
                        $("#investigationProjectProjectScientificArticle-editform").find(".custom-file-label").text("Seleccionar archivo");
                        $("#investigationProjectProjectScientificArticle-editform").find(".custom-file").append('<div class="form-control-feedback">*Seleccionar en caso sea necesario reemplazar el documento vigente.</div>');
                        
                    }).fail(function (error) {
                        toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                        if (error.responseText !== null && error.responseText !== "") $("#add_form_msg_txt").html(error.responseText);
                        else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                        $("#add_form_msg").removeClass("m--hide").show();
                    }).always(function () {

                    });

                },
                show: function (id) {
                    $("#btnEditInvestigationProjectScientificArticle").removeLoader();
                    modal.investigationProjectScientificArticle.edit.load(id);
                    $("#investigationProjectProjectEditScientificArticle").modal("toggle");
                },
                clear: function () {
                    modal.investigationProjectScientificArticle.edit.object.resetForm();
                },
                events: function () {
                    $("#investigationProjectProjectEditScientificArticle").on("hidden.bs.modal", function () {
                        modal.investigationProjectScientificArticle.edit.clear();
                    });
                }
            },
            delete: function (id) {
                swal({
                    title: "¿Está seguro?",
                    text: "El Articulo Cientifico será eliminado.",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonText: "Sí, eliminarlo",
                    confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                    cancelButtonText: "Cancelar",
                    showLoaderOnConfirm: true,
                    preConfirm: () => {
                        return new Promise((resolve) => {
                            $.ajax({
                                url: (`/investigador/proyectos/${investigationProjectId}/detalle?handler=DeleteProjectScientificArticle`).proto().parseURL(),
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
                                        text: "El Articulo Cientifico ha sido eliminado con exito",
                                        confirmButtonText: "Excelente"
                                    }).then(datatable.investigationProjectScientificArticle.reload());
                                },
                                error: function (errormessage) {
                                    swal({
                                        type: "error",
                                        title: "Error",
                                        confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                        confirmButtonText: "Entendido",
                                        text: "El Articulo Cientifico presenta información relacionada"
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
            this.investigationProjectTask.init();
            this.investigationProjectExpense.init();
            this.investigationProjectReport.init();
            this.investigationProjectTeamMembers.init();
            this.investigationProjectFinalDocument.init();
            this.investigationProjectScientificArticle.init();

        }
    };
    var select = {
        init: function () {
            this.researcherUser.init();
            this.researcherUserRole.init();
            this.projectType.init();
            this.projectTask.init();
        },
        researcherUser: {
            init: function () {
                $("#investigationProjectTeamMember-form select[name=UserId]").select2({
                    dropdownParent: $("#investigationProjectTeamMember"),
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
                $("#investigationProjectProjectTeamMember-editform select[name=UserId]").select2({
                    dropdownParent: $("#investigationProjectProjectEditTeamMemberModal"),
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
                    $('#investigationProjectTeamMember-form select[name="TeamMemberRoleId"]').select2({
                        data: result,
                    });
                    $('#investigationProjectProjectTeamMember-editform select[name="TeamMemberRoleId"]').select2({
                        data: result,
                    });
                });
            }
        },
        projectType: {
            init: function () {
                this.load();
            },
            load: function () {
                $("#Input_InvestigationProjectTypeId").select2();
                $.ajax({
                    url: (`/api/tiposdeproyecto/select/get`).proto().parseURL(),
                    type: "GET"
                }).done(function (result) {
                    $("#Input_InvestigationProjectTypeId").select2({
                        data: result,
                    });
                });
            },
        },
        projectTask: {
            init: function () {
                this.load();
            },
            reload: function () {
                $('#investigationProjectExpense-form select[name="InvestigationProjectTaskId"]').html('<option value="0" selected disabled>Seleccione Tarea</option>');
                $('#investigationProjectExpense-editform select[name="InvestigationProjectTaskId"]').html('<option value="0" selected disabled>Seleccione Tarea</option>');
                this.load();
            },
            load: function () {
                $.ajax({
                    url: (`/api/tareas/${investigationProjectId}/select/get`).proto().parseURL(),
                    type: "GET"
                }).done(function (result) {
                    $('#investigationProjectExpense-form select[name="InvestigationProjectTaskId"]').select2({
                        data: result,
                        dropdownParent: $("#investigationProjectExpenseModal")
                    });
                    $('#investigationProjectExpense-editform select[name="InvestigationProjectTaskId"]').select2({
                        data: result,
                        dropdownParent: $("#investigationProjectExpenseEditModal")
                    });
                });
            },
        }
    };


    return {
        init: function () {
            datatable.init();
            modal.init();
            select.init();
            summernote.init();
            form.init();

            //SummernoteIgnore validator
            $('form').each(function () {
                if ($(this).data('validator'))
                    $(this).data('validator').settings.ignore = ".note-editor *";
            });
        }
    }
}();

$(function () {
    InitApp.init();
})