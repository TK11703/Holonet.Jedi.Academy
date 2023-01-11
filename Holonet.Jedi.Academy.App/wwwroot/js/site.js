$(document).ready(function () {
    ConfigureSearchUI();
    ConfigureSessionTimeoutNotifier();
    GetNotifications();
    var repeatEvery = 1000 * 60 * 2; //5 minutes
    setInterval(GetNotifications, repeatEvery);
    $("#logout").click(function () {
        $(this).addClass("disabled")
        Signout();
    });
});

function ConfigureSearchUI() {
    $("#topNavSearchField").keypress(function (e) {
        if ((e.which && e.which == 13) || (e.keyCode && e.keyCode == 13)) {
            if (!IsNullOrEmpty($("#topNavSearchField").val().trim())) {
                $("#searchForm").submit();
            }
            e.preventDefault();
            return false;
        }
        else {
            return true;
        }
    });

    $("#topNavSearchButton").click(function () {
        if (!IsNullOrEmpty($("#topNavSearchField").val().trim())) {
            $("#searchForm").submit();
        }
    });
}

function ConfigureSessionTimeoutNotifier() {
    idleAfterCalc = ((parseInt($("#sessionTimeoutMins").val()) * 60) - 60); //(60 seconds * timeout in min) - 60sec buffer
    // setup the dialog buttons
    $("#continueButton_idleDialog").click(function () {
        $("#idleDialog").modal("hide");
    });
    $("#quitButton_idleDialog").click(function () {
        $.idleTimeout.options.onTimeout.call(this);
    });

    // cache a reference to the countdown element so we don't have to query the DOM for it each second.
    var $countdown = $("#idleDialog-countdown");

    // start the idle timer plugin
    $.idleTimeout('#idleDialog', '#idleDialog #continueButton_idleDialog', {
        idleAfter: idleAfterCalc,
        pollingInterval: 2, //KeepAlive is not needed
        keepAliveURL: '/api/utility/KeepAlive', //KeepAlive is not needed
        failedRequests: 1, //KeepAlive is not needed
        onTimeout: function () {
            window.location = "/Errors/SessionExpired";
        },
        onIdle: function () {
            $(this).modal("show");
        },
        onCountdown: function (counter) {
            $countdown.html(counter); // update the counter
        },
        onResume: function () {
            // the dialog closes.  nothing else needs to be done
        }
    });
}

function GetNotifications() {
    $.getJSON("/api/notifications/GetNotifications")
        .fail(function (jqxhr, textStatus, error) {
            ShowAjaxError(jqxhr);
        })
        .done(function (serviceResponse) {
            if (serviceResponse != null && serviceResponse.length > 0) {
                $.each(serviceResponse, function (index, item) {
                    showPermanentInformationToast("System Notification", item.Message, function () { AckNotif(item.Id) });
                });
            }
        });
}

function AckNotif(notifId) {
    $.ajax({
        url: "/api/notifications/AckNotif",
        type: "POST",
        data: JSON.stringify({ "notifId": notifId }),
        contentType: "application/json; charset=utf-8",
        dataType: "json"
    })
        .fail(function (jqxhr, textStatus, error) {
            ShowAjaxError(jqxhr);
        })
        .done(function (serviceResponse) {           
        });
}