var InitApp = function () {

    var datatable = {
        postulations: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: "/comite/postulaciones".proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        data.handler = "PostulationsDatatable";
                        data.investigationConvocationId = $("#Convocations").val();
                        data.searchStartDate = $("#StartDate").val();
                        data.searchEndDate = $("#EndDate").val();
                        data.facultyId = $("#Faculties").val();
                        data.searchReviewState = $("#Reviews").val();
                        data.searchProgressState = $("#Progress").val();
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
                        title: "Convocatoria",
                        data: "name"
                    },
                    {
                        title: "Nombre del Proyecto",
                        data: "projectTitle"
                    },
                    {
                        title: "Nombre de Investigador",
                        data: "fullName"
                    },
                    {
                        title: "Facultad",
                        data: "faculty"
                    },
                    {
                        title: "Fecha de Registro Postulación",
                        data: "createdAt"
                    },
                    {
                        title: "%Avance",
                        data: "percentA"
                    },
                    {
                        title: "Estado de Avance",
                        data: null,
                        render: function (data) {
                            switch (data.progressState) {
                                case 0:
                                    return `<span class="m--font-info">${data.progressStateText}</span>`;
                                case 1:
                                    return `<span class="m--font-success">${data.progressStateText}</span>`;
                                default:
                                    return `<span class="m--font-primary">${data.progressStateText}</span>`;
                            }
                        }
                    },
                    {
                        title: "Estado de Revisión",
                        data: null,
                        render: function (data) {
                            switch (data.reviewState) {
                                case 0:
                                    return `<span class="m--font-warning">${data.reviewStateText}</span>`;
                                case 1:
                                    return `<span class="m--font-info">${data.reviewStateText}</span>`;
                                case 2:
                                    return `<span class="m--font-danger">${data.reviewStateText}</span>`;
                                case 3:
                                    return `<span class="m--font-success">${data.reviewStateText}</span>`;
                                default:
                                    return `<span class="m--font-primary">${data.reviewStateText}</span>`;
                            }
                        }
                    },

                    {
                        title: "Opciones",
                        data: null,
                        orderable: false,
                        render: function (data) {
                            var detailUrl = `/comite/postulaciones/detalle/${data.id}`;
                            var template = "";

                            //Edit                       
                            template += `<a class='btn btn-info m-btn btn-sm m-btn--icon-only' href='${detailUrl}' `;
                            template += `<span><i class="la la-edit"></i><span>Detalle</span></span></a> `;


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
            }
        },
        init: function () {
            this.postulations.init();
        }
    };
    var search = {
        init: function () {
            $("#search").doneTyping(function () {
                datatable.postulations.reload();
            });
        }
    };
    var datePickers = {
        init: function () {
            this.postulationsDate.init();
        },
        postulationsDate: {
            init: function () {
                $("#StartDate").datepicker({
                    clearBtn: true,
                    orientation: "bottom",
                    format: _app.constants.formats.datepicker
                })
                $("#EndDate").datepicker({
                    clearBtn: true,
                    orientation: "bottom",
                    format: _app.constants.formats.datepicker
                })
            }
        }
    };
    var select = {
        init: function () {
            this.convocations.init();
            this.faculty.init();
            this.progress.init();
            this.reviews.init();
        },
        convocations: {
            init: function () {
                this.load();
                this.events();
            },
            load: function () {
                $.ajax({
                    url: (`/api/convocatorias/select/get`).proto().parseURL(),
                    type: "GET"
                }).done(function (result) {
                    $("#Convocations").select2({
                        data: result,
                    });
                });

            },
            events: function () {
                $('#Convocations').on("change", function () {
                    let id = $(this).val();
                    select.convocations.load(id);
                    datatable.postulations.reload();
                });
                $('#StartDate').on("change", function () {
                    let id = $(this).val();
                    select.convocations.load(id);
                    datatable.postulations.reload();
                });
                $('#EndDate').on("change", function () {
                    let id = $(this).val();
                    select.convocations.load(id);
                    datatable.postulations.reload();
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
                    $("#Faculties").select2({
                        data: result,
                    });
                });

            },
            events: function () {
                $('#Faculties').on("change", function () {
                    datatable.postulations.reload();
                });

            }
        },
        progress: {
            init: function () {
                this.load();
                this.events();
            },
            load: function () {
                $.ajax({
                    url: (`/api/convocatorias/progress/get`).proto().parseURL(),
                    type: "GET"
                }).done(function (result) {
                    $("#Progress").select2({
                        data: result,
                    });
                });

            },
            events: function () {
                $('#Progress').on("change", function () {
                    datatable.postulations.reload();
                });

            }
        },
        reviews: {
            init: function () {
                this.load();
                this.events();
            },
            load: function () {
                $.ajax({
                    url: (`/api/convocatorias/reviews/get`).proto().parseURL(),
                    type: "GET"
                }).done(function (result) {
                    $("#Reviews").select2({
                        data: result,
                    });
                });

            },
            events: function () {
                $('#Reviews').on("change", function () {
                    datatable.postulations.reload();
                });

            }
        },
    };
    return {
        init: function () {
            select.init();
            datatable.init();
            search.init();
            datePickers.init();
        }
    }
}();

$(function () {
    InitApp.init();
})
