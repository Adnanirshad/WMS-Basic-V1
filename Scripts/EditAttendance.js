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
    });
    $('#JobCardType').change(function () {
        var test = $(this).val();
        if (test == '5') {
            $("#TimeIn").show();
        }
    });
});