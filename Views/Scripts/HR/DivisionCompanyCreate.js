$(document).ready(function () {

    $('#DivID').empty();
    //var URL = '/WMS/Division/DivisionList';
    var URL = '/Division/DivisionList';
    $.getJSON(URL ,function (data) {
        var items;
        $.each(data, function (i, state) {
            items += "<option value='" + state.Value + "'>" + state.Text + "</option>";
            // state.Value cannot contain ' character. We are OK because state.Value = cnt++;
        });
        $('#DivID').html(items);
    });


  

});