app.controller('myCtrlEdit', function ($scope, $http) {
    //Add specialities
    $scope.addLocation = function () {
        if (!$scope.locations) $scope.locations = [];
        if ($scope.newLocation.name != '') {
            $scope.locations.push($scope.newLocation);
            $scope.newLocation = {
                name: ''
            };
        }
    };
    //Remove Speciality
    $scope.removeLocation = function (index) {
        $scope.locations.splice(index, 1);
    };
    // init function
    $scope.initFunction = function (ID) {
        //console.log(UserID);
        $http.get('/User/UserLocationList/').success(function (res) {
            $scope.AllLocations = res;
            console.log(res);
        });
        $http.get('/User/SelectedUserLocList/' + ID).success(function (res) {
            $scope.locations = [];
            for (i = 0 ; i < res.length; i++) {
                $scope.locations.push({ name: res[i].Text });
            }
            console.log(res);
        });
    }
});