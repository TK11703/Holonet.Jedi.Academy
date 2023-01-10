$(document).ready(function () {
    $("#acceptButton").click(function () {
        $(this).text("Loading...").prepend("<span class=\"spinner-grow spinner-grow-sm mx-2\"></span>").addClass("disabled");
        AcknowledgeDisclaimer();
    });
});

function AcknowledgeDisclaimer() {
    window.setTimeout(function () {
        $.ajax({
            type: "GET",
            url: "api/Utility/AcknowledgeDisclaimer",
            dataType: "json",
            data: '{}',
            contentType: "application/json; charset=utf-8",
            success: function (serviceResponse) {
                if (serviceResponse.Success) {
                    if (window.sessionStorage) {
                        sessionStorage.dodAck = "true";
                    }
                    window.setTimeout(function () {
                        var referrer = queryString("refer");
                        var url = null;
                        if (referrer != null && referrer.length > 0) {
                            url = decodeURIComponent(referrer);
                        }
                        else {
                            url = "/home";
                        }
                        window.location.href = url;
                    }, 500);
                }
                else {
                    $("#acceptButton").text("Accept").remove("span.spinner-grow").removeClass("disabled");
                }
            },
            error: function (xhr, status, error) {
                $("#acceptButton").text("Accept").remove("span.spinner-grow").removeClass("disabled");
                ShowAjaxError(xhr);
            }
        });
    }, 500);
}