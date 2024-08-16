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
                    url: "/Investigacionasesoria/asesoriaalumno".proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                 
                    data: function (data) {
                        data.handler = "Datatable";
                        data.searchValue = $("#search").val();
                        data.searchIdtipo = $("#searchIdtipo").val();
                    }
                },
                pageLength: 50,
                orderable: [],
                columns: [
                    {
                        title: "Año",
                        data: "anio"
                    },
                    {
                        title: "Tipo de Investigación",
                        data: "nombretipoinvestigacion"
                    },
                    {
                        title: "Carrera",
                        data: "nombrecarrera"
                    },
                    {
                        title: "Resolución",
                        data: "nroresolucion"
                    }, {
                        title: "Asesor",
                        data: "nombreasesor",
                    },
                    {
                        title: "Alumno",
                        data: null,
                        render: function (data) {
                            var template = "";
                            template += data.nombrealumno;
                            template += '/';
                            template += data.nombrealumno2;
                            return template;
                        }
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
                        width: 70,
                        orderable: false,
                        render: function (data) {
                            var template = "";

                           
                                  //conf
                            template += "<a title='Mis actividades' ";
                                template += " style='background-color:#aaeee8;color:#000000' ";
                                template += " class='btn  btn-conf ";
                            template += "m-btn btn-sm  m-btn--icon-only' ";
                            template += " href='/Investigacionasesoria/asesoriaalumnodetalle?id=" + data.id + "'";
                                template += " data-id='" + data.id + "'>";
                                template += "<i class='la la-book'></i> Mis actividades</a>";

                              
                            
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
                    url: "/Investigacionasesoria/asesoriaalumno".proto().parseURL(),
                    type: "GET",
                    data: {
                        handler: "Detail",
                        id: id
                    },
                }).done(function (result) {
                    $("#edit-form input[name=Id]").val(result.id);
                    $("#edit-form select[name=IdCarrera]").val(result.idCarrera);
                    $("#edit-form select[name=IdCarrera]").trigger("change");
                    $("#edit-form select[name=IdAlumno]").val(result.idAlumno);
                    $("#edit-form select[name=IdAlumno]").trigger("change");
                    $("#edit-form select[name=IdAsesor]").val(result.idAsesor);
                    $("#edit-form select[name=IdAsesor]").trigger("change");
                    $("#edit-form select[name=IdTipotrabajoinvestigacion]").val(result.idTipotrabajoinvestigacion);
                    $("#edit-form select[name=IdTipotrabajoinvestigacion]").trigger("change");
                    $("#edit-form select[name=IdAlumno2]").val(result.idAlumno2);
                    $("#edit-form select[name=IdAlumno2]").trigger("change");
                    $("#edit-form select[name=IdAnio]").val(result.idAnio);
                    $("#edit-form select[name=IdAnio]").trigger("change");
                    $("#edit-form input[name=nroresolucion]").val(result.nroresolucion);
                    $("#edit-form input[name=fechaini]").val(result.fechaini);
                    $("#edit-form input[name=fechafin]").val(result.fechafin);
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
                            url: ("/Investigacionasesoria/asesoriaalumno?handler=Delete").proto().parseURL(),
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
                            url: ("/Investigacionasesoria/asesoriaalumno?handler=EnviarPlan").proto().parseURL(),
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
