var InitApp = function () {  
    var pageInfo = {
        investigationConvocations: {
            load: function () {
                $.ajax({
                    url: `/convocatorias-de-investigacion?handler=InvestigationConvocations`,
                    type: "GET",
                    dataType: 'html'
                }).done(function (data) {
                    $("#prueba").html(data);
                }).fail(function (error) {
                }).always(function () {
                })
            },
            init: function () {
                this.load();
            }
        },      
        init: function () {
            this.investigationConvocations.init();
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
