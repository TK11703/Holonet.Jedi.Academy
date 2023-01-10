// *****************************************************************************************************************
//                                      I n f i n i t y   S c r o l l                                             **
//                                                                                                                **
//                                   by Thomas Brockmann  mail@tbrockmann.de                                      **
// *****************************************************************************************************************
//EDITED BY Beowshawitz

function InfinityScroll(pagingContainer, iAction, iParams) {
    this.Container = pagingContainer;        // Reference to the table where data should be added
    this.action = iAction;      // Name of the conrtoller action
    this.params = iParams;      // Additional parameters to pass to the controller
    this.loading = false;       // true if asynchronous loading is in process
    this.AddNextPage = function (firstItem) {
        this.loading = true;
        $("#" + self.Container).append(self.GetLoadingIndicator());
        this.params.firstItem = firstItem;
        // $("#footer").css("display", "block"); // show loading info
        $.ajax({
            type: 'POST',
            url: self.action,
            data: JSON.stringify(self.params),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
        })
            .done(function (result) {
                if (result) {
                    $("#" + self.Container).append(result);
                    self.loading = false;
                }
            })
            .fail(function (xhr, ajaxOptions, thrownError) {
                console.log("Error in AddNextPage:", thrownError);
                ShowAjaxError(xhr);
            })
            .always(function () {
                $("#" + self.Container).find("div.pageLoading").remove();
                if ($('#' + self.Container + ' div.card').length == 0) {
                    $("#" + self.Container).append(self.GetNoResultsIndicator());
                }
                else {
                    $("#" + self.Container).find("div.noResults").remove();
                }
            });
    }
    this.GetLoadingIndicator = function() {
        const loadingTemplate = `
<div class="col pageLoading">
    <button class="btn btn-primary" type="button" disabled>
      <span class="spinner-grow spinner-grow-sm" role="status" aria-hidden="true"></span>
      Loading...
    </button>
</div>
`.trim();
        return loadingTemplate;
    }
    this.GetNoResultsIndicator = function () {
        const noResultsTemplate = `
<div class="col noResults">
    <div class="alert alert-warning fade show" role="alert">
      <strong>No Results!</strong> There were no results to display for you. You can try adjusting or clearing the filters, if you have set any.
    </div>
</div>
`.trim();
        return noResultsTemplate;
    }

    var self = this;
    window.onscroll = function (ev) {
        if ((window.innerHeight + window.scrollY) >= document.body.offsetHeight) {
            //User is currently at the bottom of the page
            if (!self.loading) {
                var itemCount = $('#' + self.Container + ' div.card').length - 1;
                self.AddNextPage(itemCount);
            }
        }
    };
    this.AddNextPage(0);
}
