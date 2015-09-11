app.controller("hourReason", ["$scope", "$state", "$alert", "$filter", "settingsDataService", "hourReasonSource", function ($scope, $state, $alert, $filter, settingsDataService, hourReasonSource) {
    $scope.init = function () {
        $scope.isHourReasonCollapsed = true;
        $scope.newHourReason = { id: 0, name: '', codeexport: '' };
        $scope.dataSource = hourReasonSource;
        $scope.defaultOrder = '+name';
        $scope.currentPageData = [];
    };

    $scope.getHourReasons = function () {
        settingsDataService.getHourReasons('tabPage').then(function (resp) {
            if (resp.succeed) {
                $scope.dataSource = resp.data;
            }
        });
    }

    $scope.insertHourReason = function () {
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

                $scope.newHourReason = { id: 0, name: '', codeexport: '' };
                $scope.getHourReasons();
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

    $scope.updateHourReason = function () {
        settingsDataService.updateHourReason('insBtnL', $scope.newHourReason).then(function (resp) {
            if ($scope.alert)
                $scope.alert.hide();

            if (resp.succeed) {
                $scope.alert = $alert({
                    content: 'Aggiornamento avvenuto con successo',
                    animation: 'fadeZoomFadeDown',
                    type: 'info',
                    duration: 5
                });

                $scope.newHourReason = { id: 0, name: '', codeexport: '' };
                $scope.getHourReasons();
            } else {
                $scope.alert = $alert({
                    content: 'Errore durante l\'aggiornamento: ' + resp.errors[0].message,
                    animation: 'fadeZoomFadeDown',
                    type: 'info',
                    duration: 5
                });
            }
        }).catch(function () {
            $scope.alert = $alert({
                content: 'Errore durante l\'aggiornamento',
                animation: 'fadeZoomFadeDown',
                type: 'info',
                duration: 5
            });
        });
    }

    $scope.deleteHourReason = function (item) {
        settingsDataService.deleteHourReason('insBtnL', item).then(function (resp) {
            if ($scope.alert)
                $scope.alert.hide();

            if (resp.succeed) {
                $scope.alert = $alert({
                    content: 'Cancellazione causale \'' + item.name + '\' avvenuta con successo',
                    animation: 'fadeZoomFadeDown',
                    type: 'info',
                    duration: 5
                });

                $scope.getHourReasons();
            } else {
                $scope.alert = $alert({
                    content: 'Errore durante la cancellazione: ' + resp.errors[0].message,
                    animation: 'fadeZoomFadeDown',
                    type: 'info',
                    duration: 5
                });
            }
        }).catch(function () {
            $scope.alert = $alert({
                content: 'Errore durante la cancellazione',
                animation: 'fadeZoomFadeDown',
                type: 'info',
                duration: 5
            });
        });
    };

    $scope.getFilterObject = function () {
        if ($scope.showDeleted) {
            return {};
        } else {
            return { isdeleted: false };
        }
    };

    $scope.thereAreSelected = function (count) {
        var selection = $filter('filter')($scope.currentPageData, function (value, index) { return value._selected && !value.isdeleted; });
        if (count)
            return selection && selection.length === count;
        return selection && selection.length > 0;
    };

    $scope.deleteSelected = function () {
        var selection = $filter('filter')($scope.currentPageData, function (value, index) { return value._selected; });
        settingsDataService.deleteHourReason('main', selection).then(function (respData) {
            if ($scope.alert)
                $scope.alert.hide();

            if (respData.succeed) {
                $scope.getHourReasons();
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

    $scope.updateSelected = function() {
        var selection = $filter('filter')($scope.currentPageData, function (value, index) { return value._selected; });
        $scope.newHourReason = angular.copy(selection[0]);
    };

    $scope.selectionToggle = function() {
        var alreadySelected = false;
        angular.forEach($scope.currentPageData, function (key) {
            if (key._selected) {
                alreadySelected = true;
                $scope.selectedAll = false;
                key._selected = $scope.selectedAll;
            }
        });
        if (!alreadySelected) {
            $scope.selectedAll = !$scope.selectedAll;
            angular.forEach($scope.currentPageData, function (key) {
                key._selected = $scope.selectedAll;
            });
        }
    }

    $scope.onDataChange = function(currentPageData) {
        $scope.currentPageData = currentPageData;
    }

    $scope.init();
}]);