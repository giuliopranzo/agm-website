var viewBasePath = '/App/';
var controllerBasePath = '/App/';
var factoryBasePath = '/App/';
var app = angular.module('agm', ['ui.router', 'satellizer']);

app.controller("main", ['$scope', function ($scope) {
    $scope.greetMe = 'World';
}]);

function resolveViewPath(viewName) {
    return viewBasePath + viewName;
}

function resolveControllerPath(controllerName) {
    return controllerBasePath + controllerName;
}

function resolveFactoryPath(factoryName) {
    return factoryBasePath + factoryName;
}
