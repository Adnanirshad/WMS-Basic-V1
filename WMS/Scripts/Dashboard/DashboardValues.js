$(document).ready(function () {

    var URL = '/Home/GetDahboard';
    //var URL = '/Emp/CrewList';
    $.getJSON(URL, function (data) {
            //alert(data.DateTime);
        $("#Date").text(data.DateTime);
        $("#RegEmps").text(data.TotalEmps);
        $("#PEmps").text(data.Present);
        $("#AEmps").text(data.Absent);
        $("#LEmps").text(data.Leaves);
        $("#LIEmps").text(data.LateIn);
        $("#LOEmps").text(data.LateOut);
        $("#EIEmps").text(data.EarlyIn);
        $("#EOEmps").text(data.EarlyOut);
        $("#OTEmps").text(data.OverTime);
        $("#SLEmps").text(data.ShortLeaves);
        $("#JCFieldTour").text(data.JCFieldTour);
        $("#JCTraining").text(data.JCTraining);
        $("#JCSeminar").text(data.JCSeminar);
        $("#JCOD").text(data.JCOD);
        $("#EWork").text(data.EWork);
        $("#AWork").text(data.AWork);
        $("#LWork").text(data.LWork);
            //document.getElementById("test").value = "Ahsin";
    });



});