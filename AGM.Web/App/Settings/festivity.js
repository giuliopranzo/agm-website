app.controller("festivity", ["$scope", "$alert", "$filter", "settingsDataService", "festivitySource", function ($scope, $alert, $filter, settingsDataService, festivitySource) {
    $scope.init = function () {
        $scope.isFestivityCollapsed = true;
        $scope.newFestivity = { id: 0, date: null };
        $scope.dataSource = festivitySource;
        $scope.currentPageData = [];
        $scope.defaultOrder = '-date';
    };

    $scope.getFestivities = function () {
        settingsDataService.getFestivities('tabPage').then(function (resp) {
            if (resp.succeed) {
                $scope.dataSource = resp.data;
            }
        });
    }

    $scope.insertFestivity = function () {
        $scope.newFestivity.date = $filter('date')($scope.newFestivity.date, 'yyyy-MM-dd');
        settingsDataService.insertFestivity('insBtnL', $scope.newFestivity).then(function (resp) {
            if ($scope.alert)
                $scope.alert.hide();

            if (resp.succeed) {
                $scope.alert = $alert({
                    content: 'Inserimento avvenuto con successo',
                    animation: 'fadeZoomFadeDown',
                    type: 'info',
                    duration: 5
                });

                $scope.newFestivity = { id: 0, date: null };
                $scope.getFestivities();
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

    $scope.selectionToggle = function () {
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

    $scope.thereAreSelected = function (count) {
        var selection = $filter('filter')($scope.currentPageData, function (value, index) { return value._selected && !value.isdeleted; });
        if (count)
            return selection && selection.length === count;
        return selection && selection.length > 0;
    };

    $scope.deleteSelected = function () {
        var selection = $filter('filter')($scope.currentPageData, function (value, index) { return value._selected; });
        settingsDataService.deleteFestivity('main', selection).then(function (respData) {
            if ($scope.alert)
                $scope.alert.hide();

            if (respData.succeed) {
                $scope.getFestivities();
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

    $scope.onDataChange = function(currentPageData) {
        $scope.currentPageData = currentPageData;
    }

    $scope.init();
}]);