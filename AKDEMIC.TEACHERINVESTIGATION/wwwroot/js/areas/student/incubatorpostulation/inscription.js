var InitApp = function () {

    var incubatorPostulationId = $("#IncubatorPostulationId").val();

    var sections = {
        init: function () {
            this.generalInformation.init();
            this.investigationTeam.init();
            this.businessIdea.init();
            this.businessPlan.init();
            this.schedule.init();
        },
        generalInformation: {
            init: function () {
                this.load();
            },
            edit: {
                object: $("#general-information-form").validate({
                    submitHandler: function (formElement, e) {
                        $("#btnGeneralInformationSave").addLoader();
                        e.preventDefault();
                        let formData = new FormData(formElement);
                        formData.append("DepartmentText", $("#DepartmentId option:selected").text());
                        formData.append("ProvinceText", $("#ProvinceId option:selected").text());
                        formData.append("DistrictText", $("#DistrictId option:selected").text());
                        $.ajax({
                            url: $(formElement).attr("action"),
                            type: "POST",
                            data: formData,
                            contentType: false,
                            processData: false
                        }).done(function () {
                            toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                            $("#btnGeneralInformationSave").removeLoader();
                        }).fail(function (error) {
                            toastr.error(error.responseText, _app.constants.toastr.title.error);
                            $("#btnGeneralInformationSave").removeLoader();
                        }).always(function () {

                        });
                    }
                }),
            },
            load: function () {
                $.ajax({
                    url: `/alumno/postulacion-convocatoria-emprendimiento/${incubatorPostulationId}/inscripcion`.proto().parseURL(),
                    type: "GET",
                    data: {
                        handler: "GeneralInformationLoad",
                    },
                }).done(function (result) {
                    $('#general-information-form input[Name=IncubatorConvocationCode]').val(result.incubatorConvocationCode);
                    $('#general-information-form input[Name=IncubatorConvocationName]').val(result.incubatorConvocationName);
                    $('#general-information-form input[Name=Title]').val(result.title);
                    $('#general-information-form input[Name=Budget]').val(result.budget);
                    $('#general-information-form input[Name=MonthDuration]').val(result.monthDuration);
                    $('#general-information-form textarea[Name=GeneralGoals]').val(result.generalGoals);
                    $('#general-information-form select[Name=DepartmentId]').val(result.departmentId).trigger("change");
                    $('#general-information-form select[Name=ProvinceId]').val(result.provinceId).trigger("change");
                    $('#general-information-form select[Name=DistrictId]').val(result.districtId).trigger("change");
                });
            }
        },
        investigationTeam: {
            init: function () {
                this.load();
            },
            edit: {
                object: $("#investigation-team-form").validate({
                    submitHandler: function (formElement, e) {
                        $("#btnInvestigationTeamSave").addLoader();
                        e.preventDefault();
                        $.ajax({
                            url: $(formElement).attr("action"),
                            type: "POST",
                            data: $(formElement).serialize()
                        }).done(function () {
                            toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                            $("#btnInvestigationTeamSave").removeLoader();
                        }).fail(function (error) {
                            toastr.error(error.responseText, _app.constants.toastr.title.error);
                            $("#btnInvestigationTeamSave").removeLoader();
                        }).always(function () {

                        });
                    }
                }),
            },
            load: function () {
               
                $.ajax({
                    url: `/alumno/postulacion-convocatoria-emprendimiento/${incubatorPostulationId}/inscripcion`.proto().parseURL(),
                    type: "GET",
                    data: {
                        handler: "InvestigationTeamLoad",
                    },
                }).done(function (result) {
                    if (result.adviserId != null) {
                        $.ajax({
                            url: `/api/docentes/${result.adviserId}/get-info`.proto().parseURL(),
                            type: "GET"
                        }).done(function (result) {

                            $("#AdviserId").empty();
                            $("#AdviserId").html(`<Option value="0" selected disabled>${result.userName} - ${result.fullName}<option/>`)

                            $("#investigation-team-form input[name='AdviserFullName']").val(result.fullName);
                            $("#investigation-team-form input[name='AdviserDni']").val(result.dni);
                            $("#investigation-team-form input[name='AdviserAcademicDepartment']").val(result.academicDepartment);
                            $("#investigation-team-form input[name='AdviserCategory']").val(result.category);
                            $("#investigation-team-form input[name='AdviserDedication']").val(result.teacherDedication);
                        });
                    }

                    if (result.coAdviserId != null) {
                        $.ajax({
                            url: `/api/docentes/${result.coAdviserId}/get-info`.proto().parseURL(),
                            type: "GET"
                        }).done(function (result) {

                            $("#CoAdviserId").empty();
                            $("#CoAdviserId").html(`<Option value="0" selected disabled>${result.userName} - ${result.fullName}<option/>`)

                            $("#investigation-team-form input[name='CoAdviserFullName']").val(result.fullName);
                            $("#investigation-team-form input[name='CoAdviserDni']").val(result.dni);
                            $("#investigation-team-form input[name='CoAdviserAcademicDepartment']").val(result.academicDepartment);
                            $("#investigation-team-form input[name='CoAdviserCategory']").val(result.category);
                            $("#investigation-team-form input[name='CoAdviserDedication']").val(result.teacherDedication);
                        });
                    }
                });






            }
        },
        businessIdea: {
            init: function () {
                this.load();
            },
            edit: {
                object: $("#business-idea-form").validate({
                    submitHandler: function (formElement, e) {
                        $("#btnBusinessIdeaSave").addLoader();
                        e.preventDefault();
                        $.ajax({
                            url: $(formElement).attr("action"),
                            type: "POST",
                            data: $(formElement).serialize()
                        }).done(function () {
                            toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                            $("#btnBusinessIdeaSave").removeLoader();
                        }).fail(function (error) {
                            toastr.error(error.responseText, _app.constants.toastr.title.error);
                            $("#btnBusinessIdeaSave").removeLoader();
                        }).always(function () {

                        });
                    }
                }),
            },
            load: function () {
                $.ajax({
                    url: `/alumno/postulacion-convocatoria-emprendimiento/${incubatorPostulationId}/inscripcion`.proto().parseURL(),
                    type: "GET",
                    data: {
                        handler: "BusinessIdeaLoad",
                    },
                }).done(function (result) {
                    $('#business-idea-form textarea[Name=BusinessIdeaDescription]').val(result.businessIdeaDescription);
                    $('#business-idea-form textarea[Name=CompetitiveAdvantages]').val(result.competitiveAdvantages);
                    $('#business-idea-form textarea[Name=MarketStudy]').val(result.marketStudy);
                    $('#business-idea-form textarea[Name=MarketingPlan]').val(result.marketingPlan);
                    $('#business-idea-form textarea[Name=Resources]').val(result.resources);
                    $('#business-idea-form textarea[Name=PotentialStrategicPartners]').val(result.potentialStrategicPartners);
                });
            }
        },
        businessPlan: {
            init: function () {
                this.load();
            },
            edit: {
                object: $("#business-plan-form").validate({
                    submitHandler: function (formElement, e) {
                        $("#btnBusinessPlanSave").addLoader();
                        e.preventDefault();
                        $.ajax({
                            url: $(formElement).attr("action"),
                            type: "POST",
                            data: $(formElement).serialize()
                        }).done(function () {
                            toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                            $("#btnBusinessPlanSave").removeLoader();
                        }).fail(function (error) {
                            toastr.error(error.responseText, _app.constants.toastr.title.error);
                            $("#btnBusinessPlanSave").removeLoader();
                        }).always(function () {

                        });
                    }
                }),
            },
            load: function () {
                $.ajax({
                    url: `/alumno/postulacion-convocatoria-emprendimiento/${incubatorPostulationId}/inscripcion`.proto().parseURL(),
                    type: "GET",
                    data: {
                        handler: "BusinessPlanLoad",
                    },
                }).done(function (result) {
                    $('#business-plan-form textarea[Name=Mission]').val(result.mission);
                    $('#business-plan-form textarea[Name=ProductDescription]').val(result.productDescription);
                    $('#business-plan-form textarea[Name=TechnicalViability]').val(result.technicalViability);
                    $('#business-plan-form textarea[Name=EconomicViability]').val(result.economicViability);
                    $('#business-plan-form textarea[Name=MerchandisingPlan]').val(result.merchandisingPlan);
                    $('#business-plan-form textarea[Name=Breakeven]').val(result.breakeven);
                    $('#business-plan-form textarea[Name=AffectationLevel]').val(result.affectationLevel);
                });
            }
        },
        schedule: {
            init: function () {
                this.load();
                this.events();
            },
            load: function () {
                $.ajax({
                    url: `/alumno/postulacion-convocatoria-emprendimiento/${incubatorPostulationId}/inscripcion`.proto().parseURL(),
                    type: "GET",
                    data: {
                        handler: "SchedulePage",
                    },
                }).done(function (result) {
                    $("#scheduleContainer").html(result);
                });
            },
            events: function () {
                $("#schedule_tab").on("click", function () {
                    sections.schedule.load();
                    datatable.equipmentExpense.reload();
                    datatable.suppliesExpense.reload();
                    datatable.thirdPartyServiceExpense.reload();
                    datatable.otherExpense.reload();
                });
            }
        }
    };

    var select = {
        init: function () {
            this.department.init();
            this.province.init();
            this.district.init();
            this.teamMember.init();
            this.adviser.init();
            this.coAdviser.init();
        },
        department: {
            init: function () {
                this.load();
                this.events();
            },
            load: function () {
                $.ajax({
                    url: (`/api/departamentos/get`).proto().parseURL(),
                    type: "GET"
                }).done(function (result) {
                    $("#DepartmentId").select2({
                        data: result,
                    });
                });
            },
            events: function () {
                $("#DepartmentId").on("change", function () {
                    if ($(this).val() == 0) {
                        select.province.clear();
                        select.district.clear();
                    } else {
                        select.province.load();
                    }
                });
            }
        },
        province: {
            init: function () {
                $("#ProvinceId").select2();
                this.events();
            },
            clear: function () {
                $("#ProvinceId").empty();
                $("#ProvinceId").html('<option value="0" selected disabled>Selecciona Provincia</option>')
            },
            load: function () {
                select.province.clear();
                select.district.clear();
                $.ajax({
                    url: (`/api/provincias/get`).proto().parseURL(),
                    type: "GET",
                    data: {
                        departmentId: $("#DepartmentId").val()
                    }
                }).done(function (result) {
                    $("#ProvinceId").select2({
                        data: result,
                    });
                });
            },
            events: function () {
                $("#ProvinceId").on("change", function () {
                    if ($(this).val() == 0) {
                        select.district.clear();
                    } else {
                        select.district.load();
                    }
                });
            }
        },
        district: {
            init: function () {
                $("#DistrictId").select2();
            },
            clear: function () {
                $("#DistrictId").empty();
                $("#DistrictId").html('<option value="0" selected disabled>Selecciona Distrito</option>')
            },
            load: function () {
                select.district.clear();
                $.ajax({
                    url: (`/api/distritos/get`).proto().parseURL(),
                    type: "GET",
                    data: {
                        provinceId: $("#ProvinceId").val()
                    }
                }).done(function (result) {
                    $("#DistrictId").select2({
                        data: result,
                    });
                });
            }
        },
        teamMember: {
            init: function () {
                $("#teamMember-add-form select[name=UserId]").select2({
                    width: "100%",
                    dropdownParent: $('#teamMemberAddModal'),
                    placeholder: "Buscar...",
                    ajax: {
                        url: "/api/estudiantes/select-search".proto().parseURL(),
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


                $("#teamMember-add-form select[name=UserId]").on("change", function () {
                    let userName = $(this).select2('data')[0].userName;
                    let paternalSurname = $(this).select2('data')[0].paternalSurname;
                    let maternalSurname = $(this).select2('data')[0].maternalSurname;
                    let name = $(this).select2('data')[0].name;
                    let career = $(this).select2('data')[0].career;
                    let sex = $(this).select2('data')[0].sex;

                    $("#teamMember-add-form input[name='UserName']").val(userName);
                    $("#teamMember-add-form input[name='PaternalSurname']").val(paternalSurname);
                    $("#teamMember-add-form input[name='MaternalSurname']").val(maternalSurname);
                    $("#teamMember-add-form input[name='Name']").val(name);
                    $("#teamMember-add-form input[name='Sex']").val(sex);
                    $("#teamMember-add-form input[name='CareerText']").val(career);
                });
            }
        },
        adviser: {
            init: function () {

                $("#investigation-team-form select[name=AdviserId]").select2({
                    width: "100%",
                    placeholder: "Buscar...",
                    ajax: {
                        url: "/api/docentes/select-search".proto().parseURL(),
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



                $("#investigation-team-form select[name=AdviserId]").on("change", function () {
                    let fullName = $(this).select2('data')[0].fullName;
                    let dni = $(this).select2('data')[0].dni;
                    let academicDepartment = $(this).select2('data')[0].academicDepartment;
                    let category = $(this).select2('data')[0].category;
                    let teacherDedication = $(this).select2('data')[0].teacherDedication;

                    $("#investigation-team-form input[name='AdviserFullName']").val(fullName);
                    $("#investigation-team-form input[name='AdviserDni']").val(dni);
                    $("#investigation-team-form input[name='AdviserAcademicDepartment']").val(academicDepartment);
                    $("#investigation-team-form input[name='AdviserCategory']").val(category);
                    $("#investigation-team-form input[name='AdviserDedication']").val(teacherDedication);
                });
            }
        },
        coAdviser: {
            init: function () {
                $("#investigation-team-form select[name=CoAdviserId]").select2({
                    width: "100%",
                    placeholder: "Buscar...",
                    ajax: {
                        url: "/api/docentes/select-search".proto().parseURL(),
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

                $("#investigation-team-form select[name=CoAdviserId]").on("change", function () {
                    let fullName = $(this).select2('data')[0].fullName;
                    let dni = $(this).select2('data')[0].dni;
                    let academicDepartment = $(this).select2('data')[0].academicDepartment;
                    let category = $(this).select2('data')[0].category;
                    let teacherDedication = $(this).select2('data')[0].teacherDedication;

                    $("#investigation-team-form input[name='CoAdviserFullName']").val(fullName);
                    $("#investigation-team-form input[name='CoAdviserDni']").val(dni);
                    $("#investigation-team-form input[name='CoAdviserAcademicDepartment']").val(academicDepartment);
                    $("#investigation-team-form input[name='CoAdviserCategory']").val(category);
                    $("#investigation-team-form input[name='CoAdviserDedication']").val(teacherDedication);
                });
            }
        }
    };

    var modal = {
        init: function () {
            this.specificGoal.init();
            this.equipmentExpense.init();
            this.suppliesExpense.init();
            this.thirdPartyServiceExpense.init();
            this.otherExpense.init();
            this.teamMember.init();
            this.activity.init();
        },
        specificGoal: {
            init: function () {
                this.create.events();
            },
            create: {
                object: $("#specific-goal-add-form").validate({
                    submitHandler: function (formElement, e) {
                        $("#btnSpecificGoalAdd").addLoader();
                        e.preventDefault();
                        $.ajax({
                            url: $(formElement).attr("action"),
                            type: "POST",
                            data: $(formElement).serialize()
                        }).done(function () {
                            $(".modal").modal("hide");
                            $(".m-alert").addClass("m--hide");
                            toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                            datatable.specificGoal.reload();
                            modal.specificGoal.create.clear();
                            $("#btnSpecificGoalAdd").removeLoader();
                        }).fail(function (error) {
                            toastr.error(error.responseText, _app.constants.toastr.title.error);
                            $("#btnSpecificGoalAdd").removeLoader();
                        }).always(function () {

                        });
                    }
                }),
                show: function () {
                    $("#btnSpecificGoalAdd").removeLoader();
                    $("#specificGoalAddModal").modal("toggle");
                },
                clear: function () {
                    modal.specificGoal.create.object.resetForm();
                },
                events: function () {
                    $("#specificGoalAddModal").on("hidden.bs.modal", function () {
                        modal.specificGoal.create.clear();
                    });
                }
            },
            edit: {
                object: $("#specific-goal-edit-form").validate({
                    submitHandler: function (formElement, e) {
                        $("#btnSpecificGoalEdit").addLoader();
                        e.preventDefault();
                        $.ajax({
                            url: $(formElement).attr("action"),
                            type: "POST",
                            data: $(formElement).serialize()
                        }).done(function () {
                            $(".modal").modal("hide");
                            $(".m-alert").addClass("m--hide");
                            toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                            datatable.specificGoal.reload();
                            modal.specificGoal.edit.clear();
                        }).fail(function (error) {
                            toastr.error(error.responseText, _app.constants.toastr.title.error);
                            $("#btnSpecificGoalEdit").removeLoader();
                        }).always(function () {

                        });
                    }
                }),
                load: function (id) {
                    $.ajax({
                        url: `/alumno/postulacion-convocatoria-emprendimiento/${incubatorPostulationId}/inscripcion`.proto().parseURL(),
                        type: "GET",
                        data: {
                            handler: "SpecificGoalLoad",
                            id: id
                        },
                    }).done(function (result) {
                        $("#specific-goal-edit-form input[name=Id]").val(result.id);
                        $("#specific-goal-edit-form input[name=Description]").val(result.description);
                        $("#specific-goal-edit-form input[name=OrderNumber]").val(result.orderNumber);
                    }).fail(function (error) {
                        toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                    }).always(function () {

                    });
                },
                show: function (id) {
                    $("#btnSpecificGoalEdit").removeLoader();
                    modal.specificGoal.edit.load(id);
                    $("#specificGoalEditModal").modal("toggle");
                },
                clear: function () {
                    modal.specificGoal.edit.object.resetForm();
                },
                events: function () {
                    $("#specificGoalEditModal").on("hidden.bs.modal", function () {
                        modal.specificGoal.edit.clear();
                    });
                }
            },
            delete: function (id){
                swal({
                    title: "¿Está seguro?",
                    text: "El objetivo especifico será eliminado.",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonText: "Sí, eliminarlo",
                    confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                    cancelButtonText: "Cancelar",
                    showLoaderOnConfirm: true,
                    preConfirm: () => {
                        return new Promise((resolve) => {
                            $.ajax({
                                url: `/alumno/postulacion-convocatoria-emprendimiento/${incubatorPostulationId}/inscripcion?handler=SpecificGoalDelete`.proto().parseURL(),
                                type: "POST",
                                data: {
                                    id: id
                                },
                                beforeSend: function (xhr) {
                                    xhr.setRequestHeader("XSRF-TOKEN",
                                        $('#general-information-form input:hidden[name="__RequestVerificationToken"]').val());
                                },
                                success: function (result) {
                                    swal({
                                        type: "success",
                                        title: "Completado",
                                        text: "El objetivo especifico ha sido eliminado con exito",
                                        confirmButtonText: "Excelente"
                                    }).then(datatable.specificGoal.reload());
                                },
                                error: function (errormessage) {
                                    swal({
                                        type: "error",
                                        title: "Error",
                                        confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                        confirmButtonText: "Entendido",
                                        text: "El objetivo especifico tiene información relacionada"
                                    });
                                }
                            });
                        });
                    },
                    allowOutsideClick: () => !swal.isLoading()
                });
            }
        },
        equipmentExpense: {
            init: function () {
                this.create.events();
            },
            create: {
                object: $("#equipment-expense-add-form").validate({
                    submitHandler: function (formElement, e) {
                        $("#btnEquipmentExpenseAdd").addLoader();
                        e.preventDefault();
                        $.ajax({
                            url: $(formElement).attr("action"),
                            type: "POST",
                            data: $(formElement).serialize()
                        }).done(function () {
                            $(".modal").modal("hide");
                            $(".m-alert").addClass("m--hide");
                            toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                            datatable.equipmentExpense.reload();
                            modal.equipmentExpense.create.clear();
                            $("#btnEquipmentExpenseAdd").removeLoader();
                        }).fail(function (error) {
                            toastr.error(error.responseText, _app.constants.toastr.title.error);
                            $("#btnEquipmentExpenseAdd").removeLoader();
                        }).always(function () {

                        });
                    }
                }),
                show: function () {
                    $("#btnEquipmentExpenseAdd").removeLoader();
                    $("#equipmentExpenseAddModal").modal("toggle");
                },
                clear: function () {
                    modal.equipmentExpense.create.object.resetForm();
                },
                events: function () {
                    $("#equipmentExpenseAddModal").on("hidden.bs.modal", function () {
                        modal.equipmentExpense.create.clear();
                    });
                }
            },
            edit: {
                object: $("#equipment-expense-edit-form").validate({
                    submitHandler: function (formElement, e) {
                        $("#btnEquipmentExpenseEdit").addLoader();
                        e.preventDefault();
                        $.ajax({
                            url: $(formElement).attr("action"),
                            type: "POST",
                            data: $(formElement).serialize()
                        }).done(function () {
                            $(".modal").modal("hide");
                            $(".m-alert").addClass("m--hide");
                            toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                            datatable.equipmentExpense.reload();
                            modal.equipmentExpense.edit.clear();
                            $("#btnEquipmentExpenseEdit").removeLoader();
                        }).fail(function (error) {
                            toastr.error(error.responseText, _app.constants.toastr.title.error);
                            $("#btnEquipmentExpenseEdit").removeLoader();
                        }).always(function () {

                        });
                    }
                }),
                load: function (id) {
                    $.ajax({
                        url: `/alumno/postulacion-convocatoria-emprendimiento/${incubatorPostulationId}/inscripcion`.proto().parseURL(),
                        type: "GET",
                        data: {
                            handler: "EquipmentExpenseLoad",
                            id: id
                        },
                    }).done(function (result) {
                        $("#equipment-expense-edit-form input[name=Id]").val(result.id);
                        $("#equipment-expense-edit-form input[name=ExpenseCode]").val(result.expenseCode);
                        $("#equipment-expense-edit-form input[name=Description]").val(result.description);
                        $("#equipment-expense-edit-form input[name=MeasureUnit]").val(result.measureUnit);
                        $("#equipment-expense-edit-form input[name=Quantity]").val(result.quantity);
                        $("#equipment-expense-edit-form input[name=UnitPrice]").val(result.unitPrice);
                        $("#equipment-expense-edit-form input[name=ActivityJustification]").val(result.activityJustification);
                    }).fail(function (error) {
                        toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                    }).always(function () {

                    });
                },
                show: function (id) {
                    $("#btnEquipmentExpenseEdit").removeLoader();
                    modal.equipmentExpense.edit.load(id);
                    $("#equipmentExpenseEditModal").modal("toggle");
                },
                clear: function () {
                    modal.equipmentExpense.edit.object.resetForm();
                },
                events: function () {
                    $("#equipmentExpenseEditModal").on("hidden.bs.modal", function () {
                        modal.equipmentExpense.edit.clear();
                    });
                }
            },
            delete: function (id) {
                swal({
                    title: "¿Está seguro?",
                    text: "El Gasto de Equipo o Bien será eliminado.",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonText: "Sí, eliminarlo",
                    confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                    cancelButtonText: "Cancelar",
                    showLoaderOnConfirm: true,
                    preConfirm: () => {
                        return new Promise((resolve) => {
                            $.ajax({
                                url: `/alumno/postulacion-convocatoria-emprendimiento/${incubatorPostulationId}/inscripcion?handler=EquipmentExpenseDelete`.proto().parseURL(),
                                type: "POST",
                                data: {
                                    id: id
                                },
                                beforeSend: function (xhr) {
                                    xhr.setRequestHeader("XSRF-TOKEN",
                                        $('#general-information-form input:hidden[name="__RequestVerificationToken"]').val());
                                },
                                success: function (result) {
                                    swal({
                                        type: "success",
                                        title: "Completado",
                                        text: "El Gasto de Equipo o Bien  ha sido eliminado con exito",
                                        confirmButtonText: "Excelente"
                                    }).then(datatable.equipmentExpense.reload());
                                },
                                error: function (errormessage) {
                                    swal({
                                        type: "error",
                                        title: "Error",
                                        confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                        confirmButtonText: "Entendido",
                                        text: "El Gasto de Equipo o Bien  tiene información relacionada"
                                    });
                                }
                            });
                        });
                    },
                    allowOutsideClick: () => !swal.isLoading()
                });
            }
        },
        suppliesExpense: {
            init: function () {
                this.create.events();
            },
            create: {
                object: $("#supplies-expense-add-form").validate({
                    submitHandler: function (formElement, e) {
                        $("#btnSuppliesExpenseAdd").addLoader();
                        e.preventDefault();
                        $.ajax({
                            url: $(formElement).attr("action"),
                            type: "POST",
                            data: $(formElement).serialize()
                        }).done(function () {
                            $(".modal").modal("hide");
                            $(".m-alert").addClass("m--hide");
                            toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                            datatable.suppliesExpense.reload();
                            modal.suppliesExpense.create.clear();
                            $("#btnSuppliesExpenseAdd").removeLoader();
                        }).fail(function (error) {
                            toastr.error(error.responseText, _app.constants.toastr.title.error);
                            $("#btnSuppliesExpenseAdd").removeLoader();
                        }).always(function () {

                        });
                    }
                }),
                show: function () {
                    $("#btnSuppliesExpenseAdd").removeLoader();
                    $("#suppliesExpenseAddModal").modal("toggle");
                },
                clear: function () {
                    modal.suppliesExpense.create.object.resetForm();
                },
                events: function () {
                    $("#suppliesExpenseAddModal").on("hidden.bs.modal", function () {
                        modal.suppliesExpense.create.clear();
                    });
                }
            },
            edit: {
                object: $("#supplies-expense-edit-form").validate({
                    submitHandler: function (formElement, e) {
                        $("#btnSuppliesExpenseEdit").addLoader();
                        e.preventDefault();
                        $.ajax({
                            url: $(formElement).attr("action"),
                            type: "POST",
                            data: $(formElement).serialize()
                        }).done(function () {
                            $(".modal").modal("hide");
                            $(".m-alert").addClass("m--hide");
                            toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                            datatable.suppliesExpense.reload();
                            modal.suppliesExpense.edit.clear();
                            $("#btnSuppliesExpenseEdit").removeLoader();
                        }).fail(function (error) {
                            toastr.error(error.responseText, _app.constants.toastr.title.error);
                            $("#btnSuppliesExpenseEdit").removeLoader();
                        }).always(function () {

                        });
                    }
                }),
                load: function (id) {
                    $.ajax({
                        url: `/alumno/postulacion-convocatoria-emprendimiento/${incubatorPostulationId}/inscripcion`.proto().parseURL(),
                        type: "GET",
                        data: {
                            handler: "SuppliesExpenseLoad",
                            id: id
                        },
                    }).done(function (result) {
                        $("#supplies-expense-edit-form input[name=Id]").val(result.id);
                        $("#supplies-expense-edit-form input[name=ExpenseCode]").val(result.expenseCode);
                        $("#supplies-expense-edit-form input[name=Description]").val(result.description);
                        $("#supplies-expense-edit-form input[name=MeasureUnit]").val(result.measureUnit);
                        $("#supplies-expense-edit-form input[name=Quantity]").val(result.quantity);
                        $("#supplies-expense-edit-form input[name=UnitPrice]").val(result.unitPrice);
                        $("#supplies-expense-edit-form input[name=ActivityJustification]").val(result.activityJustification);
                    }).fail(function (error) {
                        toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                    }).always(function () {

                    });
                },
                show: function (id) {
                    $("#btnSuppliesExpenseEdit").removeLoader();
                    modal.suppliesExpense.edit.load(id);
                    $("#suppliesExpenseEditModal").modal("toggle");
                },
                clear: function () {
                    modal.suppliesExpense.edit.object.resetForm();
                },
                events: function () {
                    $("#suppliesExpenseEditModal").on("hidden.bs.modal", function () {
                        modal.suppliesExpense.edit.clear();
                    });
                }
            },
            delete: function (id) {
                swal({
                    title: "¿Está seguro?",
                    text: "El Insumo o Material será eliminado.",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonText: "Sí, eliminarlo",
                    confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                    cancelButtonText: "Cancelar",
                    showLoaderOnConfirm: true,
                    preConfirm: () => {
                        return new Promise((resolve) => {
                            $.ajax({
                                url: `/alumno/postulacion-convocatoria-emprendimiento/${incubatorPostulationId}/inscripcion?handler=SuppliesExpenseDelete`.proto().parseURL(),
                                type: "POST",
                                data: {
                                    id: id
                                },
                                beforeSend: function (xhr) {
                                    xhr.setRequestHeader("XSRF-TOKEN",
                                        $('#general-information-form input:hidden[name="__RequestVerificationToken"]').val());
                                },
                                success: function (result) {
                                    swal({
                                        type: "success",
                                        title: "Completado",
                                        text: "El Insumo o Material ha sido eliminado con exito",
                                        confirmButtonText: "Excelente"
                                    }).then(datatable.suppliesExpense.reload());
                                },
                                error: function (errormessage) {
                                    swal({
                                        type: "error",
                                        title: "Error",
                                        confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                        confirmButtonText: "Entendido",
                                        text: "El Insumo o Material tiene información relacionada"
                                    });
                                }
                            });
                        });
                    },
                    allowOutsideClick: () => !swal.isLoading()
                });
            }
        },
        thirdPartyServiceExpense: {
            init: function () {
                this.create.events();
            },
            create: {
                object: $("#third-party-service-expense-add-form").validate({
                    submitHandler: function (formElement, e) {
                        $("#btnThirdPartyServiceExpenseAdd").addLoader();
                        e.preventDefault();
                        $.ajax({
                            url: $(formElement).attr("action"),
                            type: "POST",
                            data: $(formElement).serialize()
                        }).done(function () {
                            $(".modal").modal("hide");
                            $(".m-alert").addClass("m--hide");
                            toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                            datatable.thirdPartyServiceExpense.reload();
                            modal.thirdPartyServiceExpense.create.clear();
                            $("#btnThirdPartyServiceExpenseAdd").removeLoader();
                        }).fail(function (error) {
                            toastr.error(error.responseText, _app.constants.toastr.title.error);
                            $("#btnThirdPartyServiceExpenseAdd").removeLoader();
                        }).always(function () {

                        });
                    }
                }),
                show: function () {
                    $("#btnThirdPartyServiceExpenseAdd").removeLoader();
                    $("#thirdPartyServiceExpenseAddModal").modal("toggle");
                },
                clear: function () {
                    modal.thirdPartyServiceExpense.create.object.resetForm();
                },
                events: function () {
                    $("#thirdPartyServiceExpenseAddModal").on("hidden.bs.modal", function () {
                        modal.thirdPartyServiceExpense.create.clear();
                    });
                }
            },
            edit: {
                object: $("#third-party-service-expense-edit-form").validate({
                    submitHandler: function (formElement, e) {
                        $("#btnThirdPartyServiceExpenseEdit").addLoader();
                        e.preventDefault();
                        $.ajax({
                            url: $(formElement).attr("action"),
                            type: "POST",
                            data: $(formElement).serialize()
                        }).done(function () {
                            $(".modal").modal("hide");
                            $(".m-alert").addClass("m--hide");
                            toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                            datatable.thirdPartyServiceExpense.reload();
                            modal.thirdPartyServiceExpense.edit.clear();
                            $("#btnThirdPartyServiceExpenseEdit").removeLoader();
                        }).fail(function (error) {
                            toastr.error(error.responseText, _app.constants.toastr.title.error);
                            $("#btnThirdPartyServiceExpenseEdit").removeLoader();
                        }).always(function () {

                        });
                    }
                }),
                load: function (id) {
                    $.ajax({
                        url: `/alumno/postulacion-convocatoria-emprendimiento/${incubatorPostulationId}/inscripcion`.proto().parseURL(),
                        type: "GET",
                        data: {
                            handler: "ThirdPartyServiceExpenseLoad",
                            id: id
                        },
                    }).done(function (result) {
                        $("#third-party-service-expense-edit-form input[name=Id]").val(result.id);
                        $("#third-party-service-expense-edit-form input[name=ExpenseCode]").val(result.expenseCode);
                        $("#third-party-service-expense-edit-form input[name=Description]").val(result.description);
                        $("#third-party-service-expense-edit-form input[name=MeasureUnit]").val(result.measureUnit);
                        $("#third-party-service-expense-edit-form input[name=Quantity]").val(result.quantity);
                        $("#third-party-service-expense-edit-form input[name=UnitPrice]").val(result.unitPrice);
                        $("#third-party-service-expense-edit-form input[name=ActivityJustification]").val(result.activityJustification);
                    }).fail(function (error) {
                        toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                    }).always(function () {

                    });
                },
                show: function (id) {
                    $("#btnThirdPartyServiceExpenseEdit").removeLoader();
                    modal.thirdPartyServiceExpense.edit.load(id);
                    $("#thirdPartyServiceExpenseEditModal").modal("toggle");
                },
                clear: function () {
                    modal.thirdPartyServiceExpense.edit.object.resetForm();
                },
                events: function () {
                    $("#thirdPartyServiceExpenseEditModal").on("hidden.bs.modal", function () {
                        modal.thirdPartyServiceExpense.edit.clear();
                    });
                }
            },
            delete: function (id) {
                swal({
                    title: "¿Está seguro?",
                    text: "El Servicio de Terceros será eliminado.",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonText: "Sí, eliminarlo",
                    confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                    cancelButtonText: "Cancelar",
                    showLoaderOnConfirm: true,
                    preConfirm: () => {
                        return new Promise((resolve) => {
                            $.ajax({
                                url: `/alumno/postulacion-convocatoria-emprendimiento/${incubatorPostulationId}/inscripcion?handler=ThirdPartyServiceExpenseDelete`.proto().parseURL(),
                                type: "POST",
                                data: {
                                    id: id
                                },
                                beforeSend: function (xhr) {
                                    xhr.setRequestHeader("XSRF-TOKEN",
                                        $('#general-information-form input:hidden[name="__RequestVerificationToken"]').val());
                                },
                                success: function (result) {
                                    swal({
                                        type: "success",
                                        title: "Completado",
                                        text: "El Servicio de Terceros ha sido eliminado con exito",
                                        confirmButtonText: "Excelente"
                                    }).then(datatable.thirdPartyServiceExpense.reload());
                                },
                                error: function (errormessage) {
                                    swal({
                                        type: "error",
                                        title: "Error",
                                        confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                        confirmButtonText: "Entendido",
                                        text: "El Servicio de Terceros tiene información relacionada"
                                    });
                                }
                            });
                        });
                    },
                    allowOutsideClick: () => !swal.isLoading()
                });
            }
        },
        otherExpense: {
            init: function () {
                this.create.events();
            },
            create: {
                object: $("#other-expense-add-form").validate({
                    submitHandler: function (formElement, e) {
                        $("#btnOtherExpenseAdd").addLoader();
                        e.preventDefault();
                        $.ajax({
                            url: $(formElement).attr("action"),
                            type: "POST",
                            data: $(formElement).serialize()
                        }).done(function () {
                            $(".modal").modal("hide");
                            $(".m-alert").addClass("m--hide");
                            toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                            datatable.otherExpense.reload();
                            modal.otherExpense.create.clear();
                            $("#btnOtherExpenseAdd").removeLoader();
                        }).fail(function (error) {
                            toastr.error(error.responseText, _app.constants.toastr.title.error);
                            $("#btnOtherExpenseAdd").removeLoader();
                        }).always(function () {

                        });
                    }
                }),
                show: function () {
                    $("#btnOtherExpenseAdd").removeLoader();
                    $("#otherExpenseAddModal").modal("toggle");
                },
                clear: function () {
                    modal.otherExpense.create.object.resetForm();
                },
                events: function () {
                    $("#otherExpenseAddModal").on("hidden.bs.modal", function () {
                        modal.otherExpense.create.clear();
                    });
                }
            },
            edit: {
                object: $("#other-expense-edit-form").validate({
                    submitHandler: function (formElement, e) {
                        $("#btnOtherExpenseEdit").addLoader();
                        e.preventDefault();
                        $.ajax({
                            url: $(formElement).attr("action"),
                            type: "POST",
                            data: $(formElement).serialize()
                        }).done(function () {
                            $(".modal").modal("hide");
                            $(".m-alert").addClass("m--hide");
                            toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                            datatable.otherExpense.reload();
                            modal.otherExpense.edit.clear();
                            $("#btnOtherExpenseEdit").removeLoader();
                        }).fail(function (error) {
                            toastr.error(error.responseText, _app.constants.toastr.title.error);
                            $("#btnOtherExpenseEdit").removeLoader();
                        }).always(function () {

                        });
                    }
                }),
                load: function (id) {
                    $.ajax({
                        url: `/alumno/postulacion-convocatoria-emprendimiento/${incubatorPostulationId}/inscripcion`.proto().parseURL(),
                        type: "GET",
                        data: {
                            handler: "OtherExpenseLoad",
                            id: id
                        },
                    }).done(function (result) {
                        $("#other-expense-edit-form input[name=Id]").val(result.id);
                        $("#other-expense-edit-form input[name=ExpenseCode]").val(result.expenseCode);
                        $("#other-expense-edit-form input[name=Description]").val(result.description);
                        $("#other-expense-edit-form input[name=MeasureUnit]").val(result.measureUnit);
                        $("#other-expense-edit-form input[name=Quantity]").val(result.quantity);
                        $("#other-expense-edit-form input[name=UnitPrice]").val(result.unitPrice);
                        $("#other-expense-edit-form input[name=ActivityJustification]").val(result.activityJustification);
                    }).fail(function (error) {
                        toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                    }).always(function () {

                    });
                },
                show: function (id) {
                    $("#btnOtherExpenseEdit").removeLoader();
                    modal.otherExpense.edit.load(id);
                    $("#otherExpenseEditModal").modal("toggle");
                },
                clear: function () {
                    modal.otherExpense.edit.object.resetForm();
                },
                events: function () {
                    $("#otherExpenseEditModal").on("hidden.bs.modal", function () {
                        modal.otherExpense.edit.clear();
                    });
                }
            },
            delete: function (id) {
                swal({
                    title: "¿Está seguro?",
                    text: "El registro de Gastos Varios será eliminado.",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonText: "Sí, eliminarlo",
                    confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                    cancelButtonText: "Cancelar",
                    showLoaderOnConfirm: true,
                    preConfirm: () => {
                        return new Promise((resolve) => {
                            $.ajax({
                                url: `/alumno/postulacion-convocatoria-emprendimiento/${incubatorPostulationId}/inscripcion?handler=OtherExpenseDelete`.proto().parseURL(),
                                type: "POST",
                                data: {
                                    id: id
                                },
                                beforeSend: function (xhr) {
                                    xhr.setRequestHeader("XSRF-TOKEN",
                                        $('#general-information-form input:hidden[name="__RequestVerificationToken"]').val());
                                },
                                success: function (result) {
                                    swal({
                                        type: "success",
                                        title: "Completado",
                                        text: "El registro de Gastos Varios ha sido eliminado con exito",
                                        confirmButtonText: "Excelente"
                                    }).then(datatable.otherExpense.reload());
                                },
                                error: function (errormessage) {
                                    swal({
                                        type: "error",
                                        title: "Error",
                                        confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                        confirmButtonText: "Entendido",
                                        text: "El registro de Gastos Varios tiene información relacionada"
                                    });
                                }
                            });
                        });
                    },
                    allowOutsideClick: () => !swal.isLoading()
                });
            }
        },
        teamMember: {
            init: function () {
                this.create.events();
            },
            create: {
                object: $("#teamMember-add-form").validate({
                    submitHandler: function (formElement, e) {
                        $("#btnTeamMemberAdd").addLoader();
                        e.preventDefault();
                        let formData = new FormData(formElement);
                        formData.append("UserName", $("#UserName").val());
                        formData.append("PaternalSurname", $("#PaternalSurname").val());
                        formData.append("MaternalSurname", $("#MaternalSurname").val());
                        formData.append("Name", $("#Name").val());
                        formData.append("UserName", $("#UserName").val());
                        formData.append("CareerText", $("#CareerText").val());
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
                            datatable.teamMember.reload();
                            modal.teamMember.create.clear();
                            $("#btnTeamMemberAdd").removeLoader();
                        }).fail(function (error) {
                            toastr.error(error.responseText, _app.constants.toastr.title.error);
                            $("#btnTeamMemberAdd").removeLoader();
                        }).always(function () {

                        });
                    }
                }),
                show: function () {
                    $("#btnTeamMemberAdd").removeLoader();
                    $("#teamMemberAddModal").modal("toggle");
                },
                clear: function () {
                    modal.teamMember.create.object.resetForm();
                },
                events: function () {
                    $("#teamMemberAddModal").on("hidden.bs.modal", function () {
                        modal.teamMember.clear();
                    });
                },
            },
            delete: function (id) {
                swal({
                    title: "¿Está seguro?",
                    text: "El equipo de trabajo será eliminado.",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonText: "Sí, eliminarlo",
                    confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                    cancelButtonText: "Cancelar",
                    showLoaderOnConfirm: true,
                    preConfirm: () => {
                        return new Promise((resolve) => {
                            $.ajax({
                                url: `/alumno/postulacion-convocatoria-emprendimiento/${incubatorPostulationId}/inscripcion?handler=TeamMemberDelete`.proto().parseURL(),
                                type: "POST",
                                data: {
                                    id: id
                                },
                                beforeSend: function (xhr) {
                                    xhr.setRequestHeader("XSRF-TOKEN",
                                        $('#general-information-form input:hidden[name="__RequestVerificationToken"]').val());
                                },
                                success: function (result) {
                                    swal({
                                        type: "success",
                                        title: "Completado",
                                        text: "El equipo de trabajo ha sido eliminado con exito",
                                        confirmButtonText: "Excelente"
                                    }).then(datatable.teamMember.reload());
                                },
                                error: function (errormessage) {
                                    swal({
                                        type: "error",
                                        title: "Error",
                                        confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                        confirmButtonText: "Entendido",
                                        text: "El equipo de trabajo tiene información relacionada"
                                    });
                                }
                            });
                        });
                    },
                    allowOutsideClick: () => !swal.isLoading()
                });
            }
        },
        activity: {
            init: function () {
                this.create.events();
            },
            create: {
                object: $("#activity-add-form").validate({
                    submitHandler: function (formElement, e) {
                        $("#btnActivityAdd").addLoader();
                        e.preventDefault();
                        $.ajax({
                            url: $(formElement).attr("action"),
                            type: "POST",
                            data: $(formElement).serialize(),
                        }).done(function () {
                            $(".modal").modal("hide");
                            $(".m-alert").addClass("m--hide");
                            toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                            sections.schedule.load();
                            $("#btnActivityAdd").removeLoader();
                        }).fail(function (error) {
                            toastr.error(error.responseText, _app.constants.toastr.title.error);
                            $("#btnActivityAdd").removeLoader();
                        }).always(function () {

                        });
                    }
                }),
                show: function () {
                    $("#btnActivityAdd").removeLoader();
                    $("#activityAddModal").modal("toggle");
                },
                clear: function () {
                    modal.activity.create.object.resetForm();
                },
                events: function () {
                    $("#activityAddModal").on("hidden.bs.modal", function () {
                        modal.activity.create.clear();
                        console.log('eee');
                    });
                },
            },
            edit: {
                object: $("#activity-edit-form").validate({
                    submitHandler: function (formElement, e) {
                        $("#btnActivityEdit").addLoader();
                        e.preventDefault();
                        $.ajax({
                            url: $(formElement).attr("action"),
                            type: "POST",
                            data: $(formElement).serialize()
                        }).done(function () {
                            $(".modal").modal("hide");
                            $(".m-alert").addClass("m--hide");
                            toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                            sections.schedule.load();
                            $("#btnActivityEdit").removeLoader();
                        }).fail(function (error) {
                            toastr.error(error.responseText, _app.constants.toastr.title.error);
                            $("#btnActivityEdit").removeLoader();
                        }).always(function () {

                        });
                    }
                }),
                load: function (id) {
                    $.ajax({
                        url: `/alumno/postulacion-convocatoria-emprendimiento/${incubatorPostulationId}/inscripcion`.proto().parseURL(),
                        type: "GET",
                        data: {
                            handler: "ActivityLoad",
                            id: id
                        },
                    }).done(function (result) {
                        $("#activity-edit-form input[name=Id]").val(result.id);
                        $("#activity-edit-form input[name=SpecificGoalDescription]").val(result.specificGoalDescription);
                        $("#activity-edit-form input[name=IncubatorPostulationSpecificGoalId]").val(result.incubatorPostulationSpecificGoalId);
                        $("#activity-edit-form input[name=Description]").val(result.description);
                        $("#activity-edit-form input[name=OrderNumber]").val(result.orderNumber);

                    }).fail(function (error) {
                        toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                    }).always(function () {

                    });
                },
                show: function (id) {
                    $("#btnActivityEdit").removeLoader();
                    modal.activity.edit.load(id);
                    $("#activityEditModal").modal("toggle");
                },
                clear: function () {
                    modal.activity.edit.object.resetForm();
                },
                events: function () {
                    $("#activityEditModal").on("hidden.bs.modal", function () {
                        modal.activity.edit.clear();
                    });
                }
            },
            delete: function (id) {
                swal({
                    title: "¿Está seguro?",
                    text: "La actividad será eliminada.",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonText: "Sí, eliminarla",
                    confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                    cancelButtonText: "Cancelar",
                    showLoaderOnConfirm: true,
                    preConfirm: () => {
                        return new Promise((resolve) => {
                            $.ajax({
                                url: `/alumno/postulacion-convocatoria-emprendimiento/${incubatorPostulationId}/inscripcion?handler=ActivityDelete`.proto().parseURL(),
                                type: "POST",
                                data: {
                                    id: id
                                },
                                beforeSend: function (xhr) {
                                    xhr.setRequestHeader("XSRF-TOKEN",
                                        $('#general-information-form input:hidden[name="__RequestVerificationToken"]').val());
                                },
                                success: function (result) {
                                    swal({
                                        type: "success",
                                        title: "Completado",
                                        text: "La actividad ha sido eliminado con exito",
                                        confirmButtonText: "Excelente"
                                    }).then(sections.schedule.load());
                                },
                                error: function (errormessage) {
                                    swal({
                                        type: "error",
                                        title: "Error",
                                        confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                        confirmButtonText: "Entendido",
                                        text: "La actividad tiene información relacionada"
                                    });
                                }
                            });
                        });
                    },
                    allowOutsideClick: () => !swal.isLoading()
                });
            }
        },
    };

    var datatable = {
        init: function () {
            this.specificGoal.init();
            this.equipmentExpense.init();
            this.suppliesExpense.init();
            this.thirdPartyServiceExpense.init();
            this.otherExpense.init();
            this.teamMember.init();
        },
        specificGoal: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: `/alumno/postulacion-convocatoria-emprendimiento/${incubatorPostulationId}/inscripcion`.proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        delete data.columns;
                        data.handler = "SpecificGoalDatatable";
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
                        title: "N° Orden",
                        data: "orderNumber"
                    },
                    {
                        title: "Opciones",
                        data: null,
                        orderable: false,
                        render: function (data) {
                            var template = "";
                            //Edit
                            template += '<button type="button" ';
                            template += "class='btn btn-info ";
                            template += "m-btn btn-sm m-btn--icon btn-edit' ";
                            template += " data-id='" + data.id + "'>";
                            template += "<span><i class='la la-edit'></i><span>Editar</span></span></button> ";
                            //Delete
                            template += '<button type="button" ';
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
                this.object = $("#specificGoals-datatable").DataTable(this.options);
                this.events();
            },
            events: function () {
                $("#specificGoals-datatable").on('click', '.btn-edit', function () {
                    var id = $(this).data("id");
                    modal.specificGoal.edit.show(id);
                });

                $("#specificGoals-datatable").on('click', '.btn-delete', function () {
                    var id = $(this).data("id");
                    modal.specificGoal.delete(id);
                });
            }
        },
        equipmentExpense: {
            object: null,
            options: {
                serverSide: false,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: `/alumno/postulacion-convocatoria-emprendimiento/${incubatorPostulationId}/inscripcion`.proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        delete data.columns;
                        data.handler = "EquipmentExpenseDatatable";
                    },
                    complete: function (e) {
                        var totalSum = 0;
                        if (e.responseText != undefined) {
                            var result = JSON.parse(e.responseText);

                            $.each(result.data, function (i, v) {
                                totalSum = totalSum + v.total;
                            });
                        }
                        $("#equipmentExpense-totalAmount").val(`S/. ${totalSum.toFixed(2)}`);

                    }
                },
                pageLength: 5,
                orderable: [],
                columns: [
                    {
                        title: "Especifica de gasto",
                        data: "expenseCode"
                    },
                    {
                        title: "Descripción",
                        data: "description"
                    },
                    {
                        title: "Unidad de Medida",
                        data: "measureUnit"
                    },
                    {
                        title: "Cantidad",
                        data: "quantity"
                    },
                    {
                        title: "Costo Unitario",
                        data: "unitPrice",
                        render: function (data) {
                            return "S/. " + data.toFixed(2);
                        }
                    },
                    {
                        title: "Total",
                        data: "total",
                        render: function (data) {
                            return "S/. " + data.toFixed(2);
                        }
                    },
                    {
                        title: "Actividades en la que se emplea",
                        data: "activityJustification"
                    },
                    {
                        title: "Opciones",
                        data: null,
                        orderable: false,
                        render: function (data) {
                            var template = "";
                            //Edit
                            template += '<button type="button" ';
                            template += "class='btn btn-info ";
                            template += "m-btn btn-sm m-btn--icon btn-edit' ";
                            template += " data-id='" + data.id + "'>";
                            template += "<span><i class='la la-edit'></i><span>Editar</span></span></button> ";
                            //Delete
                            template += '<button type="button" ';
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
                this.object = $("#equipmentExpense-datatable").DataTable(this.options);
                this.events();
            },
            events: function () {
                $("#equipmentExpense-datatable").on('click', '.btn-edit', function () {
                    var id = $(this).data("id");
                    modal.equipmentExpense.edit.show(id);
                });

                $("#equipmentExpense-datatable").on('click', '.btn-delete', function () {
                    var id = $(this).data("id");
                    modal.equipmentExpense.delete(id);
                });
            }
        },
        suppliesExpense: {
            object: null,
            options: {
                serverSide: false,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: `/alumno/postulacion-convocatoria-emprendimiento/${incubatorPostulationId}/inscripcion`.proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        delete data.columns;
                        data.handler = "SuppliesExpenseDatatable";
                    },
                    complete: function (e) {
                        var totalSum = 0;
                        if (e.responseText != undefined) {
                            var result = JSON.parse(e.responseText);

                            $.each(result.data, function (i, v) {
                                totalSum = totalSum + v.total;
                            });
                        }
                        $("#suppliesExpense-totalAmount").val(`S/. ${totalSum.toFixed(2) }`);
                    }
                },
                pageLength: 5,
                orderable: [],
                columns: [
                    {
                        title: "Especifica de gasto",
                        data: "expenseCode"
                    },
                    {
                        title: "Descripción",
                        data: "description"
                    },
                    {
                        title: "Unidad de Medida",
                        data: "measureUnit"
                    },
                    {
                        title: "Cantidad",
                        data: "quantity"
                    },
                    {
                        title: "Costo Unitario",
                        data: "unitPrice",
                        render: function (data) {
                            return "S/. " + data.toFixed(2);
                        }
                    },
                    {
                        title: "Total",
                        data: "total",
                        render: function (data) {
                            return "S/. " + data.toFixed(2);
                        }
                    },
                    {
                        title: "Actividades en la que se emplea",
                        data: "activityJustification"
                    },
                    {
                        title: "Opciones",
                        data: null,
                        orderable: false,
                        render: function (data) {
                            var template = "";
                            //Edit
                            template += '<button type="button" ';
                            template += "class='btn btn-info ";
                            template += "m-btn btn-sm m-btn--icon btn-edit' ";
                            template += " data-id='" + data.id + "'>";
                            template += "<span><i class='la la-edit'></i><span>Editar</span></span></button> ";
                            //Delete
                            template += '<button type="button" ';
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
                this.object = $("#suppliesExpense-datatable").DataTable(this.options);
                this.events();
            },
            events: function () {
                $("#suppliesExpense-datatable").on('click', '.btn-edit', function () {
                    var id = $(this).data("id");
                    modal.suppliesExpense.edit.show(id);
                });

                $("#suppliesExpense-datatable").on('click', '.btn-delete', function () {
                    var id = $(this).data("id");
                    modal.suppliesExpense.delete(id);
                });
            }
        },
        thirdPartyServiceExpense: {
            object: null,
            options: {
                serverSide: false,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: `/alumno/postulacion-convocatoria-emprendimiento/${incubatorPostulationId}/inscripcion`.proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        delete data.columns;
                        data.handler = "ThirdPartyServiceExpenseDatatable";
                    },
                    complete: function (e) {
                        var totalSum = 0;
                        if (e.responseText != undefined) {
                            var result = JSON.parse(e.responseText);

                            $.each(result.data, function (i, v) {
                                totalSum = totalSum + v.total;
                            });
                        }
                        $("#thirdPartyServiceExpense-totalAmount").val(`S/. ${totalSum.toFixed(2) }`);
                    }
                },
                pageLength: 5,
                orderable: [],
                columns: [
                    {
                        title: "Especifica de gasto",
                        data: "expenseCode"
                    },
                    {
                        title: "Descripción",
                        data: "description"
                    },
                    {
                        title: "Unidad de Medida",
                        data: "measureUnit"
                    },
                    {
                        title: "Cantidad",
                        data: "quantity"
                    },
                    {
                        title: "Costo Unitario",
                        data: "unitPrice",
                        render: function (data) {
                            return "S/. " + data.toFixed(2);
                        }
                    },
                    {
                        title: "Total",
                        data: "total",
                        render: function (data) {
                            return "S/. " + data.toFixed(2);
                        }
                    },
                    {
                        title: "Actividades en la que se emplea",
                        data: "activityJustification"
                    },
                    {
                        title: "Opciones",
                        data: null,
                        orderable: false,
                        render: function (data) {
                            var template = "";
                            //Edit
                            template += '<button type="button" ';
                            template += "class='btn btn-info ";
                            template += "m-btn btn-sm m-btn--icon btn-edit' ";
                            template += " data-id='" + data.id + "'>";
                            template += "<span><i class='la la-edit'></i><span>Editar</span></span></button> ";
                            //Delete
                            template += '<button type="button" ';
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
                this.object = $("#thirdPartyServiceExpense-datatable").DataTable(this.options);
                this.events();
            },
            events: function () {
                $("#thirdPartyServiceExpense-datatable").on('click', '.btn-edit', function () {
                    var id = $(this).data("id");
                    modal.thirdPartyServiceExpense.edit.show(id);
                });

                $("#thirdPartyServiceExpense-datatable").on('click', '.btn-delete', function () {
                    var id = $(this).data("id");
                    modal.thirdPartyServiceExpense.delete(id);
                });
            }
        },
        otherExpense: {
            object: null,
            options: {
                serverSide: false,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: `/alumno/postulacion-convocatoria-emprendimiento/${incubatorPostulationId}/inscripcion`.proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        delete data.columns;
                        data.handler = "OtherExpenseDatatable";
                    },
                    complete: function (e) {
                        var totalSum = 0;
                        if (e.responseText != undefined) {
                            var result = JSON.parse(e.responseText);

                            $.each(result.data, function (i, v) {
                                totalSum = totalSum + v.total;
                            });
                        }
                        $("#otherExpense-totalAmount").val(`S/. ${totalSum.toFixed(2)}`);
                    }
                },
                pageLength: 5,
                orderable: [],
                columns: [
                    {
                        title: "Especifica de gasto",
                        data: "expenseCode"
                    },
                    {
                        title: "Descripción",
                        data: "description"
                    },
                    {
                        title: "Unidad de Medida",
                        data: "measureUnit"
                    },
                    {
                        title: "Cantidad",
                        data: "quantity"
                    },
                    {
                        title: "Costo Unitario",
                        data: "unitPrice",
                        render: function (data) {
                            return "S/. " + data.toFixed(2);
                        }
                    },
                    {
                        title: "Total",
                        data: "total",
                        render: function (data) {
                            return "S/. " + data.toFixed(2);
                        }
                    },
                    {
                        title: "Actividades en la que se emplea",
                        data: "activityJustification"
                    },
                    {
                        title: "Opciones",
                        data: null,
                        orderable: false,
                        render: function (data) {
                            var template = "";
                            //Edit
                            template += '<button type="button" ';
                            template += "class='btn btn-info ";
                            template += "m-btn btn-sm m-btn--icon btn-edit' ";
                            template += " data-id='" + data.id + "'>";
                            template += "<span><i class='la la-edit'></i><span>Editar</span></span></button> ";
                            //Delete
                            template += '<button type="button" ';
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
                this.object = $("#otherExpense-datatable").DataTable(this.options);
                this.events();
            },
            events: function () {
                $("#otherExpense-datatable").on('click', '.btn-edit', function () {
                    var id = $(this).data("id");
                    modal.otherExpense.edit.show(id);
                });

                $("#otherExpense-datatable").on('click', '.btn-delete', function () {
                    var id = $(this).data("id");
                    modal.otherExpense.delete(id);
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
                    url: `/alumno/postulacion-convocatoria-emprendimiento/${incubatorPostulationId}/inscripcion`.proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        delete data.columns;
                        data.handler = "TeamMemberDatatable";
                    }
                },
                pageLength: 50,
                orderable: [],
                columns: [
                    {
                        title: "Cod Alumno",
                        data: "userName"
                    },
                    {
                        title: "Apellido Paterno",
                        data: "paternalSurname"
                    },
                    {
                        title: "Apellido Materno",
                        data: "maternalSurname"
                    },
                    {
                        title: "Nombres",
                        data: "name"
                    },
                    {
                        title: "Escuela Profesional",
                        data: "careerText"
                    },
                    {
                        title: "Opciones",
                        data: null,
                        orderable: false,
                        render: function (data) {
                            var template = "";
                            //Delete
                            template += '<button type="button" ';
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
                $("#teamMember-datatable").on('click', '.btn-delete', function () {
                    var id = $(this).data("id");
                    modal.teamMember.delete(id);
                });
            }
        },
    };

    return {
        init: function () {
            sections.init();
            datatable.init();
            select.init();
        },
        reloadScheduleSection: function () {
            sections.schedule.load();
        },
        activityShow: function (id) {
            modal.activity.edit.show(id);
        }

    }
}();

$(function () {
    InitApp.init();
})