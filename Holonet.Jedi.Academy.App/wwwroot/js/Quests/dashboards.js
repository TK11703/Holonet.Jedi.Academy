var questParticipationPieChart = null;

$(document).ready(function () {
    GenerateChartItems();
    var repeatEvery = 1000 * 60 * 2; //5 minutes
    setInterval(GenerateChartItems, repeatEvery);
});

function GenerateChartItems() {
    GetQuestParticipation();
    GetQuestAverageCompletionTime();
}

function GetQuestParticipation() {
    if (questParticipationPieChart != null) {
        questParticipationPieChart.destroy();
    }
    $.ajax({
        url: "/api/dashboard/GetQuestParticipation",
        type: "POST",
        data: JSON.stringify({ "questId": $("#questId").val() }),
        contentType: "application/json; charset=utf-8",
        dataType: "json"
    })
        .fail(function (jqxhr, textStatus, error) {
            ShowAjaxError(jqxhr);
        })
        .done(function (serviceResponse) {
            questParticipationPieChart = ShowPieChart($("#questParticipation"), true, serviceResponse);
        });
}

function GetQuestAverageCompletionTime() {
    $.ajax({
        url: "/api/dashboard/GetQuestAvgCompletion",
        type: "POST",
        data: JSON.stringify({ "questId": $("#questId").val() }),
        contentType: "application/json; charset=utf-8",
        dataType: "json"
    })
        .fail(function (jqxhr, textStatus, error) {
            ShowAjaxError(jqxhr);
        })
        .done(function (serviceResponse) {
            $("#questAvgCompletion").text(serviceResponse);
        });
}