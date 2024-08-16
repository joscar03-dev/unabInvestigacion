var InitApp = function () {
    var selectareaacademica = {
        init: function () {
            this.researcherMaestroAreaacademica.init();
        },
        researcherMaestroAreaacademica: {
            init: function () {
                this.load();
            },
            load: function () {
                $.ajax({
                    url: (`/api/maestroareaacademica/select/get`).proto().parseURL(),
                    type: "GET"
                }).done(function (result) {
                    $('.input_IdAreaacademica').select2({
                        data: result,
                        maximumSelectionLength: 1,
                    });
                });
            }
        },
    };
    var selectdepartamento = {
        init: function () {
            this.researcherMaestroDepartamento.init();
        },
        researcherMaestroDepartamento: {
            init: function () {
                this.load();
            },
            load: function () {
                $.ajax({
                    url: (`/api/maestrodepartamento/select/get`).proto().parseURL(),
                    type: "GET"
                }).done(function (result) {
                    $('.input_IdDepartamento').select2({
                        data: result,
                        maximumSelectionLength: 1,
                    });
                });
            }
        },
    };
    var selectcarrera = {
        init: function () {
            this.researcherMaestroCarrera.init();
        },
        researcherMaestroCarrera: {
            init: function () {
                this.load();
            },
            load: function () {
                $.ajax({
                    url: (`/api/maestrocarrera/select/get`).proto().parseURL(),
                    type: "GET"
                }).done(function (result) {
                    $('.input_IdCarrera').select2({
                        data: result,
                        maximumSelectionLength: 1,
                    });
                });
            }
        },
    };
    var selectlinea = {
        init: function () {
            this.researcherMaestroLinea.init();
        },
        researcherMaestroLinea: {
            init: function () {
                this.load();
            },
            load: function (IdAreaacademica, Id) {
                $.ajax({
                    url: (`/api/maestrolinea/select/get`).proto().parseURL(),
                    type: "GET",
                    data: (IdAreaacademica ? (IdAreaacademica == '' ? '' : "IdAreaacademica=" + IdAreaacademica) : ''),
                }).done(function (result) {


                    $('.input_IdLinea').select2({
                        data: result,
                        maximumSelectionLength: 1,

                    });
                    if (Id) {
                        $(".input_IdLinea").val(Id);
                    }
                    $(".input_IdLinea").trigger("change");
                });
            }
        },
    };
    var selecttipoevento = {
        init: function () {
            this.researcherInvestigacionformativaTipoevento.init();
        },
        researcherInvestigacionformativaTipoevento: {
            init: function () {
                this.load();
            },
            load: function () {
                $.ajax({
                    url: (`/api/investigacionformativatipoevento/select/get`).proto().parseURL(),
                    type: "GET"
                }).done(function (result) {
                    $('.input_IdTipoevento').select2({
                        data: result,
                        maximumSelectionLength: 1,
                    });
                });
            }
        },
    };
    var selecttiporesultado = {
        init: function () {
            this.researcherInvestigacionformativaTiporesultado.init();
        },
        researcherInvestigacionformativaTiporesultado: {
            init: function () {
                this.load();
            },
            load: function () {
                $.ajax({
                    url: (`/api/investigacionformativatiporesultado/select/get`).proto().parseURL(),
                    type: "GET"
                }).done(function (result) {
                    $('.input_IdTiporesultado').select2({
                        data: result,
                        maximumSelectionLength: 1,
                    });
                });
            }
        },
    };
    var selectdocente = {
        init: function () {
            this.researcherMaestroDocente.init();
        },
        researcherMaestroDocente: {
            init: function () {
                this.load();
            },
            load: function () {
                $.ajax({
                    url: (`/api/maestrodocentedocente/select/get`).proto().parseURL(),
                    type: "GET"
                }).done(function (result) {
                    $('.input_IdDocente').select2({
                        data: result,
                        maximumSelectionLength: 1,
                    });
                });
            }
        },
    };
    var selectanio = {
        init: function () {
            this.researcherInvestigacionformativaAnio.init();
        },
        researcherInvestigacionformativaAnio: {
            init: function () {
                this.load();
            },
            load: function () {
                $.ajax({
                    url: (`/api/investigacionformativamaestroanio/select/get`).proto().parseURL(),
                    type: "GET",
                }).done(function (result) {
                    $('.input_lst_searchanio').select2({
                        data: result,
                        maximumSelectionLength: 1,

                    });
                    $(".input_lst_searchanio").on('change', function () {
                        $("#searchanio").val(this.value);
                        if (this.value != "") {
                            datatable.investigacionformativaPlantrabajo.reload();
                        }
                    });




                });
            }
        },
    };

    var datatable = {
        investigacionformativaPlantrabajo: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: "/investigacionformativa/plantrabajodocente".proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        data.handler = "Datatable";
                        data.searchValue = $("#search").val();
                        data.tipoplan = $("#tipoplan").val();
                        data.searchanioValue = $("#searchanio").val();
                        
                        
                    },
                   

                   
                },
                dom: 'Bfrtip',
                buttons: [
                    'excel', 'pdf'
                ],
               
                pageLength: 200,
                orderable: [],
                columns: [

                    {
                        title: "Facultad/Area",
                        data: null,
                        render: function (data) {
                            var template = "";
                            template += data.nombrefacultad;
                            template += "/";
                            template += data.nombreareaacademica;
                            return template;
                        }
                    },
                    {
                        title: "Docente",
                        data: "fullname",
                        width: 200,

                    },
                    {
                        title: "Título",
                        data: "titulo"
                    },

                    {
                        title: "Act",
                        data: "total",
                        width:50
                    
                    },
                    {
                        title: "Act Env.",                  
                        data: null,
                        width: 50,
                        render: function (data) {
                            var template = "";
                            if (data.totalenviado == '0') {
                                template = '<span class="badge rounded-pill badge-danger">0</span>';
                            } else {
                                template = '<span class="badge rounded-pill badge-success">' + (data.totalenviado) + '</span>';

                            }
                            return template;
                        }
                    },
                    {
                        title: "Act Apro",
                        data: null,
                        width: 50,
                        render: function (data) {
                            var template = "";
                            if (data.totalaprobado == '0') {
                                template = '<span class="badge rounded-pill badge-danger">0</span>';
                            } else {
                                template = '<span class="badge rounded-pill badge-success">' + (data.totalaprobado) + '</span>';

                            }
                            return template;
                        }
                    },
                    {
                        title: "Act Obs",
                        data: null,
                        width: 50,
                        render: function (data) {
                            var template = "";
                            if (data.totalobservado == '0') {
                                template = '<span class="badge rounded-pill badge-danger">0</span>';
                            } else {
                                template = '<span class="badge rounded-pill badge-success">' + (data.totalobservado )+ '</span>';

                            }

                                
                            return template;
                        }
                    },
                  /*  {
                        title: "Autor",
                        data: null,
                        orderable: false,
                        render: function (data) {
                            var template = '<li>' + (data.coautor1 == null ? '' : data.coautor1 )+ '</li>';
                            template += '<li>' + (data.coautor2 == null ? '' : data.coautor2 ) + '</li>';
                            return template;
                        }
                    },*/
                    {
                        title: "Anexo",
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
                        width: 100,
                        render: function (data) {
                            var btnsearch = "";
                            btnsearch += "<button title='Observaciones' class='btn btn-danger btn-observacion ";
                            btnsearch += " m-btn btn-sm  m-btn--icon-only' ";
                            btnsearch += " data-id=' " + data.observacioncomite + "' > ";
                            btnsearch += "<i class='la la-search'></i></button>";


                            var btncalendar = "";
                            btncalendar += "<button title='Cronograma de Activiades' class='btn btn-danger btn-actividades ";
                            btncalendar += " m-btn btn-sm  m-btn--icon-only' ";
                            btncalendar += " data-id=' " + data.id + "' > ";
                            btncalendar += "<i class='la la-calendar'></i></button>";

                            var estado = data.estado == 0 ? "EN PROCESO" : data.estado == 1 ? "ENVIADO" : data.estado == 2 ? "PLAN APROBADO" : "OBSERVAR";
                            var label = data.estado == 0 ? "label-warning" : data.estado == 1 ? "label-success" : data.estado == 2 ? "label-success" : "label-danger"; // 
                            var template = "";
                            template += "<span class='btn m-btn btn-sm m-btn--" + label + "'>";
                            template += estado + " " + (data.estado == 3 ? btnsearch : (data.estado == 2 ? btncalendar : ""));
                            template += "</span> ";
                            return template;
                        }
                    },
                    {
                        title: "Opciones",
                        data: null,
                        width: 210,
                        orderable: false,
                        render: function (data) {
                            var template = "";
                            


                            template += "<button title='Agregar alumnos'  style='background-color:#aaeee8' class='btn  btn-editautores";
                            template += " m-btn btn-sm  m-btn--icon-only' ";
                            template += " data-id='" + data.id + "' > ";
                            template += "<span><i class='la la-user'></i><span>Alumnos</span></span></button>";
                            if (data.estado == "0" || data.estado == "3") {
                                //enviar
                                if (data.archivourl != '') {
                                    template += "<button title='Enviar al comite' class='btn btn-warning btn-enviar ";
                                    template += " m-btn btn-sm  m-btn--icon-only' ";
                                    template += " data-id='" + data.id + "' > ";
                                    template += "<i class='la la-send'></i></button>";
                                }

                                //anexo
                                template += "<button title='Subir anexo' class='btn btn-success m-btn btn-sm m-btn--icon-only btn-editanexo '  data-id='" + data.id + "'>";
                                template += "<span><i class='la la-edit'></i><span>Anexo</span></span></button>";
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
                            } else {
                                /* if (data.tipoplan == 'C') {
                                     template += "<button title='Derivar Aprobación/Observación' class='btn btn-danger btn-editobservacion ";
                                     template += " m-btn btn-sm  m-btn--icon-only' ";
                                     template += " data-id='" + data.id + "' > ";
                                     template += "<i class='la la-send'></i></button>";
                                 }*/
                                if (data.admin == "0") {
                                    template += "<button class='btn btn-success.disabled m-btn btn-sm m-btn--icon-only  '  data-id=''>";
                                    template += "<span><i class='la la-edit'></i><span>Anexo</span></span></button>";
                                } else {
                                    template += "<button title='Subir anexo' class='btn btn-success m-btn btn-sm m-btn--icon-only btn-editanexo '  data-id='" + data.id + "'>";
                                    template += "<span><i class='la la-edit'></i><span>Anexo</span></span></button>";
                                }
                               

                               
                                //Edit
                                template += "<button ";
                                template += "class='btn btn-info.disabled  ";
                                template += "m-btn btn-sm  m-btn--icon-only' ";
                                template += " data-id=''>";
                                template += "<i class='la la-edit'></i></button>";

                                //Delete
                                template += "<button ";
                                template += "class='btn btn-danger.disabled ";
                                template += "m-btn btn-sm  m-btn--icon-only' ";
                                template += " data-id=''>";
                                template += "<i class='la la-trash'></i></button>";
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
                    modal.edit.show(id);
                });

                $("#data-table").on('click', '.btn-editanexo', function () {
                    var id = $(this).data("id");
                    modal.editanexo.show(id);
                });

                $("#data-table").on('click', '.btn-editautores', function () {
                    var id = $(this).data("id");
                    modal.editautores.show(id);
                });

                $("#data-table").on('click', '.btn-delete', function () {
                    var id = $(this).data("id");
                    modal.delete(id);

                });

                $("#data-table").on('click', '.btn-enviar', function () {
                    var id = $(this).data("id");
                    modal.enviar(id);

                });
                $("#data-table").on('click', '.btn-observacion', function () {
                    var id = $(this).data("id");
                    modal.observacion(id);

                });

                $("#data-table").on('click', '.btn-actividades', function () {
                    var id = $(this).data("id");
                    modal.addactividades.show(id);
                });


            }
        },
        init: function () {
            this.investigacionformativaPlantrabajo.init();
        }
    };
    var search = {
        init: function () {
            $("#search").doneTyping(function () {
                datatable.investigacionformativaPlantrabajo.reload();
            });
        }
    };
    var modal = {
        editanexo: {
            object: $("#editanexo-form").validate({
                submitHandler: function (form, e) {
                    $("#btnEditanexo").addLoader();
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

                        datatable.investigacionformativaPlantrabajo.reload();
                        modal.editanexo.clear();
                    }).fail(function (error) {
                        toastr.error(error.responseText, _app.constants.toastr.title.error);
                        $("#btnEditanexo").removeLoader();
                        //if (error.responseText !== null && error.responseText !== "") $("#add_form_msg_txt").html(error.responseText);
                        //else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                        //$("#add_form_msg").removeClass("m--hide").show();
                    }).always(function () {

                    });
                }
            }),
            load: function (id) {
                $.ajax({
                    url: `/investigacionformativa/plantrabajodocente`.proto().parseURL(),
                    type: "GET",
                    data: {
                        handler: "DetailFile",
                        id: id
                    },
                }).done(function (result) {
                    $("#editanexo-form input[name=Id]").val(result.id);                   
                    $("#editanexo-form").find(".custom-file-label").text("Seleccionar archivo");
                    $("#editanexo-form").find(".custom-file").append('<div class="form-control-feedback">*Seleccionar en caso sea necesario reemplazar el documento vigente.</div>');
                }).fail(function (error) {
                    toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                    if (error.responseText !== null && error.responseText !== "") $("#add_form_msg_txt").html(error.responseText);
                    else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                    $("#add_form_msg").removeClass("m--hide").show();
                }).always(function () {

                });
            },
            show: function (id) {
                $("#btnEditanexo").removeLoader();
                modal.editanexo.load(id);
                $("#editModalAnexo").modal("toggle");
            },
            clear: function () {
                $("#editanexo-form").find(".custom-file-label").text("Seleccionar archivo");
                modal.editanexo.object.resetForm();
            },
            events: function () {
                $("#editModalAnexo").on("hidden.bs.modal", function () {
                    modal.editanexo.clear();
                });
            }
        },
        editautores: {
            object: $("#editautores-form").validate({
                submitHandler: function (form, e) {
                    $("#btnEditautores").addLoader();
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

                        datatable.investigacionformativaPlantrabajo.reload();
                        modal.editanexo.clear();
                    }).fail(function (error) {
                        toastr.error(error.responseText, _app.constants.toastr.title.error);
                        $("#btnEditautores").removeLoader();
                        //if (error.responseText !== null && error.responseText !== "") $("#add_form_msg_txt").html(error.responseText);
                        //else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                        //$("#add_form_msg").removeClass("m--hide").show();
                    }).always(function () {

                    });
                }
            }),
            load: function (id) {
                $.ajax({
                    url: `/investigacionformativa/plantrabajodocente`.proto().parseURL(),
                    type: "GET",
                    data: {
                        handler: "DetailUsuarios",
                        id: id
                    },
                }).done(function (result) {
                    $("#editautores-form input[name=Id]").val(result.id);
                    $("#editautores-form input[name=coautor1]").val(result.coautor1);
                    $("#editautores-form input[name=coautor2]").val(result.coautor2);

                }).fail(function (error) {
                    toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                    if (error.responseText !== null && error.responseText !== "") $("#add_form_msg_txt").html(error.responseText);
                    else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                    $("#add_form_msg").removeClass("m--hide").show();
                }).always(function () {

                });
            },
            show: function (id) {
                $("#btnEditautores").removeLoader();
                modal.editautores.load(id);
                $("#editModalAutores").modal("toggle");
            },
            clear: function () {
                $("#editautores-form").find(".custom-file-label").text("Seleccionar archivo");
                modal.editautores.object.resetForm();
            },
            events: function () {
                $("#editModalAutores").on("hidden.bs.modal", function () {
                    modal.editautores.clear();
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
                        datatable.investigacionformativaPlantrabajo.reload();
                        modal.create.clear();
                        $("#btnSave").removeLoader();
                        location.reload();
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
                    $.ajax({
                        url: $(form).attr("action"),
                        type: "POST",
                        data: $(form).serialize()
                    }).done(function () {
                        $(".modal").modal("hide");
                        $(".m-alert").addClass("m--hide");
                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                        datatable.investigacionformativaPlantrabajo.reload();
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
                    url: "/investigacionformativa/plantrabajodocente".proto().parseURL(),
                    type: "GET",
                    data: {
                        handler: "Detail",
                        id: id
                    },
                }).done(function (result) {
                    $("#edit-form input[name=Id]").val(result.id);
                    $("#edit-form select[name=IdDepartamento]").val(result.idDepartamento);
                    $("#edit-form select[name=IdDepartamento]").trigger("change");
                    $("#edit-form select[name=IdAreaacademica]").val(result.idAreaacademica);
                    $("#edit-form select[name=IdAreaacademica]").trigger("change");
                    $("#edit-form select[name=IdLinea]").empty();
                    selectlinea.researcherMaestroLinea.load(result.idAreaacademica, (result.idLinea));
                    $("#edit-form select[name=IdLinea]").val(result.idLinea);
                    $("#edit-form select[name=IdLinea]").trigger("change");
                    $("#edit-form select[name=IdCarrera]").val(result.idCarrera);
                    $("#edit-form select[name=IdCarrera]").trigger("change");
                    $("#edit-form select[name=IdDocente]").val(result.idDocente);
                    $("#edit-form select[name=IdDocente]").trigger("change");
                    $("#edit-form select[name=IdTipoevento]").val(result.idTipoevento);
                    $("#edit-form select[name=IdTipoevento]").trigger("change");
                    $("#edit-form select[name=IdTiporesultado]").val(result.idTiporesultado);
                    $("#edit-form select[name=IdTiporesultado]").trigger("change");
                    $("#edit-form input[name=titulo]").val(result.titulo);
                    $("#edit-form input[name=fechaini]").val(result.fechaini);
                    $("#edit-form input[name=fechafin]").val(result.fechafin);
                    $("#edit-form textarea[name=objetivo]").val(result.objetivo);
                    $("#edit-form textarea[name=descripcion]").val(result.descripcion);
                    $("#edit-form input[name=activo]").prop("checked", result.activo == 1 ? true : false);

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
        addactividades: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: "/investigacionformativa/plantrabajodocente".proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        data.handler = "Datatableactividades";
                        data.id = $("#addactividades-form input[name=Id]").val();
                        data.tipoplan = $("#tipoplan").val();
                    }
                },
                pageLength: 50,
                orderable: [],
                columns: [

                    {
                        title: "Fecha Incial",
                        data: "fechaini",
                         width: 100
                    },
                    {
                        title: "Fecha final",
                        data: "fechafin",
                        width: 100
                    },
                    {
                        title: "Título",
                        data: null,
                        render: function (data) {
                            var template = "";
                            if (data.informefinal == '1') {
                                template = '<b style="color:#990000">' + data.titulo + '</b>'; 
                            } else {
                                template = data.titulo;
                            }
                            return template;
                        }
                    },

                   
                    {
                        title: "Desarrollo de la investigación/Informe FInal (PDF)",
                        data: null,
                        orderable: false,
                        width: 100,
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
                        title: "Anexo Informe FInal (PDF)",
                        data: null,
                        orderable: false,
                        width: 100,
                        render: function (data) {
                            var template = "";
                            var fileUrl = `/documentos/${data.anexourl}`.proto().parseURL();
                            //FileURL
                            if (data.anexourl != '') {
                                template += `<a href="${fileUrl}"  `;
                                template += "class='btn btn-success ";
                                template += "m-btn btn-sm m-btn--icon' download>";
                                template += "<span><i class='flaticon-file'></i></a> ";
                            }
                            return template;
                        }
                    }, {
                        title: "Observación (PDF)",
                        data: null,
                        orderable: false,
                        width: 100,
                        render: function (data) {
                            var template = "";
                            var fileUrl = `/documentos/${data.archivoobservacionurl}`.proto().parseURL();
                            //FileURL
                            if (data.archivoobservacionurl != '') {
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
                        width: 100,
                        render: function (data) {
                            var template = "";
                            
                            var btnsearch = "";
                            btnsearch += "<button type='button' title='Observaciones' class='btn btn-danger btn-observacionactividad ";
                            btnsearch += " m-btn btn-sm  m-btn--icon-only' ";
                            btnsearch += " data-id=' " + data.observacion + "' > ";
                            btnsearch += "<i class='la la-search'></i></button>";

                            var estado = data.estado == 0 ? "EN PROCESO" : data.estado == 1 ? "ENVIADO" : data.estado == 2 ? (data.informefinal == "0" ? "APROBADO ACOMPAÑANTE" : "APROBADO COMITE") : (data.informefinal == "0" ? "OBSERVADO ACOMPAÑANTE" : "OBSERVADO COMITE")  ;
                            var label = data.estado == 0 ? "label-warning" : data.estado == 1 ? "label-success" : data.estado == 2 ? "label-success" : "label-danger"; //
                            //data.informefinal == "1"
                            if (data.informefinal == "1" && data.dias < 0) {
                                var estado = "CERRADO";
                                var label = "label-metal"
                            }

                            template += "<span class='btn m-btn btn-sm m-btn--" + label + "'>";
                            template += estado + " " + (data.estado == 3 ? btnsearch : "");
                            template += "</span> ";
                            return template;
                        }
                    },
                    {
                        title: "Opciones",
                        data: null,
                        width: 100,
                        orderable: false,
                        render: function (data) {
                            var template = "";

                        
                            //enviar
                            if (data.estado == '0' || data.estado == '3') {
                                if (data.tipoplan != 'C' && data.tipoplan != 'F') { 
                                if (data.archivourl != '') {
                                    template += "<button title='Enviar al acompañante' type='button' ";
                                    template += " class='btn btn-warning btn-enviaractividad ";
                                    template += " m-btn btn-sm  m-btn--icon-only' ";
                                    template += " data-id='" + data.id + "' > ";
                                    template += "<i class='la la-send'></i></button>";
                                }
                                    //Edit
                                    if (data.informefinal == '1' ) {
                                        template += "<button title='Editar'  type='button'  ";
                                        template += "class='btn btn-info btn-editactividadfinal ";
                                        template += "m-btn btn-sm  m-btn--icon-only' ";
                                        template += " data-id='" + data.id + "'>";
                                        template += "<i class='la la-edit'></i> Informe Final</button>";
                                    } else {
                                        template += "<button title='Editar'  type='button'  ";
                                        template += "class='btn btn-info btn-editactividad ";
                                        template += "m-btn btn-sm  m-btn--icon-only' ";
                                        template += " data-id='" + data.id + "'>";
                                        template += "<i class='la la-edit'></i></button>";
                                    }
                               
                                }
                            } else {
                                if (data.tipoplan == 'C' && data.estado == '1') {
                                    template += "<button type='button' title='Derivar Aprobación/Observación' "
                                    template += " class='btn btn-danger btn-editobservacionactividad ";
                                    template += " m-btn btn-sm  m-btn--icon-only' ";
                                    template += " data-id='" + data.id + "' > ";
                                    template += "<i class='la la-send'></i></button>";
                                }
                                if (data.tipoplan == 'F' && data.estado == '1') {
                                    template += "<button type='button' title='Informe Final Aprobar/Observar' "
                                    template += " class='btn btn-danger btn-editobservacionactividad ";
                                    template += " m-btn btn-sm  m-btn--icon-only' ";
                                    template += " data-id='" + data.id + "' > ";
                                    template += "<i class='la la-send'></i></button>";
                                }
                            }
                              
                            if (data.informefinal == "1" && data.dias < 0) {
                                template = "";
                            }

                            if (template == "" && data.informefinal == "1" && data.admin== "1") {
                                template += "<button title='Editar'  type='button'  ";
                                template += "class='btn btn-info btn-editactividadfinal ";
                                template += "m-btn btn-sm  m-btn--icon-only' ";
                                template += " data-id='" + data.id + "'>";
                                template += "<i class='la la-edit'></i> Informe Final</button>";
                            }

                              
                            return template;
                        }
                    }
                ],
                rowCallback: function (row, data) {
                    if (contaractividad == 0) {
                        if (data.estado == 2) {
                            arrayactividad[data.fechafin] = '1';
                            contaraprobado = contaraprobado + 1;
                        }
                    } else {
                        if (data.estado == 2) {
                            arrayactividad[data.fechafin] = '1';
                        } else {
                            if (!arrayactividad[data.fechaini]) {
                                row.style.display = "none";
                            }

                        };
                    }

                    if (data.informefinal == '1' && contaraprobado > 0) {
                        row.style.display = "";
                    }

                    if (data.informefinal == '1') {
                        row.style.backgroundColor = "#DEEEF7";
                    }

                    contaractividad = contaractividad + 1;                   
                   
                },
            },
            load: function (id) {
                $("#addactividades-form input[name=Id]").val(id);
                if (this.object == null) {
                    this.object = $("#data-tableactividades").DataTable(this.options);
                } else {
                    this.reload();
                }
                modal.addactividades.events();
                arrayactividad = new Array();
                contaractividad = 0;
                contaraprobado = 0;
            
            },
            reload: function () {
                this.object.ajax.reload();
                arrayactividad = new Array();
                contaractividad = 0;
            },
            show: function (id) {
                //$("#btnEdit").removeLoader();
                
                modal.addactividades.load(id);
                $("#addModalActividades").modal("toggle");
            },
            clear: function () {
               // modal.addactividades.object.resetForm();
            },
            events: function () {
              
                $("#addModalActividades").on("hidden.bs.modal", function () {
                    modal.addactividades.clear();
                });
                $("#addModalActividades").on("hidden.bs.modal", function () {
                    modal.addactividades.clear();
                });

             
                $("#data-tableactividades").on('click', '.btn-editactividad', function () {
                  
                    var id = $(this).data("id");
                    modal.editactividades.show(id);
                });
                $("#data-tableactividades").on('click', '.btn-editactividadfinal', function () {

                    var id = $(this).data("id");
                    modal.editactividadesfinal.show(id);
                });
                $("#data-tableactividades").on('click', '.btn-editobservacionactividad', function () {

                    var id = $(this).data("id");
                    modal.editobservacionactividad.show(id);
                });
                
                $("#data-tableactividades").on('click', '.btn-enviaractividad', function () {
                    var id = $(this).data("id");
                    modal.enviaractividad(id);

                });
                $("#data-tableactividades").on('click', '.btn-observacionactividad', function () {
                    var id = $(this).data("id");
                    modal.observacionactividad(id);

                });

                
            }
        },
        editactividades: {
            object: $("#editactividades-form").validate({
                submitHandler: function (form, e) {
                    $("#btnEditactividades").addLoader();
                    e.preventDefault();
                    let formData = new FormData(form);
                    $.ajax({
                        url: $(form).attr("action"),
                        type: "POST",
                        data: formData,
                        contentType: false,
                        processData: false
                    }).done(function () {
                        $("#editModalActividades").modal("hide");
                        $(".m-alert").addClass("m--hide");
                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                        modal.addactividades.reload();                       
                        modal.editactividades.clear();
                    }).fail(function (error) {
                        toastr.error(error.responseText, _app.constants.toastr.title.error);
                        $("#btnEditactividades").removeLoader();
                        //if (error.responseText !== null && error.responseText !== "") $("#add_form_msg_txt").html(error.responseText);
                        //else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                        //$("#add_form_msg").removeClass("m--hide").show();
                    }).always(function () {

                    });
                }
            }),
            load: function (id) {
              
                $("#editactividades-form input[name=Id]").val(id);
                $.ajax({
                    url: "/investigacionformativa/plantrabajodocente".proto().parseURL(),
                    type: "GET",
                    data: {
                        handler: "DetailFileActividades",
                        id: id
                    },
                }).done(function (result) {
                    $("#editactividades-form input[name=Id]").val(result.id);                  
                    $("#editactividades-form input[name=titulo]").val(result.titulo);                
                    $("#editactividades-form textarea[name=descripcion]").val(result.descripcion);;

                }).fail(function (error) {
                    toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                    if (error.responseText !== null && error.responseText !== "") $("#add_form_msg_txt").html(error.responseText);
                    else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                    $("#add_form_msg").removeClass("m--hide").show();
                }).always(function () {

                });
            },
            show: function (id) {
                $("#btnEditactividades").removeLoader();
                modal.editactividades.load(id);
                $("#editModalActividades").modal("toggle");
            },
            clear: function () {
                $("#editactividades-form").find(".custom-file-label").text("Seleccionar archivo");
                modal.editactividades.object.resetForm();
            },
            events: function () {
                $("#editModalActividades").on("hidden.bs.modal", function () {
                    modal.editactividades.clear();
                });
            }
        },
        editactividadesfinal: {
            object: $("#editactividadesfinal-form").validate({
                submitHandler: function (form, e) {
                    $("#btnEditactividadesfinal").addLoader();
                    e.preventDefault();
                    let formData = new FormData(form);
                    $.ajax({
                        url: $(form).attr("action"),
                        type: "POST",
                        data: formData,
                        contentType: false,
                        processData: false
                    }).done(function () {
                        $("#editModalActividadesfinal").modal("hide");
                        $(".m-alert").addClass("m--hide");
                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                        modal.addactividades.reload();
                        modal.editactividadesfinal.clear();
                    }).fail(function (error) {
                        toastr.error(error.responseText, _app.constants.toastr.title.error);
                        $("#btnEditactividadesfinal").removeLoader();
                        //if (error.responseText !== null && error.responseText !== "") $("#add_form_msg_txt").html(error.responseText);
                        //else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                        //$("#add_form_msg").removeClass("m--hide").show();
                    }).always(function () {

                    });
                }
            }),
            load: function (id) {

                $("#editactividadesfinal-form input[name=Id]").val(id);
                $.ajax({
                    url: "/investigacionformativa/plantrabajodocente".proto().parseURL(),
                    type: "GET",
                    data: {
                        handler: "DetailFileActividadesfinal",
                        id: id
                    },
                }).done(function (result) {
                    $("#editactividades-form input[name=Id]").val(result.id);
                  

                }).fail(function (error) {
                    toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                    if (error.responseText !== null && error.responseText !== "") $("#add_form_msg_txt").html(error.responseText);
                    else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                    $("#add_form_msg").removeClass("m--hide").show();
                }).always(function () {

                });
            },
            show: function (id) {
                $("#btnEditactividadesfinal").removeLoader();
                modal.editactividadesfinal.load(id);
                $("#editModalActividadesfinal").modal("toggle");
            },
            clear: function () {
                $("#editactividadesfinal-form").find(".custom-file-label").text("Seleccionar archivo");
                modal.editactividadesfinal.object.resetForm();
            },
            events: function () {
                $("#editModalActividadesfinal").on("hidden.bs.modal", function () {
                    modal.editactividadesfinal.clear();
                });
            }
        },
        editobservacionactividad: {
            object: $("#editobservacionactividad-form").validate({
                submitHandler: function (form, e) {
                    $("#btnEditobservacionactividad").addLoader();
                    e.preventDefault();
                    let formData = new FormData(form);
                    $.ajax({
                        url: $(form).attr("action"),
                        type: "POST",
                        data: formData,
                        contentType: false,
                        processData: false
                    }).done(function () {
                        $("#editModalObservacionactividad").modal("hide");
                        $(".m-alert").addClass("m--hide");
                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                        modal.addactividades.reload();
                        modal.editobservacionactividad.clear();
                    }).fail(function (error) {
                        toastr.error(error.responseText, _app.constants.toastr.title.error);
                        $("#btnEditobservacionactividad").removeLoader();
                        //if (error.responseText !== null && error.responseText !== "") $("#add_form_msg_txt").html(error.responseText);
                        //else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                        //$("#add_form_msg").removeClass("m--hide").show();
                    }).always(function () {

                    });
                }
            }),
            load: function (id) {

                $("#editobservacionactividad-form input[name=Id]").val(id);
                $.ajax({
                    url: "/investigacionformativa/plantrabajodocente".proto().parseURL(),
                    type: "GET",
                    data: {
                        handler: "DetailFileActividades",
                        id: id
                    },
                }).done(function (result) {
                   // $("#editobservacionactividad-form input[name=Id]").val(result.id);
                    $("#btnEditobservacionactividad").addLoader();
                   
                }).fail(function (error) {
                    toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                    if (error.responseText !== null && error.responseText !== "") $("#add_form_msg_txt").html(error.responseText);
                    else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                    $("#add_form_msg").removeClass("m--hide").show();
                }).always(function () {

                });
            },
            show: function (id) {
                $("#btnEditobservacionactividad").removeLoader();
                $("#btnEditobservacionactividad").addLoader();
                modal.editobservacionactividad.load(id);
                $("#editModalObservacionactividad").modal("toggle");
            },
            clear: function () {
                $("#editobservacionactividad-form").find(".custom-file-label").text("Seleccionar archivo");
                modal.editobservacionactividad.object.resetForm();
            },
            events: function () {
                $("#editModalObservacionactividad").on("hidden.bs.modal", function () {
                    modal.editobservacionactividad.clear();
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
                            url: ("/investigacionformativa/plantrabajodocente?handler=Delete").proto().parseURL(),
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
                                }).then(datatable.investigacionformativaPlantrabajo.reload());
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
        observacion: function (observacion) {
            swal({
                title: "Observación del Comite",
                text: observacion,
                type: "warning",
                customClass: "swal-wide",
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
                            url: ("/investigacionformativa/plantrabajodocente?handler=EnviarPlan").proto().parseURL(),
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
                                }).then(datatable.investigacionformativaPlantrabajo.reload());
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
        enviaractividad: function (id) {
            swal({
                title: "¿Está seguro?",
                text: "La actividad sera enviada.",
                type: "warning",
                showCancelButton: true,
                confirmButtonText: "Sí, enviar",
                confirmButtonClass: "btn btn-warning m-btn m-btn--custom",
                cancelButtonText: "Cancelar",
                showLoaderOnConfirm: true,
                preConfirm: () => {
                    return new Promise((resolve) => {
                        $.ajax({
                            url: ("/investigacionformativa/plantrabajodocente?handler=EnviarPlanactividad").proto().parseURL(),
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
                                }).then(modal.addactividades.reload());
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

        observacionactividad: function (observacion) {
            swal({
                title: "Observación del acompañante",
                text: observacion,
                type: "warning",
                customClass: "swal-wide",
                confirmButtonText: "OK",
                showConfirmButton: true
            })
        },
        init: function () {
            this.create.events();
            this.edit.events();
            this.editanexo.events();
            this.editautores.events();
            this.addactividades.events();
            this.editactividades.events
            this.editactividadesfinal.events();
            this.editobservacionactividad.events();
        }

    };
    var datePickers = {
        init: function () {
            $("#fechaini").datepicker({
                clearBtn: true,
                orientation: "bottom",
                format: _app.constants.formats.datepicker
            });
            $("#fechafin").datepicker({
                clearBtn: true,
                orientation: "bottom",
                format: _app.constants.formats.datepicker
            });
        }
    };

  


    return {
        init: function () {
            arrayactividad = new Array();
            contaractividad = 0;
            datatable.init();
            search.init();
            modal.init();
            datePickers.init();
            selectareaacademica.init();
            selectlinea.init();
            selecttipoevento.init();
            selectdepartamento.init();
            selecttiporesultado.init();
            selectcarrera.init();
            selectdocente.init();
            selectanio.init();
        }
    }
}();

$(function () {
    InitApp.init();
   
})
