var infinityScroll = null;
$(document).ready(function () {

    $("#applyFilter").click(function () {
        $('#questMasonry').empty();
        var rankId = parseInt($("#RankFilter").val());        
        infinityScroll.params = { textFilter: $("#TextFilter").val(), rankFilter: rankId, completed: GetCompletedFilterStatus()};
        infinityScroll.AddNextPage(0);
    });
    $("#clearFilter").click(function () {
        $("#TextFilter").val("");
        $("#RankFilter").val("");
        $("#CompletedFilter").val("");
        $('#questMasonry').empty();
        infinityScroll.params = { textFilter: "", rankFilter: null, completed: null };
        infinityScroll.AddNextPage(0);
    });

    var $grid = $('#questMasonry').masonry({
        // disable initial layout
        initLayout: false,
        percentPosition: true
    });

    // trigger initial layout
    $grid.masonry();

    infinityScroll = new InfinityScroll("questMasonry", "/api/quests/GetIndexPage", { textFilter: "", rankFilter: null, completed: null });
});

function GetCompletedFilterStatus() {
    let completedStatus = null;
    if ($("#CompletedFilter").val() == "1")
        return true;
    if ($("#CompletedFilter").val() == "0")
        return false;
    return completedStatus;
}
