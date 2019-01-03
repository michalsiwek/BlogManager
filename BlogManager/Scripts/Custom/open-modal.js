$('.img-container').click(function (event) {
    var picId = $(this).children('.gallery-picture').attr('img-id');
    var modalId = "#modal-" + picId;
    $(modalId).modal();
});

$('#edit-data-btn').click(function (event) {
    var modalId = "#edit-data-modal";
    $(modalId).modal();
});

$('#change-password-btn').click(function (event) {
    var modalId = "#change-password-modal";
    $(modalId).modal();
});

$('#new-subcat-btn').click(function (event) {
    var modalId = "#add-new-subcat-modal";
    $(modalId).modal();
});

$('#edit-subcat-btn').click(function (event) {
    var selectedSubcatId = $("#ContentCategory_Subcategories option:selected").val();
    var modalId = "#edit-subcat-modal-" + selectedSubcatId;
    $(modalId).modal();
});