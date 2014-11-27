﻿app.config(function ($locationProvider) {
    $locationProvider.html5Mode(true);
});

app.config(function (localStorageServiceProvider) {
    localStorageServiceProvider.setStorageType('sessionStorage');
    localStorageServiceProvider.setPrefix('agm');
});

app.config(function($dropdownProvider) {
    angular.extend($dropdownProvider.defaults, {
        html: true
    });
})