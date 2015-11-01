app.controller('authentication', ['$rootScope', '$scope', '$alert', '$location', '$stateParams', 'authenticationDataService', 'authenticationHelper', function($rootScope, $scope, $alert, $location, $stateParams, authenticationDataService, authenticationHelper) {
    $rootScope.$on('loader_show', function (event, callId) {
        if (callId == 'auth_logcheck')
            $scope.checking_data = true;
    });

    $rootScope.$on('loader_hide', function (event, callId) {
        if (callId == 'auth_logcheck')
            $scope.checking_data = false;
        
    });

    $scope.checking_data = false;
    $scope.email = '';
    $scope.password = '';
    $scope.returnPath = $stateParams.returnPath;
    if (!$scope.returnPath)
        $scope.returnPath = '/';

    $scope.alert = $alert({
        animation: 'fadeZoomFadeDown',
        duration: 5,
        show: false
    });

    $scope.authenticate = function () {
        var postData = { Email: $scope.email, Password: $scope.password };
        authenticationDataService.authenticate('auth_logcheck', postData)
        .then(function (resp) {
            if (resp.succeed) {
                $scope.alert.hide();
                $scope.alert = $alert({
                    content: 'Login effettuato con successo',
                    animation: 'fadeZoomFadeDown',
                    type: 'info',
                    duration: 5
                });
                $location.url($scope.returnPath);
            } else {
                $scope.alert.hide();
                $scope.alert = $alert({
                    content: resp.errors[0].message,
                    animation: 'fadeZoomFadeDown',
                    type: 'error',
                    duration: 5
                });
            }
        })
        .catch(function () {
            $scope.alert.hide();
            $alert({
                content: response.data.message,
                animation: 'fadeZoomFadeDown',
                type: 'error',
                duration: 30
            });
        });
    };

    $scope.isAuthenticated = function () {
        authenticationDataService.isAuthenticated('auth_logcheck').then(function (resp) {
            $alert({ content: resp.data.toString(), type: 'material', duration: 3 });
        });
    };
}]);