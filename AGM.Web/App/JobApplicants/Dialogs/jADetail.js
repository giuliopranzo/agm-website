app.controller('jADetail', ['$scope', 'detailSource', function($scope, detailSource){
    $scope.init = function () {
        $scope.detail = detailSource;
    };

    $scope.init();
}]);