$(document).ready(function () {
    ExecuteLiteProfileCheck();
});

function ExecuteLiteProfileCheck() {
    var isProfileComplete = $("#isProfileComplete").val();
    if (isProfileComplete === "false" || isProfileComplete === "False") {
        var issueJSON = $("#profileIssues").val();
        if (issueJSON != null && issueJSON != "") {
            showPermanentInformationToast("User Profile Warnings", "Your profile does not appear to be complete. Click here to view the detected issues.", DisplayProfileCheckResults);
        }
        else {
            showPermanentInformationToast("User Profile Warnings", "Your profile appears to be missing. Click here to enter profile information.", function () { window.location.href = "UserProfile"; });
        }
    }
}

function DisplayProfileCheckResults() {
    var isProfileComplete = $("#isProfileComplete").val();
    if (isProfileComplete === "false" || isProfileComplete === "False") {
        var data = JSON.parse($("#profileIssues").val());
        var issues = data.issues.join("\n");
        issues = "A completed profile will help personalize the results display for your selected preferences." + "\n\n" + issues;
        siteAlert(issues)
    }
}