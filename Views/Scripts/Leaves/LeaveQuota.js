//$(document).ready(function () {

//    ////var URL = '/WMS/Emp/SectionList';
//    //var URL = '/Home/TestData';
//    //$.getJSON(URL, function (data) {
//    //    console.log(data);
//    //});


//});
$(document).ready(function () {

    $("#ByEmpDiv").show();
    $("#ByEmpTypeDiv").hide();
    $("#ByCatDiv").hide();
    $("input[name$='CreateLeaveCriteria']").click(function () {
        var test = $(this).val();
        if (test == "ByEmployee")
        {
            $("div.desc").hide();
            $("#ByEmpDiv").show();
        }
        if (test == "ByEmpType") {
            $("div.desc").hide();
            $("#ByEmpTypeDiv").show();
        }
        if (test == "ByCategory") {
            $("div.desc").hide();
            $("#ByCatDiv").show();
        }
    });
    $('#JobCardType').change(function () {
        var test = $(this).val();
        if (test == '5') {
            $("#TimeIn").show();
        }
    });
});