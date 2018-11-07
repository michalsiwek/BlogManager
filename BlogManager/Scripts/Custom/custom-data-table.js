$(document).ready(function () {
    $('#custom-data-table').DataTable({
        "sDom": '<"row view-filter"<"col-sm-12 table-top"<"pull-right"f><"clearfix">>>t' +
            '<"row table-bottom text-center"<"form-group"<"col-md-12"p>>>',
        "ordering": false
    });
});