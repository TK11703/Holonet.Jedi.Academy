function IsNullOrEmpty(value) {
    return value == null || value === "";
}

var window_focus = false;

// Return Query String Parameter
function queryString(key) {
    var re = new RegExp('(?:\\?|&)' + key + '=(.*?)(?=&|$)', 'gi');
    var r = [], m;
    while ((m = re.exec(document.location.search)) != null) r.push(m[1]);
    return r;
}

function GetQueryStringParams() {
    var queryParameters = {};
    var querystring = location.search.substring(1);
    if (querystring == null || querystring == "" || querystring == undefined) {
        queryParameters = null;
    }
    else {
        var re = /([^&=]+)=([^&]*)/g;
        var map;
        while (m = re.exec(querystring)) {
            queryParameters[decodeURIComponent(m[1])] = decodeURIComponent(m[2]);
        }
    }

    return queryParameters;
}

function LoadingMask(isOn, message) {
    if (isOn) {
        var regex = /(<([^>]+)>)/ig;
        if (regex.test(message)) {
            message = NewlineToBreak(message);
            bootbox.dialog({ message: message, closeButton: false });
        }
        else {
            var constructedMessage = "<h5 class=\"text-center\">Processing Your Request</h5>";
            constructedMessage += "<div class=\"text-center\">" + NewlineToBreak(message) + "</div>";
            constructedMessage += "<div class=\"text-center my-3\"><img src=\"Library/images/stampede.gif\" /></div>";
            bootbox.dialog({ message: constructedMessage, closeButton: false });
        }
    }
    else {
        bootbox.hideAll();
    }
}

function ShowWebServiceErrors(serviceResponse, callback) {
    //Alert user to the issues in the ajaxResponse
    var issuesToShow = "A problem occurred during the last request. The details are indicated below:";
    var issueList = '<ul>';
    if (serviceResponse.Errors) {
        for (var i = 0; i < serviceResponse.Errors.length; i++) {
            issueList += '<li>' + NewlineToBreak(serviceResponse.Errors[i]) + "</li>";
        }
    }
    else {
        issueList += "<li>There were no details specified for this error.</li>";
    }
    issueList += '</ul>';
    issuesToShow += issueList;
    issuesToShow = NewlineToBreak(issuesToShow);
    bootbox.alert(issuesToShow, function () {
        if (callback && typeof (callback === "function")) {
            callback();
        }
    });
}

function ShowAjaxError(xhrResponse, callback) {
    if (xhrResponse != null && !IsNullOrEmpty(xhrResponse.responseText)) {
        var issuesToShow = "The last request to the server was not successful.";
        var issueList = '<ul>';
        var ajaxError = "";
        try {
            ajaxError = JSON.parse(xhrResponse.responseText);
        }
        catch (e) {
            try {
                ajaxError = { "Message": xhrResponse.statusText };
            }
            catch (inner) {
                ajaxError = { "Message": "The last request to the server was not successful, but the response and status text could not be used to display error information." };
            }
        }

        issuesToShow += " " + ajaxError.Message;
        if (ajaxError.ExceptionMessage != undefined && !IsNullOrEmpty(ajaxError.ExceptionMessage)) {
            issueList += "<li>" + ajaxError.ExceptionMessage + "</li>";
        }
        issueList += '</ul>';
        issuesToShow += issueList;
        issuesToShow = NewlineToBreak(issuesToShow);
        bootbox.alert(issuesToShow, function () {
            if (callback && typeof (callback === "function")) {
                callback();
            }
        });
    }
    else if (xhrResponse != null && !IsNullOrEmpty(xhrResponse.statusText)) {
        var issuesToShow = "The last request to the server was not successful.";
        var ajaxError = "";
        try {
            ajaxError = JSON.parse(xhrResponse.statusText);
        }
        catch (e) {
            try {
                ajaxError = { "Message": xhrResponse.statusText };
            }
            catch (inner) {
                ajaxError = { "Message": "The last request to the server was not successful, but the response and status text could not be used to display error information." };
            }
        }

        issuesToShow += " " + ajaxError.Message;
        issuesToShow = NewlineToBreak(issuesToShow);
        bootbox.alert(issuesToShow, function () {
            if (callback && typeof (callback === "function")) {
                callback();
            }
        });
    }
    else {
        var errorPage = "<html><head><title>ELISA - AJAX Error</title></head><body>";
        errorPage += "<h1>The Page Detected An AJAX Error</h1>";
        errorPage += "<p>An AJAX error occurred while attempting the last request. The response from the server did not include any details to share. There could be a few causes for this:";
        errorPage += "<ul><li>The server may be unreachable.</li><li>Your computer may not have any network connection.</li><li>A firewall or security software could be blocking the connection.</li></ul>";
        errorPage += "</p>";
        errorPage += "<p>It is recommended to close all your browser windows and attempt to connect to the site again. Please contact SPAN support if you continue to experience this error.</p>";
        errorPage += "</body></html>";
        errorPage = NewlineToBreak(errorPage);
        siteAlert("An error occurred while attempting the last request. The error was not handled correctly within this page, but the error details should be shown in another window.");
        var errorWin = popupWindow('', 'AJAXERROR', 600, 800, true);
        errorWin.document.open().write(errorPage);
    }
}


function siteAlert(alertMessage, callback) {
    alertMessage = NewlineToBreak(alertMessage);
    bootbox.alert(alertMessage, function () {
        if (callback && typeof (callback === "function")) {
            callback();
        }
    });
}

function siteConfirm(confirmMessage, cancelButtonLabel, proceedButtonLabel, callback) {
    var cancelButtonText = "No";
    var proceedButtonText = "Yes"

    if (!IsNullOrEmpty($.trim(cancelButtonLabel)))
        cancelButtonText = cancelButtonLabel;
    if (!IsNullOrEmpty($.trim(proceedButtonLabel)))
        proceedButtonText = proceedButtonLabel;

    confirmMessage = NewlineToBreak(confirmMessage);
    bootbox.confirm({
        message: confirmMessage,
        buttons: {
            confirm: {
                label: proceedButtonText,
                className: 'btn-success'
            },
            cancel: {
                label: cancelButtonText,
                className: 'btn-danger'
            }
        },
        callback: function (result) {
            if (callback && typeof (callback === "function")) {
                callback(result);
            }
        }
    });
}

function sitePrompt(promptMessage, cancelButtonLabel, confirmButtonLabel, inputType, callback) {
    var cancelButtonText = "Cancel";
    var confirmButtonText = "OK"
    var defaultInput = IsNullOrEmpty($.trim(inputType))
    if (!IsNullOrEmpty($.trim(cancelButtonLabel)))
        cancelButtonText = cancelButtonLabel;
    if (!IsNullOrEmpty($.trim(confirmButtonLabel)))
        confirmButtonText = confirmButtonLabel;
    promptMessage = NewlineToBreak(promptMessage);
    if (defaultInput) {
        bootbox.prompt({
            title: promptMessage,
            buttons: {
                confirm: {
                    label: confirmButtonText,
                    className: 'btn-success'
                },
                cancel: {
                    label: cancelButtonText,
                    className: 'btn-danger'
                }
            },
            callback: function (result) {
                if (callback && typeof (callback === "function")) {
                    callback(result);
                }
            }
        });
    }
    else {
        bootbox.prompt({
            title: promptMessage,
            inputType: inputType,
            buttons: {
                confirm: {
                    label: confirmButtonText,
                    className: 'btn-success'
                },
                cancel: {
                    label: cancelButtonText,
                    className: 'btn-danger'
                }
            },
            callback: function (result) {
                if (callback && typeof (callback === "function")) {
                    callback(result);
                }
            }
        });
    }
}

function showSuccessToast(itemTitle, itemMessage) {
    toastr.options = { positionClass: "toast-bottom-right", toastClass: 'toastr' };
    if (!IsNullOrEmpty($.trim(itemTitle))) {
        toastr.success(itemMessage, itemTitle);
    }
    else {
        toastr.success(itemMessage);
    }
}

function showWarningToast(itemTitle, itemMessage) {
    toastr.options = { positionClass: "toast-bottom-right", toastClass: 'toastr' };
    if (!IsNullOrEmpty($.trim(itemTitle))) {
        toastr.warning(itemMessage, itemTitle);
    }
    else {
        toastr.warning(itemMessage);
    }
}

function showPermanentWarningToast(itemTitle, itemMessage) {
    toastr.options = { positionClass: "toast-bottom-right", toastClass: 'toastr', "closeButton": true, "timeOut": 0, "preventDuplicates": true };
    if (!IsNullOrEmpty($.trim(itemTitle))) {
        toastr.warning(itemMessage, itemTitle);
    }
    else {
        toastr.warning(itemMessage);
    }
}

function showErrorToast(itemTitle, itemMessage) {
    toastr.options = { positionClass: "toast-bottom-right", toastClass: 'toastr' };
    if (!IsNullOrEmpty($.trim(itemTitle))) {
        toastr.error(itemMessage, itemTitle);
    }
    else {
        toastr.error(itemMessage);
    }
}

function showPermanentErrorToast(itemTitle, itemMessage) {
    toastr.options = { positionClass: "toast-bottom-right", toastClass: 'toastr', "closeButton": true, "timeOut": 0, "preventDuplicates": true };
    if (!IsNullOrEmpty($.trim(itemTitle))) {
        toastr.error(itemMessage, itemTitle);
    }
    else {
        toastr.error(itemMessage);
    }
}

function showInformationToast(itemTitle, itemMessage, callback) {
    toastr.options = { positionClass: "toast-bottom-right", toastClass: 'toastr', "timeOut": 5000 };
    if (callback && typeof (callback === "function")) {
        toastr.options.onclick = callback;
    }

    if (!IsNullOrEmpty($.trim(itemTitle))) {
        toastr.info(itemMessage, itemTitle);
    }
    else {
        toastr.info(itemMessage);
    }
}

function showPermanentInformationToast(itemTitle, itemMessage, callback) {
    toastr.options = { positionClass: "toast-bottom-right", toastClass: 'toastr', "closeButton": true, "timeOut": 0, "preventDuplicates": true };
    if (callback && typeof (callback === "function")) {
        toastr.options.onclick = callback;
    }
    if (!IsNullOrEmpty($.trim(itemTitle))) {
        toastr.info(itemMessage, itemTitle);
    }
    else {
        toastr.info(itemMessage);
    }
}

function showPermanentErrorToast(itemTitle, itemMessage, callback) {
    toastr.options = { positionClass: "toast-bottom-right", toastClass: 'toastr', "closeButton": true, "timeOut": 0, "preventDuplicates": true };
    if (callback && typeof (callback === "function")) {
        toastr.options.onclick = callback;
    }
    if (!IsNullOrEmpty($.trim(itemTitle))) {
        toastr.error(itemMessage, itemTitle);
    }
    else {
        toastr.error(itemMessage);
    }
}

function showPopover(itemTitle, itemMessage, duration, placement, parentObj) {
    //placement = left, right, top, bottom
    if (placement == null || placement == "" || placement == undefined) {
        placement = "right";
    }
    var options = {
        "title": itemTitle,
        "content": itemMessage,
        "placement": placement,
        "trigger": "manual",
        container: 'body'
    };

    $($(parentObj)).popover(options).popover("show");

    if (duration == null || duration == "" || duration == undefined) {
        duration = 5000;
    }
    window.setTimeout(function () { $($(parentObj)).popover("destroy"); }, duration);
}

function siteSuccess(el, message) {
    $(el).prepend('<div class="alert alert-success site-success fade in"><a class="close" data-dismiss="alert">&times</a><span>' + message + '</span></div>');
    $('.site-success').delay(2000).fadeOut("slow", function () { $(this).remove(); });
}

function siteError(el, message) {
    $(el).prepend('<div class="alert alert-error site-error fade in"><a class="close" data-dismiss="alert">&times</a><span>' + message + '</span></div>');
    $('.site-error').delay(2000).fadeOut("slow", function () { $(this).remove(); });
}

function BreakToNewline(str) {
    return str.replace(/<br\s*\/?>/mg, "\r");
}

function NewlineToBreak(str) {
    return str.replace(/\n/g, "<br/>");
}

function CarriageReturnNewlineToBreak(str) {
    return str.replace(/\r\n/g, "<br/>");
}

function compareString(string1, string2, ignoreCase) {
    return compareString(string1, string2, ignoreCase, false);
}

function compareString(string1, string2, ignoreCase, useLocale) {
    if (ignoreCase) {
        if (useLocale) {
            string1 = string1.toLocaleLowerCase();
            string2 = string2.toLocaleLowerCase();
        }
        else {
            string1 = string1.toLowerCase();
            string2 = string2.toLowerCase();
        }
    }
    return string1 === string2;
}

function popupWindow(url, title, w, h, scrollable) {
    var left = (screen.width / 2) - (w / 2);
    var top = (screen.height / 2) - (h / 2);
    var scrollbars = (scrollable) ? "yes" : "no";
    return window.open(url, title, 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=' + scrollbars + ', resizable=no, copyhistory=no, width=' + w + ',height=' + h + ', top=' + top + ', left=' + left, true);
}

function PulseItem(obj) {
    $(obj).fadeTo(100, 0.3, function () { $(this).fadeTo(500, 1.0); });
}

// Cookie functions from w3schools
function setCookie(cname, cvalue, exdays) {
    if (!IsNullOrEmpty($.trim(cname)) && !IsNullOrEmpty($.trim(cvalue))) {
        var d = new Date();
        d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
        var expires = "expires=" + d.toUTCString();
        document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
    }
}

function getCookie(cname) {
    var name = cname + "=";
    var decodedCookie = decodeURIComponent(document.cookie);
    var ca = decodedCookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) === ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) === 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}
function isASCII(str, extended) {
    return (extended ? /^[\x00-\xFF]*$/ : /^[\x00-\x7F]*$/).test(str);
}

function SanitizeAPI(input, friendlyFieldName, callback) {
    $.post("api/utility/SanitizeInput", { "inputToSanitize": input })
        .fail(function (jqxhr, textStatus, error) {
            ShowAjaxError(jqxhr);
        })
        .done(function (serviceResponse) {
            if (serviceResponse.Success) {
                callback(serviceResponse.Result);
                if (serviceResponse.Changed) {
                    showInformationToast("System Notification", "The text in '" + friendlyFieldName + "' has been sanitized.");
                }
            }
            else {
                ShowWebServiceErrors(serviceResponse, null);
            }
        });
}