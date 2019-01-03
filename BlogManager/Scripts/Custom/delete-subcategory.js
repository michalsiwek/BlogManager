$('#delete-subcat-btn').click(function () {
    var selectedSubcatId = $("#ContentCategory_Subcategories option:selected").val();
    var token = $('input[name="__RequestVerificationToken"]').val();

    if (confirm('Are you sure?')) {
        $.ajax({
            url: "/ContentCategories/DeleteSubcategory/" + selectedSubcatId,
            type: "POST",
            traditional: true,
            data: {
                __RequestVerificationToken: token
            },
            success: function () {
                location.reload();
            },
            error: function () {
                alert("An error has occured!");
            }
        });
    }
});