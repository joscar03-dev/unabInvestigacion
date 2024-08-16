var InitApp = function () {
    var eventId = $("#guest_inscription input[name='EventId']").val();
    var currentPage = window.location.href;
    var form = {
        guestInscription: {
            init: function () {
                $("#guest_inscription").validate({
                    submitHandler: function (formElement, e) {
                        $("#btnRegisterAsGuest").addLoader();
                        e.preventDefault();
                        $.ajax({
                            url: $(formElement).attr("action"),
                            type: "POST",
                            data: $(formElement).serialize()
                        }).done(function (result) {
                            $("#inscription_modal").modal("hide");
                            toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                            $("#btnRegisterAsGuest").removeLoader();
                        }).fail(function (error) {
                            toastr.error(error.responseText, _app.constants.toastr.title.error);
                            $("#btnRegisterAsGuest").removeLoader();
                        }).always(function () {
                            $("#btnRegisterAsGuest").removeLoader();
                        });
                    }
                });
            }
        },
        userInscription: {
            init: function () {
                $("#user_inscription").validate({
                    submitHandler: function (formElement, e) {
                        $("#btnRegisterAsUser").addLoader();
                        e.preventDefault();
                        $.ajax({
                            url: $(formElement).attr("action"),
                            type: "POST",
                            data: $(formElement).serialize()
                        }).done(function (result) {
                            $("#inscription_modal").modal("hide");
                            toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                            $("#btnRegisterAsUser").removeLoader();
                            window.location.href = currentPage;
                        }).fail(function (error) {
                            toastr.error(error.responseText, _app.constants.toastr.title.error);
                            $("#btnRegisterAsUser").removeLoader();
                        }).always(function () {
                            $("#btnRegisterAsUser").removeLoader();
                        });
                    }
                });
            }
        },
        init: function () {
            this.userInscription.init();
            this.guestInscription.init();
        }
    };
    var modal = {
        inscription: {
            clear: function () {
                if ($("#guest_inscription").length) {
                    $("#guest_inscription").validate().resetForm();
                }
                if ($("#user_inscription").length) {
                    $("#user_inscription").validate().resetForm();
                }
                $("#btnRegisterToEvent").removeLoader();
            },
            events: function () {
                $("#inscription_modal").on("hidden.bs.modal", function () {
                    modal.inscription.clear();
                });

                $("#guest_inscription input[name='Dni']").keypress(function (event) {
                    if (event.which === 46 || (event.which < 48 || event.which > 57)) {
                        event.preventDefault();
                    }
                }).on('paste', function (event) {
                    event.preventDefault();
                });

                $("#guest_inscription input[name='PhoneNumber']").keypress(function (event) {
                    if (event.which === 46 || (event.which < 48 || event.which > 57)) {
                        event.preventDefault();
                    }
                }).on('paste', function (event) {
                    event.preventDefault();
                });

                $("#btnLogin").on("click", function () {
                    window.location.href = `/login?ReturnUrl=/eventos/${eventId}/detalle`;
                });
            },
            init: function () {
                this.events();
            }
        },
        init: function () {
            this.inscription.init();
        }
    };

    var datePickers = {
        init: function () {
            this.birthDate.init();
        },
        birthDate: {
            init: function () {
                $("#guest_inscription input[name='BirthDate']").datepicker({
                    clearBtn: true,
                    orientation: "bottom",
                    format: _app.constants.formats.datepicker
                });
            }
        }
    };


    return {
        init: function () {
            form.init();
            modal.init();
            datePickers.init();
        }
    }
}();

$(function () {
    InitApp.init();
});