var app = angular.module('agm', ['satellizer']);

app.controller("main", ['$scope', function ($scope) {
    $scope.greetMe = 'World';
}]);