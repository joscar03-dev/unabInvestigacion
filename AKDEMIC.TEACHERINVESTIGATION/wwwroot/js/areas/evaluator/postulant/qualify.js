var InitApp = function () {

    var PostulantId = $("#Postulant_Id").val();
    var BaseRoute = `/evaluador/postulantes-investigacion/${PostulantId}/calificar`;

    var form = {
        qualify: {
            object: $("#main_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    var formData = new FormData(formElement);

                    $("#main_form").find(":input").attr("disabled", true);

                    mApp.block("#main_form", {
                        message : "Guardando datos..."
                    });

                    $.ajax({
                        url: `${BaseRoute}?handler=Qualify`,
                        type: "post",
                        data: formData,
                        contentType: false,
                        processData: false
                    })
                        .done(function (e) {
                            swal({
                                type: "success",
                                title: "Completado",
                                text: "Postulante calificado satisfactoriamente.",
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

                            $("#main_form").find(":input").attr("disabled", false);
                            mApp.unblock("#main_form");
                        })
                }
            })
        }
    }

    return {
        init: function () {

        }
    }
}();

$(() => {
    InitApp.init();
});