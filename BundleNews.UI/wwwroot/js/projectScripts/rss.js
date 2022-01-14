$("#insert_rss_button").click(function () {
    AddRSS();
});


function AddRSS() {
    StartLoader();
    var url = $('#insert_rssGenerator_link').val();
    var name = $("#insert_rssGenerator_name").val();
    $.ajax({
        type: "POST",
        url: "Rss/RunRssGenerator",
        async: false,
        data: { 'url': url, 'name': name },
        success: function (data) {
            ClearFilter();
        }
    });
}


function ClearFilter() {
    StartLoader();
    $("#select_order").val(-1);
    $('#search_box').val('');
    StopLoader();
}