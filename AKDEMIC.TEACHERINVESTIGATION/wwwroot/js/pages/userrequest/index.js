var InitApp = function () {
    var form = {
        create: {
            init: function () {
                $("#create-form").validate({
                    submitHandler: function (formElement, e) {
                        $("#btnSave").addLoader();
                        e.preventDefault();
                        $.ajax({
                            url: $(formElement).attr("action"),
                            type: "POST",
                            data: $(formElement).serialize()
                        }).done(function () {
                            toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                            $("#btnSave").removeLoader();
                        }).fail(function (error) {
                            toastr.error(error.responseText, _app.constants.toastr.title.error);
                            $("#btnSave").removeLoader();
                        }).always(function () {

                        });
                    }
                });
            }
        },
        init: function () {
            this.create.init();
        }
    };
    var select = {
        init: function () {
            this.userrequest_type.init();
        },
        userrequest_type: {
            init: function () {
                $("#create-form select[name='Type']").select2();
            }
        }
    };
    var events = {
        init: function () {
            $("#Input_Dni").keypress(function (event) {
                if (event.which === 46 || (event.which < 48 || event.which > 57)) {
                    event.preventDefault();
                }
            }).on('paste', function (event) {
                event.preventDefault();
            });
        }
    }
    return {
        init: function () {
            form.init();
            events.init();
            select.init();
        }
    }
}();

$(function () {
    InitApp.init();
})