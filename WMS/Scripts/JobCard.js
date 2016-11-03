$(document).ready(function () {

    $("#EmpNoDiv").show();
    $("#CrewDiv").hide();
    $("input[name$='CreateJCCriteria']").click(function () {
        var test = $(this).val();
        if (test == "ByEmployee") {
            $("#CrewDiv").hide();
            $("#EmpNoDiv").show();
        }
        if (test == "ByCrew") {
            $("#EmpNoDiv").hide();
            $("#CrewDiv").show();
        }
    });
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