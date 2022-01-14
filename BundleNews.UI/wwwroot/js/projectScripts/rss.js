$("#insert_rss_button").click(function () {
    AddRSS();
});


function AddRSS() {
    var url = $('#insert_rss_link').val();
    var name = $("#insert_category_name").val();
    $.ajax({
        type: "POST",
        url: "Rss/RunRssGenerator",
        async: false,
        data: { 'url': url, 'name': name },
        success: function (data) {
            ClearFilter();
            if (data.val == "Ekleme başarılı")
                toastr.success(data.val);
            else
                toastr.error(data.val);
        }
    });
}


function ClearFilter() {
    $("#insert_rss_link").val('');
    $('#insert_category_name').val('');
}