$(document).ready(function () {
    $("#Entry_ContentCategory_Id").change(function () {
        if ($("#Entry_ContentCategory_Id").val() > 0) {
            $.get("/Entries/GetContentSubcategories", { contentCategoryId: $("#Entry_ContentCategory_Id").val() }, function (data) {
                $("#Entry_ContentSubcategory_Id").empty();
                $("#Entry_ContentSubcategory_Id").append("<option value>Select...</option>");
                $.each(data, function (index, row) {
                    $("#Entry_ContentSubcategory_Id").append("<option value='" + row.Id + "'>" + row.Name + "</option>")
                });
            });
        }
    })
});

$(document).ready(function () {
    $("#Gallery_ContentCategory_Id").change(function () {
        if ($("#Gallery_ContentCategory_Id").val() > 0) {
            $.get("/Galleries/GetContentSubcategories", { contentCategoryId: $("#Gallery_ContentCategory_Id").val() }, function (data) {
                $("#Gallery_ContentSubcategory_Id").empty();
                $("#Gallery_ContentSubcategory_Id").append("<option value>Select...</option>");
                $.each(data, function (index, row) {
                    $("#Gallery_ContentSubcategory_Id").append("<option value='" + row.Id + "'>" + row.Name + "</option>")
                });
            });
        }
    })
});