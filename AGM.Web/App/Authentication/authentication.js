app.controller('authentication', function ($scope, $alert, $location, $stateParams, authenticationDataService, authenticationHelper) {
    $scope.username = '';
    $scope.password = '';
    $scope.returnPath = $stateParams.returnPath;
    if (!$scope.returnPath)
        $scope.returnPath = '/';

    $scope.authenticate = function () {
        var postData = { Username: $scope.username, Password: $scope.password };
        authenticationDataService.authenticate(postData)
        .then(function (resp) {
                if (resp.succeed) {
                    $alert({
                        content: 'Login effettuato con successo',
                        animation: 'fadeZoomFadeDown',
                        type: 'info',
                        duration: 5
                    });
                    $location.url($scope.returnPath);
                } else {
                    $alert({
                        content: resp.errors[0].message,
                        animation: 'fadeZoomFadeDown',
                        type: 'error',
                        duration: 30
                    });
                }
        })
        .catch(function() {
            $alert({
                content: response.data.message,
                animation: 'fadeZoomFadeDown',
                type: 'error',
                duration: 30
            });
        });
    };

    $scope.isAuthenticated = function () {
        authenticationDataService.isAuthenticated().then(function(resp) {
            $alert({ content: resp.data.toString(), type: 'material', duration: 3 });
        });
    };

  });