app.controller('monthlyReports', function($scope, $alert, $location) {
    $scope.users = usersMockup;
    $scope.detailVisible = false;
    $scope.currentLetter = '';

    $scope.showLetter = function (index, name) {
        if (index == 0)
            $scope.currentLetter = '';
        var letter = name.substr(0, 1);
        var res = (letter != $scope.currentLetter);
        $scope.currentLetter = letter;
        return res;
    };

    $scope.getLetter = function(name) {
        return name.substr(0, 1);
    }

    $scope.showDetail = function(id) {
        $location.path('/MonthlyReports/' + id);
    }
});