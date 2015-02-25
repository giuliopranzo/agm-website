app.factory('usersDataService', function ($http, $q) {
    return {
        getAllUsers: function (callId) {
            var deferred = $q.defer();
            $http({ method: 'GET', headers: { _callId: callId }, url: 'api/User/GetAll' }).success(function (respData, status, headers, config) {
                deferred.resolve(respData);
            }).error(function (respData, status, headers, config) {
                deferred.reject(respData);
            });
            return deferred.promise;
        },
        getUserDetail: function (callId, id) {
            var deferred = $q.defer();
            $http({ method: 'GET', headers: { _callId: callId }, url: 'api/User/GetDetail?id=' + id }).success(function (respData, status, headers, config) {
                deferred.resolve(respData);
            }).error(function (respData, status, headers, config) {
                deferred.reject(respData);
            });
            return deferred.promise;
        },
        setUser: function (callId, userObj) {
            var deferred = $q.defer();
            $http({ method: 'POST', headers: { _callId: callId }, url: 'api/User/Set', data: userObj }).success(function (respData, status, headers, config) {
                deferred.resolve(respData);
            }).error(function (respData, status, headers, config) {
                deferred.reject(respData);
            });
            return deferred.promise;
        },
        deleteUser: function (callId, id) {
            var deferred = $q.defer();
            $http({ method: 'POST', headers: { _callId: callId }, url: 'api/User/Delete', data: id }).success(function (respData, status, headers, config) {
                deferred.resolve(respData);
            }).error(function (respData, status, headers, config) {
                deferred.reject(respData);
            });
            return deferred.promise;
        },
        userExists: function (callId, email) {
            var deferred = $q.defer();
            $http({ method: 'GET', headers: { _callId: callId }, url: 'api/User/UserExists?email=' + email }).success(function (respData, status, headers, config) {
                deferred.resolve(respData);
            }).error(function (respData, status, headers, config) {
                deferred.reject(respData);
            });
            return deferred.promise;
        }
    }
});