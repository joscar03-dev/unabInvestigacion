var InitApp = function () {
    var datatable = {
        unit: {
            object: null,
            options: {
                serverSide: true,
                filter: false,
                lengthChange: false,
                ajax: {
                    url: "/admin/unidades".proto().parseURL(),
                    type: "GET",
                    dataType: "JSON",
                    data: function (data) {
                        data.handler = "Datatable";
                        data.searchValue = $("#search").val();
                    }
                },
                pageLength: 50,
                orderable: [],
                columns: [
                    {
                        title: "Nombre",
                        data: "name"
                    },
                    {
                        title: "Usuario",
                        data: "fullName"
                    },
                    {
                        title: "Opciones",
                        data: null,
                        orderable: false,
                        render: function (data) {
                            var template = "";
                            //Edit
                            template += "<button ";
                            template += "class='btn btn-info ";
                            template += "m-btn btn-sm m-btn--icon btn-edit' ";
                            template += " data-id='" + data.id + "'>";
                            template += "<span><i class='la la-edit'></i><span>Editar</span></span></button> ";
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
                    modal.unit.edit.show(id);
                });
                
            }
        },
        init: function () {
            this.unit.init();
        }
    };
    var search = {
        init: function () {
            $("#search").doneTyping(function () {
                datatable.unit.reload();
            });
        }
    };

    var modal = {
        unit: {
            edit: {
                object: $("#edit-form").validate({
                    submitHandler: function (formElement, e) {
                        $("#btnEdit").addLoader();
                        e.preventDefault();
                        let formData = new FormData(formElement);
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

                            datatable.unit.reload();
                            modal.unit.edit.clear();
                            $("#btnEdit").removeLoader();
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
                        url: `/admin/unidades`.proto().parseURL(),
                        type: "GET",
                        data: {
                            handler: "DetailUnit",
                            Id: id
                        },
                    }).done(function (result) {
                        $("#edit-form input[name=UserId]").val(result.userId);
                        $("#UserIdHidden").val(result.userId);
                        $("#edit-form input[name=Id]").val(result.id);
                        $("#btnEdit").removeLoader();
                    }).fail(function (error) {
                        toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                        $("#btnEdit").removeLoader();
                    }).always(function () {

                    });
                },
                show: function (id) {
                    $("#btnEdit").removeLoader();
                    modal.unit.edit.load(id);
                    $("#editModal").modal("toggle");
                },
                clear: function () {
                    modal.unit.edit.object.resetForm();
                },
                events: function () {
                    $("#editModal").on("hidden.bs.modal", function () {
                        modal.unit.edit.clear();
                    });
                }
            },
            init: function () {
                this.edit.events();
            }
        },
        init: function () {
            this.unit.init();

        }
    };

    var select = {
        init: function () {
            this.user.init();
        },
      
        user: {
            init: function () {
                $("#edit-form select[name=UserId]").select2({
                    width: "100%",
                    dropdownParent: $('#editModal'),
                    allowClear: true,
                    placeholder: "Buscar...",
                    ajax: {
                        url: "/api/usuarios/select-search".proto().parseURL(),
                        dataType: "json",
                        data: function (params) {
                            return {
                                userType: 0,
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
            }
        }


    };

    return {
        init: function () {
            datatable.init();
            search.init();
            select.init();
            modal.init();
        }
    }
}();

$(function () {
    InitApp.init();
})