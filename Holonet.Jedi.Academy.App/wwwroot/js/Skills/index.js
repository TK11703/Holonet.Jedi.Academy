var infinityScroll = null;
$(document).ready(function () {

    $("#applyFilter").click(function () {
        $('#experienceMasonry').empty();
        infinityScroll.params = { textFilter: $("#TextFilter").val(), completed: GetCompletedFilterStatus() };
        infinityScroll.AddNextPage(0);
    });
    $("#clearFilter").click(function () {
        $("#TextFilter").val("");
        $("#CompletedFilter").val("");
        $('#experienceMasonry').empty();
        infinityScroll.params = { textFilter: "", completed: null };
        infinityScroll.AddNextPage(0);
    });

    var $grid = $('#experienceMasonry').masonry({
        // disable initial layout
        initLayout: false,
        percentPosition: true
    });

    // trigger initial layout
    $grid.masonry();

    var infinityScroll = new InfinityScroll("experienceMasonry", "/api/experience/GetIndexPage", { textFilter: "", completed: null });
});

function GetCompletedFilterStatus() {
    let completedStatus = null;
    if ($("#CompletedFilter").val() == "1")
        return true;
    if ($("#CompletedFilter").val() == "0")
        return false;
    return completedStatus;
}