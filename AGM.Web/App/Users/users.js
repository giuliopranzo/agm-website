app.controller('users', function ($rootScope, $scope, $alert, $location, $state, $filter, usersSource, usersDataService, appHelper) {

    $rootScope.$on('$stateChangeSuccess', function (event, toState, toParams, fromState, fromParams) {
        if (toState.name == 'Users') {
            appHelper.scrollTo('reportTop');
        }
    });

    $rootScope.$on('loader_show', function (event, callId) {
        if (callId == 'mr_detail' || callId == 'usr_detail')
            $scope.loading_detail = true;
    });

    $rootScope.$on('loader_hide', function (event, callId) {
        if (callId == 'mr_detail' || callId == 'usr_detail')
            $scope.loading_detail = false;
    });

    $scope.init = function () {
        if (usersSource)
            $scope.users = usersSource;
        else
            $state.go('Index');
    };

    $scope.showLetter = function (index) {
        if (index == 0 || $scope.usersFiltered[index].name.substr(0, 1) != $scope.usersFiltered[index - 1].name.substr(0, 1))
            return true;
        return false;
    };

    $scope.getFirstChar = function (value) {
        return value.substr(0, 1);
    };

    $scope.showDetail = function (id) {
        $location.path('/MonthlyReports/' + id + '/' + $filter('date')(new Date(), 'yyyy-MM'));
    };

    $scope.showUserDetail = function (id) {
        $location.path('/Users/' + id);
    };

    $scope.newUser = function() {
        $location.path('/Users/0');
    }

    $scope.init();
});

app.directive('userContextMenu', ['$dropdown', function ($dropdown) {
    return {
        link: function (scope, element, attrs) {
            var dropdown = $dropdown(element, {
                html: true
            });

            dropdown.$scope.showDetail = scope.showDetail;
            dropdown.$scope.showUserDetail = scope.showUserDetail;
            dropdown.$scope.content = [
                    { text: '<i class=\"fa fa-user\"></i>&nbsp;Profilo', click: 'showUserDetail(' + attrs.userContextMenu + ')' },
                    { text: '<i class=\"fa fa-clock-o\"></i>&nbsp;Rapportini', click: 'showDetail(' + attrs.userContextMenu + ')' }
            ];
        }
    };
}]);