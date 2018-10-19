$('.img-container').click(function (event) {
    var picId = $(this).children('.gallery-picture').attr('img-id');
    var modalId = "#modal-" + picId;
    $(modalId).modal();
});