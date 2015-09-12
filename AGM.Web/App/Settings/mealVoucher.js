app.controller("mealVoucher", ["$scope", "$alert", "$filter", "settingsDataService", "mealVoucherSource", function ($scope, $alert, $filter, settingsDataService, mealVoucherSource) {
    $scope.init = function () {
        $scope.isMealVoucherCollapsed = true;
        $scope.options = mealVoucherSource;
    };

    $scope.getData = function() {

    };

    $scope.updateOptions = function() {
        settingsDataService.updateMealVoucherOptions('insBtnL', $scope.options).then(function (resp) {
            if ($scope.alert)
                $scope.alert.hide();

            if (resp.succeed) {
                $scope.alert = $alert({
                    content: 'Inserimento avvenuto con successo',
                    animation: 'fadeZoomFadeDown',
                    type: 'info',
                    duration: 5
                });

                $scope.getData();
            } else {
                $scope.alert = $alert({
                    content: 'Errore durante l\'inserimento: ' + resp.errors[0].message,
                    animation: 'fadeZoomFadeDown',
                    type: 'info',
                    duration: 5
                });
            }
        }).catch(function () {
            $scope.alert = $alert({
                content: 'Errore durante l\'inserimento',
                animation: 'fadeZoomFadeDown',
                type: 'info',
                duration: 5
            });
        });
    }

    $scope.init();
}]);