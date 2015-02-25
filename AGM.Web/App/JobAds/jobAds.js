app.controller('jobAds', ['$scope', '$rootScope', '$state', '$location', '$filter', 'textAngularManager', 'jobAdsSource', 'jobAdsDataService', function ($scope, $rootScope, $state, $location, $filter, textAngularManager, jobAdsSource, jobAdsDataService) {
    $rootScope.$on('$stateChangeSuccess', function (event, toState, toParams, fromState, fromParams) {
        $scope.isJobAdDetail = (toState.name == 'JobAds.Detail');
        if (toState.name == 'JobAds.Detail') {
            $scope.jobAd = $filter('filter')(jobAdsSource, { id: toParams.adId })[0];
            $scope.jobAdText = $state.params.jobAdTextSource;
        } else {
            $scope.jobAd = {};
        }
    });

    $rootScope.$on('loader_show', function (event, callId) {
        if (callId == 'ja_main')
            $scope.loading = true;
    });

    $rootScope.$on('loader_hide', function (event, callId) {
        if (callId == 'ja_main')
            $scope.loading = false;
    });

    $scope.init = function() {
        $scope.jobAdsCollection = $filter('filter')(jobAdsSource, function(value, index) { return (value.id != 0); });
        $scope.selectedAll = false;

        $scope.isJobAdDetail = ($state.current.name == 'JobAds.Detail');
        $scope.jobAd = $filter('filter')(jobAdsSource, { id: $state.params.adId })[0];
        $scope.jobAdText = $state.params.jobAdTextSource;

        $scope.textModel = '<p>AGM Solutions è una System Integrator attiva nel mondo ICT dal 2002, i nostri progetti spaziano dalle soluzioni infrastrutturali alla realizzazione di portali e web application fino ad arrivare a tematiche di Networking e ICT Security.</p><p>Per un nostro cliente con sede a {##} siamo alla ricerca di {##}.</p><p><span style="font-size: 10.5pt;">Il candidato ideale dovrà soddisfare i seguenti requisiti:</span><br></p>';
    };

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

    $scope.backToListPage = function () {
        $location.path('/JobAds');
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

    $scope.save = function () {
        var editor = textAngularManager.retrieveEditor('jobadtext');
        var objToSave = {
            jobAd: $scope.jobAd,
            jobAdText: editor.scope.html
        };

        jobAdsDataService.saveJobAd('ja_main', objToSave).then(function (respData) {
            if ($scope.alert)
                $scope.alert.hide();

            if (respData.succeed) {
                $scope.backToListPage();
                $scope.get();
            } else {
                $scope.alert = $alert({
                    content: (respData.errors && respData.errors.length >= 0) ? respData.errors[0].message : 'Errore durante il salvataggio dei dati',
                    animation: 'fadeZoomFadeDown',
                    type: 'error',
                    duration: 5
                });
            }
        }).catch(function (respData) {
            if ($scope.alert)
                $scope.alert.hide();
            $scope.alert = $alert({
                content: 'Errore durante il salvataggio dei dati',
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

    $scope.taTextIsEmpty = function() {
        var editor = textAngularManager.retrieveEditor('jobadtext');
        return (editor.scope.html == '');
    };

    $scope.thereAreSelected = function() {
        var selection = $filter('filter')($scope.jobAdsCollection, function (value, index) { return value._selected; });
        return selection && selection.length > 0;
    };

    $scope.insertTextModel = function() {
        $scope.jobAdText = $scope.textModel + ($scope.jobAdText? $scope.jobAdText : '');
    };

    $scope.init();
}]);