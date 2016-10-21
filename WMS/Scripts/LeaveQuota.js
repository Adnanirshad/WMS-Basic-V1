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
//});EmpInformition

$("#ByEmpTypeDiv").hide();
$("#ByEmpNoDiv").hide();
$("#EmpInformition").hide();
$('#byAll').change(function () {
    if ($(this).is(":checked")) {
        $("#ByEmpTypeDiv").hide();
        $("#ByEmpNoDiv").hide();
        $("#EmpInformition").hide();
        $("#radioValue").val("byAll");
    }
});
$('#byEmp').change(function () {
    if ($(this).is(":checked")) {
        $("#ByEmpNoDiv").show();
        $("#EmpInformition").hide();
        $("#ByEmpTypeDiv").hide();
        $("#radioValue").val("byEmp");
        
    }
});
$('#byEmpType').change(function () {
    if ($(this).is(":checked")) {
        $("#ByEmpTypeDiv").show();
        $("#ByEmpNoDiv").hide();
        $("#EmpInformition").hide();
        $("#radioValue").val("byEmpType");
    }
});
$(document).ready(function () {

    document.getElementById("EName").innerHTML = "Name: No Selected Employee";
    document.getElementById("EFName").innerHTML = "FatherName: No Selected Employee";
    document.getElementById("EDesignation").innerHTML = "Designation: No Selected Employee";
    document.getElementById("ESection").innerHTML = "Section: No Selected Employee";
    $('#buttonId').click(function () {
        var empNo = document.getElementById("EmpNo").value;
        //var URL = '/WMS/LvApp/GetEmpInfo';
            var URL = '/Emp/GetEmployeeInfo';
            $.getJSON(URL, { EmpNo: empNo }, function (data) {
                var values = data.split('@');
                document.getElementById("EName").innerHTML = values[0];
                document.getElementById("EFName").innerHTML = values[1];
                document.getElementById("EDesignation").innerHTML = values[2];
                document.getElementById("ESection").innerHTML = values[3];

            });
    });
});