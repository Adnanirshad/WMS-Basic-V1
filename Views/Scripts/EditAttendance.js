//$(document).ready(function () {

//    ////var URL = '/WMS/Emp/SectionList';
//    //var URL = '/Home/TestData';
//    //$.getJSON(URL, function (data) {
//    //    console.log(data);
//    //});


//});
$(document).ready(function () {

    $("#Option1").hide();
    $("#Option2").hide();
    $("#Option3").hide();
    $("#Option4").show();
    $("#doubleDutyDiv").hide();
    $("#BadliDiv").hide();
    $("input[name$='cars']").click(function () {
        var test = $(this).val();
        if (test == "shift") {
            $("div.desc").hide();
            $("#Option1").show();
        }
        if (test == "crew") {
            $("div.desc").hide();
            $("#Option2").show();
        }
        if (test == "section") {
            $("div.desc").hide();
            $("#Option3").show();
        }
        if (test == "employee") {
            $("div.desc").hide();
            $("#Option4").show();
        }
        if (test == "company") {
            $("div.desc").hide();
            $("#Option5").show();
        }
        if (test == "location") {
            $("div.desc").hide();
            $("#Option6").show();
        }
        var test = $('#JobCardType').val();
        if (test == '8') {
            $("#doubleDutyDiv").show();
        }
        if (test == '9') {
            $("#BadliDiv").show();
        }
    });
    $('#JobCardType').change(function () {
        var test = $(this).val();
        $("#doubleDutyDiv").hide();
        $("#BadliDiv").hide();
        if (test == '5') {
            $("#TimeIn").show();
        }
        if (test == '8') {
            $("#doubleDutyDiv").show();
        }
        if (test == '9') {
            $("#BadliDiv").show();
        }
    });

    $('#DesignationID').empty();
    var URL = '/WMS/EditAttendance/CompanyIDJobCardList';
    //var URL = '/EditAttendance/CompanyIDJobCardList';
    $.getJSON(URL + '/' + $('#CompanyIDJobCard').val(), function (data) {
        var items;
        $.each(data, function (i, state) {
                items += "<option value='" + state.Value + "'>" + state.Text + "</option>";
            // state.Value cannot contain ' character. We are OK because state.Value = cnt++;
        });
        $('#DesignationID').html(items);
    });


    $('#CompanyIDJobCard').change(function () {
        $('#DesignationID').empty();
        var URL = '/WMS/EditAttendance/CompanyIDJobCardList';
        //var URL = '/EditAttendance/CompanyIDJobCardList';
        $.getJSON(URL + '/' + $('#CompanyIDJobCard').val(), function (data) {
            var items;
            $.each(data, function (i, state) {
                    items += "<option value='" + state.Value + "'>" + state.Text + "</option>";
                // state.Value cannot contain ' character. We are OK because state.Value = cnt++;
            });
            $('#DesignationID').html(items);
        });
    });
});
