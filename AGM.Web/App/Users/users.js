app.controller('users', function ($rootScope, $scope, $alert, $location, $state, $filter, usersSource, monthlyReportsDataService, appHelper) {
    $rootScope.$on('$stateChangeSuccess', function (event, toState, toParams, fromState, fromParams) {
        if (toState.name == 'Users') {
            appHelper.scrollTo('reportTop');
        }
    });

    $rootScope.$on('loader_show', function (event, callId) {
        if (callId == 'mr_detail')
            $scope.loading_detail = true;
    });

    $rootScope.$on('loader_hide', function (event, callId) {
        if (callId == 'mr_detail')
            $scope.loading_detail = false;
    });

    $scope.init = function () {
        if (usersSource)
            $scope.users = usersSource;
    };

    $scope.showLetter = function (index) {
        if (index == 0 || $scope.usersFiltered[index].name.substr(0, 1) != $scope.usersFiltered[index - 1].name.substr(0, 1))
            return true;
        return false;
    };

    $scope.getFirstChar = function (value) {
        return value.substr(0, 1);
    };

    $scope.init();
});