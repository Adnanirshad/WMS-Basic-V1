
app.controller('myCtrl', function ($scope, $http) {
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
    $scope.removeSection= function (index) {
        $scope.sections.splice(index, 1);
    };
    // init function
    $scope.initFunction = function () {
        $http.get('/User/UserSectionList').success(function (res) {
            $scope.AllSections = res;
            console.log(res);
        });
    }
});