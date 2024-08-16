var InitApp = function () {
    var pageInfo = {
        incubatorConvocations: {
            load: function () {
                $.ajax({
                    url: `/alumno/convocatoria-emprendimiento?handler=IncubatorConvocations`,
                    type: "GET",
                    dataType: 'html'
                }).done(function (data) {
                    $("#convocations").html(data);
                }).fail(function (error) {
                }).always(function () {
                })
            },
            init: function () {
                this.load();
            }
        },
        init: function () {
            this.incubatorConvocations.init();
        }
    };


    return {
        init: function () {
            pageInfo.init();
        }
    }
}();

$(function () {
    InitApp.init()
});
