app.controller('DashboardController', function ($scope, $http) {
    //for popluating the Date input with the current date
    var today = new Date();
    $scope.DateFrom = today;
    $scope.selectedRowForToFrom = 0;
    //array with the criteria names
    $scope.MultipleSelect = false;
    var datefrom = [];
    $scope.selectedRow = 5;  // initialize our variable to null
 
    $scope.setClickedRow = function (index) {  //function that sets the value of selectedRow to current index
        $scope.selectedRow = index;
        $scope.RenderDailyAttendance();
            
    }
    $scope.names = [
        'Daily Attendance','Daily In/Out Times','Daily Expected Mins'
    ];
    $scope.Criteria = {
        repeatSelect: null,
        availableOptions: [
          { id: 'C', name: 'Company' },
          { id: 'L', name: 'Location' },
          { id: 'D', name: 'Department' },
          { id: 'E', name: 'Section' },
          { id: 'S', name: 'Shift' },
          { id: 'A', name: 'Category' }
        ]
    };
    $scope.InitFunction = function ()
    {

        $http({ method: 'GET', url: '/Graph/GetInitialValues'}).
   then(function (response) {
       $scope.Criteria.availableOptions= response.data;
       $scope.Criteria.repeatSelect = null;
     
   }, function (response) {
       // called asynchronously if an error occurs
       // or server returns response with an error status.
   });


      
            $http({ method: 'GET', url: '/Graph/GetDatesValues'}).
       then(function (response) {
           
          
           datefrom = response.data;
           $scope.DateFrom = new Date(datefrom[0]);
           $scope.DateTo = new Date(datefrom[1]);
           if ($scope.MultipleSelect == true) {
               if (datefrom[0] == datefrom[1]) {
                   $scope.names = [
            'Strength', 'Work Times'
                   ];

               }
               else {

                   $scope.names = [
           'Work Loss/Work Done', 'Over time (Hours)/OverTime(Employees)', 'Late In (Hours)/Late In(Employees)', 'Late Out(Hours)/Late Out(Employees)', 'Present/Absent', 'Leave', 'Early In(Hours)/Early In(Employees)', 'Early Out(Hours)/Early Out(Employees)'
                   ];

               }

           }
           else {

               $scope.names = [
           'Strength', 'Work Times'
               ];

           }
           

       }, function (response) {
           // called asynchronously if an error occurs
           // or server returns response with an error status.
       });

    }
    //array with the values once the criteria is selected
    $scope.Value = {
        repeatSelect: null,
        availableOptions:[]
    
    };
    //This function calls to the database to give the values stored in it 
    //by making a SUmmaryDataCriteria in the frontend
    //Daily Summaries
    //Daily Attendance
    $scope.RenderDailyAttendance = function () {
        if ($scope.MultipleSelect == true)
        {
            var fromString = $scope.DateFrom.getFullYear() + "/" + $scope.DateFrom.getMonth() + "/" + $scope.DateFrom.getDate();
            var toString = $scope.DateTo.getFullYear() + "/" + $scope.DateTo.getMonth() + "/" + $scope.DateTo.getDate();
            if (fromString == toString) {
                var ids = [];
                $scope.Value.availableOptions.forEach(function (value) { ids.push(value.id); });
                $scope.GeneralCriteria = ('' + $scope.DateFrom.getFullYear()).slice(-2) + ('0' + ($scope.DateFrom.getMonth() + 1)).slice(-2) + ('0' + $scope.DateFrom.getDate()).slice(-2) + $scope.Criteria.repeatSelect;
                $http({ method: 'POST', url: '/Graph/GetGraphValuesForMultipleSelect', data: JSON.stringify({ GeneralCriteria: $scope.GeneralCriteria, Ids: ids }) }).
        then(function (response) {
            $scope.GraphData = response.data;
            ChangeToStackedColumnGraph($scope.GraphData);

        }, function (response) { });
            }
            else {
                 var ids = [];
                $scope.Value.availableOptions.forEach(function (value) { ids.push(value.id); });
                $scope.GeneralCriteria =$scope.Criteria.repeatSelect;
                $http({ method: 'POST', url: '/Graph/GetGraphValuesForMultipleSelectAndDifferentDate', data: JSON.stringify({ GeneralCriteria: $scope.GeneralCriteria, Ids: ids,datefrom:$scope.DateFrom,dateto:$scope.DateTo }) }).
        then(function (response) {
            $scope.GraphData = response.data;
            ChangeToNormalLineGraph($scope.GraphData);

        }, function (response) { });


            }




        }
        else
        {
            
            var fromString = $scope.DateFrom.getFullYear() + "/" + $scope.DateFrom.getMonth() + "/" + $scope.DateFrom.getDate();
            var toString = $scope.DateTo.getFullYear() + "/" + $scope.DateTo.getMonth() + "/" + $scope.DateTo.getDate();
          
            if (fromString == toString) {
                $scope.finalCriteriaForDB = ('' + $scope.DateFrom.getFullYear()).slice(-2) + ('0' + ($scope.DateFrom.getMonth() + 1)).slice(-2) + ('0' + $scope.DateFrom.getDate()).slice(-2) + $scope.Criteria.repeatSelect + $scope.Value.repeatSelect;
                $http({ method: 'POST', url: '/Graph/GetGraphValues', data: JSON.stringify({ CriteriaValue: $scope.finalCriteriaForDB }) }).
          then(function (response) {
              $scope.GraphData = response.data;
              ChangeToPieGraph($scope.GraphData);
          }, function (response) { });

            }
            else {
                $scope.finalCriteriaForDB = $scope.Criteria.repeatSelect + $scope.Value.repeatSelect;
                $http({ method: 'POST', url: '/Graph/GetGraphValuesForDifferentDatesSS', data: JSON.stringify({ CriteriaValue: $scope.finalCriteriaForDB,datefrom:$scope.DateFrom,dateto:$scope.DateTo }) }).
         then(function (response) {
             $scope.GraphData = response.data;
            
             ChangeToStackedColumnGraph($scope.GraphData);
         }, function (response) { });

            }

            }
  

    };
    var ChangeToNormalLineGraph = function (graphdata)
    {
        var textOne = "";
        var textTwo = "";
        var bestWorkDone = "";
        var ChartData = [];
        var xaxis = [];
        var ChartData1 = [];
        for (var d = angular.copy($scope.DateFrom) ; d <= $scope.DateTo; d.setDate(d.getDate() + 1)) {
            var date = ('' + d.getFullYear()).slice(-2) + "-" + ('0' + (d.getMonth() + 1)).slice(-2) + "-" + ('0' + d.getDate()).slice(-2)
            xaxis.push(date);
        }
        switch ($scope.selectedRow) {
            case 0: 
                textOne = "Work Done";
                textTwo = "Work Loss";
                var ids = [];
                $scope.Value.availableOptions.forEach(function (value) { ids.push(value.name); });
                for (var id in ids) {
                    {
                        var tempcontainer = [];
                        var tempcontainer1 = [];
                        for (var key2 in graphdata)
                        {
                            if (graphdata[key2] != null && graphdata[key2].CriteriaName == ids[id]) {
                                {
                                    tempcontainer.push(parseFloat((graphdata[key2].ActualWorkMins/60).toPrecision(8)));
                                    tempcontainer1.push(parseFloat((graphdata[key2].LossWorkMins / 60).toPrecision(8)));
                                }

                            }
                        }
                        ChartData1.push({name:ids[id],data:tempcontainer1});
                        ChartData.push({ name: ids[id], data: tempcontainer });
                    
                    }
                }
               
                break;
            case 1: textOne = "Over time (Hours)";
                textTwo = "OverTime(Employees)";
                var ids = [];
                $scope.Value.availableOptions.forEach(function (value) { ids.push(value.name); });
                for (var id in ids) {
                    {
                        var tempcontainer = [];
                        var tempcontainer1 = [];
                        for (var key2 in graphdata)
                        {
                            if (graphdata[key2] != null && graphdata[key2].CriteriaName == ids[id]) {
                                {
                                    tempcontainer.push(parseFloat((graphdata[key2].OTMins/60).toPrecision(8)));
                                    tempcontainer1.push(parseFloat((graphdata[key2].OTEmps).toPrecision(8)));
                                }

                            }
                        }
                        ChartData1.push({name:ids[id],data:tempcontainer1});
                        ChartData.push({ name: ids[id], data: tempcontainer });
                    
                    }
                }

                break;
            case 2: textOne = "Late In (Hours)";
                textTwo = "Late In(Employees)";
                var ids = [];
                $scope.Value.availableOptions.forEach(function (value) { ids.push(value.name); });
                for (var id in ids) {
                    {
                        var tempcontainer = [];
                        var tempcontainer1 = [];
                        for (var key2 in graphdata) {
                            if (graphdata[key2] != null && graphdata[key2].CriteriaName == ids[id]) {
                                {
                                    tempcontainer.push(parseFloat((graphdata[key2].LIMins / 60).toPrecision(8)));
                                    tempcontainer1.push(parseFloat((graphdata[key2].LIEmps).toPrecision(8)));
                                }

                            }
                        }
                        ChartData1.push({ name: ids[id], data: tempcontainer1 });
                        ChartData.push({ name: ids[id], data: tempcontainer });

                    }
                }

                break;
            case 3: textOne = "Late Out(Hours)";
                textTwo = "Late Out(Employees)";
                var ids = [];
                $scope.Value.availableOptions.forEach(function (value) { ids.push(value.name); });
                for (var id in ids) {
                    {
                        var tempcontainer = [];
                        var tempcontainer1 = [];
                        for (var key2 in graphdata) {
                            if (graphdata[key2] != null && graphdata[key2].CriteriaName == ids[id]) {
                                {
                                    tempcontainer.push(parseFloat((graphdata[key2].LOMins / 60).toPrecision(8)));
                                    tempcontainer1.push(parseFloat((graphdata[key2].LOEmps).toPrecision(8)));
                                }

                            }
                        }
                        ChartData1.push({ name: ids[id], data: tempcontainer1 });
                        ChartData.push({ name: ids[id], data: tempcontainer });

                    }
                }

                break;
            case 4: textOne = "Present";
                textTwo = "Absent";
                var ids = [];
                $scope.Value.availableOptions.forEach(function (value) { ids.push(value.name); });
                for (var id in ids) {
                    {
                        var tempcontainer = [];
                        var tempcontainer1 = [];
                        for (var key2 in graphdata) {
                            if (graphdata[key2] != null && graphdata[key2].CriteriaName == ids[id]) {
                                {
                                    tempcontainer.push(parseFloat((graphdata[key2].PresentEmps).toPrecision(8)));
                                    tempcontainer1.push(parseFloat((graphdata[key2].AbsentEmps).toPrecision(8)));
                                }

                            }
                        }
                        ChartData1.push({ name: ids[id], data: tempcontainer1 });
                        ChartData.push({ name: ids[id], data: tempcontainer });

                    }
                }

                break;
            case 5: textOne = "Leave";
                textTwo = "Present";
                var ids = [];
                $scope.Value.availableOptions.forEach(function (value) { ids.push(value.name); });
                for (var id in ids) {
                    {
                        var tempcontainer = [];
                        var tempcontainer1 = [];
                        for (var key2 in graphdata) {
                            if (graphdata[key2] != null && graphdata[key2].CriteriaName == ids[id]) {
                                {
                                    tempcontainer.push(parseFloat(((parseFloat(data[key].LvEmps) + parseFloat(data[key].ShortLvEmps) + parseFloat(data[key].HalfLvEmps)) / parseFloat(data[key].TotalEmps)) * 100).toPrecision(4));
                                    tempcontainer1.push(parseFloat((graphdata[key2].PresentEmps).toPrecision(8)));
                                }

                            }
                        }
                        ChartData1.push({ name: ids[id], data: tempcontainer1 });
                        ChartData.push({ name: ids[id], data: tempcontainer });

                    }
                }

                break;
            case 6: textOne = "Early In(Hours)";
                textTwo = "Early In(Employees)";
                var ids = [];
                $scope.Value.availableOptions.forEach(function (value) { ids.push(value.name); });
                for (var id in ids) {
                    {
                        var tempcontainer = [];
                        var tempcontainer1 = [];
                        for (var key2 in graphdata) {
                            if (graphdata[key2] != null && graphdata[key2].CriteriaName == ids[id]) {
                                {
                                    tempcontainer.push(parseFloat((graphdata[key2].EIMins/60).toPrecision(8)));
                                    tempcontainer1.push(parseFloat((graphdata[key2].EIEmps).toPrecision(8)));
                                }

                            }
                        }
                        ChartData1.push({ name: ids[id], data: tempcontainer1 });
                        ChartData.push({ name: ids[id], data: tempcontainer });

                    }
                }

                break;
            case 7: textOne = "Early Out(Hours)";
                textTwo = "Early Out(Employees)";
                var ids = [];
                $scope.Value.availableOptions.forEach(function (value) { ids.push(value.name); });
                for (var id in ids) {
                    {
                        var tempcontainer = [];
                        var tempcontainer1 = [];
                        for (var key2 in graphdata) {
                            if (graphdata[key2] != null && graphdata[key2].CriteriaName == ids[id]) {
                                {
                                    tempcontainer.push(parseFloat((graphdata[key2].EOMins / 60).toPrecision(8)));
                                    tempcontainer1.push(parseFloat((graphdata[key2].EOEmps).toPrecision(8)));
                                }

                            }
                        }
                        ChartData1.push({ name: ids[id], data: tempcontainer1 });
                        ChartData.push({ name: ids[id], data: tempcontainer });

                    }
                }

                break;
                // $scope.names = [
              //  'Work Loss/Work Done', 'Over time (Hours)/OverTime(Employees)', 'Late In (Hours)/Late In(Employees)', 'Late Out(Hours)/Late Out(Employees)', 'Present/Absent', 'Leave/Present', 'Early In(Hours)/Early In(Employees)', 'Early Out(Hours)/Early Out(Employees)'
         //   ];
               
        }
        
       
        $scope.highchartsBG = {
            options: {
                chart: {
                    type: 'line'
                },
                colors: {
                    radialGradient: {
                        cx: 0.5,
                        cy: 0.3,
                        r: 0.7
                    }
                }
            },
            credits: {
                enabled: false
            },
            tooltip: {
                pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
            },
            plotOptions: {
               
            },
                xAxis: {
            categories: xaxis,
            title: {
                        text: null
            }
                },
            dataLabels: {
                style: {
                    textShadow: ''
                }
            },
            series: ChartData,
            title: {
                text: textOne
            },
            //once we have fetched the info from the database we will do loading :false
            loading: false
        }

        $scope.highchartsNG = {
            options: {
                chart: {
                    type: 'line'
                }
            },
            credits: {
                enabled: false
            },
            tooltip: {
                pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
            },
            plotOptions: {
                pie: {
                    borderWidhth: 0,
                    cursor: 'pointer',

                    dataLabels: {
                        enabled: true
                    },
                    showInLegend: true
                }
            },

                xAxis: {
            categories: xaxis,
            title: {
                        text: null
            }
                },
            dataLabels: {
                style: {
                    textShadow: ''
                }
            },
            series: ChartData1,
            title: {
                text: textTwo
            },
            //once we have fetched the info from the database we will do loading :false
            loading: false
        }
        switch ($scope.selectedRow) {
            case 0: $scope.highchartsNG.series[0].name = "Employees";
                $scope.highchartsBG.series[0].name = "Time In/Out";
                var chart = angular.element(document.getElementById('chart1')).highcharts();
                chart.setTitle(null, { text: (((graphdata.PresentEmps) / (graphdata.PresentEmps + graphdata.AbsentEmps + graphdata.LvEmps + graphdata.ShortLvEmps + graphdata.HalfLvEmps + graphdata.DayOffEmps)) * 100).toPrecision(4) + "% employees present" });
                var chart = angular.element(document.getElementById('HighchartforSameDate')).highcharts();
                chart.setTitle(null, { text: (((graphdata.EIEmps + graphdata.LOEmps) / (graphdata.EIEmps + graphdata.LOEmps + graphdata.EOEmps + graphdata.LIEmps)) * 100).toPrecision(4) + "% employees did overtime" });
                break;
            case 1: $scope.highchartsNG.series[0].name = "Employees";
                var chart = angular.element(document.getElementById('chart1')).highcharts();
                chart.setTitle(null, { text: (((graphdata.EIEmps + graphdata.LOEmps) / (graphdata.EIEmps + graphdata.LOEmps + graphdata.EOEmps + graphdata.LIEmps)) * 100).toPrecision(4) + "% employees did overtime" });
                var chart = angular.element(document.getElementById('HighchartforSameDate')).highcharts();
                chart.setTitle(null, { text: ((graphdata.ActualWorkMins / graphdata.ExpectedWorkMins) * 100).toPrecision(4) + "% minutes were productively utilized" });


                break;


        }


    }
    $scope.$watch('DateFrom', function () { checkForMultipleSelect(); }, true);
    $scope.$watch('DateTo', function () { checkForMultipleSelect(); }, true);
    var checkForMultipleSelect = function ()
    {
        var fromString = $scope.DateFrom.getFullYear() + "/" + $scope.DateFrom.getMonth() + "/" + $scope.DateFrom.getDate();
        var toString = $scope.DateTo.getFullYear() + "/" + $scope.DateTo.getMonth() + "/" + $scope.DateTo.getDate();

        if ($scope.MultipleSelect == true) {
            if (fromString == toString) {
                $scope.names = [
         'Strength', 'Work Times'
                ];

            }
            else {

                $scope.names = [
       'Work Loss/Work Done', 'Over time (Hours)/OverTime(Employees)', 'Late In (Hours)/Late In(Employees)', 'Late Out(Hours)/Late Out(Employees)', 'Present/Absent', 'Leave', 'Early In(Hours)/Early In(Employees)', 'Early Out(Hours)/Early Out(Employees)'
                ];


            }

        }
        else {

            $scope.names = [
        'Strength', 'Work Times'
            ];

        }
        $('#slimTable').slimScroll({
            height: '200px',
            width: '180px'

        });
    }
    $scope.$watch('MultipleSelect', function () {
        checkForMultipleSelect();

    }, true);
    $scope.$watch('Criteria', function () {
        var GraphClass = { Criteria: $scope.Criteria.repeatSelect };
        $http({ method: 'POST', url: '/Graph/GetCriteriaNames', data: JSON.stringify(GraphClass) }).
   then(function (response) {
       $scope.Value.availableOptions = response.data;
   }, function (response) {
       
   });
    },true);
    $scope.GetBestCriteria = function ()
    {
        $http({ method: 'POST', url: '/Graph/GetBestCriteria', data: JSON.stringify({ CriteriaValue: $scope.Criteria.repeatSelect }) }).
  then(function (response) {
      console.log(response.data);
      ChangeToColumnGraph(response.data);

  }, function (response) {
      // called asynchronously if an error occurs
      // or server returns response with an error status.
  });
        //

    }
    var ChangeToPieGraph = function (graphdata)
    {
        var textOne = "";
        var textTwo = "";
        var ChartData = [];
        var ChartData1 = [];
        switch ($scope.selectedRow) {
            case 0: 
                var chart = angular.element(document.getElementById('chart1')).highcharts();
                chart.setTitle(null, { text: (((graphdata.PresentEmps) / (graphdata.PresentEmps + graphdata.AbsentEmps + graphdata.LvEmps + graphdata.ShortLvEmps + graphdata.HalfLvEmps + graphdata.DayOffEmps)) * 100).toPrecision(4) + "% employees present" });
                var chart = angular.element(document.getElementById('HighchartforSameDate')).highcharts();
                chart.setTitle(null, { text: (((graphdata.EIEmps + graphdata.LOEmps) / (graphdata.EIEmps + graphdata.LOEmps + graphdata.EOEmps + graphdata.LIEmps)) * 100).toPrecision(4) + "% employees did overtime" });
               
                break;
            case 1: 
                var chart = angular.element(document.getElementById('chart1')).highcharts();
                chart.setTitle(null, { text: (((graphdata.EIEmps + graphdata.LOEmps) / (graphdata.EIEmps + graphdata.LOEmps + graphdata.EOEmps + graphdata.LIEmps)) * 100).toPrecision(4) + "% employees did overtime" });
                var chart = angular.element(document.getElementById('HighchartforSameDate')).highcharts();
                chart.setTitle(null, { text: ((graphdata.ActualWorkMins / graphdata.ExpectedWorkMins) * 100).toPrecision(4) + "% minutes were productively utilized" });


                break;


        }
        switch ($scope.selectedRow)
        {
            case 0: ChartData.push({ "name": "Absent Employees", "y": parseFloat(graphdata.AbsentEmps) });
                ChartData.push({ "name": "Present Employees", "y": parseFloat(graphdata.PresentEmps) });
                ChartData.push({ "name": "On Leave", "y": (parseFloat(graphdata.LvEmps) + parseFloat(graphdata.ShortLvEmps) + parseFloat(graphdata.HalfLvEmps)) });
                ChartData.push({ "name": "Day Off", "y": parseFloat(graphdata.DayOffEmps) });
                ChartData1.push({ "name": "Early In", "y": parseFloat(graphdata.EIEmps) });
                ChartData1.push({ "name": "Early Out", "y": parseFloat(graphdata.EOEmps) });
                ChartData1.push({ "name": "Late In", "y": parseFloat(graphdata.LIEmps) });
                ChartData1.push({ "name": "Late Out", "y": parseFloat(graphdata.LOEmps) });
                textOne = "Early/Late Comers (Employees)";
                textTwo = "Daily Attendance";

                break;
            case 1: ChartData.push({ "name": "Expected Work Hours", "y": parseFloat((graphdata.ExpectedWorkMins / 60).toPrecision(4)) });
            ChartData.push({ "name": "Actual Work Hours", "y": parseFloat((graphdata.ActualWorkMins / 60).toPrecision(4)) });
            ChartData.push({ "name": "Loss Work Hours", "y": parseFloat((graphdata.LossWorkMins / 60).toPrecision(4) )});
                ChartData1.push({ "name": "Early In", "y": parseFloat((graphdata.EIMins / 60).toPrecision(4))});
                ChartData1.push({ "name": "Early Out", "y": parseFloat((graphdata.EOMins / 60).toPrecision(4))});
                ChartData1.push({ "name": "Late In", "y": parseFloat((graphdata.LIMins / 60).toPrecision(4))});
                ChartData1.push({ "name": "Late Out", "y": parseFloat((graphdata.LOMins / 60).toPrecision(4)) });
                textOne = "Early/Late Hours";
                textTwo = "Work Hours";
               
                break;
           

        }
       
        
        $scope.highchartsBG = {
            options: {
                chart: {
                    type: 'pie'
                },
                colors: {
                    radialGradient: {
                        cx: 0.5,
                        cy: 0.3,
                        r: 0.7
                    }
                }
            },
            credits: {
                enabled: false
            },
            tooltip: {
                pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
            },
            plotOptions: {
                pie: {
                    borderWidhth: 0,
                    cursor: 'pointer',

                    dataLabels: {
                        enabled: true
                    },
                    showInLegend: true
                }
            },
            dataLabels: {
                style: {
                    textShadow: ''
                }
            },
            series: [{

                colorByPoint: true,
                data: ChartData1
            }],
            title: {
                text: textOne
            },
            //once we have fetched the info from the database we will do loading :false
            loading: false
        }

        $scope.highchartsNG = {
            options: {
                chart: {
                    type: 'pie'
                }
            },
            credits: {
                enabled: false
            },
            tooltip: {
                pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
            },
            plotOptions: {
                pie: {
                    borderWidhth:0,
                    cursor: 'pointer',
                    
                    dataLabels: {
                        enabled: true
                    },
                    showInLegend: true
                }
            },
            dataLabels: {
                style: {
                    textShadow: ''
                }
            },
            series: [{
                
                colorByPoint: true,
                data:ChartData}],
            title: {
                text: textTwo
            },
            //once we have fetched the info from the database we will do loading :false
            loading: false
        }
        switch ($scope.selectedRow) {
            case 0: $scope.highchartsNG.series[0].name = "Employees";
                $scope.highchartsBG.series[0].name = "Time In/Out";
                var chart = angular.element(document.getElementById('chart1')).highcharts();
                chart.setTitle(null, { text: (((graphdata.PresentEmps) / (graphdata.PresentEmps + graphdata.AbsentEmps + graphdata.LvEmps + graphdata.ShortLvEmps + graphdata.HalfLvEmps + graphdata.DayOffEmps)) * 100).toPrecision(4) + "% employees present" });
                var chart = angular.element(document.getElementById('HighchartforSameDate')).highcharts();
                chart.setTitle(null, { text: (((graphdata.EIEmps + graphdata.LOEmps) / (graphdata.EIEmps + graphdata.LOEmps + graphdata.EOEmps + graphdata.LIEmps)) * 100).toPrecision(4) + "% employees did overtime" });
                break;
            case 1: $scope.highchartsNG.series[0].name = "Employees";
                var chart = angular.element(document.getElementById('chart1')).highcharts();
                chart.setTitle(null, { text: (((graphdata.EIEmps + graphdata.LOEmps) / (graphdata.EIEmps + graphdata.LOEmps + graphdata.EOEmps + graphdata.LIEmps)) * 100).toPrecision(4) + "% employees did overtime" });
                var chart = angular.element(document.getElementById('HighchartforSameDate')).highcharts();
                chart.setTitle(null, { text: ((graphdata.ActualWorkMins / graphdata.ExpectedWorkMins) * 100).toPrecision(4) + "% minutes were productively utilized" });


                break;


        }
       
    }
    var ChangeToStackedColumnGraph = function (data)
    {
        
        var bestCriteriaName = "";
        var EIEmps = [];
        var EOEmps = [];
        var LIEmps = [];
        var LOEmps = [];
        var AbsentEmployees = [];
        var ActualWorkMins = [];
        var ExpectedWorkMins = [];
        var LossWorkMins = [];
        var PresentEmployees=[];
        var OnLeave=[];
        var DayOff = [];
        var xaxis = [];
        var Series = [];
        var Series1 = [];
        var dynamictype = 'bar';
        var xaxis1 = [];
        var textOne = "";
        var yAxis = "Percentages";
        var textTwo = "";
        for (var key in data) {
            if (data[key] != null) {
                var fromString = $scope.DateFrom.getFullYear() + "/" + $scope.DateFrom.getMonth() + "/" + $scope.DateFrom.getDate();
                var toString = $scope.DateTo.getFullYear() + "/" + $scope.DateTo.getMonth() + "/" + $scope.DateTo.getDate();

                if ($scope.MultipleSelect == false && fromString != toString)
                {
                    for (var d = angular.copy($scope.DateFrom) ; d <= $scope.DateTo; d.setDate(d.getDate() + 1)) {
                        {
                            var date = ('' + d.getFullYear()).slice(-2) + "-" + ('0' + (d.getMonth() + 1)).slice(-2) + "-" + ('0' + d.getDate()).slice(-2)
                            xaxis.push(date);
                        } 
                    }
                    dynamictype = 'area';
                }
                else
                xaxis.push(data[key].CriteriaName);
                switch ($scope.selectedRow)
                {
                    case 0: AbsentEmployees.push(parseFloat((parseFloat(data[key].AbsentEmps) / parseFloat(data[key].TotalEmps)) * 100).toPrecision(4));
                            PresentEmployees.push(parseFloat((parseFloat(data[key].PresentEmps) / parseFloat(data[key].TotalEmps)) * 100).toPrecision(4));
                            if (data[key].LvEmps != "0" || data[key].ShortLvEmps != "0" || data[key].HalfLvEmps != "0")
                               OnLeave.push(parseFloat(((parseFloat(data[key].LvEmps) + parseFloat(data[key].ShortLvEmps) + parseFloat(data[key].HalfLvEmps)) / parseFloat(data[key].TotalEmps)) * 100).toPrecision(4));
                            else
                               OnLeave.push(0);
                            DayOff.push(parseFloat((parseFloat(data[key].DayOffEmps) / parseFloat(data[key].TotalEmps)) * 100).toPrecision(4));
                            for (var i = 0; i < DayOff.length; i++)
                                DayOff[i] = parseInt(DayOff[i], 10);
                            var bestCompany = parseInt(PresentEmployees[0], 10);
                            
                            for (var i = 0; i < PresentEmployees.length; i++) {
                                PresentEmployees[i] = parseInt(PresentEmployees[i], 10);
                                if (bestCompany < PresentEmployees[i]) {
                                    bestCompany = PresentEmployees[i];
                                    bestCriteriaName = xaxis[i];

                                }
                            }
                            
                           

                            for (var i = 0; i < AbsentEmployees.length; i++)
                                AbsentEmployees[i] = parseInt(AbsentEmployees[i], 10);

                            for (var i = 0; i < OnLeave.length; i++)
                                OnLeave[i] = parseInt(OnLeave[i], 10);
                            EIEmps.push(parseFloat((parseFloat(data[key].EIEmps) / parseFloat(data[key].TotalEmps)) * 100).toPrecision(4));
                            EOEmps.push(parseFloat((parseFloat(data[key].EOEmps) / parseFloat(data[key].TotalEmps)) * 100).toPrecision(4));
                            LIEmps.push(parseFloat((parseFloat(data[key].LIEmps) / parseFloat(data[key].TotalEmps)) * 100).toPrecision(4));
                            LOEmps.push(parseFloat((parseFloat(data[key].LOEmps) / parseFloat(data[key].TotalEmps)) * 100).toPrecision(4));
                            for (var i = 0; i < EIEmps.length; i++)
                                EIEmps[i] = parseInt(EIEmps[i], 10);
                            for (var i = 0; i < EOEmps.length; i++)
                                EOEmps[i] = parseInt(EOEmps[i], 10);
                            for (var i = 0; i < LIEmps.length; i++)
                                LIEmps[i] = parseInt(LIEmps[i], 10);
                            for (var i = 0; i < LOEmps.length; i++)
                                LOEmps[i] = parseInt(LOEmps[i], 10);
                            textOne = "Daily Summary In Percentage";
                            textTwo = "Early/Late Employees In Percentage";
                            break;

                    
                    case 1:  ActualWorkMins.push(parseFloat((data[key].ActualWorkMins/60).toPrecision(4)));
                        ExpectedWorkMins.push(parseFloat((data[key].ExpectedWorkMins/60).toPrecision(4)));
                        yAxis = "Hours";
                        LossWorkMins.push(parseFloat((data[key].LossWorkMins/60).toPrecision(4)));  
                        for (var i = 0; i < ActualWorkMins.length; i++)
                            ActualWorkMins[i] = parseInt(ActualWorkMins[i], 10);
                        for (var i = 0; i < ExpectedWorkMins.length; i++)
                            ExpectedWorkMins[i] = parseInt(ExpectedWorkMins[i], 10);
                        for (var i = 0; i < LossWorkMins.length; i++)
                            LossWorkMins[i] = parseInt(LossWorkMins[i], 10);
                        console.log(LossWorkMins);
                        EIEmps.push(parseFloat((parseFloat(data[key].EIMins) /60).toPrecision(4)));
                        EOEmps.push(parseFloat((parseFloat(data[key].EOMins) /60 ).toPrecision(4)));
                        LIEmps.push(parseFloat((parseFloat(data[key].LIMins) / 60).toPrecision(4)));
                        LOEmps.push(parseFloat((parseFloat(data[key].LOMins) /60 ) .toPrecision(4)));
                        for (var i = 0; i < EIEmps.length; i++)
                            EIEmps[i] = parseInt(EIEmps[i], 10);
                        for (var i = 0; i < EOEmps.length; i++)
                            EOEmps[i] = parseInt(EOEmps[i], 10);
                        for (var i = 0; i < LIEmps.length; i++)
                            LIEmps[i] = parseInt(LIEmps[i], 10);
                        for (var i = 0; i < LOEmps.length; i++)
                            LOEmps[i] = parseInt(LOEmps[i], 10);
                        textOne = "Work Hours";
                        textTwo = "Early/Late Employees In Hours";
                        break;
                        
                      

                }
               
              
            }
        }
        //switch case for series this was not added in the previous switch case because that switch case
        // is surrounded by for loop
        switch ($scope.selectedRow)
        {
            case 0: Series.push({ name: "Absent Employees", data: AbsentEmployees });
                Series.push({ name: "Present Employees", data: PresentEmployees });
                Series.push({ name: "On Leave", data: OnLeave });
                Series.push({ name: "Day Off", data: DayOff });
                Series1.push({ name: "Early In", data: EIEmps });
                Series1.push({ name: "Early Out", data: EOEmps });
                Series1.push({ name: "Late In", data: LIEmps });
                Series1.push({ name: "Late Out", data: LOEmps });
                break;
           
            case 1: 
                Series.push({ name: "Actual Work Hours", data: ActualWorkMins });
                Series.push({ name: "Expected Work Hours", data: ExpectedWorkMins });
                Series.push({ name: "Loss Work Hours", data: LossWorkMins });
                Series1.push({ name: "Early In Hours", data: EIEmps });
                Series1.push({ name: "Early Out Hours", data: EOEmps });
                Series1.push({ name: "Late In Hours", data: LIEmps });
                Series1.push({ name: "Late Out Hours", data: LOEmps });
                break;
               

        }
        switch ($scope.selectedRow) {
            case 0: var chart = angular.element(document.getElementById('chart1')).highcharts();
                chart.setTitle(null, { text: "Most Present Employees In " + bestCriteriaName });

                break;
            case 1: var chart = angular.element(document.getElementById('chart1')).highcharts();
                chart.setTitle(null, { text: "" });

                break;
            case 2: var chart = angular.element(document.getElementById('chart1')).highcharts();
                chart.setTitle(null, { text: "" });

                break;

        }
        $scope.highchartsNG = {
            options: {
                chart: {
                    type: dynamictype
                }
            }, plotOptions : {
                series: {
                    stacking: "normal"
                }
            },
            title: {
                text: textOne
            },
            xAxis: {
                categories: xaxis,
                title: {
                    text: null
                }
            },
            yAxis: {
                min: 0,
                title: {
                    text: yAxis,
                    align: 'high'
                },
                labels: {
                    overflow: 'justify'
                }
            },
            tooltip: {
                valueSuffix: ' percent'
            },
           
            series:Series,
            legend: {
                layout: 'vertical',
                align: 'right',
                verticalAlign: 'top',
                x: -40,
                y: 80,
                floating: true,
                borderWidth: 1,
                backgroundColor: ((Highcharts.theme && Highcharts.theme.legendBackgroundColor) || '#FFFFFF'),
                shadow: true
            },
            credits: {
                enabled: false
            }

        };
        
        $scope.highchartsBG = {
            options: {
                chart: {
                    type: dynamictype
                }
            },
            title: {
                text: textTwo
            },
            xAxis: {
                categories: xaxis,
                title: {
                    text: null
                }
            },
            yAxis: {
                min: 0,
                title: {
                    text: yAxis,
                    align: 'high'
                },
                labels: {
                    overflow: 'justify'
                }
            },
            tooltip: {
                valueSuffix: ' percent'
            },
            plotOptions : {
                series: {
                    stacking: "normal"
                }
            },
            series: Series1,
            legend: {
                layout: 'vertical',
                align: 'right',
                verticalAlign: 'top',
                x: -40,
                y: 80,
                floating: true,
                borderWidth: 1,
                backgroundColor: ((Highcharts.theme && Highcharts.theme.legendBackgroundColor) || '#FFFFFF'),
                shadow: true
            },
            credits: {
                enabled: false
            }

        };
        var fromString = $scope.DateFrom.getFullYear() + "/" + $scope.DateFrom.getMonth() + "/" + $scope.DateFrom.getDate();
        var toString = $scope.DateTo.getFullYear() + "/" + $scope.DateTo.getMonth() + "/" + $scope.DateTo.getDate();

        if ($scope.MultipleSelect == false && fromString != toString) {
            $scope.highchartsNG.plotOptions = {
                "series": {
                    "stacking": "normal"
                }
            };
        
        $scope.highchartsBG.plotOptions = {
            "series": {
                "stacking": "normal"
            }
        };
    

        }
        else {
            $scope.highchartsNG.plotOptions = {

                bar: {
                    dataLabels: {
                        enabled: true
                    }
                }
            };
            $scope.highchartsBG.plotOptions = {

                bar: {
                    dataLabels: {
                        enabled: true
                    }
                }
            };

        }
      
       

    }
    var ChangeToColumnGraph = function (data)
    {
       
        var letspopulatedata = [];
        for (var key in data)
        {
            
            letspopulatedata.push({ "name": data[key].CriteriaName, "y":parseFloat((( data[key].ActualWorkMins / data[key].ExpectedWorkMins)*100).toPrecision(4)) });
        }
        console.log(letspopulatedata);
        $scope.highchartsNG = {
            options: {
                chart: {
                    type: 'column'
                }
            },
            dataLabels: {
                style: {
                    textShadow: ''
                }
            },
            title: {
                text: 'Comparision for the past 20 days'
            },
            subtitle: {
                text: 'Evaluated by calculating Actual Work mins for the past 20 days.'
            },
            xAxis: {
                type: 'category'
            },
            yAxis: {
                title: {
                    text: 'Percentage of Work Minutes'
                }

            },
            legend: {
                enabled: false
            },
            plotOptions: {
                series: {
                    borderWidth: 0,
                    dataLabels: {
                        enabled: true,
                        format: '{point.y:.1f}%'
                    }
                }
            },

            tooltip: {
                headerFormat: '<span style="font-size:11px">{series.name}</span><br>',
                pointFormat: '<span style="color:{point.color}">{point.name}</span>: <b>{point.y:.2f}%</b> of total<br/>'
            },

            series: [{
                name: "Actual Minutes of Work in %",
                colorByPoint: true,
                data: letspopulatedata
            }]
        }



    }



    

});