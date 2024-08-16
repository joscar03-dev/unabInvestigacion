var InitApp = function () {

    var incubatorPostulantId = $("#IncubatorPostulantId").val();

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
            load: function () {
                $.ajax({
                    url: `/admin/postulantes-convocatorias-incubadoras/${incubatorPostulantId}/detalle`.proto().parseURL(),
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
                    $('#general-information-form input[Name=DepartmentText]').val(result.departmentText).trigger("change");
                    $('#general-information-form input[Name=ProvinceText]').val(result.provinceText).trigger("change");
                    $('#general-information-form input[Name=DistrictText]').val(result.districtText).trigger("change");
                });
            }
        },
        investigationTeam: {
            init: function () {
                this.load();
            },
            load: function () {

                $.ajax({
                    url: `/admin/postulantes-convocatorias-incubadoras/${incubatorPostulantId}/detalle`.proto().parseURL(),
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
            load: function () {
                $.ajax({
                    url: `/admin/postulantes-convocatorias-incubadoras/${incubatorPostulantId}/detalle`.proto().parseURL(),
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
            load: function () {
                $.ajax({
                    url: `/admin/postulantes-convocatorias-incubadoras/${incubatorPostulantId}/detalle`.proto().parseURL(),
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
            },
            load: function () {
                $.ajax({
                    url: `/admin/postulantes-convocatorias-incubadoras/${incubatorPostulantId}/detalle`.proto().parseURL(),
                    type: "GET",
                    data: {
                        handler: "SchedulePage",
                    },
                }).done(function (result) {
                    $("#scheduleContainer").html(result);
                });
            },
        }
    };

    var datatable = {
        specificGoal: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: `/admin/postulantes-convocatorias-incubadoras/${incubatorPostulantId}/detalle`.proto().parseURL(),
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
                    url: `/admin/postulantes-convocatorias-incubadoras/${incubatorPostulantId}/detalle`.proto().parseURL(),
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
                    url: `/admin/postulantes-convocatorias-incubadoras/${incubatorPostulantId}/detalle`.proto().parseURL(),
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
                        $("#suppliesExpense-totalAmount").val(`S/. ${totalSum.toFixed(2)}`);
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
                    url: `/admin/postulantes-convocatorias-incubadoras/${incubatorPostulantId}/detalle`.proto().parseURL(),
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
                        $("#thirdPartyServiceExpense-totalAmount").val(`S/. ${totalSum.toFixed(2)}`);
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
                    url: `/admin/postulantes-convocatorias-incubadoras/${incubatorPostulantId}/detalle`.proto().parseURL(),
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
                    url: `/admin/postulantes-convocatorias-incubadoras/${incubatorPostulantId}/detalle`.proto().parseURL(),
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
        annex: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: `/admin/postulantes-convocatorias-incubadoras/${incubatorPostulantId}/detalle`.proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        data.handler = "AnnexesDatatable";
                        data.searchValue = $("#search").val();
                    }
                },
                pageLength: 50,
                orderable: [],
                columns: [
                    {
                        title: "Código",
                        data: "code"
                    },
                    {
                        title: "Nombre",
                        data: "name"
                    },
                    {
                        title: "Archivo",
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
                ],
            },
            reload: function () {
                this.object.ajax.reload();
            },
            init: function () {
                this.object = $("#annex-table").DataTable(this.options);
                this.events();
            },
            events: function () {

            }
        },
        init: function () {
            this.specificGoal.init();
            this.equipmentExpense.init();
            this.suppliesExpense.init();
            this.thirdPartyServiceExpense.init();
            this.otherExpense.init();
            this.teamMember.init();
            this.annex.init();
        }
    };

    return {
        init: function () {
            datatable.init();
            sections.init();
        },
    }
}();

$(function () {
    InitApp.init();
})