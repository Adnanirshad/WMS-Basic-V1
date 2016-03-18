//$(document).ready(function () {

//    ////var URL = '/WMS/Emp/SectionList';
//    //var URL = '/Home/TestData';
//    //$.getJSON(URL, function (data) {
//    //    console.log(data);
//    //});


//});
$(document).ready(function () {
    var ss = $("#UserType option:selected").text();
   
    if (ss == 'Admin') {
        $("#ForAdminOnly").show();
    }
    if (ss == 'Restricted') {
        $("#ForAdminOnly").hide();
    }

    $('#UserType').change(function () {
        var test = $(this).val();
        if (test == '0') {
            $("#ForAdminOnly").hide();
        }
        if (test == '1') {
            $("#ForAdminOnly").show();
        }
    });
});