var InitApp = function () {

    var incubatorPostulationId = $("#Id").val();

    var datatable = {
        teamMember: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: `/evaluador/postulantes-incubadora/${incubatorPostulationId}/detalles`.proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        data.handler = "TeamMembersDatatable";
                        data.searchValue = $("#search").val();
                    }
                },
                pageLength: 50,
                orderable: [],
                columns: [
                    {
                        title: "Código",
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
                this.object = $("#team-table").DataTable(this.options);
                this.events();
            },
            events: function () {

            }
        },
        annex: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: `/evaluador/postulantes-incubadora/${incubatorPostulationId}/detalles`.proto().parseURL(),
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
            this.teamMember.init();
            this.annex.init();
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