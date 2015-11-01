app.config(['$locationProvider', function ($locationProvider) {
    $locationProvider.html5Mode(true);
}]);

app.config(['localStorageServiceProvider', function (localStorageServiceProvider) {
    localStorageServiceProvider.setStorageType('sessionStorage');
    localStorageServiceProvider.setPrefix('agm');
}]);

app.config(['$dropdownProvider', function($dropdownProvider) {
    angular.extend($dropdownProvider.defaults, {
        html: true
    });
}])