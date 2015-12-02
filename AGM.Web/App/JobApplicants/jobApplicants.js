﻿app.controller('jobApplicants', ['$scope', 'applicants', 'statuses', 'pagerUp', 'appHelper', function ($scope, applicants, statuses, pagerUp, appHelper) {
    $scope.statusBgColors = {
        'statusId_1': '#FFFFFF',
        'statusId_2': '#CCFFFF',
        'statusId_3': '#CCFFFF',
        'statusId_4': '#CCFF99',
        'statusId_5': '#FFFF66',
        'statusId_6': '#FF6A6A',
        'statusId_8': '#CC99FF',
        'statusId_9': '#FFFFFF',
        'statusId_10': '#C3C3C3',
        'statusId_11': '#000000',
        'statusId_0': '#FFFFFF'
    }

    $scope.init = function () {
        $scope.statuses = statuses;
        $scope.dataFilterOn = false;

        $scope.pager = pagerUp;
        $scope.pageSizes = [10, 25, 50];
        $scope.pager.setPageSize(10);
        $scope.pager.setMaxItems(50);
        $scope.pager.setOrder('interviewdate', true);
        $scope.pager.setOrder('interviewdate');
        $scope.pager.setData(applicants);
        $scope.pager.setAdvancedFilterFunction($scope.filterData);

        $scope.helper = appHelper;
    };

    $scope.eraseSearch = function () {
        $scope.search = '';
    };

    $scope.getStatusBgColor = function (statusId) {
        if ($scope.statusBgColors['statusId_' + statusId])
            return $scope.statusBgColors['statusId_' + statusId];
        return '#FFFFFF';
    };

    $scope.toggleAsideFilters = function () {
        $scope.showFilters = !$scope.showFilters;
    };

    $scope.applyFilter = function () {
        $scope.dataFilterOn = true;
        $scope.toggleAsideFilters();
        $scope.pager.setPageIndex(0);
    };

    $scope.resetFilter = function () {
        $scope.filterBirthDateFrom = null;
        $scope.filterBirthDateTo = null;
        $scope.filterInterviewDateFrom = null;
        $scope.filterInterviewDateTo = null;
        $scope.filterCategory = null;
        $scope.filterStatus = null;
        $scope.dataFilterOn = false;
        $scope.toggleAsideFilters();
        $scope.pager.setPageIndex(0);
    };

    $scope.filterData = function (data) {
        if ($scope.dataFilterOn === false)
            return true;

        if ($scope.filterBirthDateFrom && new moment(data.birthdate).isBefore($scope.filterBirthDateFrom))
            return false;

        if ($scope.filterBirthDateTo && new moment(data.birthdate).isAfter($scope.filterBirthDateTo))
            return false;

        if ($scope.filterInterviewDateFrom && moment(data.interviewdate).isBefore($scope.filterInterviewDateFrom))
            return false;

        if ($scope.filterInterviewDateTo && moment(data.interviewdate).isAfter($scope.filterInterviewDateTo))
            return false;

        if ($scope.filterCategory && data.jobcategory.name.indexOf($scope.filterCategory)==-1)
            return false;

        if ($scope.filterStatus && $scope.filterStatus.name && (!data.statusreason || data.statusreason.name.indexOf($scope.filterStatus.name) == -1) && (!data.status || data.status.name.indexOf($scope.filterStatus.name) == -1))
            return false;

        return true;
    };

    $scope.init();
}]);