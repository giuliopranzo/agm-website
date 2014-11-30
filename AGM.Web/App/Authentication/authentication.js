app.controller('authentication', function ($scope, $alert, $location, $stateParams, authenticationDataService, authenticationHelper) {
    $scope.email = '';
    $scope.password = '';
    $scope.returnPath = $stateParams.returnPath;
    if (!$scope.returnPath)
        $scope.returnPath = '/';

    $scope.authenticate = function () {
        var postData = { Email: $scope.email, Password: $scope.password };
        authenticationDataService.authenticate(postData)
        .then(function (resp) {
            $alert({
                content: 'You have successfully logged in',
                animation: 'fadeZoomFadeDown',
                type: 'material',
                duration: 3
            });

            $location.url($scope.returnPath);
        })
        .catch(function() {
            $alert({
                content: response.data.message,
                animation: 'fadeZoomFadeDown',
                type: 'material',
                duration: 3
            });
        });
    };

    $scope.isAuthenticated = function () {
        authenticationDataService.isAuthenticated().then(function(resp) {
            $alert({ content: resp.data.toString(), type: 'material', duration: 3 });
        });
    };

  });