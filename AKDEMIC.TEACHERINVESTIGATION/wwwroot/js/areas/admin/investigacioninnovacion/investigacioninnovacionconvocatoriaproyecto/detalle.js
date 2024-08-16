var InitApp = function () {
    var selectcronogramaproyecto = {
        init: function () {
            this.researcherCronogramaProyecto.init();
        },
        researcherCronogramaProyecto: {
            init: function () {
                this.load();
            },
            load: function (idconvocatoriaproyecto = "edcd01ea-6404-11ee-b7b1-16d13ee00159") {
                $.ajax({
                    url: (`/api/investigacionfomentoConovocatoriaproyectocronograma/select/get`).proto().parseURL(),
                    type: "GET",
                    data: "idconvocatoriaproyecto=" + (idconvocatoriaproyecto ? idconvocatoriaproyecto : "edcd01ea-6404-11ee-b7b1-16d13ee00159"),
                }).done(function (result) {
                    $('.input_IdConvocatoriaproyectocronograma').select2({
                        data: result,
                        maximumSelectionLength: 1,

                    });
                });
            }
        },
    };
    var selectgastotipo = {
        init: function () {
            this.researcherGastotipo.init();
        },
        researcherGastotipo: {
            init: function () {
                this.load();
            },
            load: function (idconvocatoriaproyecto = "edcd01ea-6404-11ee-b7b1-16d13ee00159") {
                $.ajax({
                    url: (`/api/investigacionfomentogastotipo/select/get`).proto().parseURL(),
                    type: "GET",
                    data: "idconvocatoriaproyecto=" + (idconvocatoriaproyecto ? idconvocatoriaproyecto : "edcd01ea-6404-11ee-b7b1-16d13ee00159"),
                }).done(function (result) {
                    $('.input_IdGastotipo').select2({
                        data: result,
                        maximumSelectionLength: 1,

                    });
                });
            }
        },
    };
    var selectunidadmedida = {
        init: function () {
            this.researcherUnidadmedida.init();
        },
        researcherUnidadmedida: {
            init: function () {
                this.load();
            },
            load: function (idconvocatoriaproyecto = "edcd01ea-6404-11ee-b7b1-16d13ee00159") {
                $.ajax({
                    url: (`/api/investigacionfomentounidadmedida/select/get`).proto().parseURL(),
                    type: "GET",
                    data: "idconvocatoriaproyecto=" + (idconvocatoriaproyecto ? idconvocatoriaproyecto : "edcd01ea-6404-11ee-b7b1-16d13ee00159"),
                }).done(function (result) {
                    $('.input_IdUnidadmedida').select2({
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
                    url: "/investigacioninnovacion/convocatoriaproyecto".proto().parseURL(),
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
                        width: 120,
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
                                    template += "<i class='flaticon-file'></i> Subir Anexos</button>";

                                    template += "<button title='Enviar Anexos del proyecto' class='btn btn-warning btn-enviaranexo ";
                                    template += " m-btn btn-sm  m-btn--icon-only' ";
                                    template += " data-id='" + data.id + "' > ";
                                    template += "<i class='la la-send'></i> Enviar Anexos</button>";

                                    var detailUrl = `/Investigacionfomento/convocatoriaproyecto/detalle?id=`+data.id;
                                    template += `<a class='btn btn-success m-btn btn-sm m-btn--icon-only' href='${detailUrl}' `;
                                    template += `<span><i class='la la-send'></i> Enviar Anexos</span></span></a> `;
                                }
                            //Enviar
                            if (data.archivourlcarta != '' && (data.estado == "0" || data.estado == "3")) {
                                template += "<button title='Enviar el proyecto' class='btn btn-warning btn-enviar ";
                                template += " m-btn btn-sm  m-btn--icon-only' ";
                                template += " data-id='" + data.id + "' > ";
                                template += "<i class='la la-send'></i> Enviar Proyecto</button>";
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

                                    template += "<button type='button' title='Miembros'  ";
                                    template += "class='btn btn-success btn-editmiembro ";
                                    template += "m-btn btn-sm  m-btn--icon-only' ";
                                    template += " data-id='" + data.id + "'>";
                                    template += "<i class='la la-user'></i> Miembros</button>";
                                    //Edit
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

                $("#data-table").on('click', '.btn-editmiembro', function () {
                    var id = $(this).data("id");
                    modal.addmiembro.show(id);
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
        editProblema: {
            object: $("#edit-form-problema").validate({
                submitHandler: function (form, e) {
                    $("#btnEditProblema").addLoader();
                    e.preventDefault();
                    let formData = new FormData(form);
                    $.ajax({
                        url: $(form).attr("action"),
                        type: "POST",
                        data: formData,
                        contentType: false,
                        processData: false
                    }).done(function () {
                        $(".m-alert").addClass("m--hide");
                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                        $("#btnEditProblema").removeLoader();
                        modal.desbloqueartab();
                    }).fail(function (error) {
                        toastr.error(error.responseText, _app.constants.toastr.title.error);
                        $("#btnEditProblema").removeLoader();
                        modal.desbloqueartab();
                        //if (error.responseText !== null && error.responseText !== "") $("#add_form_msg_txt").html(error.responseText);
                        //else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                        //$("#add_form_msg").removeClass("m--hide").show();
                    }).always(function () {

                    });
                }
            }),
           
            show: function () {
                $("#btnEditProblema").removeLoader();
             
            },
           
        },
        editAntecedente: {
            object: $("#edit-form-antecedente").validate({
                submitHandler: function (form, e) {
                    $("#btnEditAntecedente").addLoader();
                    e.preventDefault();
                    let formData = new FormData(form);
                    $.ajax({
                        url: $(form).attr("action"),
                        type: "POST",
                        data: formData,
                        contentType: false,
                        processData: false
                    }).done(function () {
                        $(".m-alert").addClass("m--hide");
                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                        $("#btnEditAntecedente").removeLoader();

                    }).fail(function (error) {
                        toastr.error(error.responseText, _app.constants.toastr.title.error);
                        $("#btnEditAntecedente").removeLoader();
                        //if (error.responseText !== null && error.responseText !== "") $("#add_form_msg_txt").html(error.responseText);
                        //else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                        //$("#add_form_msg").removeClass("m--hide").show();
                    }).always(function () {

                    });
                }
            }),

            show: function () {
                $("#btnEditProblema").removeLoader();

            },

        },
        editResultado: {
            object: $("#edit-form-resultado").validate({
                submitHandler: function (form, e) {
                    $("#btnEditResultado").addLoader();
                    e.preventDefault();
                    let formData = new FormData(form);
                    $.ajax({
                        url: $(form).attr("action"),
                        type: "POST",
                        data: formData,
                        contentType: false,
                        processData: false
                    }).done(function () {
                        $(".m-alert").addClass("m--hide");
                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                        $("#btnEditResultado").removeLoader();

                    }).fail(function (error) {
                        toastr.error(error.responseText, _app.constants.toastr.title.error);
                        $("#btnEditResultado").removeLoader();
                        //if (error.responseText !== null && error.responseText !== "") $("#add_form_msg_txt").html(error.responseText);
                        //else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                        //$("#add_form_msg").removeClass("m--hide").show();
                    }).always(function () {

                    });
                }
            }),

            show: function () {
                $("#btnEditProblema").removeLoader();

            },

        },
        editJustificacion: {
            object: $("#edit-form-justificacion").validate({
                submitHandler: function (form, e) {
                    $("#btnEditJustificacion").addLoader();
                    e.preventDefault();
                    let formData = new FormData(form);
                    $.ajax({
                        url: $(form).attr("action"),
                        type: "POST",
                        data: formData,
                        contentType: false,
                        processData: false
                    }).done(function () {
                        $(".m-alert").addClass("m--hide");
                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                        $("#btnEditJustificacion").removeLoader();

                    }).fail(function (error) {
                        toastr.error(error.responseText, _app.constants.toastr.title.error);
                        $("#btnEditJustificacion").removeLoader();
                        //if (error.responseText !== null && error.responseText !== "") $("#add_form_msg_txt").html(error.responseText);
                        //else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                        //$("#add_form_msg").removeClass("m--hide").show();
                    }).always(function () {

                    });
                }
            }),

            show: function () {
                $("#btnEditProblema").removeLoader();

            },

        },
        editHipotesis: {
            object: $("#edit-form-hipotesis").validate({
                submitHandler: function (form, e) {
                    $("#btnEditHipotesis").addLoader();
                    e.preventDefault();
                    let formData = new FormData(form);
                    $.ajax({
                        url: $(form).attr("action"),
                        type: "POST",
                        data: formData,
                        contentType: false,
                        processData: false
                    }).done(function () {
                        $(".m-alert").addClass("m--hide");
                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                        $("#btnEditHipotesis").removeLoader();

                    }).fail(function (error) {
                        toastr.error(error.responseText, _app.constants.toastr.title.error);
                        $("#btnEditHipotesis").removeLoader();
                        //if (error.responseText !== null && error.responseText !== "") $("#add_form_msg_txt").html(error.responseText);
                        //else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                        //$("#add_form_msg").removeClass("m--hide").show();
                    }).always(function () {

                    });
                }
            }),

            show: function () {
                $("#btnEditProblema").removeLoader();

            },

        },
        editPreguntas: {
            object: $("#edit-form-preguntas").validate({
                submitHandler: function (form, e) {
                    $("#btnEditPreguntas").addLoader();
                    e.preventDefault();
                    let formData = new FormData(form);
                    $.ajax({
                        url: $(form).attr("action"),
                        type: "POST",
                        data: formData,
                        contentType: false,
                        processData: false
                    }).done(function () {
                        $(".m-alert").addClass("m--hide");
                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                        $("#btnEditPreguntas").removeLoader();

                    }).fail(function (error) {
                        toastr.error(error.responseText, _app.constants.toastr.title.error);
                        $("#btnEditPreguntas").removeLoader();
                        //if (error.responseText !== null && error.responseText !== "") $("#add_form_msg_txt").html(error.responseText);
                        //else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                        //$("#add_form_msg").removeClass("m--hide").show();
                    }).always(function () {

                    });
                }
            }),

            show: function () {
                $("#btnEditProblema").removeLoader();

            },

        },
        editObjetivogeneral: {
            object: $("#edit-form-objetivogeneral").validate({
                submitHandler: function (form, e) {
                    $("#btnEditObjetivogeneral").addLoader();
                    e.preventDefault();
                    let formData = new FormData(form);
                    $.ajax({
                        url: $(form).attr("action"),
                        type: "POST",
                        data: formData,
                        contentType: false,
                        processData: false
                    }).done(function () {
                        $(".m-alert").addClass("m--hide");
                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                        $("#btnEditObjetivogeneral").removeLoader();

                    }).fail(function (error) {
                        toastr.error(error.responseText, _app.constants.toastr.title.error);
                        $("#btnEditObjetivogeneral").removeLoader();
                        //if (error.responseText !== null && error.responseText !== "") $("#add_form_msg_txt").html(error.responseText);
                        //else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                        //$("#add_form_msg").removeClass("m--hide").show();
                    }).always(function () {

                    });
                }
            }),

            show: function () {
                $("#btnEditProblema").removeLoader();

            },

        },
        editObjetivoespecifico: {
            object: $("#edit-form-objetivoespecifico").validate({
                submitHandler: function (form, e) {
                    $("#btnEditObjetivoespecifico").addLoader();
                    e.preventDefault();
                    let formData = new FormData(form);
                    $.ajax({
                        url: $(form).attr("action"),
                        type: "POST",
                        data: formData,
                        contentType: false,
                        processData: false
                    }).done(function () {
                        $(".m-alert").addClass("m--hide");
                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                        $("#btnEditObjetivoespecifico").removeLoader();

                    }).fail(function (error) {
                        toastr.error(error.responseText, _app.constants.toastr.title.error);
                        $("#btnEditObjetivoespecifico").removeLoader();
                        //if (error.responseText !== null && error.responseText !== "") $("#add_form_msg_txt").html(error.responseText);
                        //else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                        //$("#add_form_msg").removeClass("m--hide").show();
                    }).always(function () {

                    });
                }
            }),

            show: function () {
                $("#btnEditProblema").removeLoader();

            },

        },
        editMetodologia: {
            object: $("#edit-form-metodologia").validate({
                submitHandler: function (form, e) {
                    $("#btnEditMetodologia").addLoader();
                    e.preventDefault();
                    let formData = new FormData(form);
                    $.ajax({
                        url: $(form).attr("action"),
                        type: "POST",
                        data: formData,
                        contentType: false,
                        processData: false
                    }).done(function () {
                        $(".m-alert").addClass("m--hide");
                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                        $("#btnEditMetodologia").removeLoader();

                    }).fail(function (error) {
                        toastr.error(error.responseText, _app.constants.toastr.title.error);
                        $("#btnEditMetodologia").removeLoader();
                        //if (error.responseText !== null && error.responseText !== "") $("#add_form_msg_txt").html(error.responseText);
                        //else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                        //$("#add_form_msg").removeClass("m--hide").show();
                    }).always(function () {

                    });
                }
            }),

            show: function () {
                $("#btnEditProblema").removeLoader();

            },

        },
        editRiesgos: {
            object: $("#edit-form-riesgos").validate({
                submitHandler: function (form, e) {
                    $("#btnEditRiesgos").addLoader();
                    e.preventDefault();
                    let formData = new FormData(form);
                    $.ajax({
                        url: $(form).attr("action"),
                        type: "POST",
                        data: formData,
                        contentType: false,
                        processData: false
                    }).done(function () {
                        $(".m-alert").addClass("m--hide");
                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                        $("#btnEditRiesgos").removeLoader();

                    }).fail(function (error) {
                        toastr.error(error.responseText, _app.constants.toastr.title.error);
                        $("#btnEditRiesgos").removeLoader();
                        //if (error.responseText !== null && error.responseText !== "") $("#add_form_msg_txt").html(error.responseText);
                        //else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                        //$("#add_form_msg").removeClass("m--hide").show();
                    }).always(function () {

                    });
                }
            }),

            show: function () {
                $("#btnEditProblema").removeLoader();

            },

        },
        editResumencientifico: {
            object: $("#edit-form-resumencientifico").validate({
                submitHandler: function (form, e) {
                    $("#btnEditResumencientifico").addLoader();
                    e.preventDefault();
                    let formData = new FormData(form);
                    $.ajax({
                        url: $(form).attr("action"),
                        type: "POST",
                        data: formData,
                        contentType: false,
                        processData: false
                    }).done(function () {
                        $(".m-alert").addClass("m--hide");
                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                        $("#btnEditResumencientifico").removeLoader();

                    }).fail(function (error) {
                        toastr.error(error.responseText, _app.constants.toastr.title.error);
                        $("#btnEditResumencientifico").removeLoader();
                        //if (error.responseText !== null && error.responseText !== "") $("#add_form_msg_txt").html(error.responseText);
                        //else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                        //$("#add_form_msg").removeClass("m--hide").show();
                    }).always(function () {

                    });
                }
            }),

            show: function () {
                $("#btnEditProblema").removeLoader();

            },

        },
        editEquipamiento: {
            object: $("#edit-form-equipamiento").validate({
                submitHandler: function (form, e) {
                    $("#btnEditEquipamiento").addLoader();
                    e.preventDefault();
                    let formData = new FormData(form);
                    $.ajax({
                        url: $(form).attr("action"),
                        type: "POST",
                        data: formData,
                        contentType: false,
                        processData: false
                    }).done(function () {
                        $(".m-alert").addClass("m--hide");
                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                        $("#btnEditEquipamiento").removeLoader();

                    }).fail(function (error) {
                        toastr.error(error.responseText, _app.constants.toastr.title.error);
                        $("#btnEditEquipamiento").removeLoader();
                        //if (error.responseText !== null && error.responseText !== "") $("#add_form_msg_txt").html(error.responseText);
                        //else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                        //$("#add_form_msg").removeClass("m--hide").show();
                    }).always(function () {

                    });
                }
            }),

            show: function () {
                $("#btnEditProblema").removeLoader();

            },

        },
        editResultados: {
            object: $("#edit-form-resultados").validate({
                submitHandler: function (form, e) {
                    $("#btnEditResultados").addLoader();
                    e.preventDefault();
                    let formData = new FormData(form);
                    $.ajax({
                        url: $(form).attr("action"),
                        type: "POST",
                        data: formData,
                        contentType: false,
                        processData: false
                    }).done(function () {
                        $(".m-alert").addClass("m--hide");
                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                        $("#btnEditResultados").removeLoader();

                    }).fail(function (error) {
                        toastr.error(error.responseText, _app.constants.toastr.title.error);
                        $("#btnEditResultados").removeLoader();
                        //if (error.responseText !== null && error.responseText !== "") $("#add_form_msg_txt").html(error.responseText);
                        //else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                        //$("#add_form_msg").removeClass("m--hide").show();
                    }).always(function () {

                    });
                }
            }),

            show: function () {
                $("#btnEditProblema").removeLoader();

            },

        },    
        editSostenibilidad: {
            object: $("#edit-form-sostenibilidad").validate({
                submitHandler: function (form, e) {
                    $("#btnEditSostenibilidad").addLoader();
                    e.preventDefault();
                    let formData = new FormData(form);
                    $.ajax({
                        url: $(form).attr("action"),
                        type: "POST",
                        data: formData,
                        contentType: false,
                        processData: false
                    }).done(function () {
                        $(".m-alert").addClass("m--hide");
                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                        $("#btnEditSostenibilidad").removeLoader();

                    }).fail(function (error) {
                        toastr.error(error.responseText, _app.constants.toastr.title.error);
                        $("#btnEditSostenibilidad").removeLoader();
                        //if (error.responseText !== null && error.responseText !== "") $("#add_form_msg_txt").html(error.responseText);
                        //else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                        //$("#add_form_msg").removeClass("m--hide").show();
                    }).always(function () {

                    });
                }
            }),

            show: function () {
                $("#btnEditProblema").removeLoader();

            },

        },    
        editImpacto: {
            object: $("#edit-form-impacto").validate({
                submitHandler: function (form, e) {
                    $("#btnEditImpacto").addLoader();
                    e.preventDefault();
                    let formData = new FormData(form);
                    $.ajax({
                        url: $(form).attr("action"),
                        type: "POST",
                        data: formData,
                        contentType: false,
                        processData: false
                    }).done(function () {
                        $(".m-alert").addClass("m--hide");
                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                        $("#btnEditImpacto").removeLoader();

                    }).fail(function (error) {
                        toastr.error(error.responseText, _app.constants.toastr.title.error);
                        $("#btnEditImpacto").removeLoader();
                        //if (error.responseText !== null && error.responseText !== "") $("#add_form_msg_txt").html(error.responseText);
                        //else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                        //$("#add_form_msg").removeClass("m--hide").show();
                    }).always(function () {

                    });
                }
            }),

            show: function () {
                $("#btnEditProblema").removeLoader();

            },

        },    
        editDescripcionemprendimiento: {
            object: $("#edit-form-descripcionemprendimiento").validate({
                submitHandler: function (form, e) {
                    $("#btnEditDescripcionemprendimiento").addLoader();
                    e.preventDefault();
                    let formData = new FormData(form);
                    $.ajax({
                        url: $(form).attr("action"),
                        type: "POST",
                        data: formData,
                        contentType: false,
                        processData: false
                    }).done(function () {
                        $(".m-alert").addClass("m--hide");
                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                        $("#btnEditDescripcionemprendimiento").removeLoader();

                    }).fail(function (error) {
                        toastr.error(error.responseText, _app.constants.toastr.title.error);
                        $("#btnEditDescripcionemprendimiento").removeLoader();
                        //if (error.responseText !== null && error.responseText !== "") $("#add_form_msg_txt").html(error.responseText);
                        //else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                        //$("#add_form_msg").removeClass("m--hide").show();
                    }).always(function () {

                    });
                }
            }),

            show: function () {
                $("#btnEditDescripcionemprendimiento").removeLoader();

            },

        },    
        editVentajascompetitivascomparativas: {
            object: $("#edit-form-ventajascompetitivascomparativas").validate({
                submitHandler: function (form, e) {
                    $("#btnEditVentajascompetitivascomparativas").addLoader();
                    e.preventDefault();
                    let formData = new FormData(form);
                    $.ajax({
                        url: $(form).attr("action"),
                        type: "POST",
                        data: formData,
                        contentType: false,
                        processData: false
                    }).done(function () {
                        $(".m-alert").addClass("m--hide");
                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                        $("#btnEditVentajascompetitivascomparativas").removeLoader();

                    }).fail(function (error) {
                        toastr.error(error.responseText, _app.constants.toastr.title.error);
                        $("#btnEditVentajascompetitivascomparativas").removeLoader();
                        //if (error.responseText !== null && error.responseText !== "") $("#add_form_msg_txt").html(error.responseText);
                        //else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                        //$("#add_form_msg").removeClass("m--hide").show();
                    }).always(function () {

                    });
                }
            }),

            show: function () {
                $("#btnEditVentajascompetitivascomparativas").removeLoader();

            },

        },    
        editEstudiomercado: {
            object: $("#edit-form-estudiomercado").validate({
                submitHandler: function (form, e) {
                    $("#btnEditEstudiomercado").addLoader();
                    e.preventDefault();
                    let formData = new FormData(form);
                    $.ajax({
                        url: $(form).attr("action"),
                        type: "POST",
                        data: formData,
                        contentType: false,
                        processData: false
                    }).done(function () {
                        $(".m-alert").addClass("m--hide");
                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                        $("#btnEditEstudiomercado").removeLoader();

                    }).fail(function (error) {
                        toastr.error(error.responseText, _app.constants.toastr.title.error);
                        $("#btnEditEstudiomercado").removeLoader();
                        //if (error.responseText !== null && error.responseText !== "") $("#add_form_msg_txt").html(error.responseText);
                        //else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                        //$("#add_form_msg").removeClass("m--hide").show();
                    }).always(function () {

                    });
                }
            }),

            show: function () {
                $("#btnEditEstudiomercado").removeLoader();

            },

        },    
        editEstrategiamarketing: {
            object: $("#edit-form-estrategiamarketing").validate({
                submitHandler: function (form, e) {
                    $("#btnEditEstrategiamarketing").addLoader();
                    e.preventDefault();
                    let formData = new FormData(form);
                    $.ajax({
                        url: $(form).attr("action"),
                        type: "POST",
                        data: formData,
                        contentType: false,
                        processData: false
                    }).done(function () {
                        $(".m-alert").addClass("m--hide");
                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                        $("#btnEditEstrategiamarketing").removeLoader();

                    }).fail(function (error) {
                        toastr.error(error.responseText, _app.constants.toastr.title.error);
                        $("#btnEditEstrategiamarketing").removeLoader();
                        //if (error.responseText !== null && error.responseText !== "") $("#add_form_msg_txt").html(error.responseText);
                        //else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                        //$("#add_form_msg").removeClass("m--hide").show();
                    }).always(function () {

                    });
                }
            }),

            show: function () {
                $("#btnEditEstrategiamarketing").removeLoader();

            },

        },    
        editPotencialessociosestrategicos: {
            object: $("#edit-form-potencialessociosestrategicos").validate({
                submitHandler: function (form, e) {
                    $("#btnEditPotencialessociosestrategicos").addLoader();
                    e.preventDefault();
                    let formData = new FormData(form);
                    $.ajax({
                        url: $(form).attr("action"),
                        type: "POST",
                        data: formData,
                        contentType: false,
                        processData: false
                    }).done(function () {
                        $(".m-alert").addClass("m--hide");
                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                        $("#btnEditPotencialessociosestrategicos").removeLoader();

                    }).fail(function (error) {
                        toastr.error(error.responseText, _app.constants.toastr.title.error);
                        $("#btnEditPotencialessociosestrategicos").removeLoader();
                        //if (error.responseText !== null && error.responseText !== "") $("#add_form_msg_txt").html(error.responseText);
                        //else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                        //$("#add_form_msg").removeClass("m--hide").show();
                    }).always(function () {

                    });
                }
            }),

            show: function () {
                $("#btnEditPotencialessociosestrategicos").removeLoader();

            },

        },    

        
        addactividades: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: "/Investigacionfomento/convocatoriaproyecto/detalle".proto().parseURL(),
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
                        data: "titulo",
                        width:400,
                    },
                    {
                        title: "Meses",
                        data: "nombremeses",
                    },
                    {
                        title: "Opciones",
                        data: null,
                        width:30,
                        render: function (data) {
                            var template = "";

                            template += "<button  type='button' title='Asignar cronograma' class='btn btn-danger btn-addCronograma ";
                            template += " m-btn btn-sm  m-btn--icon-only' ";
                            template += ` onclick='$("#addactividad-form input[name=Id]").val("` + data.id + `");' `;
                            template += " data-id='" + data.id + "' data-idconvocatoriaproyecto='" + data.idConvocatoriaproyecto + "'> ";
                            template += "<i class='la la-calendar'></i></button>";


                           /* //Edit
                            template += "<button type='button' title='Cumplimiento' ";
                            template += ` onclick='$("#addactividad-form input[name=Id]").val("` + data.id + `");`
                            template += ` $("#addactividad-form input[name=titulo]").val("` + data.titulo + `");`
                            template += ` $("#addactividad-form input[name=estado]").prop("checked",` + (data.estado == 1 ? true : false) + `) ;' class='btn btn-info btn-addActividad `;
                            template += " m-btn btn-sm  m-btn--icon-only' ";
                            template += " data-id='" + data.id + "'>";
                            template += "<i class='la la-edit'></i></button>";*/
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

            },
            init: function () {
                this.object = $("#data-tableactividades").DataTable(this.options);
                this.events();
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
        addcronogramas: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: "/Investigacionfomento/convocatoriaproyecto/detalle".proto().parseURL(),
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
                            template += ` onclick='$("#addactividad-form input[name=Id]").val("` + data.id + `");' class='btn btn-info btn-deletecronograma' "`;
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
                modal.addactividades.reload()

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
                $("#data-tablecronogramas").on('click', '.btn-addCronogrma', function () {
                    var id = $(this).data("id");
                    modal.addcronogrma.show(id);
                });

                $("#data-tablecronogramas").on('click', '.btn-deletecronograma', function () {
                    var id = $(this).data("id");
                    modal.deletecronograma(id);

                });


            },
            init: function () {
                this.events();
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

        addanexofaltantes: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: "/Investigacionfomento/convocatoriaproyecto/detalle".proto().parseURL(),
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
                            template += ` onclick=' $("#addanexo-form input[name=IdRequisito]").val("` + data.idRequisito + `");$("#addanexo-form input[name=Id]").val("` + data.idProyectorequisito + `");' class='btn btn-info btn-addAnexo "`;
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


            },
            init: function () {
                this.object = $("#data-tableanexofaltantes").DataTable(this.options);
                this.events();
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


        addactividadespresupuesto: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: "/Investigacionfomento/convocatoriaproyecto/detalle".proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        data.handler = "DetailPlanActividadespresupuesto";
                        data.id = $("#editactividad-form input[name=Id]").val();
                    }
                },
                pageLength: 50,
                orderable: [],
                columns: [

                    {
                        title: "Tipo de Gasto",
                        data: "nombregastotipo",
                    },
                    {
                        title: "Unidad de Medida",
                        data: "nombreunidadmedida",
                    },
                    {
                        title: "Concepto",
                        data: "descripcion",
                    },
                    {
                        title: "Costo Unitario",
                        data: "costounitario",
                        width: 30,
                    },
                    {
                        title: "Cantidad",
                        data: "cantidad",
                        width: 30,
                    },
                    {
                        title: "Total",
                        data: "total",
                        width: 30,
                    },
                    {
                        title: "Opciones",
                        data: null,
                        width: 30,
                        render: function (data) {
                            var template = "";
                            var template = "";


                            //Edit
                            template += "<button type='button' title='Elimnar mes' ";
                            template += ` onclick='$("#addactividad-form input[name=Id]").val("` + data.id + `");' class='btn btn-info btn-deletecronogramapresupuesto' "`;
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

                $("#editactividadpresupuesto-form input[name=Id]").val(id);
                if (this.object == null) {
                    this.object = $("#data-tableactividadespresupuesto").DataTable(this.options);
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
                $("#editModalActividadpresupuesto").modal("toggle");
            },
            clear: function () {
                // modal.addobservaciones.object.resetForm();
            },
            events: function () {

                $("#editModalActividadpresupuesto").on("hidden.bs.modal", function () {
                    modal.addactividades.clear();
                });
                $("#editModalActividadpresupuesto").on("hidden.bs.modal", function () {
                    modal.addactividades.clear();
                });
                $("#data-tableactividadespresupuesto").on('click', '.btn-addActividad', function () {
                    var id = $(this).data("id");
                    modal.addactividad.show(id);
                });
                $("#data-tableactividadespresupuesto").on('click', '.btn-addActividad', function () {
                    var id = $(this).data("id");
                    modal.addactividad.show(id);
                });
                $("#data-tableactividadespresupuesto").on('click', '.btn-deletecronogramapresupuesto', function () {
                    var id = $(this).data("id");
                    modal.deletecronogramapresupuesto(id);

                });
                $("#data-tableactividadespresupuesto").on('click', '.btn-addCronograma', function () {
                    var id = $(this).data("id");
                    var idconvocatoriaproyecto = $(this).data("idconvocatoriaproyecto");
                    $("#editcronograma-form select[name=IdConvocatoriaproyectocronograma]").empty();
                    selectcronogramaproyecto.researcherCronogramaProyecto.load(idconvocatoriaproyecto);
                    $("#editcronograma-form select[name=IdConvocatoriaproyectocronograma]").trigger("change");
                    $("#editcronograma-form input[name=Id]").val(id);
                    modal.addcronogramas.show(id);
                });

            },
            init: function () {
                this.object = $("#data-tableactividadespresupuesto").DataTable(this.options);
                this.events();
            }
        },
        newcronogramapresupuesto: {
            object: $("#newcronogramapresupuesto-form").validate({
                submitHandler: function (form, e) {
                    $("#btnNewcronogramapresupuesto").addLoader();
                    e.preventDefault();
                    let formData = new FormData(form);
                    $.ajax({
                        url: $(form).attr("action"),
                        type: "POST",
                        data: formData,
                        contentType: false,
                        processData: false
                    }).done(function () {
                        $("#newModalCronogramapresupuesto").modal("hide");
                        $(".m-alert").addClass("m--hide");
                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                        modal.addactividadespresupuesto.reload();
                        id = $("#newcronogramapresupuesto-form input[name=IdConvocatoriaproyecto]").val();
                        $("#newcronogramapresupuesto-form select[name=IdGastotipo]").val("");
                        $("#newcronogramapresupuesto-form select[name=IdGastotipo]").trigger("change");
                        $("#newcronogramapresupuesto-form select[name=IdUnidadmedida]").val("");
                        $("#newcronogramapresupuesto-form select[name=IdUnidadmedida]").trigger("change");
                        modal.newcronogramapresupuesto.clear();
                        $("#newcronogramapresupuesto-form input[name=IdConvocatoriaproyecto]").val(id)
                        $("#btnNewcronogramapresupuesto").removeLoader();
                    }).fail(function (error) {
                        toastr.error(error.responseText, _app.constants.toastr.title.error);
                        $("#btnNewcronogramapresupuesto").removeLoader();
                        //if (error.responseText !== null && error.responseText !== "") $("#add_form_msg_txt").html(error.responseText);
                        //else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                        //$("#add_form_msg").removeClass("m--hide").show();
                    }).always(function () {

                    });
                }
            }),
            show: function () {
                this.events();
                $("#newModalCronogramapresupuesto").removeLoader();
                $("#newModalCronogramapresupuesto").modal("toggle");
            },
            clear: function () {
                modal.newcronogramapresupuesto.object.resetForm();
            },
            events: function () {

                $("#newModalCronogramapresupuesto").on("hidden.bs.modal", function () {
                    id = $("#newcronogramapresupuesto-form input[name=IdConvocatoriaproyecto]").val();
                    modal.newcronogramapresupuesto.clear();
                    $("#newcronogramapresupuesto-form input[name=IdConvocatoriaproyecto]").val(id);
                });

                $("#newcronogramapresupuesto-form select[name=IdAreaacademica]").on("change", function (e) {
                    $("#newcronogramapresupuesto-form select[name=IdLinea]").empty();
                    selectlinea.researcherMaestroLinea.load($("#newcronogramapresupuesto-form select[name=IdConvocatoriaproyecto]").val());
                    $("#newcronogramapresupuesto-form select[name=IdLinea]").trigger("change");
                });



            }
        },

        deletecronograma: function (id) {
            swal({
                title: "¿Está seguro?",
                text: "El cronograma sera eliminado",
                type: "warning",
                showCancelButton: true,
                confirmButtonText: "Sí, eliminarlo",
                confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                cancelButtonText: "Cancelar",
                showLoaderOnConfirm: true,
                preConfirm: () => {
                    return new Promise((resolve) => {
                        $.ajax({
                            url: ("/Investigacionfomento/convocatoriaproyecto/detalle?handler=Deletecronograma").proto().parseURL(),
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
                                    text: "El Cronograma ha sido eliminado con exito",
                                    confirmButtonText: "Excelente"
                                }).then(
                                    modal.addcronogramas.reload()
                                    
                                );
                                },
                                
                            error: function (errormessage) {
                                swal({
                                    type: "error",
                                    title: "Error",
                                    confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                    confirmButtonText: "Entendido",
                                    text: "El Cronograma presenta información relacionada"
                                });
                            }
                        });
                    });
                },
                allowOutsideClick: () => !swal.isLoading()
            });
        },
        deletecronogramapresupuesto: function (id) {
            swal({
                title: "¿Está seguro?",
                text: "El presupuesto sera eliminado",
                type: "warning",
                showCancelButton: true,
                confirmButtonText: "Sí, eliminarlo",
                confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                cancelButtonText: "Cancelar",
                showLoaderOnConfirm: true,
                preConfirm: () => {
                    return new Promise((resolve) => {
                        $.ajax({
                            url: ("/Investigacionfomento/convocatoriaproyecto/detalle?handler=Deletecronogramapresupuesto").proto().parseURL(),
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
                                    text: "El Presupuesto ha sido eliminado con exito",
                                    confirmButtonText: "Excelente"
                                }).then(
                                    modal.addactividadespresupuesto.reload()

                                );
                            },

                            error: function (errormessage) {
                                swal({
                                    type: "error",
                                    title: "Error",
                                    confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                    confirmButtonText: "Entendido",
                                    text: "El Presupuesto presenta información relacionada"
                                });
                            }
                        });
                    });
                },
                allowOutsideClick: () => !swal.isLoading()
            });
        },

        init: function () {
            this.newactividadcronograma.events();
            $(".btn-cancelar").on("click", function () {
                location.reload();
            });

            $(".btn-grabar").on("click", function () {
                modal.desbloqueartab();
            });

            $(".m-input-textarea").on("keyup", function () {
                var value = $(this).val();    
                var id = this.id;
                var longitud = 0;
                if (id == "problema") { longitud = 5000 }
                if (id == "antecedente") { longitud = 10000 }
                if (id == "resultado") { longitud = 4000 }
                if (id == "justificacion") { longitud = 5000 }
                if (id == "hipotesis") { longitud = 999999 }
                if (id == "preguntas") { longitud = 5000 }
                if (id == "objetivogeneral") { longitud = 2000 }
                if (id == "objetivoespecifico") { longitud = 2000 }
                if (id == "metodologia") { longitud = 15000 }
                if (id == "riesgos") { longitud = 5000 }
                if (id == "resumencientifico") { longitud = 5000 }
                if (id == "equipamiento") { longitud = 5000 }
                if (id == "resultados") { longitud = 999999 }
                if (id == "sostenibilidad") { longitud = 2000 }
                if (id == "impacto") { longitud = 2000 }
                if (id == "descripcionemprendimiento") { longitud = 999999 }
                if (id == "ventajascompetitivascomparativas") { longitud = 999999 }
                if (id == "estudiomercado") { longitud = 999999 }
                if (id == "estrategiamarketing") { longitud = 999999 }
                if (id == "potencialessociosestrategicos") { longitud = 999999 }
              

                if (value.length > longitud) {
                    value = value.substr(0, longitud);
                    $(this).val(value);
                }
                $("#sp" + id).html(value.length + ' caracteres');
                modal.bloqueartab();
            });
        },
        bloqueartab: function () {
            $('.nav-link').addClass('disabled');   
            $('.nav-link').css("color", "#7b7e8a42"); 
            
        },
        desbloqueartab: function () {
            $('.nav-link').removeClass('disabled');
            $('.nav-link').css("color", "#7b7e8a"); 
        }
        

    };
    return {
        init: function () {
            datatable.init();
            modal.init();
            modal.addactividades.init();
            modal.addcronogramas.init();
            modal.addactividadespresupuesto.init();
            search.init();
            selectcronogramaproyecto.init();
            selectgastotipo.init();
            selectunidadmedida.init();
            modal.addanexofaltantes.init();
        }
    }
}();

$(function () {
    InitApp.init();
})
