var LvValue;

$(document).ready(function () {

    LvValue = console.log($("#LvType").val());

    var URL = '/ESS/ESSLeave/LeaveBalance';
    $('#LvType').change(function () {
        LvValue = $("#LvType").val();
        $.getJSON(URL + '/' + LvValue, function (data) {
            if (data.value != -1) {
                $("#LeavesRemaining").text("You have " + data.value + " leaves remaining");
                $("#LeavesRemainingDiv").show();
            }
            else {
                $("#LeavesRemaining").text("You dont have leaves for this type. Contact your HR administrator");
                
                $("#LeavesRemainingDiv").show();
            }

        });
    });


});