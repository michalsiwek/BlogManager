$("#show-preview").click(function () {

    String.prototype.replaceAll = function (search, replacement) {
        var target = this;
        return target.split(search).join(replacement);
    };

    String.prototype.twoDigitsDate = function () {
        if (this.length > 1)
            return this;
        else
            return "0" + this;
    };

    var title = $("#entry-title").val();
    var content = $("#entry-content").val();
    var img = $("#entry-img").val();
    content = content.replaceAll("\n\n", "</p>\n<p>");
    content = "<p>" + content + "</p>";

    var currentdate = new Date();
    var shortDate = currentdate.getDate().toString().twoDigitsDate() + "."
        + (currentdate.getMonth() + 1).toString().twoDigitsDate() + "."
        + currentdate.getFullYear();
    var shortTime = currentdate.getHours().toString().twoDigitsDate() + ":" + currentdate.getMinutes().toString().twoDigitsDate();

    var output = "<div id='container'><main id='entry-main'><div id='content'><header><h1>" + title + "</h1></header>";
    output = output + "<p id='entry-data'>" + shortDate + " | " + shortTime + "</p><br><div class='main-img-container'>";
    output = output + "<img src='" + img + "' alt='entry-main-img'>";
    output = output + "</div><br>" + content + "</div><br><hr></main></div>";

    $("#preview-header").html("Preview");
    $(output).appendTo("#preview");
    $("<div class='text-center'><input id='close-preview' type='button' value='Close Preview' class='btn btn-default btn-bg' /></div>").appendTo("#preview");
    $("#entry-form").attr("hidden", true);

    $("#close-preview").click(function () {
        $("#preview").html("");
        $("#preview-header").html("");
        $("#entry-form").removeAttr("hidden");
    });
})