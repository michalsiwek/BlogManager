$("#show-preview").click(function () {
    $("#preview").html("<span>Hello <b>Again</b></span><input id='close-preview' type='button' value='Close' class='btn btn-default btn-bg' />");
    $("#entry-form").attr("hidden", true);
})

$("#show-preview").click(function () {
    $("#preview").html("");
    $("#entry-form").attr("hidden", true);
})