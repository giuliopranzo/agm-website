app.controller('jobAdsDetail', ['$rootScope', '$scope', '$state', '$location', '$filter', '$alert', 'jobAdsSource', 'jobAdsDataService', 'applicationGlobals', function ($rootScope, $scope, $state, $location, $filter, $alert, jobAdsSource, jobAdsDataService, applicationGlobals) {
    $rootScope.$on('$stateChangeSuccess', function (event, toState, toParams, fromState, fromParams) {
        if (toState.name == 'Root.JobAds.Detail') {
            $scope.jobAd = $filter('filter')(jobAdsSource, { id: Number(toParams.adId) }, true)[0];
            if (angular.isUndefined($scope.jobAd))
                $scope.jobAd = {}
            $scope.jobAdText = $state.params.jobAdTextSource;
        } else {
            $scope.jobAd = {};
        }
    });

    $scope.init = function () {
        $scope.applicationGlobals = applicationGlobals;
        $scope.editorOptions = {
            language: 'it'
        };
        $scope.jobAd = $filter('filter')(jobAdsSource, { id: Number($state.params.adId) }, true)[0];
        if (angular.isUndefined($scope.jobAd))
            $scope.jobAd = {}
        $scope.jobAdText = $state.params.jobAdTextSource;

        $scope.textModel = '<p>AGM Solutions è una System Integrator attiva nel mondo ICT dal 2002, i nostri progetti spaziano dalle soluzioni infrastrutturali alla realizzazione di portali e web application fino ad arrivare a tematiche di Networking e ICT Security.</p><p>Per un nostro cliente con sede a {##} siamo alla ricerca di {##}.</p><p><span style="font-size: 10.5pt;">Il candidato ideale dovrà soddisfare i seguenti requisiti:</span><br/><br/></p>';
    };

    $scope.backToListPage = function () {
        jobAdsDataService.getJobAds('ja_main').then(function (respData) {
            jobAdsSource = respData.data;
            $scope.initData();
			$state.go('Root.JobAds', {}, {reload: true});
        });
    };

    $scope.save = function () {
        var objToSave = {
            jobAd: $scope.jobAd,
            jobAdText: $scope.jobAdText
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

    $scope.insertTextModel = function () {
        $scope.jobAdText = $scope.textModel + ($scope.jobAdText ? $scope.jobAdText : '');
    };

    $scope.init();
}]);