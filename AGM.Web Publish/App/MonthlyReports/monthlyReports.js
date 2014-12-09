﻿app.controller('monthlyReports', function ($rootScope, $scope, $alert, $location, $state, $filter, usersSource, monthlyReportsDataService) {
    $scope.alert = $alert({
        animation: 'fadeZoomFadeDown',
        duration: 5,
        show: false
    });

    $scope.init = function () {
        if (usersSource)
            $scope.users = usersSource;
        $scope.detail = {};
        $scope.detailVisible = false;

        if (userReportSource) {
            $scope.detail = userReportSource;
            $scope.selectedDate = userReportSource.currentmonth;
            $scope.detailVisible = true;
        }
    }

    $rootScope.$on('$stateChangeSuccess',
            function (event, toState, toParams, fromState, fromParams) {
                if (toState.name == 'MonthlyReports.detail') {
                    $scope.detail = toParams.userReportSource;
                    $scope.selectedDate = toParams.userReportSource.currentmonth;
                    $scope.detailVisible = true;
                }
            });

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
        //$location.path('/MonthlyReports/' + id);
        //$scope.detailVisible = true;
        //$scope.getUserDetail();
        $location.path('/MonthlyReports/' + $scope.reportId + '/' + $filter('date')(new Date(), 'yyyy-MM'));
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

    $scope.iscurrentmonth = function(obj) {
        return obj.iscurrentmonth;
    }

    $scope.getHourBadge = function(id) {
        return "<i class='fa fa-trash-o' ng-click='removeHour(" + id + ")'></i>";
    }

    $scope.showStrip = function (id) {
        if ($scope.stripToShow == id)
            $scope.stripToShow = 0;
        else
            $scope.stripToShow = id;
    }

    $scope.removeHour = function(id) {
        $scope.alert.hide();
        $scope.alert = $alert({
            content: 'Eliminazione houritem',
            animation: 'fadeZoomFadeDown',
            type: 'info',
            duration: 5
        });
    }

    $scope.toggleAsideInsert = function() {
        $scope.showInsert = !$scope.showInsert;
    }

    $scope.init();
});