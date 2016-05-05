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