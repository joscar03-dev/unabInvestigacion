var InitApp = function () {
    var selectfacultad = {
        init: function () {
            this.researcherMaestroFacultad.init();
        },
        researcherMaestroFacultad: {
            init: function () {
                this.load();
            },
            load: function () {
                $.ajax({
                    url: (`/api/maestrofacultad/select/get`).proto().parseURL(),
                    type: "GET"
                }).done(function (result) {
                    $('.input_IdFacultad').select2({
                        data: result,
                    });
                });
            }
        },
    };
    var selectacategoriaconvocatoria = {
        init: function () {
            this.researcherMaestroCategoriaconvocatoria.init();
        },
        researcherMaestroCategoriaconvocatoria: {
            init: function () {
                this.load();
            },
            load: function () {
                $.ajax({
                    url: (`/api/maestrocategoriaconvocatoria/select/get`).proto().parseURL(),
                    type: "GET"
                }).done(function (result) {
                    $('.input_IdCategoriaconvocatoria').select2({
                        data: result,
                        maximumSelectionLength: 1,

                    });
                });
            }
        },
    };
    var selecttipoconvocatoria = {
        init: function () {
            this.researcherMaestroTipoconvocatoria.init();
        },
        researcherMaestroTipoconvocatoria: {
            init: function () {
                this.load();
            },
            load: function () {
                $.ajax({
                    url: (`/api/maestrotipoconvocatoria/select/get`).proto().parseURL(),
                    type: "GET",
                    data: "idoficina=5628b2c0-9a3d-11ee-b7b1-16d13ee00159",
                }).done(function (result) {
                    $('.input_IdTipoconvocatoria').select2({
                        data: result,
                        maximumSelectionLength: 1,

                    });
                });
            }
        },
    };
    var selectflujo = {
        init: function () {
            this.researcherInvestigacionfomentoFlujo.init();
        },
        researcherInvestigacionfomentoFlujo: {
            init: function () {
                this.load();
            },
            load: function () {
                $.ajax({
                    url: (`/api/investigacionfomentoflujo/select/get`).proto().parseURL(),
                    type: "GET",
                    data: "idoficina=5628b2c0-9a3d-11ee-b7b1-16d13ee00159",
                }).done(function (result) {
                    $('.input_IdFlujo').select2({
                        data: result,
                        maximumSelectionLength: 1,

                    });
                });
            }
        },
    };
    var selectrequisito = {
        init: function () {
            this.researcherInvestigacionfomentoRequisito.init();
        },
        researcherInvestigacionfomentoRequisito: {
            init: function () {
                this.load();
            },
            load: function () {
                $.ajax({
                    url: (`/api/investigacionfomentorequisito/select/get`).proto().parseURL(),
                    type: "GET",
                    data: "idoficina=5628b2c0-9a3d-11ee-b7b1-16d13ee00159",
                }).done(function (result) {
                    $('.input_IdRequisito').select2({
                        data: result,
                        maximumSelectionLength: 1,

                    });
                });
            }
        },
    };
    var selectlistaverificacion = {
        init: function () {
            this.researcherInvestigacionfomentoListaverificacion.init();
        },
        researcherInvestigacionfomentoListaverificacion: {
            init: function () {
                this.load();
            },
            load: function () {
                $.ajax({
                    url: (`/api/investigacionfomentolistaverificacion/select/get`).proto().parseURL(),
                    type: "GET",
                    data: "idoficina=5628b2c0-9a3d-11ee-b7b1-16d13ee00159",
                }).done(function (result) {
                    $('.input_IdListaverificacion').select2({
                        data: result,
                        maximumSelectionLength: 1,

                    });
                });
            }
        },
    };
    var selectarea = {
        init: function () {
            this.researcherInvestigacionfomentoArea.init();
        },
        researcherInvestigacionfomentoArea: {
            init: function () {
                this.load();
            },
            load: function (idconvocatoria, Id) {
                $.ajax({
                    url: (`/api/investigacionfomentoconvocatoriaarea/select/get`).proto().parseURL(),
                    type: "GET",
                    data: (idconvocatoria ? (idconvocatoria == '' ? '' : "idconvocatoria=" + idconvocatoria) : ''),
                }).done(function (result) {


                    $('#nuevoarealistaverificaciones-form select[name=IdArea]').select2({
                        data: result,
                        maximumSelectionLength: 1,

                    });
                    if (Id) {
                        $('#nuevoarealistaverificaciones-form select[name=IdArea]').val(Id);
                    }
                    $('#nuevoarealistaverificaciones-form select[name=IdArea]').trigger("change");
                });
            }
        },
    };
    var selectareaanexo = {
        init: function () {
            this.researcherInvestigacionfomentoAreaanexo.init();
        },
        researcherInvestigacionfomentoAreaanexo: {
            init: function () {
                this.load();
            },
            load: function (idconvocatoria, Id) {
                $.ajax({
                    url: (`/api/investigacionfomentoconvocatoriaareaanexo/select/get`).proto().parseURL(),
                    type: "GET",
                    data: (idconvocatoria ? (idconvocatoria == '' ? '' : "idconvocatoria=" + idconvocatoria) : ''),
                }).done(function (result) {

                 
                    $('#nuevoarearequisitos-form select[name=IdArea]').select2({
                        data: result,
                        maximumSelectionLength: 1,

                    });
                    if (Id) {
                        $('#nuevoarearequisitos-form select[name=IdArea]').val(Id);
                    }
                    $('#nuevoarearequisitos-form select[name=IdArea]').trigger("change");
                });
            }
        },
    };

    
    var datatable = {
        investigacionfomentoConvocatoria: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: "/InvestigacionIncubadoranew/convocatoria".proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                 
                    data: function (data) {
                        data.handler = "Datatable";
                        data.searchValue = $("#search").val();
                        data.tipoplan = $("#tipoplan").val();
                    }
                },
                pageLength: 50,
                orderable: [],
                columns: [

                    {
                        title: "Categoria",
                        data: "nombrecategoria"
                    },
                    {
                        title: "Tipo",
                        data: "nombretipo",
                    },
                    {
                        title: "nombre",
                        data: "nombre"
                    },
                   
                    {
                        title: "Fecha Inicial",
                        data: "fechaini"
                    },

                    {
                        title: "Fecha Final",
                        data: "fechafin"
                    },
                    {
                        title: "Estado",
                        data: null,
                        width: 100,
                        render: function (data) {
                           

                            var estado = data.estado == 0 ? "INACTIVO" : data.estado == 1 ? "ACTIVO" : data.estado == 2 ? "APROBADO COMITE" : "OBSERVADO COMITE";
                            var label = data.estado == 0 ? "label-warning" : data.estado == 1 ? "label-success" : data.estado == 2 ? "label-success" : "label-danger"; // 
                            var template = "";
                            template += "<span class='btn m-btn btn-sm m-btn--"+ label +"'>";
                            template += estado + " " + (data.estado == 3 ? btnsearch : "");
                            template += "</span> ";
                            return template;
                        }
                    },
                    {
                        title: "Opciones",
                        data: null,
                        width: 170,
                        orderable: false,
                        render: function (data) {
                            var template = "";

                            //Lista de verificacion
                            template += "<button title='Registrar Lista verificación x Area'  style='color:#ffffff;background-color:#990000' ";
                            template += " class='btn  btn-arealistaverificacion ";
                            template += " m-btn btn-sm  m-btn--icon-only' ";
                            template += " data-id='" + data.id + "' > ";
                            template += "<span><i class='la la-book'></i><span> Lista de Verificación</span></span></button>";


                            //requisito
                            template += "<button title='Registrar Requisitos x Area'  style='background-color:#aaeee8' ";
                            template += " class='btn  btn-arearequisito ";
                            template += " m-btn btn-sm  m-btn--icon-only' ";
                            template += " data-id='" + data.id + "' > ";
                            template += "<span><i class='la la-bar-chart-o'></i><span> Requisitos</span></span></button>";

                                  //Edit
                                template += "<button title='Editar' ";
                                template += "class='btn btn-info btn-edit ";
                                template += "m-btn btn-sm  m-btn--icon-only' ";
                                template += " data-id='" + data.id + "'>";
                                template += "<i class='la la-edit'></i></button>";

                                //Delete
                                template += "<button title='Eliminar'";
                                template += "class='btn btn-danger btn-delete ";
                                template += "m-btn btn-sm  m-btn--icon-only' ";
                                template += " data-id='" + data.id + "'>";
                                template += "<i class='la la-trash'></i></button>";
                            
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
                    modal.edit.show(id);
                });

              

                $("#data-table").on('click', '.btn-delete', function () {
                    var id = $(this).data("id");
                    modal.delete(id);

                });

                $("#data-table").on('click', '.btn-arearequisito', function () {
                  
                    var id = $(this).data("id");
                    modal.addarearequisito.show(id);
                });

                $("#data-table").on('click', '.btn-arealistaverificacion', function () {

                    var id = $(this).data("id");
                    modal.addarealistaverificacion.show(id);
                });
                
            }
        },
        init: function () {
            this.investigacionfomentoConvocatoria.init();
        }
    };
    var search = {
        init: function () {
            $("#search").doneTyping(function () {
                datatable.investigacionfomentoConvocatoria.reload();
            });
        }
    };
    var modal = {
        addarearequisito: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: "/InvestigacionIncubadoranew/convocatoria".proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        data.handler = "DatatableArearequisito";
                        data.id = $("#addarearequisitos-form input[name=Id]").val();
                    }
                },
                pageLength: 50,
                orderable: [],
                columns: [

                    {
                        title: "Area",
                        data: "nombrearea"
                    },
                    {
                        title: "Requisito",
                        data: "nombrerequisito"
                    },
                    {
                        title: "Anexo a subir",
                        data: null,
                        orderable: false,
                        render: function (data) {
                            var template = "";
                            var fileUrl = `/documentos/${data.archivourl}`.proto().parseURL();
                            //FileURL
                            if (data.archivourl != '') {
                                template += `<a href="${fileUrl}"  `;
                                template += "class='btn btn-success ";
                                template += "m-btn btn-sm m-btn--icon' download>";
                                template += "<span><i class='flaticon-file'></i></a> ";
                            }
                            return template;
                        }
                    },
                    {
                        title: "Estado",
                        data: null,
                        width: 50,
                        render: function (data) {
                            var estado = data.activo == 1 ? "ACTIVO" : "ACTIVO";
                            var label = data.activo == 1 ? "label-success" : "label-success";
                            var template = "";
                            template += "<span class='btn m-btn btn-sm m-btn--" + label + "'>";
                            template += estado;
                            template += "</span> ";
                            return template;
                        }
                    },
                    {
                        title: "Opciones",
                        data: null,
                        width: 40,
                        orderable: false,
                        render: function (data) {
                            var template = "";



                            //Edit
                            template += "<button style='display:none' title='Editar'  type='button'  ";
                            template += "class='btn btn-info btn-editarearequisito ";
                            template += "m-btn btn-sm  m-btn--icon-only' ";
                            template += " data-id='" + data.id + "'>";
                            template += "<i class='la la-edit'></i></button>";

                            //Delete
                            template += "<button type='button' ";
                            template += "class='btn btn-danger btn-deletearearequisito ";
                            template += "m-btn btn-sm  m-btn--icon-only' ";
                            template += " data-id='" + data.id + "'>";
                            template += "<i class='la la-trash'></i></button> ";




                            return template;
                        }
                    }
                ],

            },
            load: function (id) {
                $("#addarearequisitos-form input[name=Id]").val(id);
                $("#nuevoarearequisitos-form input[name=IdConvocatoria]").val(id);
                selectareaanexo.researcherInvestigacionfomentoAreaanexo.load(id);
                if (this.object == null) {
                    this.object = $("#data-tablearearequisitos").DataTable(this.options);
                } else {
                    this.reload();
                }
                modal.addarearequisito.events();
              

            },
            reload: function () {
                this.object.ajax.reload();
                
            },
            show: function (id) {
                modal.addarearequisito.load(id);
                $("#addModalArearequisitos").modal("toggle");
            },
            clear: function () {
                // modal.addarearequisito.object.resetForm();
            },
            events: function () {

                $("#addModalArearequisitos").on("hidden.bs.modal", function () {
                    modal.addarearequisito.clear();
                });
                $("#addModalArearequisitos").on("hidden.bs.modal", function () {
                    modal.addarearequisito.clear();
                });


                $("#data-tablearearequisitos").on('click', '.btn-editarearequisito', function () {
                    var id = $(this).data("id");
                    modal.editarearequisito.show(id);
                });

                $("#data-tablearearequisitos").on('click', '.btn-deletearearequisito', function () {
                    var id = $(this).data("id");
                    modal.deletearearequisito(id);

                });



            }
        },
        addarealistaverificacion: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: "/InvestigacionIncubadoranew/convocatoria".proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        data.handler = "DatatableArealistaverificacion";
                        data.id = $("#addarealistaverificaciones-form input[name=Id]").val();
                    }
                },
                pageLength: 50,
                orderable: [],
                columns: [

                    {
                        title: "Area",
                        data: "nombrearea"
                    },
                    {
                        title: "Lista de verificación",
                        data: "nombrelistaverificacion"
                    },
                    {
                        title: "Estado",
                        data: null,
                        width: 50,
                        render: function (data) {
                            var estado = data.activo == 1 ? "ACTIVO" : "ACTIVO";
                            var label = data.activo == 1 ? "label-success" : "label-success";
                            var template = "";
                            template += "<span class='btn m-btn btn-sm m-btn--" + label + "'>";
                            template += estado;
                            template += "</span> ";
                            return template;
                        }
                    },
                    {
                        title: "Opciones",
                        data: null,
                        width: 40,
                        orderable: false,
                        render: function (data) {
                            var template = "";



                            //Edit
                            template += "<button style='display:none' title='Editar'  type='button'  ";
                            template += "class='btn btn-info btn-editarealistaverificacion ";
                            template += "m-btn btn-sm  m-btn--icon-only' ";
                            template += " data-id='" + data.id + "'>";
                            template += "<i class='la la-edit'></i></button>";

                            //Delete
                            template += "<button type='button' ";
                            template += "class='btn btn-danger btn-deletearealistaverificacion ";
                            template += "m-btn btn-sm  m-btn--icon-only' ";
                            template += " data-id='" + data.id + "'>";
                            template += "<i class='la la-trash'></i></button> ";




                            return template;
                        }
                    }
                ],

            },
            load: function (id) {
                $("#addarealistaverificaciones-form input[name=Id]").val(id);
                $("#nuevoarealistaverificaciones-form input[name=IdConvocatoria]").val(id);
                selectarea.researcherInvestigacionfomentoArea.load(id);
                if (this.object == null) {
                    this.object = $("#data-tablearealistaverificaciones").DataTable(this.options);
                } else {
                    this.reload();
                }
                modal.addarealistaverificacion.events();


            },
            reload: function () {
                this.object.ajax.reload();

            },
            show: function (id) {
                modal.addarealistaverificacion.load(id);
                $("#addModalArealistaverificaciones").modal("toggle");
            },
            clear: function () {
                // modal.addarearequisito.object.resetForm();
            },
            events: function () {

                $("#addModalArealistaverificaciones").on("hidden.bs.modal", function () {
                    modal.addarealistaverificacion.clear();
                });
                $("#addModalArealistaverificaciones").on("hidden.bs.modal", function () {
                    modal.addarealistaverificacion.clear();
                });


                $("#data-tablearealistaverificaciones").on('click', '.btn-editarealistaverificacion', function () {
                    var id = $(this).data("id");
                    modal.editarealistaverificacion.show(id);
                });

                $("#data-tablearealistaverificaciones").on('click', '.btn-deletearealistaverificacion', function () {
                    var id = $(this).data("id");
                    modal.deletearealistaverificacion(id);

                });



            }
        },
        createarearequisito: {
            object: $("#nuevoarearequisitos-form").validate({
                submitHandler: function (form, e) {
                    $("#btnNuevoarearequisitos").addLoader();
                    id = $("#addarearequisitos-form input[name=Id]").val();
                    $("#nuevoarearequisitos-form input[name=IdConvocatoria]").val(id);
                    e.preventDefault();
                    let formData = new FormData(form);
                    $.ajax({
                        url: $(form).attr("action"),
                        type: "POST",
                        data: formData,
                        contentType: false,
                        processData: false
                    }).done(function () {
                        $("#nuevoarearequisitos-form select[name=IdArea]").val("");
                        $("#nuevoarearequisitos-form select[name=IdArea]").trigger("change");
                        $("#nuevoarearequisitos-form select[name=IdRequisito]").val("");
                        $("#nuevoarearequisitos-form select[name=IdRequisito]").trigger("change");
                        $("#nuevoModalArearequisitos").modal("hide");
                        $(".m-alert").addClass("m--hide");
                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);

                        modal.addarearequisito.reload();
                        modal.createarearequisito.clear();
                        $("#btnNuevoarearequisitos").removeLoader();

                        //location.reload();
                    }).fail(function (error) {
                        toastr.error(error.responseText, _app.constants.toastr.title.error);
                        $("#btnNuevoarearequisitos").removeLoader();
                        //if (error.responseText !== null && error.responseText !== "") $("#add_form_msg_txt").html(error.responseText);
                        //else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                        //$("#add_form_msg").removeClass("m--hide").show();
                    }).always(function () {

                    });
                }
            }),
            show: function () {
                $("#btnNuevoarearequisitos").removeLoader();
                $("#nuevoModalArearequisitos").modal("toggle");
            },
            clear: function () {
                modal.createarearequisito.object.resetForm();
            },
            events: function () {
                $("#nuevoModalArearequisitos").on("hidden.bs.modal", function () {
                    modal.createarearequisito.clear();
                });

            }
        },
        createarealistaverificacion: {
            object: $("#nuevoarealistaverificaciones-form").validate({
                submitHandler: function (form, e) {
                    $("#btnNuevoarealistaverificaciones").addLoader();
                    id = $("#addarealistaverificaciones-form input[name=Id]").val();
                    $("#nuevoarealistaverificaciones-form input[name=IdConvocatoria]").val(id);
                    e.preventDefault();
                    let formData = new FormData(form);
                    $.ajax({
                        url: $(form).attr("action"),
                        type: "POST",
                        data: formData,
                        contentType: false,
                        processData: false
                    }).done(function () {
                        $("#nuevoarealistaverificaciones-form select[name=IdArea]").val("");
                        $("#nuevoarealistaverificaciones-form select[name=IdArea]").trigger("change");
                        $("#nuevoarealistaverificaciones-form select[name=IdListaverificacion]").val("");
                        $("#nuevoarealistaverificaciones-form select[name=IdListaverificacion]").trigger("change");
                        $("#nuevoModalArealistaverificaciones").modal("hide");
                        $(".m-alert").addClass("m--hide");
                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);

                        modal.addarealistaverificacion.reload();
                        modal.createarealistaverificacion.clear();
                        $("#btnNuevoarealistaverificaciones").removeLoader();

                        //location.reload();
                    }).fail(function (error) {
                        toastr.error(error.responseText, _app.constants.toastr.title.error);
                        $("#btnNuevoareaIdListaverificaciones").removeLoader();
                        //if (error.responseText !== null && error.responseText !== "") $("#add_form_msg_txt").html(error.responseText);
                        //else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                        //$("#add_form_msg").removeClass("m--hide").show();
                    }).always(function () {

                    });
                }
            }),
            show: function () {
                $("#btnNuevoareaIdListaverificaciones").removeLoader();
                $("#nuevoModalAreaIdListaverificaciones").modal("toggle");
            },
            clear: function () {
                modal.createarealistaverificacion.object.resetForm();
            },
            events: function () {
                $("#nuevoModalAreaIdListaverificaciones").on("hidden.bs.modal", function () {
                    modal.createarealistaverificacion.clear();
                });

            }
        },
        create: {
            object: $("#add-form").validate({
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
                        $(".modal").modal("hide");
                        $(".m-alert").addClass("m--hide");
                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                        datatable.investigacionfomentoConvocatoria.reload();
                        modal.create.clear();
                        $("#btnSave").removeLoader();
                    }).fail(function (error) {
                        toastr.error(error.responseText, _app.constants.toastr.title.error);
                        $("#btnSave").removeLoader();
                        //if (error.responseText !== null && error.responseText !== "") $("#add_form_msg_txt").html(error.responseText);
                        //else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                        //$("#add_form_msg").removeClass("m--hide").show();
                    }).always(function () {

                    });
                }
            }),
            show: function () {
                this.events();
                $("#btnSave").removeLoader();
                $("#addModal").modal("toggle");
            },
            clear: function () {
                modal.create.object.resetForm();
            },
            events: function () {
                
                $("#addModal").on("hidden.bs.modal", function () {
                    modal.create.clear();
                });

                $("#add-form select[name=IdAreaacademica]").on("change", function (e) {
                    $("#add-form select[name=IdLinea]").empty();
                    selectlinea.researcherMaestroLinea.load($("#add-form select[name=IdAreaacademica]").val());
                    $("#add-form select[name=IdLinea]").trigger("change");
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
                        datatable.investigacionfomentoConvocatoria.reload();
                        modal.edit.clear();
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
                    url: "/InvestigacionIncubadoranew/convocatoria".proto().parseURL(),
                    type: "GET",
                    data: {
                        handler: "Detail",
                        id: id
                    },
                }).done(function (result) {
                    $("#edit-form input[name=Id]").val(result.id);
                    $("#edit-form select[name=IdCategoriaconvocatoria]").val(result.idCategoriaconvocatoria);
                    $("#edit-form select[name=IdCategoriaconvocatoria]").trigger("change");
                    $("#edit-form select[name=IdTipoconvocatoria]").val(result.idTipoconvocatoria);
                    $("#edit-form select[name=IdTipoconvocatoria]").trigger("change");
                    $("#edit-form select[name=IdFlujo]").val(result.idFlujo);
                    $("#edit-form select[name=IdFlujo]").trigger("change");
                    $("#edit-form select[name=IdFacultad]").val(result.idFacultad);
                    $("#edit-form select[name=IdFacultad]").trigger("change");
                    $("#edit-form input[name=nombre]").val(result.nombre);
                    $("#edit-form input[name=fechaini]").val(result.fechaini);
                    $("#edit-form input[name=fechafin]").val(result.fechafin);
                    $("#edit-form textarea[name=descripcion]").val(result.descripcion);
                    $("#edit-form input[name=dirigidoa]").val(result.dirigidoa);
                    $("#edit-form input[name=nroplaza]").val(result.nroplaza);
                    $("#edit-form input[name=presupuesto]").val(result.presupuesto);
                    $("#edit-form input[name=fechainiinscripcion]").val(result.fechainiinscripcion);
                    $("#edit-form input[name=fechafininscripcion]").val(result.fechafininscripcion);
                    if (result.imagenarchivo != "") {
                        $('#edit-form img[id=current-picture]').attr('src', '/imagenes/' + result.imagenarchivo);

                    } else {
                        $('#edit-form img[id=current-picture]').attr('src', "/images/default-banner.jpg");

                    }

                    $("#edit-form input[name=activo]").prop("checked", result.activo == 1 ? true : false);
                    datePickers.init();

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
                modal.edit.load(id);
                $("#editModal").modal("toggle");
            },
            clear: function () {
                modal.edit.object.resetForm();
            },
            events: function () {
                $("#editModal").on("hidden.bs.modal", function () {
                    modal.edit.clear();
                });


                $("#edit-form select[name=IdAreaacademica]").on("change", function (e) {
                    $("#edit-form select[name=IdLinea]").empty();
                    selectlinea.researcherMaestroLinea.load($("#edit-form select[name=IdAreaacademica]").val());
                    $("#edit-form select[name=IdLinea]").trigger("change");
                });
            }
        },
        editarearequisito: {
            object: $("#editarearequisitos-form").validate({
                submitHandler: function (form, e) {
                    $("#btnEditarearequisitos").addLoader();
                    e.preventDefault();
                    let formData = new FormData(form);
                    $.ajax({
                        url: $(form).attr("action"),
                        type: "POST",
                        data: formData,
                        contentType: false,
                        processData: false
                    }).done(function () {
                        $("#editModalArearequisitos").modal("hide");
                        $(".m-alert").addClass("m--hide");
                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                        modal.addarearequisitos.reload();
                        modal.editarearequisitos.clear();
                    }).fail(function (error) {
                        toastr.error(error.responseText, _app.constants.toastr.title.error);
                        $("#btnEditarearequisitos").removeLoader();
                        //if (error.responseText !== null && error.responseText !== "") $("#add_form_msg_txt").html(error.responseText);
                        //else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                        //$("#add_form_msg").removeClass("m--hide").show();
                    }).always(function () {

                    });
                }
            }),
            load: function (id) {

                $("#editarearequisitos-form input[name=Id]").val(id);
                $.ajax({
                    url: "/InvestigacionIncubadoranew/convocatoria".proto().parseURL(),
                    type: "GET",
                    data: {
                        handler: "DetailArearequisito",
                        id: id
                    },
                }).done(function (result) {
                    $("#editarearequisitos-form input[name=Id]").val(result.id);
                    $("#editarearequisitos-form input[name=IdArea]").val(result.idArea);
                    $("#editarearequisitos-form select[name=IdUser]").val(result.idUser);
                    $("#editarearequisitos-form select[name=IdUser]").trigger("change");
                    $("#editarearequisitos-form input[name=activo]").prop("checked", result.activo == 1 ? true : false);

                }).fail(function (error) {
                    toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                    if (error.responseText !== null && error.responseText !== "") $("#add_form_msg_txt").html(error.responseText);
                    else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                    $("#add_form_msg").removeClass("m--hide").show();
                }).always(function () {

                });
            },
            show: function (id) {
                $("#btnEditarearequisitos").removeLoader();
                modal.editarearequisito.load(id);
                $("#editModalArearequisitos").modal("toggle");
            },
            clear: function () {
                $("#editarearequisitos-form").find(".custom-file-label").text("Seleccionar archivo");
                modal.editarearequisito.object.resetForm();
            },
            events: function () {
                $("#editModalArearequisitos").on("hidden.bs.modal", function () {
                    modal.editarearequisito.clear();
                });
            }
        },
        delete: function (id) {
            swal({
                title: "¿Está seguro?",
                text: "El docente será eliminado.",
                type: "warning",
                showCancelButton: true,
                confirmButtonText: "Sí, eliminarlo",
                confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                cancelButtonText: "Cancelar",
                showLoaderOnConfirm: true,
                preConfirm: () => {
                    return new Promise((resolve) => {                        
                        $.ajax({
                            url: ("/InvestigacionIncubadoranew/convocatoria?handler=Delete").proto().parseURL(),
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
                                    text: "El Docente ha sido eliminado con exito",
                                    confirmButtonText: "Excelente"
                                }).then(datatable.investigacionfomentoConvocatoria.reload());
                            },
                            error: function (errormessage) {
                                swal({
                                    type: "error",
                                    title: "Error",
                                    confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                    confirmButtonText: "Entendido",
                                    text: "El Docente presenta información relacionada"
                                });
                            }
                        });
                    });
                },
                allowOutsideClick: () => !swal.isLoading()
            });
        },
        deletearearequisito: function (id) {
            swal({
                title: "¿Está seguro?",
                text: "El requisito será eliminado.",
                type: "warning",
                showCancelButton: true,
                confirmButtonText: "Sí, eliminarlo",
                confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                cancelButtonText: "Cancelar",
                showLoaderOnConfirm: true,
                preConfirm: () => {
                    return new Promise((resolve) => {
                        $.ajax({
                            url: ("/InvestigacionIncubadoranew/convocatoria?handler=Deletearearequisito").proto().parseURL(),
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
                                    text: "El Area ha sido eliminado con exito",
                                    confirmButtonText: "Excelente"
                                }).then(modal.addarearequisito.reload());
                            },
                            error: function (errormessage) {
                                swal({
                                    type: "error",
                                    title: "Error",
                                    confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                    confirmButtonText: "Entendido",
                                    text: "El Area presenta información relacionada"
                                });
                            }
                        });
                    });
                },
                allowOutsideClick: () => !swal.isLoading()
            });
        },
        deletearealistaverificacion: function (id) {
            swal({
                title: "¿Está seguro?",
                text: "La lista de verificación será eliminada.",
                type: "warning",
                showCancelButton: true,
                confirmButtonText: "Sí, eliminarlo",
                confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                cancelButtonText: "Cancelar",
                showLoaderOnConfirm: true,
                preConfirm: () => {
                    return new Promise((resolve) => {
                        $.ajax({
                            url: ("/InvestigacionIncubadoranew/convocatoria?handler=Deletearealistaverificacion").proto().parseURL(),
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
                                    text: "La lista de verificación ha sido eliminado con exito",
                                    confirmButtonText: "Excelente"
                                }).then(modal.addarealistaverificacion.reload());
                            },
                            error: function (errormessage) {
                                swal({
                                    type: "error",
                                    title: "Error",
                                    confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                    confirmButtonText: "Entendido",
                                    text: "La lista de verificación presenta información relacionada"
                                });
                            }
                        });
                    });
                },
                allowOutsideClick: () => !swal.isLoading()
            });
        },
        observacion: function (observacion) {
            swal({
                title: "Observación del Comite",
                text: observacion,
                type: "warning",
                customClass:"swal-wide",
                confirmButtonText: "OK",
                showConfirmButton: true
            })
        },
        enviar: function (id) {
            swal({
                title: "¿Está seguro?",
                text: "El plan de trabajo sera enviado.",
                type: "warning",
                showCancelButton: true,
                confirmButtonText: "Sí, enviar",
                confirmButtonClass: "btn btn-warning m-btn m-btn--custom",
                cancelButtonText: "Cancelar",
                showLoaderOnConfirm: true,
                preConfirm: () => {
                    return new Promise((resolve) => {
                        $.ajax({
                            url: ("/InvestigacionIncubadoranew/convocatoria?handler=EnviarPlan").proto().parseURL(),
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
                                    text: "El plan de trabajo ha sido enviado para su revisión",
                                    confirmButtonText: "Excelente"
                                }).then(datatable.investigacionfomentoConvocatoria.reload());
                            },
                            error: function (errormessage) {
                                swal({
                                    type: "error",
                                    title: "Error",
                                    confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                    confirmButtonText: "Entendido",
                                    text: "El plan de trabajo tubo un problema de envio"
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
            this.editarearequisito.events();
            this.addarearequisito.events();
            this.createarearequisito.events();
            this.createarealistaverificacion.events();
            

        }

    };
   /* var loadPicture = {
        init: function () {
            $("#Input_PictureFile").on("change",
                function (e) {

                    var tgt = e.target || window.event.srcElement,
                        files = tgt.files;
                    // FileReader support
                    if (FileReader && files && files.length) {
                        var fr = new FileReader();
                        fr.onload = function () {
                            $("#current-picture").attr("src", fr.result);
                        }
                        fr.readAsDataURL(files[0]);
                    }
                    // Not supported
                    else {
                        console.log("File Reader not supported.");
                    }
                });
        }
    };*/
    var datePickers = {
        init: function () {

            $("#add-form input[name=fechaini]").datepicker({
                clearBtn: true,
                orientation: "bottom",
                format: _app.constants.formats.datepicker
            });
            $("#add-form input[name=fechafin]").datepicker({
                clearBtn: true,
                orientation: "bottom",
                format: _app.constants.formats.datepicker
            });
            $("#edit-form input[name=fechaini]").datepicker({
                clearBtn: true,
                orientation: "bottom",
                format: _app.constants.formats.datepicker
            });
            $("#edit-form input[name=fechafin]").datepicker({
                clearBtn: true,
                orientation: "bottom",
                format: _app.constants.formats.datepicker
            });

            $("#add-form input[name=fechainiinscripcion]").datetimepicker({
                clearBtn: true,
                orientation: "bottom",
                format: _app.constants.formats.datetimepicker
            });

            $("#add-form input[name=fechafininscripcion]").datetimepicker({
                clearBtn: true,
                orientation: "bottom",
                format: _app.constants.formats.datetimepicker
            });


            $("#edit-form input[name=fechainiinscripcion]").datetimepicker({
                clearBtn: true,
                orientation: "bottom",
                format: _app.constants.formats.datetimepicker
            });

            $("#edit-form input[name=fechafininscripcion]").datetimepicker({
                clearBtn: true,
                orientation: "bottom",
                format: _app.constants.formats.datetimepicker
            });
        }
    }
    return {
        init: function () {
            datatable.init();
            search.init();
            modal.init();
            datePickers.init();
           // loadPicture.init();
            selectflujo.init();
            selectfacultad.init();
            selectacategoriaconvocatoria.init();
            selecttipoconvocatoria.init();
            selectrequisito.init();
            selectlistaverificacion.init();
           
        }
    }
}();

$(function () {
    InitApp.init();
})
