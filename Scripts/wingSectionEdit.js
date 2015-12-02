$(document).ready(function () {

    $('#SecID').empty();
    //var URL = '/WMS/Emp/SectionList';
    var URL = '/Emp/SectionList';
    $.getJSON(URL + '/' + $('#DeptID').val(), function (data) {
        var items;
        $.each(data, function (i, state) {
            items += "<option value='" + state.Value + "'>" + state.Text + "</option>";
            // state.Value cannot contain ' character. We are OK because state.Value = cnt++;
        });
        $('#SecID').html(items);
        $('#SectionDivID').show();
    });


    $('#DeptID').change(function () {
        $('#SecID').empty();
        //var URL = '/WMS/Emp/SectionList';
        var URL = '/Emp/SectionList';
        $.getJSON(URL + '/' + $('#DeptID').val(), function (data) {
            var items;
            $.each(data, function (i, state) {
                items += "<option value='" + state.Value + "'>" + state.Text + "</option>";
                // state.Value cannot contain ' character. We are OK because state.Value = cnt++;
            });
            $('#SecID').html(items);
            $('#SectionDivID').show();
        });
    });

});