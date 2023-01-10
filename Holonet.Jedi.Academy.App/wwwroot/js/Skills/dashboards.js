var skillParticipationPieChart = null;

$(document).ready(function () {
    GenerateChartItems();
    var repeatEvery = 1000 * 60 * 2; //5 minutes
    setInterval(GenerateChartItems, repeatEvery);
});

function GenerateChartItems() {
    GetSkillParticipation();
    GetSkillAverageCompletionTime();
}

function GetSkillParticipation() {
    if (skillParticipationPieChart != null) {
        skillParticipationPieChart.destroy();
    }
    $.ajax({
        url: "/api/dashboard/GetSkillParticipation",
        type: "POST",
        data: JSON.stringify({ "knowledgeId": $("#knowledgeId").val() }),
        contentType: "application/json; charset=utf-8",
        dataType: "json"
    })
        .fail(function (jqxhr, textStatus, error) {
            ShowAjaxError(jqxhr);
        })
        .done(function (serviceResponse) {
            skillParticipationPieChart = ShowPieChart($("#skillParticipation"), true, serviceResponse);
        });
}

function GetSkillAverageCompletionTime() {
    $.ajax({
        url: "/api/dashboard/GetSkillAvgCompletion",
        type: "POST",
        data: JSON.stringify({ "knowledgeId": $("#knowledgeId").val() }),
        contentType: "application/json; charset=utf-8",
        dataType: "json"
    })
        .fail(function (jqxhr, textStatus, error) {
            ShowAjaxError(jqxhr);
        })
        .done(function (serviceResponse) {
            $("#skillAvgCompletion").text(serviceResponse);
        });
}