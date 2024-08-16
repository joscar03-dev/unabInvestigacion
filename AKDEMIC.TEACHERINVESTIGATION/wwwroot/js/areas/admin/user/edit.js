var InitApp = function () {
    var form = {
        init: function () {
            $("#edit-form").validate({
                submitHandler: function (form, e) {
                    $("#btnSave").addLoader();
                    e.preventDefault();
                    let formData = new FormData(form);
                    $.ajax({
                        url: $(form).attr("action"),
                        type: "POST",
                        data: formData,
                        contentType: false,
                        processData: false
                    }).done(function () {
                        toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                        location.href = `/admin/usuarios`.proto().parseURL();
                    }).fail(function (error) {
                        toastr.error(error.responseText, _app.constants.toastr.title.error);
                        $("#btnSave").removeLoader();
                    }).always(function () {

                    });
                }
            });
        }
    };

    var select = {
        init: function () {
            this.roles.init();
        },
        roles: {
            init: function () {
                this.load();
            },
            load: function () {
                $.ajax({
                    url: (`/api/roles/select/get`).proto().parseURL(),
                    type: "GET"
                }).done(function (result) {
                    $('#Input_RolesId').select2({
                        data: result,
                    });
                    select.roles.userRole();
                });
            },
            userRole: function () {
                var userId = $("#Input_UserId").val();
                mApp.block(".m-content", { type: "loader", message: "Cargando..." });
                $.ajax({
                    url: `/api/usuario-rol/${userId}/select/get`.proto().parseURL()
                }).done(function (data) {
                        data.forEach(function (item) {
                            var opt = new Option(item.text, item.id, true, true);
                            $("#Input_RolesId").append(opt).trigger("change");
                        });
                    }).always(function (result) {
                        mApp.unblock(".m-content");
                    });
            }
        },
    };
    var datePickers = {
        init: function () {
            $("#Input_BirthDate").datepicker({
                clearBtn: true,
                orientation: "bottom",
                format: _app.constants.formats.datepicker
            });
        }
    }


    return {
        init: function () {
            form.init();
            datePickers.init();
            select.init();
        }
    }
}();

$(function () {
    InitApp.init();
})