$(document).ready(function () {
    document.getElementById("ELName").innerHTML = "Name: No Selected Employee";
    document.getElementById("ELDesignation").innerHTML = "Designation: No Selected Employee";
    document.getElementById("ELSection").innerHTML = "Section: No Selected Employee";
    document.getElementById("ELType").innerHTML = "Type: No Selected Employee";
    document.getElementById("ELJoin").innerHTML = "Join Date: No Selected Employee";

    document.getElementById("ELBCasual").innerHTML = "Casual: 0";
    document.getElementById("ELBAnnual").innerHTML = "Annaul: 0";
    document.getElementById("ELBSick").innerHTML = "Sick: 0";
    document.getElementById("ELBCPL").innerHTML = "CPL: 0";
    $('#buttonId').click(function () {
        var EmpNo = document.getElementById("EmpNo").value;                
        var URL = '/LvApp/GetEmpInfo';
        $.getJSON(URL, { EmpNo: EmpNo }, function (data) {
            var values = data.split('@');
            document.getElementById("ELName").innerHTML = values[0];
            document.getElementById("ELDesignation").innerHTML = values[1];
            document.getElementById("ELSection").innerHTML = values[2];
            document.getElementById("ELType").innerHTML = values[3];
            document.getElementById("ELJoin").innerHTML = values[4];
        });
        var URL2 = '/LvApp/GetEmpLeaveBalance';
        $.getJSON(URL2, { EmpNo: EmpNo }, function (data) {
            var values = data.split('@');
            document.getElementById("ELBCasual").innerHTML = values[0];
            document.getElementById("ELBAnnual").innerHTML = values[1];
            document.getElementById("ELBSick").innerHTML = values[2];
            document.getElementById("ELBCPL").innerHTML = values[3];
        });
    });
});