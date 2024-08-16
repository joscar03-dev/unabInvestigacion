var add = function () {

    var UserId = $("#ExternalUser_Id").val();
    var BaseRoute = `/admin/usuarios-externos/editar/${UserId}`;

    var form = {
        object: $("#main_form").validate({
            submitHandler: function (formElement, e) {
                e.preventDefault();

                var formData = new FormData(formElement);
                $("#main_form").find(":input").attr("disabled", true);

                mApp.block("#main_form", {
                    message: "Actualizando usuario..."
                });

                $.ajax({
                    url: `${BaseRoute}?handler=Edit`,
                    method: "post",
                    data: formData,
                    contentType: false,
                    processData: false
                })
                    .done(function (e) {
                        swal({
                            type: "success",
                            title: "Completado",
                            text: "Usuario editado satisfactoriamente.",
                            confirmButtonText: "Aceptar",
                            allowOutsideClick: false
                        }).then(function (isConfirm) {
                            if (isConfirm) {
                                window.location.reload();
                            }
                        });
                    })
                    .fail(function (e) {
                        swal({
                            type: "error",
                            title: "Error",
                            confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                            confirmButtonText: "Aceptar",
                            text: e.status === 502 ? "No hay respuesta del servidor" : e.responseText
                        });
                    })
                    .always(function () {
                        $("#main_form").find(":input").attr("disabled", false);
                        mApp.unblock("#main_form");
                    });
            }
        })
    }

    return {
        init: function () {

        }
    }
}();

$(() => {
    add.init();
})