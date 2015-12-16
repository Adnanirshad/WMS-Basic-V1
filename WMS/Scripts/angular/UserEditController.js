app.controller('myCtrlEdit', function ($scope, $http) {

    $scope.newCity = { name: "" };
    $scope.newLocation = { name: "" };
    $scope.newRegion = { name: "" };
    $scope.newSection = { name: "" };
    $scope.newDepartment = { name: "" };
    $scope.newDivision = { name: "" };
    //Add locations
    $scope.addLocation = function () {
        if (!$scope.locations) $scope.locations = [];
        if ($scope.newLocation.name != '') {
            $scope.locations.push($scope.newLocation);
            $scope.newLocation = {
                name: ''
            };
        }
    };
    //Remove locations
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




    // init function
    $scope.initFunction = function (ID) {
        // get Locations
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
            if ($scope.locations.length > 0)
                $scope.UserAccessLevelL = 'L';
        });


        // get Cities
        $http.get('/User/UserCityList/').success(function (res) {
            $scope.AllCities = res;
            console.log(res);
        });
        $http.get('/User/SelectedUserCityList/' + ID).success(function (res) {
            $scope.cities = [];
            for (i = 0 ; i < res.length; i++) {
                $scope.cities.push({ name: res[i].Text });
            }
            console.log(res);
            if ($scope.cities.length > 0)
                $scope.UserAccessLevelL = 'C';
        });
        


        // get Regions
        $http.get('/User/UserRegionList/').success(function (res) {
            $scope.AllRegions = res;
            console.log(res);
        });
        $http.get('/User/SelectedUserRegionList/' + ID).success(function (res) {
            $scope.regions = [];
            for (i = 0 ; i < res.length; i++) {
                $scope.regions.push({ name: res[i].Text });
            }
            console.log(res);
            if ($scope.regions.length > 0)
                $scope.UserAccessLevelL = 'R';
        });
        



        // get sections
        $http.get('/User/UserSectionList/').success(function (res) {
            $scope.AllSections = res;
            console.log(res);
        });
        $http.get('/User/SelectedUserSectionList/' + ID).success(function (res) {
            $scope.sections = [];
            for (i = 0 ; i < res.length; i++) {
                $scope.sections.push({ name: res[i].Text });
            }
            console.log(res);
            if ($scope.sections.length > 0)
                $scope.UserAccessLevelD = 'S';
        });


        // get departments
        $http.get('/User/UserDepartmentList/').success(function (res) {
            $scope.AllDepartments = res;
            console.log(res);
        });
        $http.get('/User/SelectedUserDepartmentList/' + ID).success(function (res) {
            $scope.departments = [];
            for (i = 0 ; i < res.length; i++) {
                $scope.departments.push({ name: res[i].Text });
            }
            console.log(res);
            if ($scope.departments.length > 0)
                $scope.UserAccessLevelD = 'D';
        });


        //get Divisions
        $http.get('/User/UserDivisionList/').success(function (res) {
            $scope.AllDivisions = res;
            console.log(res);
        });
        $http.get('/User/SelectedUserDivisionList/' + ID).success(function (res) {
            $scope.divisions = [];
            for (i = 0 ; i < res.length; i++) {
                $scope.divisions.push({ name: res[i].Text });
            }
            console.log(res);
            if ($scope.divsions.length > 0)
                $scope.UserAccessLevelD = 'V';
        });


    }
});