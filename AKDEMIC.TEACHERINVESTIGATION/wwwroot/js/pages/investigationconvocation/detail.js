var InitApp = function () {
    var investigationConvocationId = $("#InvestigationConvocationId").val();
    var currentPage = window.location.href;
    var form = {
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
                            window.location.href = `/investigador/convocatoria/${result}/inscripcion`;
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
        }
    };
    var modal = {
        inscription: {
            show: function () {
                $("#inscription_modal").modal("toggle");
            },
            clear: function () {
                if ($("#user_inscription").length) {
                    $("#user_inscription").validate().resetForm();
                }
                $("#btnRegisterToEvent").removeLoader();
            },
            events: function () {
                $("#btnInscription").on('click', function () {
                    modal.inscription.show();
                });

                $("#inscription_modal").on("hidden.bs.modal", function () {
                    modal.inscription.clear();
                });

                $("#btnLogin").on("click", function () {
                    window.location.href = `/login?ReturnUrl=/convocatorias-de-investigacion/${investigationConvocationId}/detalle`;
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

    return {
        init: function () {
            form.init();
            modal.init();
        }
    }
}();

$(function () {
    InitApp.init();
});