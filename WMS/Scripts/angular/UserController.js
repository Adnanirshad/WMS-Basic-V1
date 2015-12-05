
app.controller('myCtrl', function ($scope, $http) {
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
    $scope.initFunction = function () {
        $http.get('/User/UserLocationList').success(function (res) {
            $scope.AllLocations = res;
            console.log(res);
        });
    }
});