var ESSApp = angular.module('ESSApp', ['ui.bootstrap']);
ESSApp.controller('ESSUserCtrl', function ($scope, $http, filterFilter) {


    // init function
    $scope.initFunction = function () {
        // get Locations
        $http.get('/ESSUser/ESSEmpsList/').success(function (res) {
            $scope.AllEmps = res;


            // init paging
            $scope.search = '';
            $scope.currentPage = 1; //current page
            $scope.maxSize = 5; //pagination max size
            $scope.entryLimit = 7; //max rows for data table

            /* init pagination with $scope.list */
            $scope.noOfPages = Math.ceil($scope.AllEmps.length / $scope.entryLimit);
        });
    }

    // Generate all function
    $scope.generateAll = function () {
        $http.get('/ESSUser/GenerateAll/').success(function (res) {
            if (res === 'success') {
                for (var i = 0; i < $scope.AllEmps.length; i++)
                    $scope.AllEmps[i].HasAccess = true;
            }
        });
    }

    $scope.deleselectAll = function () {
        $scope.AllEmps.filter(function (emp, index) {
            emp.isSelected = false;
        });
    }

    $scope.generateSelected = function () {
        var selectedEmps = [];
        for (var i = 0; i < $scope.AllEmps.length; i++) {
            if ($scope.AllEmps[i].isSelected) {
                selectedEmps.push($scope.AllEmps[i]);
            }
        }
        $http.post("/ESSUser", JSON.stringify(selectedEmps)).success(function (data) {
            for (var i = 0; i < selectedEmps.length; i++)
                selectedEmps[i].HasAccess = true;
        })
    }

    $scope.generateUser = function (emp) {
        $http.get("/ESSUser/GenerateESSUser?EmpID=" + emp.EmpID).success(function (result) {
            if(result === 'success')
                emp.HasAccess = true;

        })
    }

    $scope.restrictUser = function (emp) {
        $http.get("/ESSUser/RestrictESSUser?EmpID=" + emp.EmpID).success(function (result) {
            if (result === 'success')
                emp.HasAccess = false;
        })
    }

    $scope.$watch('search', function (term) {
        // Create $scope.filtered and then calculat $scope.noOfPages, no racing!
        if ($scope.AllEmps) {
            $scope.filtered = filterFilter($scope.AllEmps, term);
            $scope.noOfPages = Math.ceil($scope.filtered.length / $scope.entryLimit);
        }
    });
});






ESSApp.filter('startFrom', function () {
    return function (input, start) {
        if (input) {
            start = +start; //parse to int
            return input.slice(start);
        }
        return [];
    }
});

