$(document).ready(function () {
    $('#location').css('display', 'none');
    $('#employee').css('display', 'none');
    $('#empDetails').css('display', 'none');
    $('#CriteriaID').change(function () {
        switch ($("#CriteriaID option:selected").html()) {
            case 'Location': $('#location').css('display', 'inline');
                $('#category').css('display', 'none');
                $('#company').css('display', 'none');
                $('#CatProcess').css('display', 'inline');
                $('#CategoryID').css('display', 'inline');
                $('#employee').css('display', 'none');
                $('#empDetails').css('display', 'none');
                break;
            case 'Category': $('#location').css('display', 'none');
                $('#category').css('display', 'inline');
                $('#company').css('display', 'none');
                break;
            case 'Company': $('#location').css('display', 'none');
                $('#category').css('display', 'none');
                $('#company').css('display', 'inline');
                $('#CategoryID').css('display', 'inline');
                $('#CatProcess').css('display', 'inline');
                $('#employee').css('display', 'none');
                $('#empDetails').css('display', 'none');
                break;
            case 'Employee': $('#location').css('display', 'none');
                $('#CatProcess').css('display', 'none');
                $('#company').css('display', 'none');
                $('#CategoryID').css('display', 'none');
                $('#employee').css('display', 'inline');
                $('#empDetails').css('display', 'none');          
                break;               
        }
    });
    $('#ProcessCats').change(function () {
        switch ($("#ProcessCats option:selected").html()) {
            case 'Yes':
                $('#CategoryID').css('display', 'inline');
                break;
            case 'No': $('#CategoryID').css('display', 'none');
                break;
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
});
