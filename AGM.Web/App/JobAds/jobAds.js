app.controller('jobAds', ['$rootScope', '$scope', '$state', '$location', '$filter', '$alert', 'jobAdsSource', 'jobAdsDataService', 'applicationGlobals', 'appDataService', function ($rootScope, $scope, $state, $location, $filter, $alert, jobAdsSource, jobAdsDataService, applicationGlobals, appDataService) {
    $rootScope.$on('$stateChangeSuccess', function (event, toState, toParams, fromState, fromParams) {
        $scope.isJobAdDetail = (toState.name == 'Root.JobAds.Detail');
    });

    $rootScope.$on('loader_show', function (event, callId) {
        if (callId == 'ja_main')
            $scope.loading = true;
    });

    $rootScope.$on('loader_hide', function (event, callId) {
        if (callId == 'ja_main')
            $scope.loading = false;
    });

    $scope.orderOptions = [
        { label: 'Titolo ASC', value: 'title$A' },
        { label: 'Titolo DESC', value: 'title$D' },
        { label: 'Località ASC', value: 'location$A' },
        { label: 'Località DESC', value: 'location$D' },
        { label: 'Inizio val. ASC', value: 'datefrom$A' },
        { label: 'Inizio val. DESC', value: 'datefrom$D' },
        { label: 'Fine val. ASC', value: 'dateto$A' },
        { label: 'Fine val. DESC', value: 'dateto$D' },
        { label: 'Riferimento ASC', value: 'refcode$A' },
        { label: 'Riferimento DESC', value: 'refcode$D' }
    ];

    $scope.orderField = $scope.orderOptions[5].value;

    $scope.init = function () {
        $scope.applicationGlobals = applicationGlobals;
        $scope.initData();

        $scope.isJobAdDetail = ($state.current.name == 'Root.JobAds.Detail');
    };

    $scope.initData = function() {
        $scope.jobAdsCollection = $filter('filter')(jobAdsSource, function (value, index) { return (value.id != 0); });
        $scope.selectedAll = false;
    }

    $scope.selectionToggle = function () {
        var alreadySelected = false;
        angular.forEach($scope.jobAdsCollection, function (key) {
            if (key._selected) {
                alreadySelected = true;
                $scope.selectedAll = false;
                key._selected = $scope.selectedAll;
            }
        });
        if (!alreadySelected) {
            $scope.selectedAll = !$scope.selectedAll;
            angular.forEach($scope.jobAdsCollection, function(key) {
                key._selected = $scope.selectedAll;
            });
        }
    };

    $scope.goToDetail = function(id) {
        $location.path('/JobAds/' + id);
    };

    $scope.get = function() {
        jobAdsDataService.getJobAds('ja_main').then(function (respData) {
            if ($scope.alert)
                $scope.alert.hide();

            if (respData.succeed) {
                jobAdsSource = respData.data;
                $scope.jobAdsCollection = $filter('filter')(jobAdsSource, function (value, index) { return (value.id != 0); });
            } else {
                $scope.alert = $alert({
                    content: (respData.errors && respData.errors.length >= 0) ? respData.errors[0].message : 'Errore durante la richiesta al server',
                    animation: 'fadeZoomFadeDown',
                    type: 'error',
                    duration: 5
                });
            }
        }).catch(function (respData) {
            if ($scope.alert)
                $scope.alert.hide();
            $scope.alert = $alert({
                content: 'Errore durante la richiesta al server',
                animation: 'fadeZoomFadeDown',
                type: 'error',
                duration: 5
            });
        });
    };

    $scope.insert = function() {
        $scope.goToDetail(0);
    };

    $scope.deleteSelected = function() {
        var selection = $filter('filter')($scope.jobAdsCollection, function (value, index) { return value._selected; });
        jobAdsDataService.deleteJobAds('ja_main', selection).then(function (respData) {
            if ($scope.alert)
                $scope.alert.hide();

            if (respData.succeed) {
                $scope.get();
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
    };

    $scope.thereAreSelected = function() {
        var selection = $filter('filter')($scope.jobAdsCollection, function (value, index) { return value._selected; });
        return selection && selection.length > 0;
    };

    $scope.eraseSearch = function() {
        $scope.search = '';
    };

    $scope.getExpirationMex = function(jobAd) {
        var title = "";
        if (jobAd.expired) {
            title = "Annuncio Scaduto";
        } else if (jobAd.almostexpired) {
            title = "In scadenza";
        }
        return { title: title }
    };

    $scope.getOrder = function () {
        var val = $scope.orderField
        return val.replace(val.substr(-2), '');
    };

    $scope.isOrderDesc = function() {
        var order = $scope.orderField.substr(-1);
        return (order == 'D');
    };

    $scope.getLocation = function(viewValue) {
        return appDataService.getLocation('location', viewValue).then(function(res) {
            return $filter('filter')(res.geonames, function (value, index) {
                if (!value || !value.toponymName || !viewValue) {
                    return false;
                }
                return value.toponymName.toLowerCase().indexOf(viewValue.toLowerCase()) == 0;
            }).slice(0,5);;
        });
    };

    $scope.init();
}]);