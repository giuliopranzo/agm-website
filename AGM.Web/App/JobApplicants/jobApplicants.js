app.controller('jobApplicants', ['$rootScope', '$scope', '$uibModal', '$location', '$state', '$filter', '$alert', 'authenticationContainer', 'applicants', 'statuses', 'pagerUp', 'appHelper', 'jobCategories', 'interviewers', 'jobApplicantsDataService', function ($rootScope, $scope, $uibModal, $location, $state, $filter, $alert, authenticationContainer, applicants, statuses, pagerUp, appHelper, jobCategories, interviewers, jobApplicantsDataService) {
    $rootScope.$on('$stateChangeSuccess', function (event, toState, toParams, fromState, fromParams) {
        $scope.isJobApplicantDetail = (toState.name == 'Root.JobApplicants.Detail');
    });

    $rootScope.$on('loader_show', function (event, callId) {
        if (callId == 'japp_main') {
            if (!$rootScope.loader)
                $rootScope.loader = {};
            if (!$rootScope.loader['japp_main'])
                $rootScope.loader['japp_main'] = 0;
            $rootScope.loader['japp_main'] = $rootScope.loader['japp_main'] + 1;
            $scope.loading = true;
        }
    });

    $rootScope.$on('loader_hide', function (event, callId) {
        if (callId == 'japp_main') {
            if ($rootScope.loader['japp_main'] > 0)
                $rootScope.loader['japp_main'] = $rootScope.loader['japp_main'] - 1;
            if ($rootScope.loader['japp_main'] == 0)
                $scope.loading = false;
        }
    });

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
        $scope.currentUser = authenticationContainer.currentUser;
        $scope.statuses = statuses;
        $scope.dataFilterOn = false;
        
        $scope.pager = pagerUp;
        $scope.pageSizes = [10, 25, 50];
        $scope.pager.setPageSize(10);
        $scope.pager.setMaxItems(0);
        $scope.pager.setOrder('interviewdate', true);
        $scope.pager.setOrder('interviewdate');
        $scope.pager.setData(applicants);
        $scope.pager.setAdvancedFilterFunction($scope.filterData);

        $scope.helper = appHelper;
        $scope.jobCategories = jobCategories;
        $scope.interviewers = interviewers;
        $scope.hiredFilterValues = [{ value: null, description: 'Stato Assunto' }, { value: true, description: 'Assunto' }, { value: false, description: 'Non Assunto' }];
        $scope.suspendedFilterValues = [{ value: null, description: 'Stato Cessato' }, { value: true, description: 'Cessato' }, { value: false, description: 'Non Cessato' }];
        $scope.isJobApplicantDetail = ($state.current.name == 'Root.JobApplicants.Detail');
        $scope.resetFilter();
    };

    $scope.checkboxStraniero = {
        value: true,
    };

    $scope.eraseSearch = function () {
        $scope.search = '';
    };

    $scope.getStatusBgColor = function (statusId) {
        if ($scope.statusBgColors['statusId_' + statusId])
            return $scope.statusBgColors['statusId_' + statusId];
        return '#FFFFFF';
    };

    $scope.toggleAsideFilters = function (val) {
        if (!val && val !== false)
            $scope.showFilters = !$scope.showFilters;
        else
            $scope.showFilters = val;
    };

    $scope.applyFilter = function () {
        $scope.dataFilterOn = true;
        $scope.toggleAsideFilters(false);
        $scope.pager.setPageIndex(0);
    };

    $scope.resetFilter = function () {
        $scope.filterBirthDateFrom = null;
        $scope.filterBirthDateTo = null;
        $scope.filterInterviewDateFrom = null;
        $scope.filterInterviewDateTo = null;
        $scope.filterCategory = [];
        $scope.filterInterviewer = [];
        $scope.filterStatus = [];
        $scope.dataFilterOn = false;
        $scope.toggleAsideFilters(false);
        $scope.pager.setPageIndex(0);
        $scope.filterHired = null;
        $scope.filterSuspended = null;
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

        if ($scope.filterCategory && $scope.filterCategory.length > 0 && (!data.jobcategory || !_.contains($scope.filterCategory, data.jobcategory.id)))
            return false;

        if ($scope.filterInterviewer && $scope.filterInterviewer.length > 0 && (!data.user || !_.contains($scope.filterInterviewer, data.user.id)))
            return false;

        if ($scope.filterStatus && $scope.filterStatus.length > 0 && (!data.statusreason || !_.contains($scope.filterStatus, data.statusreason.name)) && (!data.status || !_.contains($scope.filterStatus, data.status.name)))
            return false;

        if (($scope.filterHired === true && data._hired !== 1) || ($scope.filterHired === false && data._hired !== 0))
            return false;

        return true;
    };

    $scope.goToDetail = function (id) {
        $state.go('Root.JobApplicants.Detail', { applicantId: id }, {});
    };

    $scope.loadData = function () {
        jobApplicantsDataService.get('japp_main').then(function (respData) {
            if ($scope.alert)
                $scope.alert.hide();

            if (respData.succeed) {
                $scope.pager.setData(respData.data);
            } else {
                $scope.alert = $alert({
                    content: (respData.errors && respData.errors.length >= 0) ? respData.errors[0].message : 'Errore durante il caricamento dei dati',
                    animation: 'fadeZoomFadeDown',
                    type: 'error',
                    duration: 5
                });
            }
        }).catch(function (respData) {
            if ($scope.alert)
                $scope.alert.hide();
            $scope.alert = $alert({
                content: 'Errore durante il caricamento dei dati',
                animation: 'fadeZoomFadeDown',
                type: 'error',
                duration: 5
            });
        });
    };

    $scope.insert = function () {
        $scope.goToDetail(0);
    };

    $scope.deleteSelected = function () {
        if ($scope.alert && $scope.alert.$isShown && $scope.alert.$options.type != 'confirm_delete')
            $scope.alert.hide();

        if (!$scope.alert || !$scope.alert.$isShown)
            $scope.alert = $alert({
                content: 'Confermi la cancellazione dei candidati selezionati? Cliccare nuovamente l\'icona per procedere.',
                animation: 'fadeZoomFadeDown',
                type: 'confirm_delete',
                duration: 10
            });
        else {
            $scope.alert.hide();
            var selection = $filter('filter')($scope.pager.getCurrentPageData($scope.search), function (value, index) { return value._selected; });
            jobApplicantsDataService.delete('japp_main', selection).then(function (respData) {
                if ($scope.alert)
                    $scope.alert.hide();

                if (respData.succeed) {
                    $scope.loadData();
                } else {
                    $scope.alert = $alert({
                        content: (respData.errors && respData.errors.length >= 0) ? respData.errors[0].message : 'Errore durante la cancellazione dei dati',
                        animation: 'fadeZoomFadeDown',
                        type: 'error',
                        duration: 5
                    });
                }
            }).catch(function (respData) {
                if ($scope.alert)
                    $scope.alert.hide();
                $scope.alert = $alert({
                    content: 'Errore durante la cancellazione dei dati',
                    animation: 'fadeZoomFadeDown',
                    type: 'error',
                    duration: 5
                });
            });
        }
    };

    $scope.thereAreSelected = function () {
        var selection = $filter('filter')($scope.pager.getCurrentPageData($scope.search), function (value, index) { return value._selected; });
        return selection && selection.length > 0;
    };

    $scope.init();
}]);