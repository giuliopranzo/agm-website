app.controller('monthlyReports', function ($rootScope, $scope, $alert, $location, $state, $filter, usersSource, monthlyReportsDataService, appHelper) {
    $rootScope.$on('$stateChangeSuccess', function(event, toState, toParams, fromState, fromParams) {
        if (toState.name == 'MonthlyReports') {
            $scope.reportId = null;
            $scope.detailVisible = false;
            appHelper.scrollTo('reportTop');
        }

        if (toState.name == 'MonthlyReports.detail') {
            $scope.reportId = toParams.reportId;
            $scope.detail = toParams.userReportSource;
            $scope.selectedDate = toParams.userReportSource.currentmonth;
            $scope.selectedInsertDate = toParams.userReportSource.selectedinsertdate;
            $scope.insertHour = null;
            $scope.insertExpense = null;
            $scope.insertNote = null;
            $scope.insertSelectedHourReason = $scope.detail.hourreasons[0].id;
            $scope.insertSelectedExpenseReason = $scope.detail.expensereasons[0].id;
            $scope.detailVisible = true;
            appHelper.scrollTo('reportTop');
        }
    });

    $rootScope.$on('loader_show', function (event, callId) {
        if (callId == 'mr_detail')
            $scope.loading_detail = true;
        if (callId == 'mr_insert')
            $scope.loading_insert = true;
    });

    $rootScope.$on('loader_hide', function (event, callId) {
        if (callId == 'mr_detail')
            $scope.loading_detail = false;
        if (callId == 'mr_insert')
            $scope.loading_insert = false;
    });

    $scope.init = function() {
        if (usersSource)
            $scope.users = usersSource;
        $scope.detail = {};
        $scope.detailVisible = false;

        if ($state && $state.params && $state.params.userReportSource) {
            $scope.reportId = $state.params.reportId;
            $scope.detail = $state.params.userReportSource;
            $scope.selectedDate = $state.params.userReportSource.currentmonth;
            $scope.selectedInsertDate = $state.params.userReportSource.selectedinsertdate;
            $scope.resetInsertFields();
            $scope.detailVisible = true;
        }
    }

    $scope.iscurrentmonth = function (obj) {
        return obj.iscurrentmonth;
    }

    $scope.resetInsertFields = function() {
        $scope.insertHour = null;
        $scope.insertExpense = null;
        $scope.insertNote = null;
        $scope.insertSelectedHourReason = $scope.detail.hourreasons[0].id;
        $scope.insertSelectedExpenseReason = $scope.detail.expensereasons[0].id;
    }

    $scope.showLetter = function(index) {
        if (index == 0 || $scope.usersFiltered[index].name.substr(0, 1) != $scope.usersFiltered[index - 1].name.substr(0, 1))
            return true;
        return false;
    };

    $scope.getFirstChar = function(value) {
        return value.substr(0, 1);
    };

    $scope.showDetail = function(id) {
        $location.path('/MonthlyReports/' + id + '/' + $filter('date')(new Date(), 'yyyy-MM'));
    };

    $scope.getUserDetail = function(reportMonth) {
        $location.path('/MonthlyReports/' + $scope.reportId + '/' + $filter('date')(new Date(reportMonth), 'yyyy-MM'));
    }

    $scope.reloadUserDetail = function() {
        monthlyReportsDataService.getReportDetail('mr_detail', $scope.detail.user.id, $filter('date')(new Date($scope.selectedDate), 'yyyy-MM')).then(
            function(respData) {
                if (respData.succeed) {
                    $scope.detail = respData.data;
                } else {
                    if ($scope.alert)
                        $scope.alert.hide();

                    $scope.alert = $alert({
                        content: 'Errore durante l\'aggiornamento dei dati',
                        animation: 'fadeZoomFadeDown',
                        type: 'error',
                        duration: 5
                    });
                }
            }
        ).catch(function (respData) {
                if ($scope.alert)
                    $scope.alert.hide();
                $scope.alert = $alert({
                    content: 'Errore durante l\'aggiornamento dei dati',
                    animation: 'fadeZoomFadeDown',
                    type: 'error',
                    duration: 5
                });
            }
        );
    }

    $scope.backToUsers = function() {
        $location.path('/MonthlyReports');
    };

    $scope.toggleAsideInsert = function() {
        $scope.showInsert = !$scope.showInsert;
    }

    $scope.insertReport = function() {
        var reportDetail = {
            UserId: $scope.detail.user.id,
            Date: new Date($scope.selectedInsertDate).toDateString(),
            Hours: { HoursCount: $scope.insertHour, ReasonId: $scope.insertSelectedHourReason },
            Expenses: { Amount: $scope.insertExpense, ReasonId: $scope.insertSelectedExpenseReason },
            Note: $scope.insertNote
        };
        monthlyReportsDataService.insertReportDetail('mr_insert', reportDetail).then(
            function (respData) {
                if ($scope.alert)
                    $scope.alert.hide();

                if (respData.succeed) {
                    $scope.alert = $alert({
                        content: 'Inserimento avvenuto con successo',
                        animation: 'fadeZoomFadeDown',
                        type: 'info',
                        duration: 5
                    });
                    $scope.toggleAsideInsert();
                    $scope.resetInsertFields();
                    $scope.reloadUserDetail();
                } else {
                    $scope.alert = $alert({
                        content: 'Errore durante l\'inserimento',
                        animation: 'fadeZoomFadeDown',
                        type: 'error',
                        duration: 5
                    });
                }
            })
            .catch(
            function (respData) {
                if ($scope.alert)
                    $scope.alert.hide();
                $scope.alert = $alert({
                    content: 'Errore durante l\'inserimento',
                    animation: 'fadeZoomFadeDown',
                    type: 'error',
                    duration: 5
                });
            });
    }

    $scope.removeReport = function(type, id) {
        var commandObj = {
            Type: type,
            Id: id
        };
        monthlyReportsDataService.deleteReportDetail('mr_detail', commandObj).then(
            function (respData) {
                if ($scope.alert)
                    $scope.alert.hide();

                if (respData.succeed) {
                    $scope.alert = $alert({
                        content: 'Eliminazione avvenuta con successo',
                        animation: 'fadeZoomFadeDown',
                        type: 'info',
                        duration: 5
                    });
                    $scope.reloadUserDetail();
                } else {
                    $scope.alert = $alert({
                        content: 'Errore durante l\'eliminazione',
                        animation: 'fadeZoomFadeDown',
                        type: 'error',
                        duration: 5
                    });
                }
            })
            .catch(
            function (respData) {
                if ($scope.alert)
                    $scope.alert.hide();
                $scope.alert = $alert({
                    content: 'Errore durante l\'eliminazione',
                    animation: 'fadeZoomFadeDown',
                    type: 'error',
                    duration: 5
                });
            });
    }

    $scope.getRecurringNotes = function (viewValue) {
        if (!$scope.detailVisible)
            return null;

        return monthlyReportsDataService.getRecurringNotes($scope.detail.user.id, viewValue).then(function(respData) {
            if (respData.succeed)
                return respData.data;
            return null;
        });
    }

    $scope.init();
});