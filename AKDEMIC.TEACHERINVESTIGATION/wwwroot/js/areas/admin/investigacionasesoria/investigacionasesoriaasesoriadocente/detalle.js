var InitApp = function () {
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
    var selectasesor = {
        init: function () {
            this.researcherMaestroAsesor.init();
        },
        researcherMaestroAsesor: {
            init: function () {
                this.load();
            },
            load: function () {
                $.ajax({
                    url: (`/api/investigacionasesoriaasesor/select/get`).proto().parseURL(),
                    type: "GET"
                }).done(function (result) {
                    $('.input_IdAsesor').select2({
                        data: result,
                        maximumSelectionLength: 1,
                    });
                });
            }
        },
    };
    var selectalumno = {
        init: function () {
            this.researcherMaestroAlumno.init();
        },
        researcherMaestroAlumno: {
            init: function () {
                this.load();
            },
            load: function () {
                $.ajax({
                    url: (`/api/maestroalumno/select/get`).proto().parseURL(),
                    type: "GET"
                }).done(function (result) {
                    $('.input_IdAlumno').select2({
                        data: result,
                        maximumSelectionLength: 1,
                    });
                });
            }
        },
    };
    var selectalumno2 = {
        init: function () {
            this.researcherMaestroAlumno2.init();
        },
        researcherMaestroAlumno2: {
            init: function () {
                this.load();
            },
            load: function () {
                $.ajax({
                    url: (`/api/maestroalumno/select/get`).proto().parseURL(),
                    type: "GET"
                }).done(function (result) {
                    $('.input_IdAlumno2').select2({
                        data: result,
                        maximumSelectionLength: 1,
                    });
                });
            }
        },
    };
    var selecttipotrabajoivestigacion = {
        init: function () {
            this.researcherTipotrabajoinvestigacion.init();
        },
        researcherTipotrabajoinvestigacion: {
            init: function () {
                this.load();
            },
            load: function () {
                $.ajax({
                    url: (`/api/investigacionasesoriatipotrabajoinvestigacion/select/get`).proto().parseURL(),
                    type: "GET"
                }).done(function (result) {
                    $('.input_IdTipotrabajoinvestigacion').select2({
                        data: result,
                        maximumSelectionLength: 1,
                    });
                });
            }
        },
    };
    var selectanio = {
        init: function () {
            this.researcherAnio.init();
        },
        researcherAnio: {
            init: function () {
                this.load();
            },
            load: function () {
                $.ajax({
                    url: (`/api/maestroanio/select/get`).proto().parseURL(),
                    type: "GET"
                }).done(function (result) {
                    $('.input_IdAnio').select2({
                        data: result,
                        maximumSelectionLength: 1,
                    });
                });
            }
        },
    };

    var datatable = {
        investigacionasesoriaAsesoria: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: "/Investigacionasesoria/asesoriadocentedetalle".proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                 
                    data: function (data) {
                        data.handler = "Datatable";
                        data.idtipo = $("#hdIdTipo").val();
                        data.idasesoria = $("#hdIdAsesoria").val();                        
                    }
                },
                pageLength: 50,
                orderable: [],
                columns: [
                    {
                        title: "Estructura",
                        data: "nombre"
                    },
                    {
                        title: "Fecha de Inicio",
                        data: "fechaini"
                    },
                    {
                        title: "Fecha de Termino",
                        data: "fechafin"
                    },
                    
                    {
                        title: "Opciones",
                        data: null,
                        width: 110,
                        orderable: false,
                        render: function (data) {
                            var template = "";

                            template += `<button onclick='$("#edit-form input[name=IdEstructura]").val("` + data.idEstructura + `");'`;

                            template += " title = 'Editar' ";
                            template += "class='btn btn-info btn-edit ";
                            template += "m-btn btn-sm  m-btn--icon-only' ";
                            template += " data-id='" + data.id + "' data-idEstructura='" + data.idEstructura + "'>";
                            template += "<i class='la la-edit'></i></button>";
                                //conf
                            template += "<button title='Revisar' ";
                            template += " style='background-color:#aaeee8;color:#000000' ";
                            template += " class='btn  btn-actividades ";
                            template += "m-btn btn-sm  m-btn--icon-only' ";
                            template += " data-id='" + data.id + "' >";
                            template += "<i class='la la-book'></i> Revisar</button>";

                           
                              
                            
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

                $("#data-table").on('click', '.btn-respuesta', function () {
                    var id = $(this).data("id");
                    modal.edit.show(id);
                });
                $("#data-table").on('click', '.btn-actividades', function () {
                    var id = $(this).data("id");
                    modal.addactividades.show(id);
                });
                
            }
        },
        init: function () {
            this.investigacionasesoriaAsesoria.init();
        }
    };
    var search = {
        init: function () {
            $("#search").doneTyping(function () {
                datatable.investigacionasesoriaAsesoria.reload();
            });
        }
    };
    var modal = {   
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
                $("#btnEditobservacionactividad").removeLoader();
                $("#editobservacionactividad-form input[name=Id]").val(id);
                //modal.editobservacionactividad.load(id);
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
        addobservacionactividades: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: "/Investigacionasesoria/asesoriadocentedetalle".proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        data.handler = "Datatableactividadesobservaciones";
                        data.id = $("#editobservacionactividad-form input[name=Id]").val();
                    }
                },
                pageLength: 50,
                orderable: [],
                columns: [

                    {
                        title: "#",
                        data: "orden",
                        width: 10
                    },


                   
                    {
                        title: "Descripción",
                        data: null,
                        width: 350,
                        render: function (data) { 
                            var template = "";
                            if (contaractividad == 0) {
                                template += '<b><font color="#990000">'+data.nombreestructura+'</font></b>';
                                template += "<br>";
                            }
                            template += data.descripcion;
                            return template;
                        }

                    },
                    {
                        title: "SI",
                        data: null,
                        width: 20,
                        render: function (data) {
                            contaractividad = contaractividad + 1;
                            var template = "";
                            template += '<input ';
                            template += ` onclick ='ban=0;if(this.checked){ban=1;};$("#editobservacionactividad-form input[id=banRequisito` + contaractividad + `]").val(ban);'`;
                            template += ' name = "chk_requisito' + contaractividad +'" type = "radio" style = "width:100%" /> ';
                            return template;
                        }
                    },
                    {
                        title: "NO",
                        data: null,
                        width: 20,
                        render: function (data) {
                            var template = "";
                            template += '<input ';
                            template += ` onclick ='ban=1;if(this.checked){ban=0;};$("#editobservacionactividad-form input[id=banRequisito` + contaractividad +`]").val(ban);'`;
                            template += ' name = "chk_requisito' + contaractividad + '" type = "radio" style = "width:100%" /> ';
                            return template;
                        }
                    },
                    {
                        title: "Observacion",
                        data: null,
                        width: 150,
                        render: function (data) {
                            var template = "";
                            template += '<input  type="text" style="width:100%"  id="observacion"  name="observacion" />';
                            template += '<input value="' + data.id + '" id="idRequisito"  name="idRequisito" type="hidden" style="width:100%" />';
                            template += '<input value="" id="banRequisito' + contaractividad + '"  name="banRequisito" type="hidden" style="width:100%" required/>';
                            return template;
                        }
                    },
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
                                //row.style.display = "none";
                            }

                        };
                    }

                    if (data.informefinal == '1' && contaraprobado > 0) {
                        row.style.display = "";
                    }

                    if (data.informefinal == '1') {
                        row.style.backgroundColor = "#DEEEF7";
                    }


                },
            },
            load: function (id) {
                $("#editobservacionactividad-form input[name=Id]").val(id);
                if (this.object == null) {
                this.object = $("#data-tableactividadesobservaciones").DataTable(this.options);


                   
                } else {
                    this.reload();
                }
                modal.addobservacionactividades.events();
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

                modal.addobservacionactividades.load(id);
                $("#editModalObservacionactividad").modal("toggle");
            },
            clear: function () {
                // modal.addactividades.object.resetForm();
            },
            events: function () {

                $("#editModalObservacionactividad").on("hidden.bs.modal", function () {
                    modal.addobservacionactividades.clear();
                });
                $("#editModalObservacionactividad").on("hidden.bs.modal", function () {
                    modal.addobservacionactividades.clear();
                });


                $("#data-tableactividadesobservaciones").on('click', '.btn-editactividad', function () {

                    var id = $(this).data("id");
                    modal.addobservacionactividades.show(id);
                });
                $("#data-tableactividadesobservaciones").on('click', '.btn-editactividadfinal', function () {

                    var id = $(this).data("id");
                    modal.addobservacionactividades.show(id);
                });
                $("#data-tableactividadesobservaciones").on('click', '.btn-editobservacionactividad', function () {

                    var id = $(this).data("id");
                    modal.editobservacionactividad.show(id);
                });

                $("#data-tableactividadesobservaciones").on('click', '.btn-enviaractividad', function () {
                    var id = $(this).data("id");
                    modal.enviaractividad(id);

                });
                $("#data-tableactividadesobservaciones").on('click', '.btn-observacionactividad', function () {
                    var id = $(this).data("id");
                    modal.observacionactividad(id);

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
                    url: "/Investigacionasesoria/asesoriadocentedetalle".proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        data.handler = "Datatableactividades";
                        data.id = $("#addactividades-form input[name=Id]").val();
                    }
                },
                pageLength: 50,
                orderable: [],
                columns: [

                    {
                        title: "Alumno",
                        data: "nombre",
                        width: 250
                    },
                    

                    {
                        title: "Trabajo de Investigación",
                        data: null,
                        orderable: false,
                        width: 100,
                        render: function (data) {
                            var template = "";
                            var fileUrl = `/documentos/${data.rutaarchivo}`.proto().parseURL();
                            //FileURL
                            if (data.rutaarchivo != '') {
                                template += `<a href="${fileUrl}"  `;
                                template += "class='btn btn-success ";
                                template += "m-btn btn-sm m-btn--icon' download>";
                                template += "<span><i class='flaticon-file'></i></a> ";
                            }
                            return template;
                        }
                    },
                    {
                        title: "Observaciones",
                        data: "observacion",
                        width: 250
                    },
                    {
                        title: "Estado",
                        data: null,
                        width: 100,
                        render: function (data) {
                            var template = "";

                            var btnsearch = "";
                          

                            var estado = data.estado == 0 ? "EN PROCESO" : data.estado == 1 ? "ENVIADO" : data.estado == 2 ? (data.informefinal == "0" ? "APROBADO" : "APROBADO") : (data.informefinal == "0" ? "OBSERVADO" : "OBSERVADO");
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
                            } else {
                                
                                if (data.estado == '1') {
                                    template += "<button type='button' title='Informe Final Aprobar/Observar' "
                                    template += " class='btn btn-danger btn-editobservacionactividad ";
                                    template += " m-btn btn-sm  m-btn--icon-only' ";
                                    template += " data-id='" + data.id + "' > ";
                                    template += "<i class='la la-send'></i></button>";
                                }
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
                    modal.addobservacionactividades.show(id);
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
                        datatable.investigacionasesoriaAsesoria.reload();
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
                        datatable.investigacionasesoriaAsesoria.reload();
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
                    url: "/Investigacionasesoria/asesoriadocentedetalle".proto().parseURL(),
                    type: "GET",
                    data: {
                        handler: "Detail",
                        id: id
                    },
                }).done(function (result) {
                    $("#edit-form input[name=Id]").val(result.id);                   
                    $("#edit-form input[name=fechaini]").val(result.fechaini);
                    $("#edit-form input[name=fechafin]").val(result.fechafin);                  
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
        delete: function (id) {
            swal({
                title: "¿Está seguro?",
                text: "La asesoría será eliminado.",
                type: "warning",
                showCancelButton: true,
                confirmButtonText: "Sí, eliminarlo",
                confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                cancelButtonText: "Cancelar",
                showLoaderOnConfirm: true,
                preConfirm: () => {
                    return new Promise((resolve) => {                        
                        $.ajax({
                            url: ("/Investigacionasesoria/asesoriadocentedetalle?handler=Delete").proto().parseURL(),
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
                                    text: "La asesoría ha sido eliminado con exito",
                                    confirmButtonText: "Excelente"
                                }).then(datatable.investigacionasesoriaAsesoria.reload());
                            },
                            error: function (errormessage) {
                                swal({
                                    type: "error",
                                    title: "Error",
                                    confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                    confirmButtonText: "Entendido",
                                    text: "La asesoría presenta información relacionada"
                                });
                            }
                        });
                    });
                },
                allowOutsideClick: () => !swal.isLoading()
            });
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
                            url: ("/Investigacionasesoria/asesoriadocentedetalle?handler=EnviarPlan").proto().parseURL(),
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
                                }).then(datatable.investigacionasesoriaAsesoria.reload());
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
        }

    };
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
        }
    }
    return {
        init: function () {
            datatable.init();
            search.init();
            modal.init();
            datePickers.init();
            selectcarrera.init();
            selectasesor.init();
            selectalumno.init();
            selectanio.init();
            selectalumno2.init();
            selecttipotrabajoivestigacion.init();
           
        }
    }
}();

$(function () {
    InitApp.init();
})
