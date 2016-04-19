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




    $('#buttonId').click(function () {
        var companyid = document.getElementById("CompanyIDForEmp").value;
        var empNo = document.getElementById("EmpNo").value;
        var URL = '/WMS/AttProcessors/GetEmpInfo';
        // var URL = '/Emp/GradeList';
        $.getJSON(URL + '/' + empNo + "w" + companyid, function (data) {
            $('#empDetails').css('display', 'inline');
            if (data == "NotFound")
                document.getElementById("EName").value = "Employee not found";
            else {
                var values = data.split('@');
                document.getElementById("EName").value = values[0];
                document.getElementById("EDesignation").value = values[1];
                document.getElementById("ESection").value = values[2];
            }
        });
    });
});