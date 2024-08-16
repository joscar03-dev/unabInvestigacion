var InitApp = function () {
    var form = {
        init: function () {
            this.configuration.init();
        },
        configuration: {
            init: function () {
                $("#configForm").validate({
                    submitHandler: function (form, e) {
                        let formData = new FormData(form);
                        swal({
                            title: "Confirmacíon de cambios",
                            text: "Se actualizarán las variables del sistema. Esto afectará de manera inmediata a las funcionalidades relacionadas.",
                            type: "warning",
                            showCancelButton: true,

                            confirmButtonText: "Confirmar",
                            confirmButtonClass: "btn btn-success m-btn m-btn--custom",
                            cancelButtonText: "Cancelar"
                        }).then(function (result) {
                            if (result.value) {
                                mApp.block("#configForm");
                                $.ajax({
                                    url: $(form).attr("action"),
                                    type: "POST",
                                    contentType: false,
                                    processData: false,
                                    data: formData
                                }).done(function () {
                                    //swal({
                                    //    "title": "",
                                    //    "text": "Se actualizo, las variables del sistema.",
                                    //    "type": "Sucess",
                                    //    "confirmButtonClass": "btn btn-primary m-btn m-btn--wide"
                                    //}).then((result) => {
                                    //    window.location.reload();
                                    //})
                                    window.location.reload();
                                    toastr.success(_app.constants.toastr.message.success.task, _app.constants.toastr.title.success);
                                }).fail(function (error) {
                                    toastr.error(_app.constants.toastr.message.error.task, _app.constants.toastr.title.error);
                                }).always(function () {
                                    mApp.unblock("#configForm");
                                });
                            }
                        });
                    }
                });
            }
        },
    };

    return {
        init: function () {
            form.init();
        }
    };
}();

$(function () {
    InitApp.init();
});