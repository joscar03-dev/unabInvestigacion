var InitApp = function () {
    var investigationProjectId = $("#Input_InvestigationProjectId").val();
    var datatable = {
        investigationProjectExpense: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: `/admin/proyectos/${investigationProjectId}/detalle`.proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        data.handler = "ExpenseDatatable";
                        data.searchValue = $("#search").val();
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
                        title: "Gasto",
                        data: "amount"
                    },
                    {
                        title: "Centro de Financiamiento",
                        data: "financingInvestigation"
                    },
                ],
            },
            reload: function () {
                this.object.ajax.reload();
            },
            init: function () {
                this.object = $("#expense-datatable").DataTable(this.options);
            },
        },
        investigationProjectTeamMembers: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: `/reporte/proyecto-investigacion/${investigationProjectId}/detalle`.proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        data.handler = "ProjectTeamMemberDatatable";
                        data.searchValue = $("#search").val();
                    }
                },
                pageLength: 50,
                orderable: [],
                columns: [
                    {
                        title: "Miembro",
                        data: "fullName"
                    },
                    {
                        title: "Objetivos",
                        data: "objectives"
                    },
                    {
                        title: "Concytec",
                        data: null,
                        orderable: false,
                        render: function (data) {
                            var template = "";
                            var fileUrl = `/documentos/${data.cteVitaeConcytecUrl}`.proto().parseURL();
                            //FileURL

                            if (data.cteVitaeConcytecUrl != null) {
                                template += `<a href="${fileUrl}" target="_blank" `;
                                template += "class='btn btn-success ";
                                template += "m-btn btn-sm m-btn--icon'>";
                                template += "<span><i class='flaticon-file'></i></a> ";
                            } else
                            {
                                template += "--";
                            }


                            return template;
                        }
                    },
                ],
            },
            reload: function () {
                this.object.ajax.reload();
            },
            init: function () {
                this.object = $("#teamMember-datatable").DataTable(this.options);
            },
        },
        init: function () {
            this.investigationProjectExpense.init();
            this.investigationProjectTeamMembers.init();
        }
    };
    return {
        init: function () {
            datatable.init();
        }
    }
}();

$(function () {
    InitApp.init();
})