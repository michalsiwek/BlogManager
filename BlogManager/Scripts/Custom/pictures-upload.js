$('#files-selector').change(function () {
    var filesCount = this.files.length;
    $('#submit-gallery-form').prop('disabled', false);
    if (filesCount > 1) {
        $('#upload-file-info').attr('value', this.files.length + ' files selected');
    } else {
        $('#upload-file-info').attr('value', this.files[0].name);
    }    
});