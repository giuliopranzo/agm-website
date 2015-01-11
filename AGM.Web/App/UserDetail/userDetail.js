app.controller('userDetail', function ($rootScope, $scope, $alert, $location, $state, $filter, userSource, usersDataService, appHelper) {

    $scope.init = function () {
        if (userSource)
            $scope.user = userSource;
        else
            $state.go('Index');
    };

    $scope.init();
});