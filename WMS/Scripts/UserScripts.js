//$(document).ready(function () {

//    ////var URL = '/WMS/Emp/SectionList';
//    //var URL = '/Home/TestData';
//    //$.getJSON(URL, function (data) {
//    //    console.log(data);
//    //});


//});
$(document).ready(function () {
    $(window).load(function () {
        // this code will run after all other $(document).ready() scripts
        // have completely finished, AND all page elements are fully loaded.
    });
    var ss = $("#UserType option:selected").text();
   
    if (ss == 'Admin') {
        $("#ForUserOnly").hide();
    }
    if (ss == 'Restricted') {
        $("#ForUserOnly").show();
    }
    var uthidden = $("#UserTypeHidden").val();
    if (uthidden == 'Admin') {
        $("#ForUserOnly").hide();
    }
    if (uthidden == 'Restricted') {
        $("#UserType").val("false");
        $("#ForUserOnly").show();
    }
    //$("#UserType").val("false");
    $('#UserType').change(function () {
        var test = $(this).val();
        if (test == 'false') {
            $("#ForUserOnly").show();
        }
        if (test == 'true') {
            $("#ForUserOnly").hide();
        }
    });
});