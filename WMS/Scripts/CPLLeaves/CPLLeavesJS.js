//Index page samna ata ha pora page
$(document).ready(function () {

    //#trigers id
    //it hides id cpl


   // $("#CPL").hide();
    
    //var URL = '/WMS/LeaveSettings/CPLList';
     var URL = '/LeaveSettings/CPLList';
    $.getJSON(URL , function (data) {
        if (data.foo == "Not Found") {
            $("#CPL").hide();
        }
        console.log(data.foo);
    });

    

});