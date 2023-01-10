String.IsNullOrEmpty = function (value) {
    if (value) {
        if (typeof (value) == 'string') {
            if (value.length > 0)
                return false;
        }
    }
    return true;
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

    if (cancelButtonLabel != null && cancelButtonLabel != undefined && cancelButtonLabel != "")
        cancelButtonText = cancelButtonLabel;
    if (proceedButtonLabel != null && proceedButtonLabel != undefined && proceedButtonLabel != "")
        proceedButtonText = proceedButtonLabel;

    confirmMessage = NewlineToBreak(confirmMessage);

    var options = {
        message: confirmMessage, buttons: {
            confirm: { label: proceedButtonText, className: 'btn-success' },
            cancel: { label: cancelButtonText, className: 'btn-danger' }
        }, callback: function (result) {
            if (callback && typeof (callback === "function")) {
                callback(result);
            }
        }
    };

    bootbox.confirm(options);
}

function sitePrompt(promptMessage, callback) {
    promptMessage = NewlineToBreak(promptMessage);
    bootbox.prompt(promptMessage, function (result) {
        if (callback && typeof (callback === "function")) {
            callback(result);
        }
    });
}

function sitePrompt(promptMessage, defaultMessage, selectText, callback) {
    promptMessage = NewlineToBreak(promptMessage);
    var options = {
        title: promptMessage,
        value: defaultMessage,
        callback: function (result) {
            if (callback && typeof (callback === "function")) {
                callback(result);
            }
        }
    };

    var promptDialog = bootbox.prompt(options);
    if (selectText) {
        window.setTimeout(function () {
            promptDialog.find('.bootbox-body .bootbox-form :text').select();
        }, 500);

    }
}

function LoadingMask(isOn, message) {
    if (isOn) {
        var regex = /(<([^>]+)>)/ig;
        if (regex.test(message)) {
            message = NewlineToBreak(message);
            bootbox.dialog({ message: message, closeButton: false });
        }
        else {
            var constructedMessage = "<h5 class=\"text-center\">The System Is Processing Your Last Request</h5>";
            constructedMessage += "<div class=\"my-3\">" + NewlineToBreak(message) + "</div>";
            constructedMessage += "<div class=\"text-center\"><img src=\"/images/loading_GreyCircle.gif\" /></div>";
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
    if (!String.IsNullOrEmpty(xhrResponse.responseText)) {
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
        if (!String.IsNullOrEmpty(ajaxError.ExceptionMessage)) {
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
    else if (!String.IsNullOrEmpty(xhrResponse.statusText)) {
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
        var errorPage = "<html><head><title>DTSA WWW - AJAX Error</title></head><body>";
        errorPage += "<h1>The Page Detected An AJAX Error</h1>";
        errorPage += "<p>An AJAX error occurred while attempting the last request. The response from the server did not include any details to share. There could be a few causes for this:";
        errorPage += "<ul><li>The server may be unreachable.</li><li>Your computer may not have any network connection.</li><li>A firewall or security software could be blocking the connection.</li></ul>";
        errorPage += "</p>";
        errorPage += "<p>It is recommended to close all your browser windows and attempt to connect to the site again. Please contact IT support if you continue to experience this error.</p>";
        //errorPage += "<p>Additional AJAX error text will be displayed below for reference.</p>";
        //errorPage += "<dl>";
        //$.each(xhrResponse, function (index, value)
        //{
        //    errorPage += "<dt>" + index + "</dt>";
        //    errorPage += "<dd>" + value + "</dd>";
        //});
        //errorPage += "</dl>";
        errorPage += "</body></html>";
        errorPage = NewlineToBreak(errorPage);
        siteAlert("An error occurred while attempting the last request. The error was not handled correctly within this page, but the error details should be shown in another window.");
        var errorWin = window.open('', 'AJAXERROR', 'width=600,height=800,scrollbars=yes', true);
        errorWin.document.open().write(errorPage);
    }
}

function showSuccessToast(itemTitle, itemMessage) {
    toastr.options = { positionClass: "toast-bottom-right" };
    if (itemTitle != null && itemTitle != "") {
        toastr.success(itemMessage, itemTitle);
    }
    else {
        toastr.success(itemMessage);
    }
}

function showWarningToast(itemTitle, itemMessage) {
    toastr.options = { positionClass: "toast-bottom-right" };
    if (itemTitle != null && itemTitle != "") {
        toastr.warning(itemMessage, itemTitle);
    }
    else {
        toastr.warning(itemMessage);
    }
}

function showPermanentWarningToast(itemTitle, itemMessage) {
    toastr.options = { positionClass: "toast-bottom-right", "closeButton": true, "timeOut": 0, "preventDuplicates": true };
    if (itemTitle != null && itemTitle != "") {
        toastr.warning(itemMessage, itemTitle);
    }
    else {
        toastr.warning(itemMessage);
    }
}

function showErrorToast(itemTitle, itemMessage) {
    toastr.options = { positionClass: "toast-bottom-right" };
    if (itemTitle != null && itemTitle != "") {
        toastr.error(itemMessage, itemTitle);
    }
    else {
        toastr.error(itemMessage);
    }
}

function showPermanentErrorToast(itemTitle, itemMessage) {
    toastr.options = { positionClass: "toast-bottom-right", "closeButton": true, "timeOut": 0, "preventDuplicates": true };
    if (itemTitle != null && itemTitle != "") {
        toastr.error(itemMessage, itemTitle);
    }
    else {
        toastr.error(itemMessage);
    }
}

function showInformationToast(itemTitle, itemMessage, callback) {
    toastr.options = { positionClass: "toast-bottom-right", "timeOut": 5000 };
    if (callback && typeof (callback === "function")) {
        toastr.options.onclick = callback;
    }

    if (itemTitle != null && itemTitle != "") {
        toastr.info(itemMessage, itemTitle);
    }
    else {
        toastr.info(itemMessage);
    }
}

function showPermanentInformationToast(itemTitle, itemMessage, callback) {
    toastr.options = { positionClass: "toast-bottom-right", "closeButton": true, "timeOut": 0, "preventDuplicates": true };
    if (callback && typeof (callback === "function")) {
        toastr.options.onclick = callback;
    }
    if (itemTitle != null && itemTitle != "") {
        toastr.info(itemMessage, itemTitle);
    }
    else {
        toastr.info(itemMessage);
    }
}

function showPermanentErrorToast(itemTitle, itemMessage, callback) {
    toastr.options = { positionClass: "toast-bottom-right", "closeButton": true, "timeOut": 0, "preventDuplicates": true };
    if (callback && typeof (callback === "function")) {
        toastr.options.onclick = callback;
    }
    if (itemTitle != null && itemTitle != "") {
        toastr.error(itemMessage, itemTitle);
    }
    else {
        toastr.error(itemMessage);
    }
}

function setDatePickerDate(datePickerElem, dateInput) {
    var dateObj = null;
    if (dateInput != null && dateInput != "") {
        if (typeof dateInput == "string") {
            dateObj = JSON.dateStringToDate(dateInput);
            if (!dateObj) {
                dateObj = new Date(dateInput);
            }
        }
        else if (typeof dateInput == "object") {
            dateObj = dateInput;
        }
        if (dateObj) {
            $(datePickerElem).datepicker("update", dateObj.toString("MM/dd/yyyy"));
        }
        else {
            $(datePickerElem).val('').datepicker("update");
        }
    }
    else {
        $(datePickerElem).val('').datepicker("update");
    }
}

function getDatePickerDate(datePickerElem) {
    try {
        return Date.parse($(datePickerElem).val());
    }
    catch (e) {
        return null;
    }
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