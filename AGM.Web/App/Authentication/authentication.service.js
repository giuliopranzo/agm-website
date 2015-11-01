app.factory('authenticationDataService', ['$http', '$q', 'authenticationContainer', 'authenticationHelper', function ($http, $q, authenticationContainer, authenticationHelper) {
    return {
        authenticate: function (callId, authData) {
            var deferred = $q.defer();

            $http({ method: 'POST', headers: { _callId: callId }, url: 'api/Auth/Login', data: authData }).success(function (respData, status, headers, config) {
                if (respData && respData.succeed)
                    authenticationHelper.setAuthToken(respData.token, true);
                deferred.resolve(respData);
            }).error(function (respData, status, headers, config) {
                deferred.reject(respData);
            });
            return deferred.promise;
        },
        isAuthenticated: function (callId) {
            var deferred = $q.defer();

            $http({ method: 'GET', headers: { _callId: callId }, url: 'api/Auth/IsAuthenticated' }).success(function (respData, status, headers, config) {
                deferred.resolve(respData);
            }).error(function (respData, status, headers, config) {
                deferred.reject(respData);
            });
            return deferred.promise;
        },
        getCurrentUser: function (callId) {
            var deferred = $q.defer();

            $http({ method: 'GET', headers: { _callId: callId }, url: 'api/Auth/GetCurrentUser' }).success(function (respData, status, headers, config) {
                authenticationContainer.currentUser = respData.data;
                deferred.resolve(respData);
            }).error(function (respData, status, headers, config) {
                deferred.reject(respData);
            });
            return deferred.promise;
        }
    }
}]);

app.factory('authenticationContainer', [function() {
    var container = {
        currentUser: ''
    };

    return container;
}]);

app.factory('authenticationHelper', ['$q', 'appHelper', function ($q, appHelper) {
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
}]);