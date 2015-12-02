$(document).ready(function () {

    $('#TypeID').empty();
    var convalue = $('#CatID').val() + "s" + $('#CompanyID').val();
    //var URL = '/WMS/Emp/EmpTypeList';
    var URL = '/Emp/EmpTypeList';
    $.getJSON(URL + '/' + convalue, function (data) {
        var selectedItemID = document.getElementById("selectedTypeIdHidden").value;
        var items;
        $.each(data, function (i, state) {

            if (state.Value == selectedItemID)
                items += "<option selected value='" + state.Value + "'>" + state.Text + "</option>";
            else
                items += "<option value='" + state.Value + "'>" + state.Text + "</option>";
            // state.Value cannot contain ' character. We are OK because state.Value = cnt++;
        });
        $('#TypeID').html(items);
        $('#EmpTypeDivID').show();
    });


    $('#CatID').change(function () {
        $('#TypeID').empty();
        var convalue = $('#CatID').val() + "s" + $('#CompanyID').val();
        // var URL = '/WMS/Emp/EmpTypeList';
       var URL = '/Emp/EmpTypeList';
        $.getJSON(URL + '/' + convalue, function (data) {
            var selectedItemID = document.getElementById("selectedTypeIdHidden").value;
            var items;
            $.each(data, function (i, state) {
                if (state.Value == selectedItemID)
                    items += "<option selected value='" + state.Value + "'>" + state.Text + "</option>";
                else
                    items += "<option value='" + state.Value + "'>" + state.Text + "</option>";
                // state.Value cannot contain ' character. We are OK because state.Value = cnt++;
            });
            $('#TypeID').html(items);
            $('#EmpTypeDivID').show();
        });
    });

});