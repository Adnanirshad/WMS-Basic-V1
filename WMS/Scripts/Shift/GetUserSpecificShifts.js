$(document).ready(function () {
    $('#ShiftList').empty();
    //var convalue = $('#CatID').val() + "s" + $('#CompanyID').val();
    //var URL = '/WMS/Emp/EmpTypeList';
      var URL = '/Shift/ShiftList';
    $.getJSON(URL, function (data) {
        var items;
        $.each(data, function (i, state) {
            items += "<option value='" + state.Value + "'>" + state.Text + "</option>";
            // state.Value cannot contain ' character. We are OK because state.Value = cnt++;
        });
        $('#TypeID').html(items);
        $('#EmpTypeDivID').show();
    });

});