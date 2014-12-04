app.controller('monthlyReports', function ($scope, $alert, $location, $stateParams, $filter, usersSource, monthlyReportsDataService) {

    $scope.init = function () {
        if (usersSource)
            $scope.users = usersSource;
        $scope.detail = {};
        $scope.detailVisible = false;

        if ($stateParams.reportId && $stateParams.selectedDate) {
            $scope.reportId = $stateParams.reportId;
            $scope.detailVisible = true;
            $scope.getUserDetail($filter('date')($stateParams.selectedDate + '-01', 'MMMM yyyy'));
        }
    }

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
        $scope.getUserDetail();
    };

    $scope.backToUsers = function() {
        $scope.reportId = null;
        $location.path('/MonthlyReports');
        $scope.detailVisible = false;
    };

    $scope.getUserDetail = function(reportMonth)
    {
        monthlyReportsDataService.getReportDetail($scope.reportId, reportMonth).then(function (respData) {
            $scope.detail = respData.data;
            $scope.selectedDate = respData.data.currentmonth;
            $location.path('/MonthlyReports/' + $scope.reportId + '/' + $filter('date')($scope.selectedDate, 'yyyy-MM'));
            //var test = $filter('date')($scope.selectedDate, 'MMMM yyyy');
        });
    }

    $scope.init();
});