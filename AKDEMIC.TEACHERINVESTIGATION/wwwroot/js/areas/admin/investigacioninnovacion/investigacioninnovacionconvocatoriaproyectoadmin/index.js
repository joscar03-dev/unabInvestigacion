var InitApp = function () {
    var selectlinea = {
        init: function () {
            this.researcherMaestroLinea.init();
        },
        researcherMaestroLinea: {
            init: function () {
                this.load();
            },
            load: function () {
                $.ajax({
                    url: (`/api/maestrolinea/select/get`).proto().parseURL(),
                    type: "GET"
                }).done(function (result) {
                    $('.input_IdLinea').select2({
                        data: result,
                        maximumSelectionLength: 1,

                    });
                });
            }
        },
    };
    var selectconvocatoria = {
        init: function () {
            this.researcherInvestigacionfomentoConvocatoria.init();
        },
        researcherInvestigacionfomentoConvocatoria: {
            init: function () {
                this.load();
            },
            load: function () {
                $.ajax({
                    url: (`/api/investigacionfomentoconvocatoria/select/get`).proto().parseURL(),
                    type: "GET",
                    data: "idoficina=edcd01ea-6404-11ee-b7b1-16d13ee00159",
                }).done(function (result) {
                    $('.input_IdConvocatoria').select2({
                        data: result,
                        maximumSelectionLength: 1,

                    });
                });
            }
        },
    };
    var selectareausuario = {
        init: function () {
            this.researcherInvestigacionfomentoAreausuario.init();
        },
        researcherInvestigacionfomentoAreausuario: {
            init: function () {
                this.load();
            },
            load: function () {
                $.ajax({
                    url: (`/api/investigacionfomentoareausuario/select/get`).proto().parseURL(),
                    type: "GET"
                }).done(function (result) {
                    $('.input_lst_search').select2({
                        data: result,
                        maximumSelectionLength: 1,

                    });
                    $(".input_lst_search").on('change', function () {
                        $("#search").val(this.value)
                        datatable.investigacionfomentoConvocatoriaproyecto.reload();
                    });
                });
            }
        },
    };
    var selectcronogramaproyecto = {
        init: function () {
            this.researcherCronogramaProyecto.init();
        },
        researcherCronogramaProyecto: {
            init: function () {
                this.load();
            },
            load: function (idconvocatoriaproyecto) {
                $.ajax({
                    url: (`/api/investigacionfomentoConovocatoriaproyectocronograma/select/get`).proto().parseURL(),
                    type: "GET",
                    data: "idconvocatoriaproyecto=" + (idconvocatoriaproyecto ? idconvocatoriaproyecto : "08dbec43-6a81-40af-88f5-08e40a016e3b"),
                }).done(function (result) {
                    $('.input_IdConvocatoriaproyectocronograma').select2({
                        data: result,
                        maximumSelectionLength: 1,

                    });
                });
            }
        },
    };

    var datatable = {
        investigacionfomentoConvocatoriaproyecto: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: "/investigacioninnovacion/convocatoriaproyectoadmin".proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                 
                    data: function (data) {
                        data.handler = "Datatable";
                        data.searchValue = $("#search").val();
                        data.tipousuario = $("#tipousuario").val();
                    }
                },
                pageLength: 50,
                orderable: [],
                columns: [

                    {
                        title: "Convocatoria",
                        data: "nombreconvocatoria"
                    },
                    {
                        title: "Linea",
                        data: "nombrelinea",
                    },
                    {
                        title: "nombre",
                        data: "nombre"
                    },
                   
                    {
                        title: "presupuesto",
                        data: "presupuesto"
                    },
                    {
                        title: "Carta de Presentación (Digital)",
                        data: null,
                        width: 100,
                        render: function (data) {
                            var template = "";

                            //Carta
                            template += "<button title='Descargar Carta de Presentación' style='color:#000000;background-color:#aaeee8' ";
                            template += "class='btn  btn-cartapresentacion ";
                            template += "m-btn btn-sm  m-btn--icon-only' download";
                            template += " data-id='" + data.id + "'>";
                            template += "<i class='flaticon-file'></i> </button>";

                            return template;
                        }
                    },
                    {
                        title: "Carta de Presentación (Escaneada)",
                        data: null,
                        width: 100,
                        render: function (data) {
                            var template = "";
                            var fileUrl = `/documentos/${data.archivourlcarta}`.proto().parseURL();
                            //FileURL
                            if (data.archivourlcarta != '') {
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
                           

                            var estado = data.estado == 0 ? "EN PROCESO" : data.estado == 1 ? "ENVIADO" : data.estado == 2 ? "APROBADO" : "DESAPROBADO";
                            var label = data.estado == 0 ? "label-warning" : data.estado == 1 ? "label-success" : data.estado == 2 ? "label-success" : "label-danger"; // 
                            estado = data.retornadocente == '1' ? "ANEXAR DOCUMENTOS" : estado;
                            label = data.retornadocente == '1' ? "label-warning" : label;
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
                        width: 110,
                        orderable: false,
                        render: function (data) {
                            var template = "";

                            if (data.tipousuario == "R") {
                                template += "<button type='button' title='Derivar Aprobación/Observación' "
                                template += " class='btn btn-danger btn-editobservacion ";
                                template += " m-btn btn-sm  m-btn--icon-only' ";
                                template += ` data-id='` + data.id + `' `
                                template += ` onclick = ' $("#editobservacion-form input[name=IdProyectoflujo]").val("` + data.idProyectoflujo + `");`;
                                template += ` $("#editobservacion-form input[name=IdArea]").val("` + data.idArea + `"); `;
                                template += ` $("#editobservacion-form input[name=Retornadocente]").val("` + data.retornadocente + `");' >`;
                                template += " <i class='la la-send'></i></button>";
                            } else {

                                if (data.estado == "2") {
                                    template += "<button title='Cronograma de Activiades' class='btn btn-danger btn-actividades ";
                                    template += " m-btn btn-sm  m-btn--icon-only' ";
                                    template += " data-id=' " + data.id + "' > ";
                                    template += "<i class='la la-calendar'></i></button>";
                                }
                            
                            //Subur Carta

                                if (data.retornadocente == "1") {
                                    template += "<button title='Anexar documentos' style='color:#000000;background-color:#aaeee8' ";
                                    template += "class='btn btn-editanexofaltante ";
                                    template += "m-btn btn-sm  m-btn--icon-only' ";
                                    template += " data-id='" + data.id + "'>";
                                    template += "<i class='flaticon-file'></i> Anexos</button>";

                                    template += "<button title='Enviar Anexos del proyecto' class='btn btn-warning btn-enviaranexo ";
                                    template += " m-btn btn-sm  m-btn--icon-only' ";
                                    template += " data-id='" + data.id + "' > ";
                                    template += "<i class='la la-send'></i></button>";
                                }
                            //Enviar
                            if (data.archivourlcarta != '' && (data.estado == "0" || data.estado == "3")) {
                                template += "<button title='Enviar el proyecto' class='btn btn-warning btn-enviar ";
                                template += " m-btn btn-sm  m-btn--icon-only' ";
                                template += " data-id='" + data.id + "' > ";
                                template += "<i class='la la-send'></i></button>";
                                }

                                if (data.estado == "0") {
                                    template += "<button title='Subir carta' style='color:#000000;background-color:#aaeee8' ";
                                    template += "class='btn btn-editanexo ";
                                    template += "m-btn btn-sm  m-btn--icon-only' ";
                                    template += " data-id='" + data.id + "'>";
                                    template += "<i class='flaticon-file'></i> Subir Carta</button>";
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
                                }
                               
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

                $("#data-table").on('click', '.btn-editobservacion', function () {
                    var id = $(this).data("id");
                   // modal.editobservacion.show(id);
                    modal.addobservaciones.show(id);  
                });

                $("#data-table").on('click', '.btn-editanexofaltante', function () {
                    var id = $(this).data("id");
                  
                    // modal.editobservacion.show(id);
                    modal.addanexofaltantes.show(id);
                });

                $("#data-table").on('click', '.btn-actividades', function () {
                    var id = $(this).data("id");

                    // modal.editobservacion.show(id);
                    $("#newcronograma-form input[name=IdConvocatoriaproyecto]").val(id);
                    $("#editcronograma-form select[name=IdConvocatoriaproyectocronograma]").empty();
                    selectcronogramaproyecto.researcherCronogramaProyecto.load(id);
                    $("#editcronograma-form select[name=IdConvocatoriaproyectocronograma]").trigger("change");
                    modal.addactividades.show(id);
                });

                $("#data-table").on('click', '.btn-delete', function () {
                    var id = $(this).data("id");
                    modal.delete(id);

                });

                $("#data-table").on('click', '.btn-cartapresentacion', function () {
                    var id = $(this).data("id");
                    modal.pdfcartapresentacion(id);

                });

                $("#data-table").on('click', '.btn-enviar', function () {
                    var id = $(this).data("id");
                    modal.enviar(id);

                });

                $("#data-table").on('click', '.btn-enviaranexo', function () {
                    var id = $(this).data("id");
                    modal.enviaranexo(id);

                });
                $("#data-table").on('click', '.btn-observacion', function () {
                    var id = $(this).data("id");
                    modal.observacion(id);

                });
                
            }
        },
        init: function () {
            this.investigacionfomentoConvocatoriaproyecto.init();
        }
    };
    var search = {
        init: function () {
            $("#search").on('change', function () {
                datatable.investigacionfomentoConvocatoriaproyecto.reload();
            });
            $("#search").trigger("change");

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

                        datatable.investigacionfomentoConvocatoriaproyecto.reload();
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
                    url: `/investigacioninnovacion/convocatoriaproyecto`.proto().parseURL(),
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
        editobservacion: {
            object: $("#editobservacion-form").validate({
                submitHandler: function (form, e) {
                    $("#btnEditobservacion").addLoader();
                    e.preventDefault();
                    $.ajax({
                        url: $(form).attr("action"),
                        type: "POST",
                        data: $(form).serialize()
                    }).done(function () {
                        $(".modal").modal("hide");
                        $(".m-alert").addClass("m--hide");
                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);

                        datatable.investigacionfomentoConvocatoriaproyecto.reload();
                        modal.editaobservacion.clear();
                    }).fail(function (error) {
                        toastr.error(error.responseText, _app.constants.toastr.title.error);
                        $("#btnEditobservacion").removeLoader();
                        //if (error.responseText !== null && error.responseText !== "") $("#add_form_msg_txt").html(error.responseText);
                        //else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                        //$("#add_form_msg").removeClass("m--hide").show();
                    }).always(function () {

                    });
                }
            }),
            load: function (id) {
                $.ajax({
                    url: `/investigacioninnovacion/convocatoriaproyecto`.proto().parseURL(),
                    type: "GET",
                    data: {
                        handler: "DetailPlanObservacion",
                        id: id,
                        idArea: $("#search").val(),
                    },
                }).done(function (result) {
                    $("#editobservacion-form input[name=Id]").val(result.id);
                    $("#editobservacion-form").find(".custom-file-label").text("Seleccionar archivo");
                    $("#editobservacion-form").find(".custom-file").append('<div class="form-control-feedback">*Seleccionar en caso sea necesario reemplazar el documento vigente.</div>');
                }).fail(function (error) {
                    toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                    if (error.responseText !== null && error.responseText !== "") $("#add_form_msg_txt").html(error.responseText);
                    else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                    $("#add_form_msg").removeClass("m--hide").show();
                }).always(function () {

                });
            },
            show: function (id) {
                $("#btnEditobservacion").removeLoader();
                modal.editobservacion.load(id);
                $("#editModalObservacion").modal("toggle");
            },
            clear: function () {
                $("#editobservacion-form").find(".custom-file-label").text("Seleccionar archivo");
                modal.editobservacion.object.resetForm();
            },
            events: function () {
                $("#editModalObservacion").on("hidden.bs.modal", function () {
                    modal.editobservacion.clear();
                });
            }
        },
        editanexofaltante: {
            object: $("#editanexofaltante-form").validate({
                submitHandler: function (form, e) {
                    $("#btnEditanexofaltante").addLoader();
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

                        datatable.investigacionfomentoConvocatoriaproyecto.reload();
                        modal.editanexofaltante.clear();
                    }).fail(function (error) {
                        toastr.error(error.responseText, _app.constants.toastr.title.error);
                        $("#btnEditanexofaltante").removeLoader();
                        //if (error.responseText !== null && error.responseText !== "") $("#add_form_msg_txt").html(error.responseText);
                        //else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                        //$("#add_form_msg").removeClass("m--hide").show();
                    }).always(function () {

                    });
                }
            }),
            show: function (id) {
                $("#btnEditanexofaltante").removeLoader();
                modal.editanexofaltante.load(id);
                $("#editModalAnexofaltante").modal("toggle");
            },
            clear: function () {
                $("#editanexofaltante-form").find(".custom-file-label").text("Seleccionar archivo");
                modal.editanexofaltante.object.resetForm();
            },
            events: function () {
                $("#editModalAnexofaltante").on("hidden.bs.modal", function () {
                    modal.editanexofaltante.clear();
                });
            }
        },
        editactividad: {
            object: $("#editactividad-form").validate({
                submitHandler: function (form, e) {
                    $("#btnEdita").addLoader();
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

                        datatable.investigacionfomentoConvocatoriaproyecto.reload();
                        modal.editactividad.clear();
                    }).fail(function (error) {
                        toastr.error(error.responseText, _app.constants.toastr.title.error);
                        $("#btnEditactividad").removeLoader();
                        //if (error.responseText !== null && error.responseText !== "") $("#add_form_msg_txt").html(error.responseText);
                        //else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                        //$("#add_form_msg").removeClass("m--hide").show();
                    }).always(function () {

                    });
                }
            }),
            show: function (id) {
                $("#btnEditactividad").removeLoader();
                modal.editactividad.load(id);
                $("#editModalActividad").modal("toggle");
            },
            clear: function () {
                $("#editactividad-form").find(".custom-file-label").text("Seleccionar archivo");
                modal.editactividad.object.resetForm();
            },
            events: function () {
                $("#editModalActividad").on("hidden.bs.modal", function () {
                    modal.editactividad.clear();
                });
            }
        },

        addobservaciones: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: "/investigacioninnovacion/convocatoriaproyecto".proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        data.handler = "DetailPlanObservacion";
                        data.id = $("#editobservacion-form input[name=Id]").val(); 
                        data.idArea= $("#search").val();
                    }
                },
                pageLength: 50,
                orderable: [],
                columns: [

                    {
                        title: "Indicador",
                        data: "nombreindicador"
                    },
                    {
                        title: "Si",
                        data: null,                       
                        width: 50,
                        render: function (data) {
                            var template = "";
                                
                            template += '<input required style="width:20px" value=1 type="radio" class="form-control  m-input"  name="valores[' + contador + ']" id="valores[' + contador +']">'; //"afirmativo' + contador  +'"
                            template += '<input  style="width:50px" value="' + data.idListaverificacion +'" type="hidden" class="form-control  m-input" name="IdListaverificaciones[' + contador + ']" id="IdListaverificaciones[' + contador + ']">';
                            template += '<input  style="width:50px" value="' + data.idProyecto + '" type="hidden" class="form-control  m-input" name="IdConvocatoriaproyectos[' + contador + ']" id="IdConvocatoriaproyectos[' + contador + ']">';
                            template += '<input  style="width:50px" value="' + data.idArea + '" type="hidden" class="form-control  m-input" name="IdAreas[' + contador + ']" id="IdAreas[' + contador + ']">';
                            template += '<input  style="width:50px" value="' + data.idIndicador + '" type="hidden" class="form-control  m-input" name="IdIndicadores[' + contador + ']" id="IdIndicadores[' + contador + ']">';
                            template += '<input  style="width:50px" value="' + data.retornadocente + '" type="hidden" class="form-control  m-input" name="Retornadocentes[' + contador + ']" id="Retornadocentes[' + contador + ']">';
                            template += '<input  style="width:50px" value="' + data.idProyectoflujo + '" type="hidden" class="form-control  m-input" name="IdProyectoflujos[' + contador + ']" id="IdProyectoflujos[' + contador + ']">';

                            
                            return template;
                        }
                    },{
                        title: "NO",
                        data: null,
                        width: 50,
                        render: function (data) {
                            var template = "";
                            
                            template = '<input required style="width:20px" value=0 type="radio" class="form-control  m-input" name="valores[' + contador + ']" id="valores[' + contador +']">';
                            return template;
                        }
                    }, {
                        title: "Puntos",
                        data: null,
                        width: 50,
                        render: function (data) {
                            var template = "";

                            template = '<input  style="width:50px" value=0 type="text" class="form-control  m-input" name="puntajes[' + contador + ']" id="valores[' + contador + ']">';
                            return template;
                        }
                    }, {
                        title: "Observaciones",
                        data: null,
                        render: function (data) {
                            var template = "";

                            template = '<input  style="width:100%" value="" type="text" class="form-control  m-input" name="observaciones[' + contador + ']" id="valores[' + contador + ']">';
                            contador = contador + 1;
                            return template;
                        }
                    }
                    
                ],
                rowCallback: function (row, data) {
                   

                },
            },
            load: function (id) {
                $("#editobservacion-form input[name=Id]").val(id);
                if (this.object == null) {
                    this.object = $("#data-tableobservaciones").DataTable(this.options);
                } else {
                    this.reload();
                }
                contador = 0;
                modal.addobservaciones.events();
             

            },
            reload: function () {
                this.object.ajax.reload();
                arrayactividad = new Array();
                contaractividad = 0;
            },
            show: function (id) {
                //$("#btnEdit").removeLoader();

                modal.addobservaciones.load(id);
                $("#editModalObservacion").modal("toggle");
            },
            clear: function () {
                // modal.addobservaciones.object.resetForm();
            },
            events: function () {

                $("#editModalObservacion").on("hidden.bs.modal", function () {
                    modal.addobservaciones.clear();
                });
                $("#editModalObservacion").on("hidden.bs.modal", function () {
                    modal.addobservaciones.clear();
                });


                $("#data-tableobservaciones").on('click', '.btn-editactividad', function () {

                    var id = $(this).data("id");
                    modal.editactividades.show(id);
                });
                $("#data-tableobservaciones").on('click', '.btn-editactividadfinal', function () {

                    var id = $(this).data("id");
                    modal.editactividadesfinal.show(id);
                });
                $("#data-tableobservaciones").on('click', '.btn-editobservacionactividad', function () {

                    var id = $(this).data("id");
                    modal.editobservacionactividad.show(id);
                });

                $("#data-tableobservaciones").on('click', '.btn-enviaractividad', function () {
                    var id = $(this).data("id");
                    modal.enviaractividad(id);

                });
                $("#data-tableobservaciones").on('click', '.btn-observacionactividad', function () {
                    var id = $(this).data("id");
                    modal.observacionactividad(id);

                });


            }
        },
        addanexofaltantes: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: "/investigacioninnovacion/convocatoriaproyecto".proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        data.handler = "DetailPlanAnexofaltante";
                        data.id = $("#editanexofaltante-form input[name=Id]").val();
                    }
                },
                pageLength: 50,
                orderable: [],
                columns: [

                    {
                        title: "Requisiro",
                        data: "nombreRequisito"
                    },
                    {
                        title: "Descargar Anexo",
                        data: null,
                        width: 50,
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
                    }, {
                        title: "Anexo Subido",
                        data: null,
                        render: function (data) {
                            var template = "";

                            var fileUrl = `/documentos/${data.archivourlproyecto}`.proto().parseURL();
                            //FileURL
                            if (data.archivourlproyecto != '') {
                                template += `<a href="${fileUrl}"  `;
                                template += "class='btn btn-success ";
                                template += "m-btn btn-sm m-btn--icon' download>";
                                template += "<span><i class='flaticon-file'></i></a> ";
                            }
                            
                            return template;
                        }

                    },
                    {
                        title: "Opciones",
                        data: null,
                        width: 50,
                        render: function (data) {
                            var template = "";

                           
                            //Edit
                            template += "<button type='button' title='Agregar Anexo' ";
                            template += ` onclick=' $("#addanexo-form input[name=IdRequisito]").val("` + data.idRequisito + `");$("#addanexo-form input[name=Id]").val("` + data.idProyectorequisito +`");' class='btn btn-info btn-addAnexo "`;
                            template += "m-btn btn-sm  m-btn--icon-only' ";
                            template += " data-id='" + data.idProyectorequisito + "'>";
                            template += "<i class='la la-edit'></i></button>";
                            return template;
                        }
                    }

                ],
                rowCallback: function (row, data) {


                },
            },
            load: function (id) {
                
                $("#editanexofaltante-form input[name=Id]").val(id);
                if (this.object == null) {
                    this.object = $("#data-tableanexofaltantes").DataTable(this.options);
                } else {
                    this.reload();
                }
                contador2 = 0;
                modal.addanexofaltantes.events();


            },
            reload: function () {
                this.object.ajax.reload();
               
            },
            show: function (id) {
                //$("#btnEdit").removeLoader();
              
                modal.addanexofaltantes.load(id);
                $("#editModalAnexofaltante").modal("toggle");
            },
            clear: function () {
                // modal.addobservaciones.object.resetForm();
            },
            events: function () {

                $("#editModalAnexofaltante").on("hidden.bs.modal", function () {
                    modal.addanexofaltantes.clear();
                });
                $("#editModalAnexofaltante").on("hidden.bs.modal", function () {
                    modal.addanexofaltantes.clear();
                });
                $("#data-tableanexofaltantes").on('click', '.btn-addAnexo', function () {
                    var id = $(this).data("id");
                    modal.addanexo.show(id);
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
                    url: "/investigacioninnovacion/convocatoriaproyecto".proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        data.handler = "DetailPlanActividades";
                        data.id = $("#editactividad-form input[name=Id]").val();
                    }
                },
                pageLength: 50,
                orderable: [],
                columns: [

                    {
                        title: "Título",
                        data: "titulo"
                    },
                    {
                        title: "Meses",
                        data: "nombremeses"
                    }, {
                        title: "Concluido",
                        data: null,
                        render: function (data) {
                            var template = "";
                            if (data.estado == "1") {
                                template += "SI";

                            } else {
                                template += "NO";

                            }
                           
                            return template
                        }
                    }, {
                        title: "Archivo Adjunto",
                        data: null,
                        width: 50,
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
                        title: "Opciones",
                        data: null,
                        width: 50,
                        render: function (data) {
                            var template = "";

                            template += "<button  type='button' title='Asignar cronograma' class='btn btn-danger btn-addCronograma ";
                            template += " m-btn btn-sm  m-btn--icon-only' ";
                            template += ` onclick='$("#addactividad-form input[name=Id]").val("` + data.id + `");' `;
                            template += " data-id='" + data.id + "' data-idconvocatoriaproyecto='" + data.idConvocatoriaproyecto  +"'> ";
                            template += "<i class='la la-calendar'></i></button>";
                           
                          
                            //Edit
                            template += "<button type='button' title='Cumplimiento' ";
                            template += ` onclick='$("#addactividad-form input[name=Id]").val("` + data.id + `");`
                            template += ` $("#addactividad-form input[name=titulo]").val("` + data.titulo + `");`
                            template += ` $("#addactividad-form input[name=estado]").prop("checked",` + (data.estado == 1 ? true : false) + `) ;' class='btn btn-info btn-addActividad `;
                            template += " m-btn btn-sm  m-btn--icon-only' ";
                            template += " data-id='" + data.id + "'>";
                            template += "<i class='la la-edit'></i></button>";
                            return template;
                        }
                    }

                ],
                rowCallback: function (row, data) {


                },
            },
            load: function (id) {

                $("#editactividad-form input[name=Id]").val(id);
                if (this.object == null) {
                    this.object = $("#data-tableactividades").DataTable(this.options);
                } else {
                    this.reload();
                }
                contador2 = 0;
                modal.addactividades.events();


            },
            reload: function () {
                this.object.ajax.reload();

            },
            show: function (id) {
                //$("#btnEdit").removeLoader();

                modal.addactividades.load(id);
                $("#editModalActividad").modal("toggle");
            },
            clear: function () {
                // modal.addobservaciones.object.resetForm();
            },
            events: function () {

                $("#editModalActividad").on("hidden.bs.modal", function () {
                    modal.addactividades.clear();
                });
                $("#editModalActividad").on("hidden.bs.modal", function () {
                    modal.addactividades.clear();
                });
                $("#data-tableactividades").on('click', '.btn-addActividad', function () {
                    var id = $(this).data("id");
                    modal.addactividad.show(id);
                });
                $("#data-tableactividades").on('click', '.btn-addCronograma', function () {
                    var id = $(this).data("id");
                    var idconvocatoriaproyecto = $(this).data("idconvocatoriaproyecto");
                    $("#editcronograma-form select[name=IdConvocatoriaproyectocronograma]").empty();
                    selectcronogramaproyecto.researcherCronogramaProyecto.load(idconvocatoriaproyecto);
                    $("#editcronograma-form select[name=IdConvocatoriaproyectocronograma]").trigger("change");
                    $("#editcronograma-form input[name=Id]").val(id); 
                    modal.addcronogramas.show(id);
                });

            }
        },
        addcronogramas: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: "/investigacioninnovacion/convocatoriaproyecto".proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        data.handler = "DetailPlanCronogramas";
                        data.id = $("#editcronograma-form input[name=Id]").val();
                    }
                },
                pageLength: 50,
                orderable: [],
                columns: [

                    {
                        title: "Meses",
                        data: "nombremes"
                    },
                    {
                        title: "Opciones",
                        data: null,
                        width: 50,
                        render: function (data) {
                            var template = "";


                            //Edit
                            template += "<button type='button' title='Elimnar mes' ";
                            template += ` onclick='$("#addactividad-form input[name=Id]").val("` + data.id + `");' class='btn btn-info btn-delete "`;
                            template += "m-btn btn-sm  m-btn--icon-only' ";
                            template += " data-id='" + data.id + "'>";
                            template += "<i class='la la-trash'></i></button>";
                            return template;
                        }
                    }

                ],
                rowCallback: function (row, data) {


                },
            },
            load: function (id) {

                $("#editcronogrma-form input[name=Id]").val(id);
                if (this.object == null) {
                    this.object = $("#data-tablecronogramas").DataTable(this.options);
                } else {
                    this.reload();
                }
                contador2 = 0;
                modal.addcronogramas.events();


            },
            reload: function () {
                this.object.ajax.reload();

            },
            show: function (id) {
                //$("#btnEdit").removeLoader();

                modal.addcronogramas.load(id);
                $("#editModalCronograma").modal("toggle");
            },
            clear: function () {
                // modal.addobservaciones.object.resetForm();
            },
            events: function () {

                $("#editModalCronograma").on("hidden.bs.modal", function () {
                    modal.addcronogramas.clear();
                });
                $("#editModalCronograma").on("hidden.bs.modal", function () {
                    modal.addcronogramas.clear();
                });
                $("#data-tablecronograma").on('click', '.btn-addCronogrma', function () {
                    var id = $(this).data("id");
                    modal.addcronogrma.show(id);
                });


            }
        },
        addactividad: {
            object: $("#addactividad-form").validate({
                submitHandler: function (form, e) {
                    $("#btnAddactividad").addLoader();
                    e.preventDefault();
                    let formData = new FormData(form);
                    $.ajax({
                        url: $(form).attr("action"),
                        type: "POST",
                        data: formData,
                        contentType: false,
                        processData: false
                    }).done(function () {
                        $("#addModalActividad").modal("hide");
                        $(".m-alert").addClass("m--hide");
                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);

                        modal.addactividades.reload();
                        modal.addactividad.clear();
                    }).fail(function (error) {
                        toastr.error(error.responseText, _app.constants.toastr.title.error);
                        $("#btnAddactividad").removeLoader();
                        //if (error.responseText !== null && error.responseText !== "") $("#add_form_msg_txt").html(error.responseText);
                        //else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                        //$("#add_form_msg").removeClass("m--hide").show();
                    }).always(function () {

                    });
                }
            }),
            show: function (id) {
                $("#btnAddactividad").removeLoader();
                $("#addModalActividad").modal("toggle");
            },
            clear: function () {
                $("#addactividad-form").find(".custom-file-label").text("Seleccionar archivo");
                modal.addactividad.object.resetForm();
            },
            events: function () {
                $("#addModalActividad").on("hidden.bs.modal", function () {
                    modal.addactividad.clear();
                });
            }
        },
        addanexo: {
            object: $("#addanexo-form").validate({
                submitHandler: function (form, e) {
                    $("#btnAddanexo").addLoader();
                    e.preventDefault();
                    let formData = new FormData(form);
                    $.ajax({
                        url: $(form).attr("action"),
                        type: "POST",
                        data: formData,
                        contentType: false,
                        processData: false
                    }).done(function () {
                        $("#addModalAnexo").modal("hide");
                        $(".m-alert").addClass("m--hide");
                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);

                        modal.addanexofaltantes.reload();
                        modal.addanexo.clear();
                    }).fail(function (error) {
                        toastr.error(error.responseText, _app.constants.toastr.title.error);
                        $("#btnAddanexo").removeLoader();
                        //if (error.responseText !== null && error.responseText !== "") $("#add_form_msg_txt").html(error.responseText);
                        //else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                        //$("#add_form_msg").removeClass("m--hide").show();
                    }).always(function () {

                    });
                }
            }),
            show: function (id) {
                $("#btnAddanexo").removeLoader();
                $("#addModalAnexo").modal("toggle");
            },
            clear: function () {
                $("#addanexo-form").find(".custom-file-label").text("Seleccionar archivo");
                modal.addanexo.object.resetForm();
            },
            events: function () {
                $("#addModalAnexo").on("hidden.bs.modal", function () {
                    modal.addanexo.clear();
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
                        datatable.investigacionfomentoConvocatoriaproyecto.reload();
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
                    $.ajax({
                        url: $(form).attr("action"),
                        type: "POST",
                        data: $(form).serialize()
                    }).done(function () {
                        $(".modal").modal("hide");
                        $(".m-alert").addClass("m--hide");
                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                        datatable.investigacionfomentoConvocatoriaproyecto.reload();
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
                    url: "/investigacioninnovacion/convocatoriaproyecto".proto().parseURL(),
                    type: "GET",
                    data: {
                        handler: "Detail",
                        id: id
                    },
                }).done(function (result) {
                    $("#edit-form input[name=Id]").val(result.id);
                    $("#edit-form select[name=IdConvocatoria]").val(result.idConvocatoria);
                    $("#edit-form select[name=IdConvocatoria]").trigger("change");
                    $("#edit-form select[name=IdLinea]").val(result.idLinea);
                    $("#edit-form select[name=IdLinea]").trigger("change");
                    $("#edit-form input[name=nombre]").val(result.nombre);
                    $("#edit-form input[name=nromeses]").val(result.nromeses);

                    $("#edit-form input[name=presupuesto]").val(result.presupuesto);
                    $("#edit-form textarea[name=objetivoprincipal]").val(result.objetivoprincipal);
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


            }
        },
        newcronograma: {
            object: $("#newcronograma-form").validate({
                submitHandler: function (form, e) {
                    $("#btnNewcronograma").addLoader();
                    e.preventDefault();
                    let formData = new FormData(form);
                    $.ajax({
                        url: $(form).attr("action"),
                        type: "POST",
                        data: formData,
                        contentType: false,
                        processData: false
                    }).done(function () {
                        $("#newModalCronograma").modal("hide");
                        $(".m-alert").addClass("m--hide");
                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                        modal.addactividades.reload();
                        id = $("#newcronograma-form input[name=IdConvocatoriaproyecto]").val();
                       
                        modal.newcronograma.clear();
                        $("#newcronograma-form input[name=IdConvocatoriaproyecto]").val(id)
                        $("#btnNewcronograma").removeLoader();
                    }).fail(function (error) {
                        toastr.error(error.responseText, _app.constants.toastr.title.error);
                        $("#btnNewcronograma").removeLoader();
                        //if (error.responseText !== null && error.responseText !== "") $("#add_form_msg_txt").html(error.responseText);
                        //else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                        //$("#add_form_msg").removeClass("m--hide").show();
                    }).always(function () {

                    });
                }
            }),
            show: function () {
                this.events();
                $("#newModalCronograma").removeLoader();
                $("#newModalCronograma").modal("toggle");
            },
            clear: function () {
                modal.newcronograma.object.resetForm();
            },
            events: function () {

                $("#newModalCronograma").on("hidden.bs.modal", function () {
                    id = $("#newcronograma-form input[name=IdConvocatoriaproyecto]").val();
                    modal.newcronograma.clear();
                     $("#newcronograma-form input[name=IdConvocatoriaproyecto]").val(id);
                });

                $("#newcronograma-form select[name=IdAreaacademica]").on("change", function (e) {
                    $("#newcronograma-form select[name=IdLinea]").empty();
                    selectlinea.researcherMaestroLinea.load($("#newcronograma-form select[name=IdConvocatoriaproyecto]").val());
                    $("#newcronograma-form select[name=IdLinea]").trigger("change");
                });



            }
        },
        newactividadcronograma: {
            object: $("#editcronograma-form").validate({
                submitHandler: function (form, e) {
                    $("#btnEditcronograma").addLoader();
                    e.preventDefault();
                    let formData = new FormData(form);
                    $.ajax({
                        url: $(form).attr("action"),
                        type: "POST",
                        data: formData,
                        contentType: false,
                        processData: false
                    }).done(function () {
                       // $("#editModalCronograma").modal("hide");
                        $(".m-alert").addClass("m--hide");
                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                        modal.addcronogramas.reload();
                        modal.addactividades.reload();
                        $("#editcronograma-form select[name=IdConvocatoriaproyectocronograma]").val("");
                        $("#editcronograma-form select[name=IdConvocatoriaproyectocronograma]").trigger("change");

//                        modal.newactividadcronograma.clear();
                        $("#btnEditcronograma").removeLoader();
                    }).fail(function (error) {
                        toastr.error(error.responseText, _app.constants.toastr.title.error);
                        $("#btnEditcronograma").removeLoader();
                        //if (error.responseText !== null && error.responseText !== "") $("#add_form_msg_txt").html(error.responseText);
                        //else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                        //$("#add_form_msg").removeClass("m--hide").show();
                    }).always(function () {

                    });
                }
            }),
            show: function () {
                this.events();
                $("#editModalCronograma").removeLoader();
                $("#editModalCronograma").modal("toggle");
            },
            clear: function () {
                modal.newactividadcronograma.object.resetForm();
            },
            events: function () {

                $("#editModalCronograma").on("hidden.bs.modal", function () {
                    id = $("#editcronograma-form input[name=IdConvocatoriaproyecto]").val();
                    modal.newactividadcronograma.clear();
                    $("#editcronograma-form input[name=IdConvocatoriaproyecto]").val(id);
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
                            url: ("/investigacioninnovacion/convocatoriaproyecto?handler=Delete").proto().parseURL(),
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
                                }).then(datatable.investigacionfomentoConvocatoriaproyecto.reload());
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
                customClass:"swal-wide",
                confirmButtonText: "OK",
                showConfirmButton: true
            })
        },
        enviar: function (id) {
            swal({
                title: "¿Está seguro?",
                text: "El proyecto sera enviado.",
                type: "warning",
                showCancelButton: true,
                confirmButtonText: "Sí, enviar",
                confirmButtonClass: "btn btn-warning m-btn m-btn--custom",
                cancelButtonText: "Cancelar",
                showLoaderOnConfirm: true,
                preConfirm: () => {
                    return new Promise((resolve) => {
                        $.ajax({
                            url: ("/investigacioninnovacion/convocatoriaproyecto?handler=EnviarProyecto").proto().parseURL(),
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
                                    text: "El proyecto ha sido enviado para su revisión",
                                    confirmButtonText: "Excelente"
                                }).then(datatable.investigacionfomentoConvocatoriaproyecto.reload());
                            },
                            error: function (errormessage) {
                                swal({
                                    type: "error",
                                    title: "Error",
                                    confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                    confirmButtonText: "Entendido",
                                    text: "El proyecto tubo un problema de envio"
                                });
                            }
                        });
                    });
                },
                allowOutsideClick: () => !swal.isLoading()
            });
        },
     
        enviaranexo: function (id) {
            swal({
                title: "¿Está seguro?",
                text: "Esta seguro enviar los anexos.",
                type: "warning",
                showCancelButton: true,
                confirmButtonText: "Sí, enviar",
                confirmButtonClass: "btn btn-warning m-btn m-btn--custom",
                cancelButtonText: "Cancelar",
                showLoaderOnConfirm: true,
                preConfirm: () => {
                    return new Promise((resolve) => {
                        $.ajax({
                            url: ("/investigacioninnovacion/convocatoriaproyecto?handler=EnviarAnexoproyecto").proto().parseURL(),
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
                                    text: "El proyecto ha sido enviado para su revisión",
                                    confirmButtonText: "Excelente"
                                }).then(datatable.investigacionfomentoConvocatoriaproyecto.reload());
                            },
                            error: function (errormessage) {
                                swal({
                                    type: "error",
                                    title: "Error",
                                    confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                    confirmButtonText: "Entendido",
                                    text: "El proyecto tubo un problema de envio"
                                });
                            }
                        });
                    });
                },
                allowOutsideClick: () => !swal.isLoading()
            });
        },

        pdfcartapresentacion: function (id) {
           
                        $.ajax({
                            url: ("/investigacioninnovacion/convocatoriaproyecto?handler=Pdfcartapresentacion").proto().parseURL(),
                            type: "POST",
                            data: {
                                id: id
                            },
                            beforeSend: function (xhr) {
                                xhr.setRequestHeader("XSRF-TOKEN",
                                    $('input:hidden[name="__RequestVerificationToken"]').val());
                            },
                            success: function (result) {

                                //var file = Blob([result.file], { type: 'application/pdf' });
                               // var fileurl = URL.createObjectURL(file);
                                window.open(result.file,"cartapresentacion","width=600px;height=600px");
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
                  
        },
        init: function () {
            this.create.events();
            this.edit.events();
            this.editanexo.events();
            this.editobservacion.events();
            this.editanexofaltante.events();
            this.editactividad.events();
            this.newcronograma.events();
            this.addanexo.events();
            this.newactividadcronograma.events();
        }

    };
    return {
        init: function () {
            datatable.init();
            modal.init();
            selectlinea.init();
            selectconvocatoria.init();
            selectareausuario.init();
            selectcronogramaproyecto.init();
            search.init();
        }
    }
}();

$(function () {
    InitApp.init();
})
