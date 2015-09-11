var viewBasePath = '/App/';
var controllerBasePath = '/App/';
var factoryBasePath = '/App/';

var app = angular.module('agm', ['ngAnimate', 'ngSanitize', 'ui.router', 'ui.bootstrap', 'LocalStorageModule', 'mgcrea.ngStrap', 'angularFileUpload', 'textAngular', 'fwg.pagerUp']);

function resolveViewPath(viewName) {
    return viewBasePath + viewName;
}

function resolveControllerPath(controllerName) {
    return controllerBasePath + controllerName;
}

function resolveFactoryPath(factoryName) {
    return factoryBasePath + factoryName;
}

app.controller("main", function ($scope, $rootScope, $location, $state, $stateParams, $filter, authenticationDataService, authenticationHelper) {

    $rootScope.$on('loader_show', function (event, callId) {
        if (callId == 'mr_detail' || callId == 'usr_detail')
            $scope.loading = true;
    });

    $rootScope.$on('loader_hide', function (event, callId) {
        if (callId == 'mr_detail' || callId == 'usr_detail')
            $scope.loading = false;
    });

    $scope.user = '';
    $scope.authenticated = false;
    $scope.showMainMenu = false;

    $scope.ddUser = [
        {
            "text": "<i class=\"fa fa-user\"></i>&nbsp;Profilo",
            "click": "goToMineProfile()"
        },
        {
            "divider": true
        },
        {
            "text": "<i class=\"fa fa-sign-out\"></i>&nbsp;Logout",
            "href": "/backoffice/Logout"
        }
    ];

    $scope.ddSettings = [
        {
            "text": "<i class=\"fa fa-user\"></i>&nbsp;Festività",
            "href": "/Festivity"
        },
        {
            "text": "<i class=\"fa fa-user\"></i>&nbsp;Utenti",
            "href": "/Users"
        }
    ];

    $rootScope.$on('$stateChangeSuccess',
            function (event, toState, toParams, fromState, fromParams) {
                if (fromState.name == "Login" && toState.name != 'Login')
                    $scope.getCurrentUser($location.path());
                switch (toState.name) {
                    case 'Index':
                        if ($scope.user == '')
                            $scope.getCurrentUser();
                        break;
                    case 'Logout':
                        authenticationHelper.deleteAuthToken();
                        $scope.user = '';
                        $scope.authenticated = false;
                        $location.path('/');
                        break;
                }
            });

    $scope.init = function () {
        if ($scope.user == '')
            $scope.getCurrentUser($location.path());
    };

    $scope.getCurrentUser = function(returnPath) {
        authenticationDataService.getCurrentUser('')
            .then(function(resp) {
                $scope.user = resp.data;
                $scope.authenticated = true;
            })
            .catch(function () {
                if (returnPath && returnPath != '/Login')
                    $location.url('/Login?returnPath=' + returnPath);
                else
                    $state.go('Login');
            }
            );
    };

    $scope.goToMonthlyReports = function () {
        $scope.showMainMenu = false;
        $location.path('/MonthlyReports/' + $scope.user.id + '/' + $filter('date')(new Date(), 'yyyy-MM'));
    };

    $scope.goToMineProfile = function() {
        $scope.showMainMenu = false;
        $location.path('/Users/' + $scope.user.id);
    }

    $scope.toggleMainMenu = function () {
        $scope.showMainMenu = !$scope.showMainMenu;
    };

    $scope.goTo = function (state) {
        $scope.showMainMenu = false;
        $state.go(state);
    };

    $scope.init();
});

app.directive('printButton', [function () {
    return {
        link: function (scope, element, attrs) {
            element.click(function() {
                window.print();
            });
        }
    };
}]);
