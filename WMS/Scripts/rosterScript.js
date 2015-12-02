//$(document).ready(function () {

//    ////var URL = '/WMS/Emp/SectionList';
//    //var URL = '/Home/TestData';
//    //$.getJSON(URL, function (data) {
//    //    console.log(data);
//    //});


//});
$(document).ready(function () {
    $("#cyclesDiv").hide();

    var rosterDays = 0;
    var cycles = 1;
   
    $('#RosterType').change(function () {
        var rosterType = $(this).val();
        switch (rosterType) {
            case '1':
                cycles = 1;
                $("#cyclesDiv").hide();
                break;
            case '2':
                cycles = 1;
                $("#cyclesDiv").show();
                rosterDays = 7;
                break;
            case '3':
                cycles = 1;
                $("#cyclesDiv").hide();
                rosterDays = 15;
                break;
            case '4':
                cycles = 1;
                $("#cyclesDiv").hide();
                rosterDays = 30;
                break;
            case '5':
                cycles = 1;
                $("#cyclesDiv").show();
                rosterDays = 84;
                break;
        }
        $("#cycles").val(cycles);
        if (document.getElementById('dateStart').value) {
            refreshEndDate();
        }
    });

    $('#dateStart').change(function () {
        refreshEndDate();
    });

    $('#cycles').change(function () {
        refreshEndDate();
    });


    function refreshEndDate() {
        var tt = document.getElementById('dateStart').value;
        cycles = $("#cycles").val();

        var date = new Date(tt);
        var newdate = new Date(date);
        if(rosterDays != 30)
            newdate.setDate(newdate.getDate() + (parseInt(cycles) * rosterDays));
       
            
        var dd = newdate.getDate();
        var mm = newdate.getMonth() + 1;
        if (rosterDays == 30)
            mm += 1;
        var y = newdate.getFullYear();

        var someFormattedDate = mm + '/' + dd + '/' + y;
        document.getElementById('dateEnd').value = someFormattedDate;
        document.getElementById('dateEndHidden').value = document.getElementById('dateEnd').value;
    }



});