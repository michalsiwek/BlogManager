$('#my-file-selector').change(function () {
    $('#upload-file-info').html(this.files.length);
    $('#submit-gallery-form').prop('disabled', false);
});