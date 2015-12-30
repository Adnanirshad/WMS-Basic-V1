var ESSApp = angular.module('LvApp', ['ui.bootstrap']);
ESSApp.controller('LvAppCtrl', function ($scope, $http, filterFilter) {


    // init function
    $scope.initFunction = function () {
        // get Locations
        $http.get('/LeaveApproval/PendingLeavesList/').success(function (res) {
            $scope.AllLeaves = res;


            // init paging
            $scope.search = '';
            $scope.currentPage = 1; //current page
            $scope.maxSize = 5; //pagination max size
            $scope.entryLimit = 7; //max rows for data table

            /* init pagination with $scope.list */
            $scope.noOfPages = Math.ceil($scope.AllLeaves.length / $scope.entryLimit);
        });
    }

    // Generate all function
    $scope.approveAll = function () {
        $http.get('/LeaveApproval/ApproveAll/').success(function (res) {
            if (res === 'success') {
                for (var i = 0; i < $scope.AllLeaves.length; i++) {
                    $scope.AllLeaves[i].IsApproved = true;
                    removeLeaveFromList($scope.AllLeaves[i]);
                }
            }
        });
    }

    $scope.deleselectAll = function () {
        $scope.AllLeaves.filter(function (emp, index) {
            emp.isSelected = false;
        });
    }

    $scope.approveSelected = function () {
        var selectedLeaves = [];
        for (var i = 0; i < $scope.AllLeaves.length; i++) {
            if ($scope.AllLeaves[i].isSelected) {
                selectedLeaves.push($scope.AllLeaves[i]);
            }
        }
        $http.post("/LeaveApproval", JSON.stringify(selectedLeaves)).success(function (data) {
            for (var i = 0; i < selectedLeaves.length; i++) {
                selectedLeaves[i].IsApproved = true;
                removeLeaveFromList(selectedLeaves[i]);
            }
        })
    }

    $scope.approveLeave = function (leave) {
        $http.get("/LeaveApproval/ApproveLeave?LvID=" + leave.LvID).success(function (result) {
            if (result === 'success') {
                removeLeaveFromList(leave);
            }   
        })
    }

    $scope.revokeLeave = function (leave) {
        $http.get("/LeaveApproval/RevokeLeave?LvID=" + leave.LvID).success(function (result) {
            if (result === 'success')
                removeLeaveFromList(leave);
        })
    }

    $scope.$watch('search', function (term) {
        // Create $scope.filtered and then calculat $scope.noOfPages, no racing!
        if ($scope.AllLeaves) {
            $scope.filtered = filterFilter($scope.AllLeaves, term);
            $scope.noOfPages = Math.ceil($scope.filtered.length / $scope.entryLimit);
        }
    });

    function removeLeaveFromList(leave) {
        leave.IsApproved = true;
        $scope.AllLeaves.splice($scope.AllLeaves.indexOf(leave), 1);
        $scope.filtered = filterFilter($scope.AllLeaves, '');
        $scope.noOfPages = Math.ceil($scope.filtered.length / $scope.entryLimit);
    }

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

