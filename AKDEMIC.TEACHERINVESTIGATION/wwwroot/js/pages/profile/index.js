var InitApp = function () {
    var form = {
        init: function () {
            this.updateForm.init();
            this.changePasswordForm.init();
        },
        updateForm: {
            init: function () {
                $("#update-form").validate({
                    submitHandler: function (formElement, e) {
                        $("#btnUpdate").addLoader();
                        e.preventDefault();
                        $.ajax({
                            url: $(formElement).attr("action"),
                            type: "POST",
                            data: $(formElement).serialize()
                        }).done(function () {
                            toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                            $("#btnUpdate").removeLoader();
                        }).fail(function (error) {
                            toastr.error(error.responseText, _app.constants.toastr.title.error);
                            $("#btnUpdate").removeLoader();
                        }).always(function () {

                        });
                    }
                });
            }
        },
        changePasswordForm: {
            init: function () {
                $("#change-password-form").validate({
                    rules: {
                        RepeatPassword: {
                            equalTo: "#NewPassword"
                        }
                    },
                    submitHandler: function (formElement, e) {
                        $("#btnChangePassword").addLoader();
                        e.preventDefault();
                        $.ajax({
                            url: $(formElement).attr("action"),
                            type: "POST",
                            data: $(formElement).serialize()
                        }).done(function () {
                            toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                            $("#btnChangePassword").removeLoader();
                        }).fail(function (error) {
                            toastr.error(error.responseText, _app.constants.toastr.title.error);
                            $("#btnChangePassword").removeLoader();
                        }).always(function () {

                        });
                    }
                });
            }
        }
    };

    return {
        init: function () {
            form.init();
        }
    }
}();

$(function () {
    InitApp.init();
})