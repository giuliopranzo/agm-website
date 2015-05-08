app.controller('candidates', ['$scope', '$rootScope', '$state', '$location', '$filter', 'appDataService', 'textAngularManager', 'candidatesSource', 'candidatesDataService', function($scope, $rootScope, $state, $location, $filter, appDataService, textAngularManager, jobAdsSource, jobAdsDataService) {
        $rootScope.$on('$stateChangeSuccess', function(event, toState, toParams, fromState, fromParams) {
            $scope.isJobAdDetail = (toState.name == 'JobAds.Detail');
            if (toState.name == 'JobAds.Detail') {
                $scope.jobAd = $filter('filter')(jobAdsSource, { id: toParams.adId })[0];
                $scope.jobAdText = $state.params.jobAdTextSource;
            } else {
                $scope.jobAd = {};
            }
        });

        $rootScope.$on('loader_show', function(event, callId) {
            if (callId == 'ja_main')
                $scope.loading = true;
        });

        $rootScope.$on('loader_hide', function(event, callId) {
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

        $scope.init = function() {
            $scope.initData();

            $scope.isJobAdDetail = ($state.current.name == 'JobAds.Detail');
            $scope.jobAd = $filter('filter')(jobAdsSource, { id: $state.params.adId })[0];
            $scope.jobAdText = $state.params.jobAdTextSource;

            $scope.textModel = '<p>AGM Solutions è una System Integrator attiva nel mondo ICT dal 2002, i nostri progetti spaziano dalle soluzioni infrastrutturali alla realizzazione di portali e web application fino ad arrivare a tematiche di Networking e ICT Security.</p><p>Per un nostro cliente con sede a {##} siamo alla ricerca di {##}.</p><p><span style="font-size: 10.5pt;">Il candidato ideale dovrà soddisfare i seguenti requisiti:</span><br></p>';
        };
    }
]);