var index = function () {
    var datatable = {
        postulant: {
            object: null,
            options: {
                ajax: {
                    url: `/evaluador/postulantes-incubadora`,
                    data: function (data) {
                        delete data.columns;
                        data.handler = "PostulantsDatatable";
                        data.search = $("#search").val();
                    }
                },
                columns: [
                    {
                        data: "convocation",
                        title: "Convocatoria"
                    },
                    {
                        data: "convocationCode",
                        title: "Cod."
                    },
                    {
                        data: "userName",
                        title: "Postulante"
                    },
                    {
                        data: "fullName",
                        title: "Nombre Postulante"
                    },
                    {
                        data: "createdAt",
                        title: "Fec. Postulación"
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
                        data: null,
                        title: "Opciones",
                        render: function (row) {
                            var tpm = "";
                            tpm += `<a href="/evaluador/postulantes-incubadora/${row.id}/detalles" class="btn btn-primary m-btn btn-sm m-btn m-btn--icon"><span><i class="la la-eye"></i><span>Detalles</span></span></a> `;

                            if (row.reviewState == 0) {
                                tpm += `<a href="/evaluador/postulantes-incubadora/${row.id}/calificar" class="btn btn-primary m-btn btn-sm m-btn m-btn--icon"><span><i class="la la-edit"></i><span>Calificar</span></span></a>`;
                            }

                            return tpm;
                        }
                    }
                ]
            },
            reload: function () {
                datatable.postulant.object.ajax.reload();
            },
            init: function () {
                datatable.postulant.object = $("#data-table").DataTable(datatable.postulant.options);
            }
        },
        init: function () {
            datatable.postulant.init();
        }
    }

    var events = {
        onSearch: function () {
            $("#search").doneTyping(function () {
                datatable.postulant.reload();
            })
        },
        init: function () {
            this.onSearch();
        }
    }

    return {
        init: function () {
            datatable.init();
            events.init();
        }
    }
}();

$(() => {
    index.init();
});
