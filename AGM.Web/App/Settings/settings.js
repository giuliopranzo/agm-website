app.controller("settings",["$scope", "$state", "$alert", "settingsDataService", function($scope, $state, $alert, settingsDataService) {
    $scope.init = function () {
        $scope.newHourReason = { name: '' };
        $scope.getHourReasons();
    };

    $scope.getHourReasons = function() {
        settingsDataService.getHourReasons('tabPage').then(function (resp) {
            if (resp.succeed) {
                $scope.hourreasons = resp.data;
            }
        });
    }

    $scope.insertHourReason = function() {
        settingsDataService.insertHourReason('insBtnL', $scope.newHourReason).then(function (resp) {
            if ($scope.alert)
                $scope.alert.hide();

            if (resp.succeed) {
                $scope.alert = $alert({
                    content: 'Inserimento avvenuto con successo',
                    animation: 'fadeZoomFadeDown',
                    type: 'info',
                    duration: 5
                });

                $scope.newHourReason = { name: '' };
                $scope.getHourReasons();
            } else {
                $scope.alert = $alert({
                    content: 'Errore durante l\'inserimento: ' + resp.errors[0].message,
                    animation: 'fadeZoomFadeDown',
                    type: 'info',
                    duration: 5
                });
            }
        }).catch(function() {
            $scope.alert = $alert({
                content: 'Errore durante l\'inserimento',
                animation: 'fadeZoomFadeDown',
                type: 'info',
                duration: 5
            });
        });
    }

    $scope.getFilterObject = function() {
        if ($scope.showDeleted) {
            return {};
        } else {
            return { isDeleted: false };
        }
    };

    $scope.init();
}]);