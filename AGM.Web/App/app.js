﻿var viewBasePath = '/App/';
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

app.controller("main", function ($scope, $rootScope, $location, $state, $stateParams, $filter, appDataService, authenticationDataService, authenticationHelper, applicationGlobals) {

    $rootScope.$on('loader_show', function (event, callId) {
        if (callId == 'mr_detail' || callId == 'usr_detail')
            $scope.loading = true;
    });

    $rootScope.$on('loader_hide', function (event, callId) {
        if (callId == 'mr_detail' || callId == 'usr_detail')
            $scope.loading = false;
    });

    $rootScope.$on('$stateChangeSuccess', function (event, toState, toParams, fromState, fromParams) {
        if (applicationGlobals.xsScreenMode)
            applicationGlobals.showMainMenu = false;
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

    $scope.user = '';
    $scope.authenticated = false;
    applicationGlobals.showMainMenu = false;
    $scope.applicationGlobals = applicationGlobals;

    $scope.init = function () {
        if ($scope.user == '')
            $scope.getCurrentUser($location.path());
    };

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
        $location.path('/Users/' + $scope.user.id);
    }

    $scope.toggleMainMenu = function () {
        applicationGlobals.showMainMenu = !applicationGlobals.showMainMenu;
    };

    $scope.goTo = function (state) {
        $state.go(state);
    };

    $scope.xsWidthCheck = function (xs) {
        if (xs === true && $scope.showMainMenu) {
            applicationGlobals.showMainMenu = false;
        } else if (xs === false && !$scope.showMainMenu) {
            applicationGlobals.showMainMenu = true;
        }
        applicationGlobals.xsScreenMode = xs;
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

app.directive('widthWatch', ['applicationGlobals', function (applicationGlobals) {
    return {
        scope: {
            widthWatch: '&widthWatch'
        },
        link: function (scope, element, attrs) {
            scope.applicationGlobals = applicationGlobals;
            scope.__appWidth = -1;
            scope.widthLimit = 992;

            scope.widthWatchDo = function () {
                var newWidth = scope.applicationGlobals.windowWidth;

                var targetMethod = scope.widthWatch();
                if ((scope.__appWidth >= scope.widthLimit && newWidth < scope.widthLimit) || (scope.__appWidth == -1 && newWidth < scope.widthLimit)) {
                    targetMethod(true);
                } else if (scope.__appWidth < scope.widthLimit && newWidth >= scope.widthLimit) {
                    targetMethod(false);
                }

                scope.__appWidth = newWidth;
            };

            $(window).resize(function () {
                scope.applicationGlobals.windowWidth = $(window).width();
                scope.widthWatchDo();
                scope.$apply();
            });

            scope.applicationGlobals.windowWidth = $(window).width();
            scope.widthWatchDo(); //Execute check function for the first time
        }
    };
}]);

app.directive('scrollbarDisable', [function() {
    return {
        link: function (scope, element, attrs) {
            scope.$watch(attrs.scrollbarDisable, function (newValue, oldValue) {
                if (newValue === true) {
                    $(attrs.scrollbarTarget).attr('style','overflow:hidden !important;');
                } else {
                    $(attrs.scrollbarTarget).attr('style','');
                }
            });
        }
    };
}]);
