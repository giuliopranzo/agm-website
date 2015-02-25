app.controller('jobAds', ['$scope', '$rootScope', '$state', '$location', '$filter', 'textAngularManager', 'jobAdsSource', 'jobAdsDataService', function ($scope, $rootScope, $state, $location, $filter, textAngularManager, jobAdsSource, jobAdsDataService) {
    $rootScope.$on('$stateChangeSuccess', function (event, toState, toParams, fromState, fromParams) {
        $scope.isJobAdDetail = (toState.name == 'JobAds.Detail');
        if (toState.name == 'JobAds.Detail') {
            $scope.jobAd = $filter('filter')($scope.jobAdsCollection, { id: toParams.adId })[0];
            $scope.jobAdText = $state.params.jobAdTextSource;
        } else {
            $scope.jobAd = {};
        }
    });

    $scope.init = function() {
        $scope.jobAdsCollection = jobAdsSource;
        $scope.selectedAll = false;

        $scope.isJobAdDetail = ($state.current.name == 'JobAds.Detail');
        $scope.jobAd = $filter('filter')($scope.jobAdsCollection, { id: $state.params.adId })[0];
        $scope.jobAdText = $state.params.jobAdTextSource;
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

    $scope.save = function () {
        var editor = textAngularManager.retrieveEditor('jobadtext');
        var objToSave = {
            jobAd: $scope.jobAd,
            jobAdText: editor.scope.html
        };

        jobAdsDataService.saveJobAd('ja_main', objToSave);
    };

    $scope.init();
}]);