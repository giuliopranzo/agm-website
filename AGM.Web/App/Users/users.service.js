app.factory('usersDataService', function ($http, $q) {
    return {
        getAllUsers: function () {
            var deferred = $q.defer();
            $http({ method: 'GET', url: 'api/User/GetAll' }).success(function (respData, status, headers, config) {
                deferred.resolve(respData);
            }).error(function (respData, status, headers, config) {
                deferred.reject(respData);
            });
            return deferred.promise;
        },
        getUserDetail: function (id) {
            var deferred = $q.defer();
            $http({ method: 'GET', url: 'api/User/GetDetail?id=' + id }).success(function (respData, status, headers, config) {
                deferred.resolve(respData);
            }).error(function (respData, status, headers, config) {
                deferred.reject(respData);
            });
            return deferred.promise;
        }
    }
});