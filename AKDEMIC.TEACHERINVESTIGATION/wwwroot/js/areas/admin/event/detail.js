var InitApp = function () {
    var eventId = $("#Input_Id").val();
    var datatable = {
        eventParticipant: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: `/admin/eventos/${eventId}/detalle`.proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        data.handler = "EventParticipantDatatable";
                        data.searchValue = $("#search").val();
                    }
                },
                pageLength: 50,
                orderable: [],
                columns: [
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
                        title: "Correo",
                        data: "email"
                    }

                ],
                dom: 'Bfrtip',
                buttons: [
                    'excel', 'pdf'
                ]
            },
            reload: function () {
                this.object.ajax.reload();
            },
            init: function () {
                this.object = $("#data-table").DataTable(this.options);
            },

        },
        init: function () {
            this.eventParticipant.init();
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