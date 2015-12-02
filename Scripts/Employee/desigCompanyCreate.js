$(document).ready(function () {

    $('#DesigID').empty();
    //var URL = '/WMS/Emp/DesignationList';
    var URL = '/Emp/DesignationList';
    $.getJSON(URL + '/' + $('#CompanyID').val(), function (data) {
        var items;
        $.each(data, function (i, state) {
            items += "<option value='" + state.Value + "'>" + state.Text + "</option>";
            // state.Value cannot contain ' character. We are OK because state.Value = cnt++;
        });
        $('#DesigID').html(items);
    });


    $('#CompanyID').change(function () {
        $('#DesigID').empty();
        // var URL = '/WMS/Emp/DesignationList';
        var URL = '/Emp/DesignationList';
        $.getJSON(URL + '/' + $('#CompanyID').val(), function (data) {
            var items;
            $.each(data, function (i, state) {
                items += "<option value='" + state.Value + "'>" + state.Text + "</option>";
                // state.Value cannot contain ' character. We are OK because state.Value = cnt++;
            });
            $('#DesigID').html(items);
        });
    });

});