$(document).ready(function () {
    if (document.getElementById('CarryForward').checked) {
        $("#CarryForwardDataDiv").show();
    } else {
        $("#CarryForwardDataDiv").hide();
    }

    $('#CarryForward').change(function () {
        if ($(this).is(":checked")) {
            $("#CarryForwardDataDiv").show();
        }
        else {
            $("#CarryForwardDataDiv").hide();
        }
    });

    if (document.getElementById('HalfLeave').checked) {
        $("#HalfLeaveDataDiv").show();
    } else {
        $("#HalfLeaveDataDiv").hide();
    }
    $('#HalfLeave').change(function () {
        if ($(this).is(":checked")) {
            $("#HalfLeaveDataDiv").show();
        }
        else {
            $("#HalfLeaveDataDiv").hide();
        }
    });


});