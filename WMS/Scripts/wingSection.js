$(document).ready(function () {


    $('#DeptID').empty();
    //var URL = '/WMS/Emp/DepartmentList';
    var URL = '/Emp/DepartmentList';
    var convalue =  $('#CompanyID').val();
    $.getJSON(URL + '/' + convalue, function (data) {
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

        $('#SecID').empty();
        //var URL = '/WMS/Emp/SectionList';
        var URL = '/Emp/SectionList';
       var convalue = $('#DeptID').val();
       $.getJSON(URL + '/' + convalue, function (data) {
            var selectedItemID = document.getElementById("selectedSectionIdHidden").value;
            var items;
            $.each(data, function (i, state) {
                if (state.Value == selectedItemID)
                    items += "<option selected value='" + state.Value + "'>" + state.Text + "</option>";
                else
                    items += "<option value='" + state.Value + "'>" + state.Text + "</option>";
                // state.Value cannot contain ' character. We are OK because state.Value = cnt++;
            });
            $('#SecID').html(items);
            $('#SectionDivID').show();
        });
    });


   


    $('#DeptID').change(function () {
        $('#SecID').empty();
        //var URL = '/WMS/Emp/SectionList';
        var URL = '/Emp/SectionList';
        var convalue = $('#DeptID').val();
        $.getJSON(URL + '/' +convalue, function (data) {
            var selectedItemID = document.getElementById("selectedSectionIdHidden").value;
            var items;
            $.each(data, function (i, state) {
                if (state.Value == selectedItemID)
                    items += "<option selected value='" + state.Value + "'>" + state.Text + "</option>";
                else
                    items += "<option value='" + state.Value + "'>" + state.Text + "</option>";
                // state.Value cannot contain ' character. We are OK because state.Value = cnt++;
            });
            $('#SecID').html(items);
            $('#SectionDivID').show();
        });
    });


    $('#CompanyID').change(function () {
        $('#DeptID').empty();
        //var URL = '/WMS/Emp/DepartmentList';
        var URL = '/Emp/DepartmentList';
       var convalue =  $('#CompanyID').val();
        $.getJSON(URL + '/' + $('#CompanyID').val(), function (data) {
            var items;
            $.each(data, function (i, state) {
                items += "<option value='" + state.Value + "'>" + state.Text + "</option>";
                // state.Value cannot contain ' character. We are OK because state.Value = cnt++;
            });
            $('#DeptID').html(items);
            $('#SecID').empty();
           //var URL = '/WMS/Emp/SectionList';
            var URL = '/Emp/SectionList';
            var convalue = $('#DeptID').val() + "s" + $('#CompanyID').val();
            $.getJSON(URL + '/' + convalue, function (data) {
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

});
function fileCheck(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#blah')
                .attr('src', e.target.result)
                .width(90)
                .height(90);
            document.getElementById("blah").style.marginTop = "20px";
            document.getElementById("blah").style.marginLeft = "30px";
        };

        reader.readAsDataURL(input.files[0]);
    }
}