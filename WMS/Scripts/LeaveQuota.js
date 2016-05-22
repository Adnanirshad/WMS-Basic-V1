//$(document).ready(function () {

//    ////var URL = '/WMS/Emp/SectionList';
//    //var URL = '/Home/TestData';
//    //$.getJSON(URL, function (data) {
//    //    console.log(data);
//    //});


//});
//$(document).ready(function () {

//    $("#ByEmpDiv").show();
//    $("#ByEmpTypeDiv").hide();
//    $("input[name$='CreateLeaveCriteria']").click(function () {
//        var test = $(this).val();
//        if (test == "ByEmployee") {
//            $("#ByEmpTypeDiv").hide();
//            $("#ByEmpDiv").show();
//        }
//        if (test == "ByEmpType") {
//            $("#ByEmpDiv").hide();
//            $("#ByEmpTypeDiv").show();
//        }
//    });
//});

$('#byAll').change(function () {
    if ($(this).is(":checked")) {
        $("#ByEmpTypeDiv").hide();
        $("#ByEmpNoDiv").hide();
    }
});
$('#byEmp').change(function () {
    if ($(this).is(":checked")) {
        $("#ByEmpNoDiv").show();
        $("#ByEmpTypeDiv").hide();
    }
});
$('#byEmpType').change(function () {
    if ($(this).is(":checked")) {
        $("#ByEmpTypeDiv").show();
        $("#ByEmpNoDiv").hide();
    }
});
