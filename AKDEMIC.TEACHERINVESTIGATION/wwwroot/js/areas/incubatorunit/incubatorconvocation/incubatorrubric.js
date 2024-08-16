var index = function () {
    var ConvocationId = $("#IncubatorConvocation_Id").val();
    var baseRoute = `/unidad-emprendimiento/convocatorias-emprendimiento/${ConvocationId}/rubrica`;

    var partials = {
        rubric: {
            object: $("#rubric_container"),
            reload: function () {

                mApp.block(partials.rubric.object, {
                    message: "Cargando rúbrica..."
                })

                $.ajax({
                    url: `${baseRoute}?handler=RubricPartialView`,
                    type: "GET"
                })
                    .done(function (e) {
                        partials.rubric.object.html(e);
                    });
            },
            events: {
                onImport: function () {
                    $("#btn_import_rubric").on("click", function () {
                        modal.import.import.show();
                    })
                },
                onAddSection: function () {
                    $("#btn_add_section").on("click", function () {
                        modal.section.add.show();
                    })
                },
                onEditSection: function () {
                    partials.rubric.object.on("click", ".edit-section", function () {
                        var id = $(this).data("id");
                        var title = $(this).data("title");
                        var maxsectionscore = $(this).data("maxsectionscore");
                        modal.section.edit.show(id, title, maxsectionscore);
                    })
                },
                onDeleteSection: function () {
                    partials.rubric.object.on("click", ".delete-section", function () {
                        var id = $(this).data("id");

                        swal({
                            type: "warning",
                            title: "Eliminará la sección seleccionada.",
                            text: "¿Seguro que desea eliminarla?",
                            confirmButtonText: "Sí",
                            showCancelButton: true,
                            showLoaderOnConfirm: true,
                            allowOutsideClick: () => !swal.isLoading(),
                            preConfirm: () => {
                                return new Promise(() => {
                                    $.ajax({
                                        type: "POST",
                                        url: `${baseRoute}?handler=DeleteSection&id=${id}`,
                                        beforeSend: function (xhr) {
                                            xhr.setRequestHeader("XSRF-TOKEN",
                                                $('input:hidden[name="__RequestVerificationToken"]').val());
                                        }
                                    })
                                        .done(function () {
                                            partials.rubric.reload();
                                            swal({
                                                type: "success",
                                                title: "Completado",
                                                text: "Sección eliminada satisfactoriamente.",
                                                confirmButtonText: "Aceptar"
                                            });
                                        })
                                        .fail(function (e) {
                                            swal({
                                                type: "error",
                                                title: "Error",
                                                confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                                confirmButtonText: "Aceptar",
                                                text: e.status === 502 ? "No hay respuesta del servidor" : e.responseText
                                            });
                                        });

                                });
                            }
                        });
                    })
                },

                onAddCriterion: function () {
                    partials.rubric.object.on("click", ".add-criterion", function () {
                        var id = $(this).data("id");
                        modal.criterion.add.show(id);
                    })
                },

                onEditCriterion: function () {
                    partials.rubric.object.on("click", ".edit-criterion", function () {
                        var id = $(this).data("id");
                        var description = $(this).data("description");
                        var name = $(this).data("name");
                        modal.criterion.edit.show(id, name, description);
                    })
                },
                onDeleteCriterion: function () {
                    partials.rubric.object.on("click", ".delete-criterion", function () {
                        var id = $(this).data("id");

                        swal({
                            type: "warning",
                            title: "Eliminará el criterio seleccionado.",
                            text: "¿Seguro que desea eliminarlo?.",
                            confirmButtonText: "Sí",
                            showCancelButton: true,
                            showLoaderOnConfirm: true,
                            allowOutsideClick: () => !swal.isLoading(),
                            preConfirm: () => {
                                return new Promise(() => {
                                    $.ajax({
                                        type: "POST",
                                        url: `${baseRoute}?handler=DeleteCriterion&id=${id}`,
                                        beforeSend: function (xhr) {
                                            xhr.setRequestHeader("XSRF-TOKEN",
                                                $('input:hidden[name="__RequestVerificationToken"]').val());
                                        }
                                    })
                                        .done(function () {
                                            partials.rubric.reload();
                                            swal({
                                                type: "success",
                                                title: "Completado",
                                                text: "Criterio eliminado satisfactoriamente.",
                                                confirmButtonText: "Aceptar"
                                            });
                                        })
                                        .fail(function (e) {
                                            swal({
                                                type: "error",
                                                title: "Error",
                                                confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                                confirmButtonText: "Aceptar",
                                                text: e.status === 502 ? "No hay respuesta del servidor" : e.responseText
                                            });
                                        });

                                });
                            }
                        });
                    })
                },

                onAddLevel: function () {
                    partials.rubric.object.on("click", ".add-level", function () {
                        var id = $(this).data("id");
                        modal.level.add.show(id);
                    })
                },
                onEditLevel: function () {
                    partials.rubric.object.on("click", ".edit-level", function () {
                        var id = $(this).data("id");
                        var description = $(this).data("description");
                        var score = $(this).data("score");
                        modal.level.edit.show(id, description, score);
                    })
                },
                onDeleteLevel: function () {
                    partials.rubric.object.on("click", ".delete-level", function () {
                        var id = $(this).data("id");

                        swal({
                            type: "warning",
                            title: "Eliminará el nivel de desempeño seleccionado.",
                            text: "¿Seguro que desea eliminarlo?.",
                            confirmButtonText: "Sí",
                            showCancelButton: true,
                            showLoaderOnConfirm: true,
                            allowOutsideClick: () => !swal.isLoading(),
                            preConfirm: () => {
                                return new Promise(() => {
                                    $.ajax({
                                        type: "POST",
                                        url: `${baseRoute}?handler=DeleteLevel&id=${id}`,
                                        beforeSend: function (xhr) {
                                            xhr.setRequestHeader("XSRF-TOKEN",
                                                $('input:hidden[name="__RequestVerificationToken"]').val());
                                        }
                                    })
                                        .done(function () {
                                            partials.rubric.reload();
                                            swal({
                                                type: "success",
                                                title: "Completado",
                                                text: "Nivel de desempeño eliminado satisfactoriamente.",
                                                confirmButtonText: "Aceptar"
                                            });
                                        })
                                        .fail(function (e) {
                                            swal({
                                                type: "error",
                                                title: "Error",
                                                confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                                confirmButtonText: "Aceptar",
                                                text: e.status === 502 ? "No hay respuesta del servidor" : e.responseText
                                            });
                                        });

                                });
                            }
                        });
                    })
                },
                init: function () {
                    this.onImport();
                    this.onAddSection();
                    this.onEditSection();
                    this.onDeleteSection();


                    this.onAddCriterion();
                    this.onEditCriterion();
                    this.onDeleteCriterion();

                    this.onAddLevel();
                    this.onEditLevel();
                    this.onDeleteLevel();
                }
            },
            init: function () {
                this.reload();
                this.events.init();
            }
        },
        init: function () {
            partials.rubric.init();
        }
    }

    var modal = {
        import: {
            object: $("#rubric_import_modal"),
            form: {
                object: $("#rubric_import_form").validate({
                    ignore: ":hidden, [contenteditable='true']:not([name])",
                    submitHandler: function (formElement, e) {
                        var formData = new FormData(formElement);
                        formData.append("ConvocationId", ConvocationId);
                        modal.import.object.find("button[type='submit']").addClass("m-loader m-loader--right m-loader--light").attr("disabled", true);
                        $.ajax({
                            url: $(formElement).attr("action"),
                            type: "POST",
                            data: formData,
                            contentType: false,
                            processData: false
                        })
                            .done(function (data) {
                                modal.import.object.modal("hide");
                                partials.rubric.reload();
                                toastr.success("Tarea realizada con éxito", "Hecho");
                            })
                            .fail(function (e) {
                                toastr.error(e.responseText, "Error!");
                            })
                            .always(function () {
                                modal.import.object.find("button[type='submit']").removeClass("m-loader m-loader--right m-loader--light").attr("disabled", false);
                            });
                    }
                })
            },
            import: {
                show: function () {
                    $("#rubric_import_form").attr("action", `${baseRoute}?handler=Import`);
                    modal.import.object.find(".modal-title").text("Importar");
                    modal.import.object.modal("show");
                }
            },
            events: {
                onHidden: function () {
                    modal.import.object.on("hidden.bs.modal", function () {
                        modal.import.form.object.resetForm();
                    })
                },
                init: function () {
                    this.onHidden();
                }
            },
            init: function () {
                this.events.init();
            }
        },
        section: {
            object: $("#rubric_section_modal"),
            form: {
                object: $("#rubric_section_form").validate({
                    ignore: ":hidden, [contenteditable='true']:not([name])",
                    submitHandler: function (formElement, e) {
                        var formData = new FormData(formElement);
                        formData.append("ConvocationId", ConvocationId);
                        modal.section.object.find("button[type='submit']").addClass("m-loader m-loader--right m-loader--light").attr("disabled", true);
                        $.ajax({
                            url: $(formElement).attr("action"),
                            type: "POST",
                            data: formData,
                            contentType: false,
                            processData: false
                        })
                            .done(function (data) {
                                modal.section.object.modal("hide");
                                partials.rubric.reload();
                                toastr.success("Tarea realizada con éxito", "Hecho");
                            })
                            .fail(function (e) {
                                toastr.error(e.responseText, "Error!");
                            })
                            .always(function () {
                                modal.section.object.find("button[type='submit']").removeClass("m-loader m-loader--right m-loader--light").attr("disabled", false);
                            });
                    }
                })
            },
            add: {
                show: function () {
                    $("#rubric_section_form").attr("action", `${baseRoute}?handler=AddSection`);
                    modal.section.object.find(".modal-title").text("Agregar Sección");
                    modal.section.object.modal("show");
                }
            },
            edit: {
                show: function (id, title, maxsectionscore) {
                    $("#rubric_section_form").attr("action", `${baseRoute}?handler=EditSection`);
                    modal.section.object.find(".modal-title").text("Editar Sección");
                    modal.section.object.modal("show");
                    modal.section.object.find("[name='Id']").val(id);
                    modal.section.object.find("[name='Title']").val(title);
                    modal.section.object.find("[name='MaxSectionScore']").val(maxsectionscore);
                }
            },
            events: {
                onHidden: function () {
                    modal.section.object.on("hidden.bs.modal", function () {
                        modal.section.form.object.resetForm();
                    })
                },
                init: function () {
                    this.onHidden();
                }
            },
            init: function () {
                this.events.init();
            }
        },
        criterion: {
            object: $("#rubric_criterion_modal"),
            form: {
                object: $("#rubric_criterion_form").validate({
                    ignore: ":hidden, [contenteditable='true']:not([name])",
                    submitHandler: function (formElement, e) {
                        var formData = new FormData(formElement);
                        formData.append("ConvocationId", ConvocationId);
                        modal.criterion.object.find("button[type='submit']").addClass("m-loader m-loader--right m-loader--light").attr("disabled", true);
                        $.ajax({
                            url: $(formElement).attr("action"),
                            type: "POST",
                            data: formData,
                            contentType: false,
                            processData: false
                        })
                            .done(function (data) {
                                modal.criterion.object.modal("hide");
                                partials.rubric.reload();
                                toastr.success("Tarea realizada con éxito", "Hecho");
                            })
                            .fail(function (e) {
                                toastr.error(e.responseText, "Error!");
                            })
                            .always(function () {
                                modal.criterion.object.find("button[type='submit']").removeClass("m-loader m-loader--right m-loader--light").attr("disabled", false);
                            });
                    }
                })
            },
            add: {
                show: function (sectionId) {
                    $("#rubric_criterion_form").attr("action", `${baseRoute}?handler=AddCriterion`);
                    modal.criterion.object.find("[name='RubricSectionId']").val(sectionId);
                    modal.criterion.object.find(".modal-title").text("Agregar Criterio");
                    pageScrollTop.init();
                    modal.criterion.object.modal("show");
                }
            },
            edit: {
                show: function (id, name, description) {
                    $("#rubric_criterion_form").attr("action", `${baseRoute}?handler=EditCriterion`);
                    modal.criterion.object.find(".modal-title").text("Editar Criterio");
                    pageScrollTop.init();
                    modal.criterion.object.modal("show");
                    modal.criterion.object.find("[name='Id']").val(id);
                    summernote.criterion_description.object.summernote("code", description);
                    modal.criterion.object.find("[name='Name']").val(name);
                }
            },
            events: {
                onHidden: function () {
                    modal.criterion.object.on("hidden.bs.modal", function () {
                        modal.criterion.form.object.resetForm();
                        summernote.criterion_description.events.clean();
                    })
                },
                init: function () {
                    this.onHidden();
                }
            },
            init: function () {
                this.events.init();
            }
        },
        level: {
            object: $("#level_criterion_modal"),
            form: {
                object: $("#level_criterion_form").validate({
                    submitHandler: function (formElement, e) {
                        var formData = new FormData(formElement);
                        formData.append("ConvocationId", ConvocationId);
                        modal.level.object.find("button[type='submit']").addClass("m-loader m-loader--right m-loader--light").attr("disabled", true);
                        $.ajax({
                            url: $(formElement).attr("action"),
                            type: "POST",
                            data: formData,
                            contentType: false,
                            processData: false
                        })
                            .done(function (data) {
                                modal.level.object.modal("hide");
                                partials.rubric.reload();
                                toastr.success("Tarea realizada con éxito", "Hecho");
                            })
                            .fail(function (e) {
                                toastr.error(e.responseText, "Error!");
                            })
                            .always(function () {
                                modal.level.object.find("button[type='submit']").removeClass("m-loader m-loader--right m-loader--light").attr("disabled", false);
                            });
                    }
                })
            },
            add: {
                show: function (criterionId) {
                    $("#level_criterion_form").attr("action", `${baseRoute}?handler=AddLevel`);
                    modal.level.object.find("[name='RubricCriterionId']").val(criterionId);
                    modal.level.object.find(".modal-title").text("Agregar Nivel Desempeño");
                    modal.level.object.modal("show");
                }
            },
            edit: {
                show: function (id, description, score) {
                    $("#level_criterion_form").attr("action", `${baseRoute}?handler=EditLevel`);
                    modal.level.object.find(".modal-title").text("Editar Nivel Desempeño");
                    modal.level.object.modal("show");
                    modal.level.object.find("[name='Id']").val(id);
                    modal.level.object.find("[name='Score']").val(score);
                    modal.level.object.find("[name='Description']").val(description);
                }
            },
            events: {
                onHidden: function () {
                    modal.level.object.on("hidden.bs.modal", function () {
                        modal.level.form.object.resetForm();
                    })
                },
                init: function () {
                    this.onHidden();
                }
            },
            init: function () {
                this.events.init();
            }
        },
        init: function () {
            modal.section.init();
            modal.criterion.init();
            modal.level.init();
        }
    }

    var pageScrollTop = {
        init: function () {
            window.scrollTo({
                top: 0,
                left: 0,
                behavior: 'smooth'
            });
        }
    }

    var summernote = {
        options: {
            height: 250,
            toolbar: [
                ['style', ['bold', 'italic', 'underline']],
                ['fontsize', ['fontsize']],
                ['color', ['color']],
                ['para', ['ul', 'ol', 'paragraph']],
                ['height', ['height']]
            ]
        },
        criterion_description: {
            object: null,
            events: {
                clean: function () {
                    summernote.criterion_description.object.summernote("code", "");
                }
            },
            init: function () {
                this.object = $("#criterion_description_summernote").summernote(summernote.options);
            }
        },
        init: function () {
            this.criterion_description.init();
        }
    }


    return {
        init: function () {
            partials.init();
            summernote.init();
            modal.init();
        }
    }
}();

$(() => {
    index.init();
});