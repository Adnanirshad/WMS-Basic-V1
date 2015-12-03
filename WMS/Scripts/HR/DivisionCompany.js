$(document).ready(function () {

    $('#DeptID').empty();
   //var URL = '/WMS/Emp/DepartmentList';
    var URL = '/Emp/DepartmentList';
    $.getJSON(URL , function (data) {
        var selectedItemID = document.getElementById("selectedDeptIDHidden").value;
        var items;
        $.each(data, function (i, state) {
            if (state.Value == selectedItemID)
                items += "<option selected value='" + state.Value + "'>" + state.Text + "</option>";
            else
                items += "<option value='" + state.Value + "'>" + state.Text + "</option>";
            // state.Value cannot contain ' character. We are OK because state.Value = cnt++;
        });
        $('#DeptID').html(items);
    });


   

});