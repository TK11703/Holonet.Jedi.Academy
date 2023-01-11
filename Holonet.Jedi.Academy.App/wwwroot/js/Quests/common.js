$(document).ready(function () {
    $("#objectiveList").change(function () {
        AddObjective($(this).val());
    });
    $("#removeObjectives").click(function () {
        RemoveSelectedObjectives();
    });
    ConfigureExpandableTextAreas(); //global.js
});

function AddObjective(objId) {
    if (objId !== "") {
        if (!ObjectiveExists(objId)) {
            GetObjective(objId);
        }
        else {
            showWarningToast("Objective Not Added", "The selected objective is already associated with this quest. Please select a different objective, if desired.");
        }
    }
}

function ObjectiveExists(objId) {
    var exists = false;
    $("#selectedObjectives .list-group-item").each(function () {
        if ($(this).data("objectiveid") != null && parseInt($(this).data("objectiveid")) == parseInt(objId)) {
            exists = true;
            return;
        }
    });
    return exists;
}

function GetObjective(objId) {
    $.ajax({
        url: "/api/objectives/GetObjective",
        type: "POST",
        data: JSON.stringify({ "objectiveId": objId }),
        contentType: "application/json; charset=utf-8",
        dataType: "json"
    })
        .fail(function (jqxhr, textStatus, error) {
            ShowAjaxError(jqxhr);
        })
        .done(function (serviceResponse) {
            const objective = serviceResponse;
            $("#selectedObjectives .alert-warning").remove();
            $("#selectedObjectives").append(objectiveTemplate(objective));
            $("#removeObjectives").prop("disabled", false);
            AddObjectiveId(objective.Id);
        });
}

function AddObjectiveId(val) {
    let newId = parseInt(val);
    let found = false;
    $("[name='SelectedObjectiveIds']").each(function () {
        if (parseInt($(this).val()) === newId)
            found = true;
    });
    if (!found)
        $("#selectedIdCollection").append(selectedInputTemplate(newId));
}

function RemoveObjectiveId(val) {
    let itemId = parseInt(val);
    let currentObjectives = new Array();
    $("[name='SelectedObjectiveIds']").each(function () {
        if (parseInt($(this).val()) === itemId) {
            $(this).remove();
            return;
        }
    });
}

const selectedInputTemplate = (objectiveId) => `
<input type="hidden" name="SelectedObjectiveIds" value="${objectiveId}" />
`.trim();

const objectiveTemplate = (objectiveObj) => `
    <label class="list-group-item d-flex gap-3" data-objectiveid="${objectiveObj.Id}">
		<input class="form-check-input flex-shrink-0" type="checkbox" value="" data-objectiveid="${objectiveObj.Id}" style="font-size: 1.375em;">
		<span class="pt-1 form-checked-content">
			<strong>${objectiveObj.Name}</strong>
			<small class="d-block text-muted">
				${objectiveObj.Description}
			</small>
		</span>
	</label>                    
`.trim();

function RemoveSelectedObjectives() {
    $("#selectedObjectives .list-group-item :checkbox").each(function () {
        if ($(this).is(":checked")) {
            RemoveObjectiveId($(this).data("objectiveid"));
            $(this).parents("label.list-group-item").remove();
        }
    });

    if ($("[name='SelectedObjectiveIds']").length == 0) {
        $("#removeObjectives").prop("disabled", true);
    }
}
