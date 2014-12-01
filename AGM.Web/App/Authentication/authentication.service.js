﻿app.factory('authenticationDataService', function ($http, $q, authenticationContainer, authenticationHelper) {
    return {
        authenticate: function (authData) {
            var deferred = $q.defer();

            $http({ method: 'POST', url: 'api/Auth/Login', data: authData }).success(function (respData, status, headers, config) {
                authenticationHelper.setAuthToken(respData.token, true);
                deferred.resolve(respData);
            }).error(function (respData, status, headers, config) {
                deferred.reject(respData);
            });
            return deferred.promise;
        },
        isAuthenticated: function (glimpseObj) {
            var deferred = $q.defer();

            $http({ method: 'GET', url: 'api/Auth/IsAuthenticated' }).success(function (respData, status, headers, config) {
                deferred.resolve(respData);
            }).error(function (respData, status, headers, config) {
                deferred.reject(respData);
            });
            return deferred.promise;
        },
        getCurrentUser: function () {
            var deferred = $q.defer();

            $http({ method: 'GET', url: 'api/Auth/GetCurrentUser' }).success(function (respData, status, headers, config) {
                authenticationContainer.currentUser = respData.data;
                deferred.resolve(respData);
            }).error(function (respData, status, headers, config) {
                deferred.reject(respData);
            });
            return deferred.promise;
        }
    }
});

app.factory('authenticationContainer', function() {
    var container = {
        currentUser: ''
    };

    return container;
});

app.factory('authenticationHelper', function ($q, appHelper) {
        return {
            setAuthToken: function (value, cancelOnClose) {
                appHelper.setCookie('SSTKN', value, '/', ((cancelOnClose) ? null : 30));
            },
            getAuthToken: function () {
                return appHelper.getCookie('SSTKN');
            },
            deleteAuthToken: function () {
                appHelper.deleteCookie('SSTKN', '/');
            }
        }
    }
);