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
        $("#ForUserOnly").hide();
    }
    if (ss == 'Restricted') {
        $("#ForUserOnly").show();
    }

    $('#UserType').change(function () {
        var test = $(this).val();
        if (test == '0') {
            $("#ForUserOnly").show();
        }
        if (test == '1') {
            $("#ForUserOnly").hide();
        }
    });
});