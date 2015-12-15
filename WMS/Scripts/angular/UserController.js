app.controller('myCtrl', function ($scope, $http) {
    $scope.newCity = { name: "" };
    $scope.newLocation = { name: "" };
    $scope.newRegion = { name: "" };
    $scope.newSection = { name: "" };
    $scope.newDepartment = { name: "" };
    $scope.newDivision = { name: "" };
    // init function
    $scope.initFunction = function () {
        $http.get('/User/UserLocationList').success(function (res) {
            $scope.AllLocations = res;
            console.log(res);
        });
        $http.get('/User/UserCityList').success(function (res) {
            $scope.AllCities = res;
            console.log(res);
        });
        $http.get('/User/UserRegionList').success(function (res) {
            $scope.AllRegions = res;
            console.log(res);
        });
        $http.get('/User/UserSectionList').success(function (res) {
            $scope.AllSections = res;
            console.log(res);
        });
        $http.get('/User/UserDepartmentList').success(function (res) {
            $scope.AllDepartments = res;
            console.log(res);
        });
        $http.get('/User/UserDivisionList').success(function (res) {
            $scope.AllDivisions = res;
            console.log(res);
        });
    }
    //Add Location
    $scope.addLocation = function () {
        if (!$scope.locations) $scope.locations = [];
        if ($scope.newLocation.name != '') {
            $scope.locations.push($scope.newLocation);
            $scope.newLocation = {
                name: ''
            };
        }
    };
    //Remove Location
    $scope.removeLocation = function (index) {
        $scope.locations.splice(index, 1);
    };

    //Add City
    $scope.addCity = function () {
        if (!$scope.cities) $scope.cities = [];
        if ($scope.newCity.name != '') {
            $scope.cities.push($scope.newCity);
            $scope.newCity = {
                name: ''
            };
        }
    };
    //Remove City
    $scope.removeCity = function (index) {
        $scope.cities.splice(index, 1);
    };

    //Add REGION
    $scope.addRegion = function () {
        if (!$scope.regions) $scope.regions = [];
        if ($scope.newRegion.name != '') {
            $scope.regions.push($scope.newRegion);
            $scope.newRegion = {
                name: ''
            };
        }
    };
    //Remove Region
    $scope.removeRegion = function (index) {
        $scope.regions.splice(index, 1);
    };

    //Add Section
    $scope.addSection = function () {
        if (!$scope.sections) $scope.sections = [];
        if ($scope.newSection.name != '') {
            $scope.sections.push($scope.newSection);
            $scope.newSection = {
                name: ''
            };
        }
    };
    //Remove Section
    $scope.removeSection = function (index) {
        $scope.sections.splice(index, 1);
    };

    //Add Department
    $scope.addDepartment = function () {
        if (!$scope.departments) $scope.departments = [];
        if ($scope.newDepartment.name != '') {
            $scope.departments.push($scope.newDepartment);
            $scope.newDepartment = {
                name: ''
            };
        }
    };
    //Remove Department
    $scope.removeDepartment = function (index) {
        $scope.departments.splice(index, 1);
    };

    //Add Division
    $scope.addDivision = function () {
        if (!$scope.divisions) $scope.divisions = [];
        if ($scope.newDivision.name != '') {
            $scope.divisions.push($scope.newDivision);
            $scope.newDivision = {
                name: ''
            };
        }
    };
    //Remove Division
    $scope.removeDivision = function (index) {
        $scope.divisions.splice(index, 1);
    };
    
});