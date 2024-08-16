var InitApp = function () {
    var pageInfo = {
        events: {
            load: function () {
                $.ajax({
                    url: `/eventos?handler=Events`,
                    type: "GET",
                    dataType: 'html'
                }).done(function (data) {
                    $("#eventsContainer").html(data);
                }).fail(function (error) {
                }).always(function () {
                })
            },
            init: function () {
                this.load();
            }
        },
        init: function () {
            this.events.init();
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
