app.config(function ($locationProvider) {
    $locationProvider.html5Mode(true);
});

app.config(function ($stateProvider) {
    $stateProvider
    .state('Login', {
        url: "/Login",
        views: {
            "content": {
                templateUrl: resolveViewPath('Login/Login.html')
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
    });
});