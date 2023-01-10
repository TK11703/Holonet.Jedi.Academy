$(document).ready(function () {
    $("#questObjectiveList .objectiveSelector").on('change', function () {
        if (this.checked) {
            PrepareCompletedObjective($(this).val());
        }
        else {
            RemoveCompletedObjective($(this).val());
        }
    });
});

function PrepareCompletedObjective(objectiveId) {
    let newId = parseInt(objectiveId);
    let found = false;
    $("[name='CompletedObjectiveIds']").each(function () {
        if (parseInt($(this).val()) === newId)
            found = true;
    });
    if (!found)
        $("#completedIdCollection").append(selectedInputTemplate(newId));
}

function RemoveCompletedObjective(objectiveId) {
    let itemId = parseInt(objectiveId);
    $("[name='CompletedObjectiveIds']").each(function () {
        if (parseInt($(this).val()) === itemId) {
            $(this).remove();
            return;
        }
    });
}

const selectedInputTemplate = (objectiveId) => `
<input type="hidden" name="CompletedObjectiveIds" value="${objectiveId}" />
`.trim();