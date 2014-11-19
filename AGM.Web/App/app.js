var viewBasePath = '/App/';
var controllerBasePath = '/App/';
var factoryBasePath = '/App/';

var app = angular.module('agm', ['ui.router', 'satellizer']);

function resolveViewPath(viewName) {
    return viewBasePath + viewName;
}

function resolveControllerPath(controllerName) {
    return controllerBasePath + controllerName;
}

function resolveFactoryPath(factoryName) {
    return factoryBasePath + factoryName;
}

app.controller("main", function ($scope, AppHelper) {
    $scope.greetMe = 'World';
    AppHelper.setCookie("test", "ok", 7, "/");
});

