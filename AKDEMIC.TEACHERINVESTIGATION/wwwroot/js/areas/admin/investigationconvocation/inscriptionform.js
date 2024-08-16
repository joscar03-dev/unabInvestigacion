var InitApp = function () {
    var investigationConvocationRequirementId = $('#Input_InvestigationConvocationRequirementId').val();
    var investigationConvocationId = $('#Input_InvestigationConvocationId').val();
    var questionPartial = {
        load: function () {
            $.ajax({
                url: `/admin/convocatoria-de-investigacion/${investigationConvocationId}/formulario-inscripcion?handler=Questions`.proto().parseURL(),
                type: "GET",
                data: {
                    investigationConvocationRequirementId: investigationConvocationRequirementId
                }
            }).done(function (data) {
                $("#aditional-questions").html(data);
            }).fail(function (error) {
                toastr.error("No se pudieron cargar las preguntas adicionales", _app.constants.toastr.title.error);
            });
        },
        init: function () {
            this.load();
        }
    }

    var form = {
        inscription: {
            object: $("#configForm").validate({
                submitHandler: function (form, e) {
                    $("#Save").addLoader();
                    e.preventDefault();
                   
                    $.ajax({
                        url: $(form).attr("action"),
                        type: "POST",
                        data: $(form).serialize(),
                    }).done(function () {
                        $("#Save").removeLoader();
                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                        window.location.href = window.location.href;
                    }).fail(function (error) {
                        toastr.error(error.responseText, _app.constants.toastr.title.error);
                        $("#Save").removeLoader();
                        //if (error.responseText !== null && error.responseText !== "") $("#add_form_msg_txt").html(error.responseText);
                        //else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                        //$("#add_form_msg").removeClass("m--hide").show();
                    }).always(function () {

                    });
                }
            }),
        }
    };
    var switchInput = {
        init: function () {
            this.requireQuestions.init();
        },
        requireQuestions: {
            init: function () {
                $(".checkbox_isrequired").bootstrapSwitch({
                    onText: "SÍ",
                    offText: "NO",
                    onColor: "success",
                    offColor: "danger"
                });
            },
        },


    };
    var modal = {
        create: {
            object: $("#add-question-form").validate({
                submitHandler: function (form, e) {
                    $("#btnAdd").addLoader();
                    e.preventDefault();
                    mApp.block("#add_question_modal");
                    var answersArray = [];
                    var answersElements = $("#add-question-form .answer");
                    if (answersElements.length < 2 && parseInt($('#add-question-form select[name="QuestionType"]').val()) !== 1) {
                        mApp.unblock("#add_question_modal");
                        toastr.error("Agregue por lo menos dos respuestas", _app.constants.toastr.title.error);
                        $("#btnAdd").removeLoader();
                        return;
                    }

                    if (parseInt($('#add-question-form select[name="QuestionType"]').val()) !== 1) {
                        for (var i = 0; i < answersElements.length; i++) {
                            if (answersElements[i].value === "") {
                                mApp.unblock("#add_question_modal");
                                toastr.error("Campos vacíos", _app.constants.toastr.title.error);
                                $("#btnAdd").removeLoader();
                                return;
                            }
                            let answer = { description: answersElements[i].value };
                            answersArray.push(answer);
                        }
                    }
                    $.ajax({
                        url: $(form).attr("action"),
                        type: "POST",
                        beforeSend: function (xhr) {
                            xhr.setRequestHeader("XSRF-TOKEN",
                                $('#add-question-form input:hidden[name="__RequestVerificationToken"]').val());
                        },
                        data: {
                            questionType: $('#add-question-form select[name="QuestionType"]').val(),
                            isRequired: $('#add-question-form input[name="IsRequired"]').is(':checked'),
                            description: $("#add-question-form textarea[name='Description']").val(),
                            investigationConvocationRequirementId: investigationConvocationRequirementId,
                            answers: answersArray
                        }
                    }).done(function () {
                        $(".modal").modal("hide");
                        $(".m-alert").addClass("m--hide");
                        questionPartial.load();
                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success); 
                        modal.create.clear();
                        mApp.unblock("#add_question_modal");
                        $("#btnAdd").removeLoader();
                    }).fail(function (error) {
                        toastr.error(error.responseText, _app.constants.toastr.title.error);
                        mApp.unblock("#add_question_modal");
                        $("#btnAdd").removeLoader();
                        //if (error.responseText !== null && error.responseText !== "") $("#add_form_msg_txt").html(error.responseText);
                        //else $("#add_form_msg_txt").html(_app.constants.ajax.message.error);
                        //$("#add_form_msg").removeClass("m--hide").show();
                    }).always(function () {

                    });
                }
            }),
            clear: function () {
                $('#add-question-form select[name="QuestionType"]').val(0).trigger("change");
                modal.create.object.resetForm();
            },
            events: function () {
                $("#add-question-form .add-answer").on('click', function () {
                    answerInput.addModal.draw();
                });

                $("#add-question-form textarea[name='Description']").on("input", function () {
                    var maxlength = $(this).attr("maxlength");
                    var currentLength = $(this).val().length;
                    $("#add-question-form .characterLeft").html(`${currentLength}/${maxlength} caracteres`);
                });

                $("#add_question_modal").on('hidden.bs.modal', function () {
                    modal.create.clear();
                });
            }    
        },
        edit: {
            object: $("#edit-question-form").validate({
                submitHandler: function (form, e) {
                    $("#btnEdit").addLoader();
                    e.preventDefault();
                    mApp.block("#edit_question_modal");
                    var answersArray = [];
                    var answersElements = $("#edit-question-form .answer");
                    if (answersElements.length < 2 && parseInt($('#edit-question-form select[name="QuestionType"]').val()) !== 1) {
                        mApp.unblock("#edit_question_modal");
                        toastr.error("Agregue por lo menos dos respuestas", _app.constants.toastr.title.error);
                        $("#btnEdit").removeLoader();
                        return;
                    }

                    if (parseInt($('#edit-question-form select[name="QuestionType"]').val()) !== 1) {
                        for (var i = 0; i < answersElements.length; i++) {
                            if (answersElements[i].value === "") {
                                mApp.unblock("#edit_question_modal");
                                toastr.error("Campos vacíos", _app.constants.toastr.title.error);
                                $("#btnEdit").removeLoader();
                                return;
                            }
                            let answer = { description: answersElements[i].value };
                            answersArray.push(answer);
                        }
                    }
                    $.ajax({
                        url: $(form).attr("action"),
                        type: "POST",
                        beforeSend: function (xhr) {
                            xhr.setRequestHeader("XSRF-TOKEN",
                                $('#edit-question-form input:hidden[name="__RequestVerificationToken"]').val());
                        },
                        data: {
                            id: $('#edit-question-form input[name="Id"]').val(),
                            questionType: $('#edit-question-form select[name="QuestionType"]').val(),
                            isRequired: $('#edit-question-form input[name="IsRequired"]').is(':checked'),
                            description: $("#edit-question-form textarea[name='Description']").val(),
                            investigationConvocationRequirementId: investigationConvocationRequirementId,
                            investigationAnswers: answersArray
                        }
                    }).done(function () {
                        $(".modal").modal("hide");
                        $(".m-alert").addClass("m--hide");
                        questionPartial.load();
                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                        modal.create.clear();
                        mApp.unblock("#edit_question_modal");
                        $("#btnEdit").removeLoader();
                    }).fail(function (error) {
                        toastr.error(error.responseText, _app.constants.toastr.title.error);
                        mApp.unblock("#edit_question_modal");
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
                    url: `/admin/convocatoria-de-investigacion/${investigationConvocationId}/formulario-inscripcion?handler=LoadQuestion`.proto().parseURL(),
                    type: "GET",
                    data: {
                        investigationQuestionId: id
                    },
                }).done(function (result) {
                    $('#edit-question-form input[name=Id]').val(result.id);
                    $('#edit-question-form select[name=QuestionType]').val(result.questionType).trigger("change");
                    $('#edit-question-form input[name=IsRequired]').prop('checked', result.isRequired).trigger("change");
                    $('#edit-question-form textarea[name=Description]').val(result.description);

                    if (result.questionType !== 1) {
                        for (var i = 0; i < result.investigationAnswers.length; i++) { 
                            var htmldata = ''
                            htmldata += '<div class="form-group col-lg-12" style="display:flex;">';
                            htmldata += `   <input class="form-control m-input answer" value="${result.investigationAnswers[i].description}" style="margin-right: 10px;" required>`;
                            htmldata += '   <button class="btn btn-danger btn-sm m-btn--icon delete-answer" type="button" onclick="this.parentNode.outerHTML = \'\';"><span><i class="la la-trash"></i></span></button>'
                            htmldata += '</div>'
                            var e = document.createElement('div');
                            e.innerHTML = htmldata;
                            $('#edit-question-form .formanswers').append(e.firstChild);
                        }
                        
                    }
                    

                }).fail(function (error) {
                    toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                    if (error.responseText !== null && error.responseText !== "") $("#edit_questionform_msg_txt").html(error.responseText);
                    else $("#edit_questionform_msg_txt").html(_app.constants.ajax.message.error);
                    $("#edit_questionform_msg").removeClass("m--hide").show();
                    $("#btnEdit").removeLoader();
                }).always(function () {

                });
            },
            show: function (id) {
                modal.edit.load(id);
                $("#btnEdit").removeLoader();
                $("#edit_question_modal").modal("toggle");
            },
            clear: function () {
                $('#edit-question-form select[name="QuestionType"]').val(0).trigger("change");
                modal.edit.object.resetForm();
                $('#edit-question-form .formanswers').empty();
            },
            events: function () {
                $("#edit-question-form .add-answer").on('click', function () {
                    answerInput.editModal.draw();
                });
                
                $("#edit-question-form textarea[name='Description']").on("input", function () {
                    var maxlength = $(this).attr("maxlength");
                    var currentLength = $(this).val().length;
                    $("#edit-question-form .characterLeft").html(`${currentLength}/${maxlength} caracteres`);
                });

                $("#edit_question_modal").on('hidden.bs.modal', function () {
                    modal.edit.clear();
                });
            }
        },
        delete: function (id) {
            swal({
                title: "¿Está seguro?",
                text: "La pregunta será eliminada",
                type: "warning",
                showCancelButton: true,
                confirmButtonText: "Sí, eliminarla",
                confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                cancelButtonText: "Cancelar",
                showLoaderOnConfirm: true,
                preConfirm: () => {
                    return new Promise((resolve) => {
                        $.ajax({
                            url: (`/admin/convocatoria-de-investigacion/${investigationConvocationId}/formulario-inscripcion?handler=DeleteQuestion`).proto().parseURL(),
                            type: "POST",
                            beforeSend: function (xhr) {
                                xhr.setRequestHeader("XSRF-TOKEN",
                                    $('#configForm input:hidden[name="__RequestVerificationToken"]').val());
                            },
                            data: {
                                investigationQuestionId: id
                            },
                            success: function (result) {
                                swal({
                                    type: "success",
                                    title: "Completado",
                                    text: "La pregunta ha sido eliminado con exito",
                                    confirmButtonText: "Excelente"
                                }).then(questionPartial.load());
                            },
                            error: function (errormessage) {
                                swal({
                                    type: "error",
                                    title: "Error",
                                    confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                    confirmButtonText: "Entendido",
                                    text: "La pregunta presenta información relacionada"
                                });
                            }
                        });
                    });
                },
                allowOutsideClick: () => !swal.isLoading()
            });
        },
        events: function () {
            this.create.events();
            this.edit.events();
            $("#aditional-questions").on('click', '.delete',
                function () {
                    var id = $(this).data("id");
                    modal.delete(id);
                });
            $("#aditional-questions").on('click', '.edit',
                function () {
                    var id = $(this).data("id");
                    modal.edit.show(id);
                });
        },
        init: function () {
            this.events();
        }
    };
    var checkbox = {
        init: function () {
            this.investigationType.init();
            this.externalEntity.init();
            this.budget.init();
            this.investigationPattern.init();
            this.area.init();
            this.faculty.init();
            this.career.init();
            this.researchCenter.init();
            this.financing.init();
            this.executionPlace.init();
            this.mainLocation.init();
            this.researchLine.init();
            this.projectTitle.init();
            this.problemDescription.init();
            this.generalGoal.init();
            this.problemFormulation.init();
            this.specificGoal.init();
            this.justification.init();
            this.theoreticalFundament.init();
            this.problemRecord.init();
            this.hypothesis.init();
            this.variable.init();
            this.methodologyType.init();
            this.methodologyDescription.init();
            this.population.init();
            this.informationCollectionTechnique.init();
            this.analysisTechnique.init();
            this.ethicalconsiderations.init();
            this.fieldWork.init();
            this.thesisDevelopment.init();
            this.expectedResults.init();
            this.bibliographicReferences.init();
            this.publishedArticle.init();
            this.broadcastArticle.init();
            this.processDevelopment.init();
            this.teamMemberUser.init();
            this.externalMember.init();
            this.projectDuration.init();
            this.postulationAttachmentFiles.init();
            this.questions.init();
        },
        investigationType: {
            init: function () {
                this.events();
                this.load();
            },
            load: function () {
                if ($("#Input_InvestigationTypeHidden").is(':checked')) {
                    $("#Input_InvestigationTypeWeight").prop("disabled", true);
                    $("#InvestigationType").prop("disabled", true);
                }
                else {
                    $("#Input_InvestigationTypeWeight").prop("disabled", false);
                    $("#InvestigationType").prop("disabled", false);
                }
            },
            events: function () {                
                $("#Input_InvestigationTypeHidden").on("click", function () {
                    if ($("#Input_InvestigationTypeHidden").is(':checked')) {
                        $("#Input_InvestigationTypeWeight").val(0);
                        $("#InvestigationType").val(0).trigger('change');
                        $("#Input_InvestigationTypeWeight").prop("disabled", true);
                        $("#InvestigationType").prop("disabled", true);
                    }
                    else {
                        $("#Input_InvestigationTypeWeight").prop("disabled", false);
                        $("#InvestigationType").prop("disabled", false);
                    }
                });
            }
        },
        externalEntity: {
            init: function () {
                this.load();
                this.events();
            },
            load: function () {
                if ($("#Input_ExternalEntityHidden").is(':checked')) {
                    $("#Input_ExternalEntityWeight").prop("disabled", true);
                    $("#ExternalEntity").prop("disabled", true);
                }
                else {
                    $("#Input_ExternalEntityWeight").prop("disabled", false);
                    $("#ExternalEntity").prop("disabled", false);
                }
            },
            events: function () {
                $("#Input_ExternalEntityHidden").on("click", function () {
                    if ($("#Input_ExternalEntityHidden").is(':checked')) {
                        $("#Input_ExternalEntityWeight").val(0);
                        $("#ExternalEntity").val(0).trigger('change');
                        $("#Input_ExternalEntityWeight").prop("disabled", true);
                        $("#ExternalEntity").prop("disabled", true);
                    }
                    else {
                        $("#Input_ExternalEntityWeight").prop("disabled", false);
                        $("#ExternalEntity").prop("disabled", false);
                    }
                });
            }
        },
        budget: {
            init: function () {
                this.load();
                this.events();
            },
            load: function () {
                if ($("#Input_BudgetHidden").is(':checked')) {
                    $("#Input_BudgetWeight").val(0);
                    $("#Input_BudgetWeight").prop("disabled", true);
                    $("#Budget").prop("disabled", true);
                }
                else {
                    $("#Input_BudgetWeight").prop("disabled", false);
                    $("#Budget").prop("disabled", false);
                }
            },
            events: function () {
                $("#Input_BudgetHidden").on("click", function () {
                    if ($("#Input_BudgetHidden").is(':checked')) {
                        $("#Input_BudgetWeight").val(0);
                        $("#Input_BudgetWeight").prop("disabled", true);
                        $("#Budget").prop("disabled", true);
                    }
                    else {
                        $("#Input_BudgetWeight").prop("disabled", false);
                        $("#Budget").prop("disabled", false);
                    }
                });
            }
        },
        investigationPattern: {
            init: function () {
                this.load();
                this.events();
            },
            load: function () {
                if ($("#Input_InvestigationPatternHidden").is(':checked')) {
                    $("#Input_InvestigationPatternWeight").val(0);
                    $("#Input_InvestigationPatternWeight").prop("disabled", true);
                    $("#InvestigationPattern").prop("disabled", true);
                }
                else {
                    $("#Input_InvestigationPatternWeight").prop("disabled", false);
                    $("#InvestigationPattern").prop("disabled", false);
                }
            },
            events: function () {
                $("#Input_InvestigationPatternHidden").on("click", function () {
                    if ($("#Input_InvestigationPatternHidden").is(':checked')) {
                        $("#Input_InvestigationPatternWeight").val(0);
                        $("#InvestigationPattern").val(0).trigger('change');
                        $("#Input_InvestigationPatternWeight").prop("disabled", true);
                        $("#InvestigationPattern").prop("disabled", true);
                    }
                    else {
                        $("#Input_InvestigationPatternWeight").prop("disabled", false);
                        $("#InvestigationPattern").prop("disabled", false);
                    }
                });
            }
        },
        area: {
            init: function () {
                this.load();
                this.events();
            },
            load: function () {
                if ($("#Input_AreaHidden").is(':checked')) {
                    $("#Input_AreaWeight").val(0);
                    $("#Input_AreaWeight").prop("disabled", true);
                    $("#Area").prop("disabled", true);
                }
                else {
                    $("#Input_AreaWeight").prop("disabled", false);
                    $("#Area").prop("disabled", false);
                }
            },
            events: function () {
                $("#Input_AreaHidden").on("click", function () {
                    if ($("#Input_AreaHidden").is(':checked')) {
                        $("#Input_AreaWeight").val(0);
                        $("#Area").val(0).trigger('change');
                        $("#Input_AreaWeight").prop("disabled", true);
                        $("#Area").prop("disabled", true);
                    }
                    else {
                        $("#Input_AreaWeight").prop("disabled", false);
                        $("#Area").prop("disabled", false);
                    }
                });
            }
        },
        faculty: {
            init: function () {
                this.load();
                this.events();
            },
            load: function () {
                if ($("#Input_FacultyHidden").is(':checked')) {
                    $("#Input_FacultyWeight").val(0);
                    $("#Input_FacultyWeight").prop("disabled", true);
                    $("#Faculty").prop("disabled", true);
                }
                else {
                    $("#Input_FacultyWeight").prop("disabled", false);
                    $("#Faculty").prop("disabled", false);
                }
            },
            events: function () {
                $("#Input_FacultyHidden").on("click", function () {
                    if ($("#Input_FacultyHidden").is(':checked')) {
                        $("#Input_FacultyWeight").val(0);
                        $("#Faculty").val(0).trigger('change');
                        $("#Input_FacultyWeight").prop("disabled", true);
                        $("#Faculty").prop("disabled", true);
                    }
                    else {
                        $("#Input_FacultyWeight").prop("disabled", false);
                        $("#Faculty").prop("disabled", false);
                    }
                });
            }
        },
        career: {
            init: function () {
                this.load();
                this.events();
            },
            load: function () {
                if ($("#Input_CareerHidden").is(':checked')) {
                    $("#Input_CareerWeight").val(0);
                    $("#Input_CareerWeight").prop("disabled", true);
                    $("#Career").prop("disabled", true);
                }
                else {
                    $("#Input_CareerWeight").prop("disabled", false);
                    $("#Career").prop("disabled", false);
                }
            },
            events: function () {
                $("#Input_CareerHidden").on("click", function () {
                    if ($("#Input_CareerHidden").is(':checked')) {
                        $("#Input_CareerWeight").val(0);
                        $("#Career").val(0).trigger('change');
                        $("#Input_CareerWeight").prop("disabled", true);
                        $("#Career").prop("disabled", true);
                    }
                    else {
                        $("#Input_CareerWeight").prop("disabled", false);
                        $("#Career").prop("disabled", false);
                    }
                });
            }
        },
        researchCenter: {
            init: function () {
                this.load();
                this.events();
            },
            load: function () {
                if ($("#Input_ResearchCenterHidden").is(':checked')) {
                    $("#Input_ResearchCenterWeight").val(0);
                    $("#Input_ResearchCenterWeight").prop("disabled", true);
                    $("#ResearchCenter").prop("disabled", true);
                }
                else {
                    $("#Input_ResearchCenterWeight").prop("disabled", false);
                    $("#ResearchCenter").prop("disabled", false);
                }
            },
            events: function () {
                $("#Input_ResearchCenterHidden").on("click", function () {
                    if ($("#Input_ResearchCenterHidden").is(':checked')) {
                        $("#Input_ResearchCenterWeight").val(0);
                        $("#ResearchCenter").val(0).trigger('change');
                        $("#Input_ResearchCenterWeight").prop("disabled", true);
                        $("#ResearchCenter").prop("disabled", true);
                    }
                    else {
                        $("#Input_ResearchCenterWeight").prop("disabled", false);
                        $("#ResearchCenter").prop("disabled", false);
                    }
                });
            }
        },
        financing: {
            init: function () {
                this.load();
                this.events();
            },
            load: function () {
                if ($("#Input_FinancingHidden").is(':checked')) {
                    $("#Input_FinancingWeight").val(0);
                    $("#Input_FinancingWeight").prop("disabled", true);
                    $("#Financing").prop("disabled", true);
                }
                else {
                    $("#Input_ResearchCenterWeight").prop("disabled", false);
                    $("#Financing").prop("disabled", false);
                }
            },
            events: function () {
                $("#Input_FinancingHidden").on("click", function () {
                    if ($("#Input_FinancingHidden").is(':checked')) {
                        $("#Input_FinancingWeight").val(0);
                        $("#Financing").val(0).trigger('change');
                        $("#Input_FinancingWeight").prop("disabled", true);
                        $("#Financing").prop("disabled", true);
                    }
                    else {
                        $("#Input_FinancingWeight").prop("disabled", false);
                        $("#Financing").prop("disabled", false);
                    }
                });
            }
        },
        executionPlace: {
            init: function () {
                this.load();
                this.events();
            },
            load: function () {
                if ($("#Input_ExecutionPlaceHidden").is(':checked')) {
                    $("#Input_ExecutionPlaceWeight").val(0);
                    $("#Input_ExecutionPlaceWeight").prop("disabled", true);
                    $("#ExecutionPlace").prop("disabled", true);
                }
                else {
                    $("#Input_ExecutionPlaceWeight").prop("disabled", false);
                    $("#ExecutionPlace").prop("disabled", false);
                }
            },
            events: function () {
                $("#Input_ExecutionPlaceHidden").on("click", function () {
                    if ($("#Input_ExecutionPlaceHidden").is(':checked')) {
                        $("#Input_ExecutionPlaceWeight").val(0);
                        $("#Input_ExecutionPlaceWeight").prop("disabled", true);
                        $("#ExecutionPlace").prop("disabled", true);
                    }
                    else {
                        $("#Input_ExecutionPlaceWeight").prop("disabled", false);
                        $("#ExecutionPlace").prop("disabled", false);
                    }
                });
            }
        },
        mainLocation: {
            init: function () {
                this.load();
                this.events();
            },
            load: function () {
                if ($("#Input_MainLocationHidden").is(':checked')) {
                    $("#Input_MainLocationWeight").val(0);
                    $("#Input_MainLocationWeight").prop("disabled", true);
                    $("#MainLocation").prop("disabled", true);
                }
                else {
                    $("#Input_MainLocationWeight").prop("disabled", false);
                    $("#MainLocation").prop("disabled", false);
                }
            },
            events: function () {
                $("#Input_MainLocationHidden").on("click", function () {
                    if ($("#Input_MainLocationHidden").is(':checked')) {
                        $("#Input_MainLocationWeight").val(0);
                        $("#Input_MainLocationWeight").prop("disabled", true);
                        $("#MainLocation").prop("disabled", true);
                    }
                    else {
                        $("#Input_MainLocationWeight").prop("disabled", false);
                        $("#MainLocation").prop("disabled", false);
                    }
                });
            }
        },
        researchLine: {
            init: function () {
                this.load();
                this.events();
            },
            load: function () {
            },
            events: function () {
                $(".researchLineCategoryhidden").on("change", function () {
                    var rowid = $(this).data("id");
                    var rownumber = $(this).data("row");
                    var inputs = $(`[data-id="${rowid}"]`);
                    if ($(this).is(':checked')) {
                        $(`input[name="Input.ResearchLineCategoryRequirements[${rownumber}].Weight"]`).val(0);
                        inputs.prop("disabled", true);
                    }
                    else {
                        inputs.prop("disabled", false); 
                    }
                    $(this).prop("disabled", false);
                });
            }
        },
        projectTitle: {
            init: function () {
                this.load();
                this.events();
            },
            load: function () {
                if ($("#Input_ProjectTitleHidden").is(':checked')) {
                    $("#Input_ProjectTitleWeight").val(0);
                    $("#Input_ProjectTitleWeight").prop("disabled", true);
                    $("#ProjectTitle").prop("disabled", true);
                }
                else {
                    $("#Input_ProjectTitleWeight").prop("disabled", false);
                    $("#ProjectTitle").prop("disabled", false);
                }
            },
            events: function () {
                $("#Input_ProjectTitleHidden").on("click", function () {
                    if ($("#Input_ProjectTitleHidden").is(':checked')) {
                        $("#Input_ProjectTitleWeight").val(0);
                        $("#Input_ProjectTitleWeight").prop("disabled", true);
                        $("#ProjectTitle").prop("disabled", true);
                    }
                    else {
                        $("#Input_ProjectTitleWeight").prop("disabled", false);
                        $("#ProjectTitle").prop("disabled", false);
                    }
                });
            }
        },
        problemDescription: {
            init: function () {
                this.load();
                this.events();
            },
            load: function () {
                if ($("#Input_ProblemDescriptionHidden").is(':checked')) {
                    $("#Input_ProblemDescriptionWeight").val(0);
                    $("#Input_ProblemDescriptionWeight").prop("disabled", true);
                    $("#ProblemDescription").prop("disabled", true);
                }
                else {
                    $("#Input_ProblemDescriptionWeight").prop("disabled", false);
                    $("#ProblemDescription").prop("disabled", false);
                }
            },
            events: function () {
                $("#Input_ProblemDescriptionHidden").on("click", function () {
                    if ($("#Input_ProblemDescriptionHidden").is(':checked')) {
                        $("#Input_ProblemDescriptionWeight").val(0);
                        $("#Input_ProblemDescriptionWeight").prop("disabled", true);
                        $("#ProblemDescription").prop("disabled", true);
                    }
                    else {
                        $("#Input_ProblemDescriptionWeight").prop("disabled", false);
                        $("#ProblemDescription").prop("disabled", false);
                    }
                });
            }
        },
        generalGoal: {
            init: function () {
                this.load();
                this.events();
            },
            load: function () {
                if ($("#Input_GeneralGoalHidden").is(':checked')) {
                    $("#Input_GeneralGoalWeight").val(0);
                    $("#Input_GeneralGoalWeight").prop("disabled", true);
                    $("#GeneralGoal").prop("disabled", true);
                }
                else {
                    $("#Input_GeneralGoalWeight").prop("disabled", false);
                    $("#GeneralGoal").prop("disabled", false);
                }
            },
            events: function () {
                $("#Input_GeneralGoalHidden").on("click", function () {
                    if ($("#Input_GeneralGoalHidden").is(':checked')) {
                        $("#Input_GeneralGoalWeight").val(0);
                        $("#Input_GeneralGoalWeight").prop("disabled", true);
                        $("#GeneralGoal").prop("disabled", true);
                    }
                    else {
                        $("#Input_GeneralGoalWeight").prop("disabled", false);
                        $("#GeneralGoal").prop("disabled", false);
                    }
                });
            }
        },
        problemFormulation: {
            init: function () {
                this.load();
                this.events();
            },
            load: function () {
                if ($("#Input_ProblemFormulationHidden").is(':checked')) {
                    $("#Input_ProblemFormulationWeight").val(0);
                    $("#Input_ProblemFormulationWeight").prop("disabled", true);
                    $("#ProblemFormulation").prop("disabled", true);
                }
                else {
                    $("#Input_ProblemFormulationWeight").prop("disabled", false);
                    $("#ProblemFormulation").prop("disabled", false);
                }
            },
            events: function () {
                $("#Input_ProblemFormulationHidden").on("click", function () {
                    if ($("#Input_ProblemFormulationHidden").is(':checked')) {
                        $("#Input_ProblemFormulationWeight").val(0);
                        $("#Input_ProblemFormulationWeight").prop("disabled", true);
                        $("#ProblemFormulation").prop("disabled", true);
                    }
                    else {
                        $("#Input_ProblemFormulationWeight").prop("disabled", false);
                        $("#ProblemFormulation").prop("disabled", false);
                    }
                });
            }
        },
        specificGoal: {
            init: function () {
                this.load();
                this.events();
            },
            load: function () {
                if ($("#Input_SpecificGoalHidden").is(':checked')) {
                    $("#Input_SpecificGoalWeight").val(0);
                    $("#Input_SpecificGoalWeight").prop("disabled", true);
                    $("#SpecificGoal").prop("disabled", true);
                }
                else {
                    $("#Input_SpecificGoalWeight").prop("disabled", false);
                    $("#SpecificGoal").prop("disabled", false);
                }
            },
            events: function () {
                $("#Input_SpecificGoalHidden").on("click", function () {
                    if ($("#Input_SpecificGoalHidden").is(':checked')) {
                        $("#Input_SpecificGoalWeight").val(0);
                        $("#Input_SpecificGoalWeight").prop("disabled", true);
                        $("#SpecificGoal").prop("disabled", true);
                    }
                    else {
                        $("#Input_SpecificGoalWeight").prop("disabled", false);
                        $("#SpecificGoal").prop("disabled", false);
                    }
                });
            }
        },
        justification: {
            init: function () {
                this.load();
                this.events();
            },
            load: function () {
                if ($("#Input_JustificationHidden").is(':checked')) {
                    $("#Input_JustificationWeight").val(0);
                    $("#Input_JustificationWeight").prop("disabled", true);
                    $("#Justification").prop("disabled", true);
                }
                else {
                    $("#Input_JustificationWeight").prop("disabled", false);
                    $("#Justification").prop("disabled", false);
                }
            },
            events: function () {
                $("#Input_JustificationHidden").on("click", function () {
                    if ($("#Input_JustificationHidden").is(':checked')) {
                        $("#Input_JustificationWeight").val(0);
                        $("#Input_JustificationWeight").prop("disabled", true);
                        $("#Justification").prop("disabled", true);
                    }
                    else {
                        $("#Input_JustificationWeight").prop("disabled", false);
                        $("#Justification").prop("disabled", false);
                    }
                });
            }
        },
        theoreticalFundament: {
            init: function () {
                this.load();
                this.events();
            },
            load: function () {
                if ($("#Input_TheoreticalFundamentHidden").is(':checked')) {
                    $("#Input_TheoreticalFundamentWeight").val(0);
                    $("#Input_TheoreticalFundamentWeight").prop("disabled", true);
                    $("#TheoreticalFundament").prop("disabled", true);
                }
                else {
                    $("#Input_JustificationWeight").prop("disabled", false);
                    $("#TheoreticalFundament").prop("disabled", false);
                }
            },
            events: function () {
                $("#Input_TheoreticalFundamentHidden").on("click", function () {
                    if ($("#Input_TheoreticalFundamentHidden").is(':checked')) {
                        $("#Input_TheoreticalFundamentWeight").val(0);
                        $("#Input_TheoreticalFundamentWeight").prop("disabled", true);
                        $("#TheoreticalFundament").prop("disabled", true);
                    }
                    else {
                        $("#Input_TheoreticalFundamentWeight").prop("disabled", false);
                        $("#TheoreticalFundament").prop("disabled", false);
                    }
                });
            }
        },
        problemRecord: {
            init: function () {
                this.load();
                this.events();
            },
            load: function () {
                if ($("#Input_ProblemRecordHidden").is(':checked')) {
                    $("#Input_ProblemRecordWeight").val(0);
                    $("#Input_ProblemRecordWeight").prop("disabled", true);
                    $("#ProblemRecord").prop("disabled", true);
                }
                else {
                    $("#Input_ProblemRecordWeight").prop("disabled", false);
                    $("#ProblemRecord").prop("disabled", false);
                }
            },
            events: function () {
                $("#Input_ProblemRecordHidden").on("click", function () {
                    if ($("#Input_ProblemRecordHidden").is(':checked')) {
                        $("#Input_ProblemRecordWeight").val(0);
                        $("#Input_ProblemRecordWeight").prop("disabled", true);
                        $("#ProblemRecord").prop("disabled", true);
                    }
                    else {
                        $("#Input_ProblemRecordWeight").prop("disabled", false);
                        $("#ProblemRecord").prop("disabled", false);
                    }
                });
            }
        },
        hypothesis: {
            init: function () {
                this.load();
                this.events();
            },
            load: function () {
                if ($("#Input_HypothesisHidden").is(':checked')) {
                    $("#Input_HypothesisWeight").val(0);
                    $("#Input_HypothesisWeight").prop("disabled", true);
                    $("#Hypothesis").prop("disabled", true);
                }
                else {
                    $("#Input_HypothesisWeight").prop("disabled", false);
                    $("#Hypothesis").prop("disabled", false);
                }
            },
            events: function () {
                $("#Input_HypothesisHidden").on("click", function () {
                    if ($("#Input_HypothesisHidden").is(':checked')) {
                        $("#Input_HypothesisWeight").val(0);
                        $("#Input_HypothesisWeight").prop("disabled", true);
                        $("#Hypothesis").prop("disabled", true);
                    }
                    else {
                        $("#Input_HypothesisWeight").prop("disabled", false);
                        $("#Hypothesis").prop("disabled", false);
                    }
                });
            }
        },
        variable: {
            init: function () {
                this.load();
                this.events();
            },
            load: function () {
                if ($("#Input_VariableHidden").is(':checked')) {
                    $("#Input_VariableWeight").val(0);
                    $("#Input_VariableWeight").prop("disabled", true);
                    $("#Variable").prop("disabled", true);
                }
                else {
                    $("#Input_VariableWeight").prop("disabled", false);
                    $("#Variable").prop("disabled", false);
                }
            },
            events: function () {
                $("#Input_VariableHidden").on("click", function () {
                    if ($("#Input_VariableHidden").is(':checked')) {
                        $("#Input_VariableWeight").val(0);
                        $("#Input_VariableWeight").prop("disabled", true);
                        $("#Variable").prop("disabled", true);
                    }
                    else {
                        $("#Input_VariableWeight").prop("disabled", false);
                        $("#Variable").prop("disabled", false);
                    }
                });
            }
        },
        methodologyType: {
            init: function () {
                this.load();
                this.events();
            },
            load: function () {
                if ($("#Input_MethodologyTypeHidden").is(':checked')) {
                    $("#Input_MethodologyTypeWeight").val(0);
                    $("#Input_MethodologyTypeWeight").prop("disabled", true);
                    $("#MethodologyType").prop("disabled", true);
                }
                else {
                    $("#Input_MethodologyTypeWeight").prop("disabled", false);
                    $("#MethodologyType").prop("disabled", false);
                }
            },
            events: function () {
                $("#Input_MethodologyTypeHidden").on("click", function () {
                    if ($("#Input_MethodologyTypeHidden").is(':checked')) {
                        $("#Input_MethodologyTypeWeight").val(0);
                        $("#Input_MethodologyTypeWeight").prop("disabled", true);
                        $("#MethodologyType").prop("disabled", true);
                    }
                    else {
                        $("#Input_MethodologyTypeWeight").prop("disabled", false);
                        $("#MethodologyType").prop("disabled", false);
                    }
                });
            }
        },
        population: {
            init: function () {
                this.load();
                this.events();
            },
            load: function () {
                if ($("#Input_PopulationHidden").is(':checked')) {
                    $("#Input_PopulationWeight").val(0);
                    $("#Input_PopulationWeight").prop("disabled", true);
                    $("#Population").prop("disabled", true);
                }
                else {
                    $("#Input_PopulationWeight").prop("disabled", false);
                    $("#Population").prop("disabled", false);
                }
            },
            events: function () {
                $("#Input_PopulationHidden").on("click", function () {
                    if ($("#Input_PopulationHidden").is(':checked')) {
                        $("#Input_PopulationWeight").val(0);
                        $("#Input_PopulationWeight").prop("disabled", true);
                        $("#Population").prop("disabled", true);
                    }
                    else {
                        $("#Input_PopulationWeight").prop("disabled", false);
                        $("#Population").prop("disabled", false);
                    }
                });
            }
        },
        methodologyType: {
            init: function () {
                this.load();
                this.events();
            },
            load: function () {
                if ($("#Input_MethodologyTypeHidden").is(':checked')) {
                    $("#Input_MethodologyTypeWeight").val(0);
                    $("#Input_MethodologyTypeWeight").prop("disabled", true);
                    $("#MethodologyType").prop("disabled", true);
                }
                else {
                    $("#Input_MethodologyTypeWeight").prop("disabled", false);
                    $("#MethodologyType").prop("disabled", false);
                }
            },
            events: function () {
                $("#Input_MethodologyTypeHidden").on("click", function () {
                    if ($("#Input_MethodologyTypeHidden").is(':checked')) {
                        $("#Input_MethodologyTypeWeight").val(0);
                        $("#Input_MethodologyTypeWeight").prop("disabled", true);
                        $("#MethodologyType").prop("disabled", true);
                    }
                    else {
                        $("#Input_MethodologyTypeWeight").prop("disabled", false);
                        $("#MethodologyType").prop("disabled", false);
                    }
                });
            }
        },
        methodologyDescription: {
            init: function () {
                this.load();
                this.events();
            },
            load: function () {
                if ($("#Input_MethodologyDescriptionHidden").is(':checked')) {
                    $("#Input_MethodologyDescriptionWeight").val(0);
                    $("#Input_MethodologyDescriptionWeight").prop("disabled", true);
                    $("#MethodologyDescription").prop("disabled", true);
                }
                else {
                    $("#Input_MethodologyDescriptionWeight").prop("disabled", false);
                    $("#MethodologyDescription").prop("disabled", false);
                }
            },
            events: function () {
                $("#Input_MethodologyDescriptionHidden").on("click", function () {
                    if ($("#Input_MethodologyDescriptionHidden").is(':checked')) {
                        $("#Input_MethodologyDescriptionWeight").val(0);
                        $("#Input_MethodologyDescriptionWeight").prop("disabled", true);
                        $("#MethodologyDescription").prop("disabled", true);
                    }
                    else {
                        $("#Input_MethodologyDescriptionWeight").prop("disabled", false);
                        $("#MethodologyDescription").prop("disabled", false);
                    }
                });
            }
        },
        informationCollectionTechnique: {
            init: function () {
                this.load();
                this.events();
            },
            load: function () {
                if ($("#Input_InformationCollectionTechniqueHidden").is(':checked')) {
                    $("#Input_InformationCollectionTechniqueWeight").val(0);
                    $("#Input_InformationCollectionTechniqueWeight").prop("disabled", true);
                    $("#InformationCollectionTechnique").prop("disabled", true);
                }
                else {
                    $("#Input_InformationCollectionTechniqueWeight").prop("disabled", false);
                    $("#InformationCollectionTechnique").prop("disabled", false);
                }
            },
            events: function () {
                $("#Input_InformationCollectionTechniqueHidden").on("click", function () {
                    if ($("#Input_InformationCollectionTechniqueHidden").is(':checked')) {
                        $("#Input_InformationCollectionTechniqueWeight").val(0);
                        $("#Input_InformationCollectionTechniqueWeight").prop("disabled", true);
                        $("#InformationCollectionTechnique").prop("disabled", true);
                    }
                    else {
                        $("#Input_InformationCollectionTechniqueWeight").prop("disabled", false);
                        $("#InformationCollectionTechnique").prop("disabled", false);
                    }
                });
            }
        },
        analysisTechnique: {
            init: function () {
                this.load();
                this.events();
            },
            load: function () {
                if ($("#Input_AnalysisTechniqueHidden").is(':checked')) {
                    $("#Input_AnalysisTechniqueWeight").val(0);
                    $("#Input_AnalysisTechniqueWeight").prop("disabled", true);
                    $("#AnalysisTechnique").prop("disabled", true);
                }
                else {
                    $("#Input_AnalysisTechniqueWeight").prop("disabled", false);
                    $("#AnalysisTechnique").prop("disabled", false);
                }
            },
            events: function () {
                $("#Input_AnalysisTechniqueHidden").on("click", function () {
                    if ($("#Input_AnalysisTechniqueHidden").is(':checked')) {
                        $("#Input_AnalysisTechniqueWeight").val(0);
                        $("#Input_AnalysisTechniqueWeight").prop("disabled", true);
                        $("#AnalysisTechnique").prop("disabled", true);
                    }
                    else {
                        $("#Input_AnalysisTechniqueWeight").prop("disabled", false);
                        $("#AnalysisTechnique").prop("disabled", false);
                    }
                });
            }
        },
        ethicalconsiderations: {
            init: function () {
                this.load();
                this.events();
            },
            load: function () {
                if ($("#Input_EthicalConsiderationHidden").is(':checked')) {
                    $("#Input_EthicalConsiderationWeight").val(0);
                    $("#Input_EthicalConsiderationWeight").prop("disabled", true);
                    $("#EthicalConsiderations").prop("disabled", true);
                }
                else {
                    $("#Input_EthicalConsiderationWeight").prop("disabled", false);
                    $("#EthicalConsideration").prop("disabled", false);
                }
            },
            events: function () {
                $("#Input_EthicalConsiderationHidden").on("click", function () {
                    if ($("#Input_EthicalConsiderationHidden").is(':checked')) {
                        $("#Input_EthicalConsiderationWeight").val(0);
                        $("#Input_EthicalConsiderationWeight").prop("disabled", true);
                        $("#EthicalConsiderations").prop("disabled", true);
                    }
                    else {
                        $("#Input_AnalysisTechniqueWeight").prop("disabled", false);
                        $("#EthicalConsiderations").prop("disabled", false);
                    }
                });
            }
        },
        fieldWork: {
            init: function () {
                this.load();
                this.events();
            },
            load: function () {
                if ($("#Input_FieldWorkHidden").is(':checked')) {
                    $("#Input_FieldWorkWeight").val(0);
                    $("#Input_FieldWorkWeight").prop("disabled", true);
                    $("#FieldWork").prop("disabled", true);
                }
                else {
                    $("#Input_FieldWorkWeight").prop("disabled", false);
                    $("#FieldWork").prop("disabled", false);
                }
            },
            events: function () {
                $("#Input_FieldWorkHidden").on("click", function () {
                    if ($("#Input_FieldWorkHidden").is(':checked')) {
                        $("#Input_FieldWorkWeight").val(0);
                        $("#Input_FieldWorkWeight").prop("disabled", true);
                        $("#FieldWork").prop("disabled", true);
                    }
                    else {
                        $("#Input_FieldWorkWeight").prop("disabled", false);
                        $("#FieldWork").prop("disabled", false);
                    }
                });
            }
        },
        thesisDevelopment: {
            init: function () {
                this.load();
                this.events();
            },
            load: function () {
                if ($("#Input_ThesisDevelopmentHidden").is(':checked')) {
                    $("#Input_ThesisDevelopmentWeight").val(0);
                    $("#Input_ThesisDevelopmentWeight").prop("disabled", true);
                    $("#ThesisDevelopment").prop("disabled", true);
                }
                else {
                    $("#Input_ThesisDevelopmentWeight").prop("disabled", false);
                    $("#ThesisDevelopment").prop("disabled", false);
                }
            },
            events: function () {
                $("#Input_ThesisDevelopmentHidden").on("click", function () {
                    if ($("#Input_ThesisDevelopmentHidden").is(':checked')) {
                        $("#Input_ThesisDevelopmentWeight").val(0);
                        $("#Input_ThesisDevelopmentWeight").prop("disabled", true);
                        $("#ThesisDevelopment").prop("disabled", true);
                    }
                    else {
                        $("#Input_ThesisDevelopmentWeight").prop("disabled", false);
                        $("#ThesisDevelopment").prop("disabled", false);
                    }
                });
            }
        },

        expectedResults: {
            init: function () {
                this.load();
                this.events();
            },
            load: function () {
                if ($("#Input_ExpectedResultsHidden").is(':checked')) {
                    $("#Input_ExpectedResultsWeight").val(0);
                    $("#Input_ExpectedResultsWeight").prop("disabled", true);
                    $("#ExpectedResults").prop("disabled", true);
                }
                else {
                    $("#Input_ExpectedResultsWeight").prop("disabled", false);
                    $("#ExpectedResults").prop("disabled", false);
                }
            },
            events: function () {
                $("#Input_ExpectedResultsHidden").on("click", function () {
                    if ($("#Input_ExpectedResultsHidden").is(':checked')) {
                        $("#Input_ExpectedResultsWeight").val(0);
                        $("#Input_ExpectedResultsWeight").prop("disabled", true);
                        $("#ExpectedResults").prop("disabled", true);
                    }
                    else {
                        $("#Input_ExpectedResultsWeight").prop("disabled", false);
                        $("#ExpectedResults").prop("disabled", false);
                    }
                });
            }
        },
        bibliographicReferences: {
            init: function () {
                this.load();
                this.events();
            },
            load: function () {
                if ($("#Input_BibliographicReferencesHidden").is(':checked')) {
                    $("#Input_BibliographicReferencesWeight").val(0);
                    $("#Input_BibliographicReferencesWeight").prop("disabled", true);
                    $("#BibliographicReferences").prop("disabled", true);
                }
                else {
                    $("#Input_BibliographicReferencesWeight").prop("disabled", false);
                    $("#BibliographicReferences").prop("disabled", false);
                }
            },
            events: function () {
                $("#Input_BibliographicReferencesHidden").on("click", function () {
                    if ($("#Input_BibliographicReferencesHidden").is(':checked')) {
                        $("#Input_BibliographicReferencesWeight").val(0);
                        $("#Input_BibliographicReferencesWeight").prop("disabled", true);
                        $("#BibliographicReferences").prop("disabled", true);
                    }
                    else {
                        $("#Input_BibliographicReferencesWeight").prop("disabled", false);
                        $("#BibliographicReferences").prop("disabled", false);
                    }
                });
            }
        },

        publishedArticle: {
            init: function () {
                this.load();
                this.events();
            },
            load: function () {
                if ($("#Input_PublishedArticleHidden").is(':checked')) {
                    $("#Input_PublishedArticleWeight").val(0);
                    $("#Input_PublishedArticleWeight").prop("disabled", true);
                    $("#PublishedArticle").prop("disabled", true);
                }
                else {
                    $("#Input_PublishedArticleWeight").prop("disabled", false);
                    $("#PublishedArticle").prop("disabled", false);
                }
            },
            events: function () {
                $("#Input_PublishedArticleHidden").on("click", function () {
                    if ($("#Input_PublishedArticleHidden").is(':checked')) {
                        $("#Input_PublishedArticleWeight").val(0);
                        $("#Input_PublishedArticleWeight").prop("disabled", true);
                        $("#PublishedArticle").prop("disabled", true);
                    }
                    else {
                        $("#Input_PublishedArticleWeight").prop("disabled", false);
                        $("#PublishedArticle").prop("disabled", false);
                    }
                });
            }
        },
        broadcastArticle: {
            init: function () {
                this.load();
                this.events();
            },
            load: function () {
                if ($("#Input_BroadcastArticleHidden").is(':checked')) {
                    $("#Input_BroadcastArticleWeight").val(0);
                    $("#Input_BroadcastArticleWeight").prop("disabled", true);
                    $("#BroadcastArticle").prop("disabled", true);
                }
                else {
                    $("#Input_BroadcastArticleWeight").prop("disabled", false);
                    $("#BroadcastArticle").prop("disabled", false);
                }
            },
            events: function () {
                $("#Input_BroadcastArticleHidden").on("click", function () {
                    if ($("#Input_BroadcastArticleHidden").is(':checked')) {
                        $("#Input_BroadcastArticleWeight").val(0);
                        $("#Input_BroadcastArticleWeight").prop("disabled", true);
                        $("#BroadcastArticle").prop("disabled", true);
                    }
                    else {
                        $("#Input_BroadcastArticleWeight").prop("disabled", false);
                        $("#BroadcastArticle").prop("disabled", false);
                    }
                });
            }
        },
        processDevelopment: {
            init: function () {
                this.load();
                this.events();
            },
            load: function () {
                if ($("#Input_ProcessDevelopmentHidden").is(':checked')) {
                    $("#Input_ProcessDevelopmentWeight").val(0);
                    $("#Input_ProcessDevelopmentWeight").prop("disabled", true);
                    $("#ProcessDevelopment").prop("disabled", true);
                }
                else {
                    $("#Input_ProcessDevelopmentWeight").prop("disabled", false);
                    $("#ProcessDevelopment").prop("disabled", false);
                }
            },
            events: function () {
                $("#Input_ProcessDevelopmentHidden").on("click", function () {
                    if ($("#Input_ProcessDevelopmentHidden").is(':checked')) {
                        $("#Input_ProcessDevelopmentWeight").val(0);
                        $("#Input_ProcessDevelopmentWeight").prop("disabled", true);
                        $("#ProcessDevelopment").prop("disabled", true);
                    }
                    else {
                        $("#Input_ProcessDevelopmentWeight").prop("disabled", false);
                        $("#ProcessDevelopment").prop("disabled", false);
                    }
                });
            }
        },
        teamMemberUser: {
            init: function () {
                this.load();
                this.events();
            },
            load: function () {
                if ($("#Input_TeamMemberUserHidden").is(':checked')) {
                    $("#Input_TeamMemberUserWeight").val(0);
                    $("#Input_TeamMemberUserWeight").prop("disabled", true);
                    $("#TeamMemberUser").prop("disabled", true);
                }
                else {
                    $("#Input_TeamMemberUserWeight").prop("disabled", false);
                    $("#TeamMemberUser").prop("disabled", false);
                }
            },
            events: function () {
                $("#Input_TeamMemberUserHidden").on("click", function () {
                    if ($("#Input_TeamMemberUserHidden").is(':checked')) {
                        $("#Input_TeamMemberUserWeight").val(0);
                        $("#Input_TeamMemberUserWeight").prop("disabled", true);
                        $("#TeamMemberUser").prop("disabled", true);
                    }
                    else {
                        $("#Input_TeamMemberUserWeight").prop("disabled", false);
                        $("#TeamMemberUser").prop("disabled", false);
                    }
                });
            }
        },
        externalMember: {
            init: function () {
                this.load();
                this.events();
            },
            load: function () {
                if ($("#Input_ExternalMemberHidden").is(':checked')) {
                    $("#Input_ExternalMemberWeight").val(0);
                    $("#Input_ExternalMemberWeight").prop("disabled", true);
                    $("#ExternalMember").prop("disabled", true);
                }
                else {
                    $("#Input_ExternalMemberWeight").prop("disabled", false);
                    $("#ExternalMember").prop("disabled", false);
                }
            },
            events: function () {
                $("#Input_ExternalMemberHidden").on("click", function () {
                    if ($("#Input_ExternalMemberHidden").is(':checked')) {
                        $("#Input_ExternalMemberWeight").val(0);
                        $("#Input_ExternalMemberWeight").prop("disabled", true);
                        $("#ExternalMember").prop("disabled", true);
                    }
                    else {
                        $("#Input_ExternalMemberWeight").prop("disabled", false);
                        $("#ExternalMember").prop("disabled", false);
                    }
                });
            }
        },
        projectDuration: {
            init: function () {
                this.load();
                this.events();
            },
            load: function () {
                if ($("#Input_ProjectDurationHidden").is(':checked')) {
                    $("#Input_ProjectDurationWeight").val(0);
                    $("#Input_ProjectDurationWeight").prop("disabled", true);
                    $("#ProjectDuration").prop("disabled", true);
                }
                else {
                    $("#Input_ProjectDurationWeight").prop("disabled", false);
                    $("#ProjectDuration").prop("disabled", false);
                }
            },
            events: function () {
                $("#Input_ProjectDurationHidden").on("click", function () {
                    if ($("#Input_ProjectDurationHidden").is(':checked')) {
                        $("#Input_ProjectDurationWeight").val(0);
                        $("#Input_ProjectDurationWeight").prop("disabled", true);
                        $("#ProjectDuration").prop("disabled", true);
                    }
                    else {
                        $("#Input_ProjectDurationWeight").prop("disabled", false);
                        $("#ProjectDuration").prop("disabled", false);
                    }
                });
            }
        },
        postulationAttachmentFiles: {
            init: function () {
                this.load();
                this.events();
            },
            load: function () {
                if ($("#Input_PostulationAttachmentFilesHidden").is(':checked')) {
                    $("#Input_PostulationAttachmentFilesWeight").val(0);
                    $("#Input_PostulationAttachmentFilesWeight").prop("disabled", true);
                    $("#PostulationAttachmentFiles").prop("disabled", true);
                }
                else {
                    $("#Input_PostulationAttachmentFilesWeight").prop("disabled", false);
                    $("#PostulationAttachmentFiles").prop("disabled", false);
                }
            },
            events: function () {
                $("#Input_PostulationAttachmentFilesHidden").on("click", function () {
                    if ($("#Input_PostulationAttachmentFilesHidden").is(':checked')) {
                        $("#Input_PostulationAttachmentFilesWeight").val(0);
                        $("#Input_PostulationAttachmentFilesWeight").prop("disabled", true);
                        $("#PostulationAttachmentFiles").prop("disabled", true);
                    }
                    else {
                        $("#Input_PostulationAttachmentFilesWeight").prop("disabled", false);
                        $("#PostulationAttachmentFiles").prop("disabled", false);
                    }
                });
            }
        },
        questions: {
            init: function () {
                this.load();
                this.events();
            },
            load: function () {
                if ($("#Input_QuestionsHidden").is(':checked')) {
                    $("#Input_QuestionsWeight").val(0);
                    $("#Input_QuestionsWeight").prop("disabled", true);
                    $("#newQuestion").prop("disabled", true);
                    $("#aditional-questions").css("display", "none");
                    
                }
                else {
                    $("#Input_QuestionsWeight").prop("disabled", false);
                    $("#newQuestion").prop("disabled", false);
                    $("#aditional-questions").css("display", "block");
                }
            },
            events: function () {
                $("#Input_QuestionsHidden").on("click", function () {
                    if ($("#Input_QuestionsHidden").is(':checked')) {
                        $("#Input_QuestionsWeight").val(0);
                        $("#Input_QuestionsWeight").prop("disabled", true);
                        $("#newQuestion").prop("disabled", true);
                        $("#aditional-questions").css("display", "none");
                    }
                    else {
                        $("#Input_QuestionsWeight").prop("disabled", false);
                        $("#newQuestion").prop("disabled", false);
                        $("#aditional-questions").css("display", "block");
                    }
                });
            }
        },

    };
    var select = {
        init: function () {
            this.investigationType.init();
            this.externalEntity.init();
            this.investigationPattern.init();
            this.area.init();
            this.faculty.init();
            this.career.init();
            this.researchCenter.init();
            this.financing.init();
            this.researchLineCategory.init();
            this.methodologyType.init();
            this.questionType.init();
        },
        investigationType: {
            init: function () {
                this.load();
            },
            load: function () {
                $.ajax({
                    url: (`/api/tiposdeinvestigacion/select/get`).proto().parseURL(),
                    type: "GET"
                }).done(function (result) {
                    $("#InvestigationType").select2({
                        data: result,
                    });
                });
            }
        },
        externalEntity: {
            init: function () {
                this.load();
            },
            load: function () {
                $.ajax({
                    url: (`/api/entidadexterna/select/get`).proto().parseURL(),
                    type: "GET"
                }).done(function (result) {
                    $("#ExternalEntity").select2({
                        data: result,
                    });
                });
            }
        },
        investigationPattern: {
            init: function () {
                this.load();
            },
            load: function () {
                $.ajax({
                    url: (`/api/formasdeinvestigacion/select/get`).proto().parseURL(),
                    type: "GET"
                }).done(function (result) {
                    $("#InvestigationPattern").select2({
                        data: result,
                    });
                });
            }
        },
        area: {
            init: function () {
                this.load();
            },
            load: function () {
                $.ajax({
                    url: (`/api/areadeinvestigacion/select/get`).proto().parseURL(),
                    type: "GET"
                }).done(function (result) {
                    $("#Area").select2({
                        data: result,
                    });
                });
            }
        },
        faculty: {
            init: function () {
                this.load();
                this.events();
            },
            load: function () {
                $.ajax({
                    url: (`/api/facultades/get`).proto().parseURL(),
                    type: "GET"
                }).done(function (result) {
                    $("#Faculty").select2({
                        data: result,
                    });
                });
            },
            events: function () {
                $("#Faculty").on("change", function () {
                    if ($(this).val() == 0) {
                        select.career.clear();
                    } else {
                        select.career.load();
                    }
                });
            }
        },
        career: {
            init: function () {
                $("#Career").select2();
            },
            clear: function () {
                $("#Career").empty();
                $("#Career").html('<option value="0" selected disabled>Selecciona Carrera</option>')
            },
            load: function () {
                select.career.clear();
                $.ajax({
                    url: (`/api/carreras/get`).proto().parseURL(),
                    type: "GET",
                    data: {
                        facultyId: $("#Faculty").val()
                    }
                }).done(function (result) {
                    $("#Career").select2({
                        data: result,
                    });
                });
            },
            
        },
        researchCenter: {
            init: function () {
                this.load();
            },
            load: function () {
                $.ajax({
                    url: (`/api/centro-investigacion/get`).proto().parseURL(),
                    type: "GET",
                }).done(function (result) {
                    $("#ResearchCenter").select2({
                        data: result,
                    });
                });
            }
        },
        financing: {
            init: function () {
                this.load();
            },
            load: function () {
                $.ajax({
                    url: (`/api/financiamiento/select/get`).proto().parseURL(),
                    type: "GET"
                }).done(function (result) {
                    $("#Financing").select2({
                        data: result,
                    });
                });
            }
        },
        researchLineCategory: {
            init: function () {
                this.load();
            },
            load: function () {
                $(".researchLineCategory-selects").select2();
            }
        },
        methodologyType: {
            init: function () {
                this.load();
            },
            load: function () {
                $.ajax({
                    url: (`/api/tiposdemetodologia/select/get`).proto().parseURL(),
                    type: "GET"
                }).done(function (result) {
                    $("#MethodologyType").select2({
                        data: result,
                    });
                });
            }
        },      
        questionType: {
            init: function () {
                this.load();
                this.events();
            },

            load: function () {
                $(".select2-questionType").select2();

            },
            events: function () {
                $('#add-question-form select[name="QuestionType"]').on('change', function () {
                    switch (this.value) {
                        case "0":
                            $("#add-question-form .question-text").css("display", "block");
                            $("#add-question-form .question-multiple").css("display", "none");
                            $("#add-question-form .formanswers").css("display", "none");
                            $('#add-question-form .formanswers').empty();
                            break;
                        case "1":
                            $("#add-question-form .question-text").css("display", "block");
                            $("#add-question-form .question-multiple").css("display", "none");
                            $("#add-question-form .formanswers").css("display", "none");
                            $('#add-question-form .formanswers').empty();
                            break;
                        case "2":
                            $("#add-question-form .question-text").css("display", "none");
                            $("#add-question-form .question-multiple").css("display", "block");
                            $("#add-question-form .formanswers").css("display", "block");
                            break;
                        case "3":
                            $("#add-question-form .question-text").css("display", "none");
                            $("#add-question-form .question-multiple").css("display", "block");
                            $("#add-question-form .formanswers").css("display", "block");
                            break;
                    }


                });

                $('#edit-question-form select[name="QuestionType"]').on('change', function () {
                    switch (this.value) {
                        case "0":
                            $("#edit-question-form .question-text").css("display", "block");
                            $("#edit-question-form .question-multiple").css("display", "none");
                            $("#edit-question-form .formanswers").css("display", "none");
                            $('#edit-question-form .formanswers').empty();
                            break;
                        case "1":
                            $("#edit-question-form .question-text").css("display", "block");
                            $("#edit-question-form .question-multiple").css("display", "none");
                            $("#edit-question-form .formanswers").css("display", "none");
                            $('#edit-question-form .formanswers').empty();
                            break;
                        case "2":
                            $("#edit-question-form .question-text").css("display", "none");
                            $("#edit-question-form .question-multiple").css("display", "block");
                            $("#edit-question-form .formanswers").css("display", "block");
                            break;
                        case "3":
                            $("#edit-question-form .question-text").css("display", "none");
                            $("#edit-question-form .question-multiple").css("display", "block");
                            $("#edit-question-form .formanswers").css("display", "block");
                            break;
                    }
                });
                
            }
        },
    };

    var answerInput = {
        addModal: {
            draw: function () {
                var htmldata = ''
                htmldata += '<div class="form-group col-lg-12" style="display:flex;">';
                htmldata += '   <input class="form-control m-input answer" style="margin-right: 10px;" required>';
                htmldata += '   <button class="btn btn-danger btn-sm m-btn--icon delete-answer" type="button" onclick="this.parentNode.outerHTML = \'\';"><span><i class="la la-trash"></i></span></button>'
                htmldata += '</div>'
                var e = document.createElement('div');
                e.innerHTML = htmldata;
                $('#add-question-form .formanswers').append(e.firstChild);
            },            
        },
        editModal: {
            draw: function () {
                var htmldata = ''
                htmldata += '<div class="form-group col-lg-12" style="display:flex;">';
                htmldata += '   <input class="answerId" hidden/>';
                htmldata += '   <input class="form-control m-input answer" style="margin-right: 10px;" required>';
                htmldata += '   <button class="btn btn-danger btn-sm m-btn--icon delete-answer" type="button" onclick="this.parentNode.outerHTML = \'\';"><span><i class="la la-trash"></i></span></button>'
                htmldata += '</div>'
                var e = document.createElement('div');
                e.innerHTML = htmldata;
                $('#edit-question-form .formanswers').append(e.firstChild);
            }
        }
    }

    return {
        init: function () {
            select.init();
            checkbox.init();
            switchInput.init();
            modal.init();
            questionPartial.init();
        }
    }
}();

$(function () {
    InitApp.init();
})