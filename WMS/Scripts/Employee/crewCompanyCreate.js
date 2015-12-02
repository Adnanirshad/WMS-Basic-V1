$(document).ready(function () {

    $('#CrewID').empty();
    //var URL = '/WMS/Emp/CrewList';
   var URL = '/Emp/CrewList';
    $.getJSON(URL + '/' + $('#CompanyID').val(), function (data) {
        var items;
        $.each(data, function (i, state) {
            items += "<option value='" + state.Value + "'>" + state.Text + "</option>";
            // state.Value cannot contain ' character. We are OK because state.Value = cnt++;
        });
        $('#CrewID').html(items);
    });


    $('#CompanyID').change(function () {
        $('#CrewID').empty();
        //var URL = '/WMS/Emp/CrewList';
         var URL = '/Emp/CrewList';
        $.getJSON(URL + '/' + $('#CompanyID').val(), function (data) {
            var items;
            $.each(data, function (i, state) {
                items += "<option value='" + state.Value + "'>" + state.Text + "</option>";
                // state.Value cannot contain ' character. We are OK because state.Value = cnt++;
            });
            $('#CrewID').html(items);
        });
    });

});