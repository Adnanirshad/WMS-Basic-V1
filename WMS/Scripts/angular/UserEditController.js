app.controller('myCtrlEdit', function ($scope, $http) {
    //Add specialities
    $scope.addSection = function () {
        if (!$scope.sections) $scope.sections = [];
        if ($scope.newSection.name != '') {
            $scope.sections.push($scope.newSection);
            $scope.newSection = {
                name: ''
            };
        }
    };
    //Remove Speciality
    $scope.removeSection = function (index) {
        $scope.sections.splice(index, 1);
    };
    // init function
    $scope.initFunction = function (ID) {
        //console.log(UserID);
        $http.get('/User/UserSectionList/').success(function (res) {
            $scope.AllSections = res;
            console.log(res);
        });
        $http.get('/User/SelectedUserSecList/' + ID).success(function (res) {
            $scope.sections = [];
            for (i = 0 ; i < res.length; i++) {
                $scope.sections.push({ name: res[i].Text });
            }
            console.log(res);
        });
    }
});