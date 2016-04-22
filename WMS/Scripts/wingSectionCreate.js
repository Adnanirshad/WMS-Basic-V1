$(document).ready(function () {


        $('#SecID').empty();
        //var URL = '/WMS/Emp/SectionList';
       var URL = '/WMS/Emp/SectionList';
        var convalue = $('#DeptID').val();
        $.getJSON(URL + '/' + convalue, function (data) {
            var items;
            $.each(data, function (i, state) {
                items += "<option value='" + state.Value + "'>" + state.Text + "</option>";
                // state.Value cannot contain ' character. We are OK because state.Value = cnt++;
            });
            $('#SecID').html(items);
        });





    $('#DeptID').change(function () {
        $('#SecID').empty();
        //var URL = '/WMS/Emp/SectionList';
        var URL = '/WMS/Emp/SectionList';
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
