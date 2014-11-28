var viewBasePath = '/App/';
var controllerBasePath = '/App/';
var factoryBasePath = '/App/';

var app = angular.module('agm', ['ngAnimate', 'ngSanitize', 'ui.router', 'LocalStorageModule', 'mgcrea.ngStrap']);

function resolveViewPath(viewName) {
    return viewBasePath + viewName;
}

function resolveControllerPath(controllerName) {
    return controllerBasePath + controllerName;
}

function resolveFactoryPath(factoryName) {
    return factoryBasePath + factoryName;
}

app.controller("main", function ($scope, $rootScope, $location, $state, $stateParams, authenticationDataService, authenticationHelper) {
    $scope.user = '';
    $scope.authenticated = false;

    $scope.ddUser = [
        {
            "text": "<i class=\"fa fa-user\"></i>&nbsp;Profilo",
            "href": "/Profile"
        },
        {
            "divider": true
        },
        {
            "text": "<i class=\"fa fa-sign-out\"></i>&nbsp;Logout",
            "href": "/Logout"
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
                switch (toState.name) {
                    case 'Index':
                        if ($scope.user == '')
                            authenticationDataService.getCurrentUser()
                                .then(function (resp) {
                                    $scope.user = resp.data;
                                    $scope.authenticated = true;
                                })
                                .catch(function () {
                                    $location.path('/Login');
                                }
                            );
                        break;
                    case 'Logout':
                        authenticationHelper.deleteAuthToken();
                        $scope.user = '';
                        $scope.authenticated = false;
                        $location.path('/');
                        break;
                }
            });
});

