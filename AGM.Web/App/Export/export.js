app.controller('export', ['$scope', '$filter', '$alert', '$state', 'exportSource', 'exportDataService', function($scope, $filter, $alert, $state, exportSource, exportDataService) {
    $scope.init = function () {
        $scope.currentExport = exportSource;
    };

    $scope.loadData = function (date) {
        var dataFiltered = $filter('date')(date, 'yyyyMM');
        $state.go('Root.Export', { selectedDate: dataFiltered });
    };

    $scope.newExport = function (date) {
        var dataFiltered = $filter('date')(date, 'yyyyMM');
        exportDataService.calculate('mr_detail', dataFiltered).then(function (res) {
            if ($scope.alert)
                $scope.alert.hide();

            if (res.succeed) {
                $scope.currentExport.data = res.data;
                $state.go('Root.Export', { selectedDate: dataFiltered });
            } else {
                $scope.alert = $alert({
                    content: 'Errore durante la lettura dei dati',
                    animation: 'fadeZoomFadeDown',
                    type: 'error',
                    duration: 5
                });
            }
        });
    };

    $scope.startExportMH = function(month) {
        exportDataService.startExport('mr_detail').then(function (res) {
            if (res.succeed)
                window.open('/backoffice/api/Export/GetExportMH?tokenId=' + res.data + '&month=' + month);
        });
    };

    $scope.startExportRI = function (month) {
        exportDataService.startExport('mr_detail').then(function (res) {
            if (res.succeed)
                window.open('/backoffice/api/Export/GetExportRI?tokenId=' + res.data + '&month=' + month);
        });
    };

    $scope.viewExportMH = function(month) {
        $state.go('Root.Export.ReportMH', { selectedDate: month });
    };

    $scope.viewExportRI = function (month) {
        $state.go('Root.Export.ReportRI', { selectedDate: month });
    };

    $scope.init();
}]);