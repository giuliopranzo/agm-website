app.controller('monthlyReports', function ($scope, $alert, $location, $stateParams) {
    $scope.users = usersMockup;
    $scope.detailVisible = false;
    $scope.reportId = $stateParams.reportId;
    if ($scope.reportId)
        $scope.detailVisible = true;

    $scope.showLetter = function (index) {
        if (index == 0 || $scope.usersFiltered[index].name.substr(0, 1) != $scope.usersFiltered[index - 1].name.substr(0, 1))
            return true;
        return false;
    };

    $scope.getFirstChar = function(value) {
        return value.substr(0, 1);
    };

    $scope.showDetail = function(id) {
        $scope.reportId = id;
        $location.path('/MonthlyReports/' + id);
        $scope.detailVisible = true;
    };

    $scope.backToUsers = function() {
        $scope.reportId = null;
        $location.path('/MonthlyReports');
        $scope.detailVisible = false;
    };

    $scope.test = function() {
        $alert({
            content: 'You have successfully logged in',
            animation: 'fadeZoomFadeDown',
            type: 'info',
            duration: 60
        });
    };
});