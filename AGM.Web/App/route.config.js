app.config(function ($stateProvider) {
    $stateProvider
    .state('Login', {
        url: "/Login",
        views: {
            "content": {
                templateUrl: resolveViewPath('Authentication/Login.html'),
                controller: 'authentication'
            },
            "modal": {
                template: ''
            }
        }
    })
    .state('Logout', {
        url: "/Logout",
        views: {
            "content": {
                templateUrl: resolveViewPath('Authentication/Login.html')
            },
            "modal": {
                template: ''
            }
        }
    })
    .state('Detail', {
        url: "/Detail/:glimpseId",
        views: {
            "content": {
                template: '',
                controller: ''
            },
            "modal": {
                templateUrl: '',
                controller: ''
            }
        }
    })
    .state('Index', {
        url: "/",
        views: {
            "content": {
                template: '',
                controller: ''
            },
            "modal": {
                template: '',
                controller: ''
            }
        }
        //,
        //resolve: {
        //    authenticationCheck: function (authenticationDataService) {
        //        debugger;
        //        return authenticationDataService.isAuthenticated().then(function(resp) { return resp.data; });
        //    }
        //}
    });
});