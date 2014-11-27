app.controller('authentication', function ($scope, $alert, $location, authenticationDataService, appHelper) {
    $scope.email = '';
    $scope.password = '';

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

            appHelper.setAuthToken(resp.token, true);
            })
        .catch(function() {
            $alert({
                content: response.data.message,
                animation: 'fadeZoomFadeDown',
                type: 'material',
                duration: 3
            });
        });
        
        //$auth.login({
        //    email: $scope.email,
        //    password: $scope.password
        //}).then(function () {
        //    $alert({
        //        content: 'You have successfully logged in',
        //        animation: 'fadeZoomFadeDown',
        //        type: 'material',
        //        duration: 3
        //    });
        //})
        //.catch(function (response) {
        //    $alert({
        //        content: response.data.message,
        //        animation: 'fadeZoomFadeDown',
        //        type: 'material',
        //        duration: 3
        //    });
        //});
    };

    $scope.isAuthenticated = function () {
        authenticationDataService.isAuthenticated().then(function(resp) {
            $alert({ content: resp.data.toString(), type: 'material', duration: 3 });
        });
    };

  });