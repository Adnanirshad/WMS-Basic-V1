$(document).ready(function () {

    $('#GradeID').empty();
    //var URL = '/WMS/Emp/GradeList';
    var URL = '/Emp/GradeList';
    $.getJSON(URL + '/' + $('#CompanyID').val(), function (data) {
        var items;
        $.each(data, function (i, state) {
                items += "<option value='" + state.Value + "'>" + state.Text + "</option>";
            // state.Value cannot contain ' character. We are OK because state.Value = cnt++;
        });
        $('#GradeID').html(items);
        $('#GradesDivID').show();
    });


    $('#CompanyID').change(function () {
        $('#GradeID').empty();
        //  var URL = '/WMS/Emp/GradeList';
        var URL = '/Emp/GradeList';
        $.getJSON(URL + '/' + $('#CompanyID').val(), function (data) {
            var items;
            $.each(data, function (i, state) {
                    items += "<option value='" + state.Value + "'>" + state.Text + "</option>";
                // state.Value cannot contain ' character. We are OK because state.Value = cnt++;
            });
            $('#GradeID').html(items);
            $('#GradesDivID').show();
        });
    });

});