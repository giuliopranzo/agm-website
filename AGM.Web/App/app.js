var viewBasePath = '/App/';
var controllerBasePath = '/App/';
var factoryBasePath = '/App/';

var app = angular.module('agm', ['ui.router', 'LocalStorageModule', 'mgcrea.ngStrap']);

function resolveViewPath(viewName) {
    return viewBasePath + viewName;
}

function resolveControllerPath(controllerName) {
    return controllerBasePath + controllerName;
}

function resolveFactoryPath(factoryName) {
    return factoryBasePath + factoryName;
}

app.controller("main", function ($scope, $location, authenticationHelper, authenticationContainer) {
    authenticationHelper.getCurrentUser().catch(
        function() { $location.path('/Login'); }
    );
});

