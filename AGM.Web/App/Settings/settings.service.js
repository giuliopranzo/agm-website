app.factory('settingsDataService', function($http, $q) {
    return {
        getHourReasons: function(callId) {
            var deferred = $q.defer();
            $http({ method: 'GET', headers: { _callId: callId }, url: 'api/Settings/GetHourReasons' }).success(function(respData, status, headers, config) {
                deferred.resolve(respData);
            }).error(function(respData, status, headers, config) {
                deferred.reject(respData);
            });
            return deferred.promise;
        },

        insertHourReason: function(callId, obj) {
            var deferred = $q.defer();
            $http({ method: 'POST', headers: { _callId: callId }, url: 'api/Settings/InsertHourReason', data: obj }).success(function(respData, status, headers, config) {
                deferred.resolve(respData);
            }).error(function(respData, status, headers, config) {
                deferred.reject(respData);
            });
            return deferred.promise;
        },

        updateHourReason: function(callId, obj) {
            var deferred = $q.defer();
            $http({ method: 'POST', headers: { _callId: callId }, url: 'api/Settings/UpdateHourReason', data: obj }).success(function(respData, status, headers, config) {
                deferred.resolve(respData);
            }).error(function(respData, status, headers, config) {
                deferred.reject(respData);
            });
            return deferred.promise;
        },

        deleteHourReason: function(callId, obj) {
            var deferred = $q.defer();
            $http({ method: 'POST', headers: { _callId: callId }, url: 'api/Settings/DeleteHourReason', data: obj }).success(function(respData, status, headers, config) {
                deferred.resolve(respData);
            }).error(function(respData, status, headers, config) {
                deferred.reject(respData);
            });
            return deferred.promise;
        },

        getFestivities: function(callId) {
            var deferred = $q.defer();
            $http({ method: 'GET', headers: { _callId: callId }, url: 'api/Settings/GetFestivities' }).success(function(respData, status, headers, config) {
                deferred.resolve(respData);
            }).error(function(respData, status, headers, config) {
                deferred.reject(respData);
            });
            return deferred.promise;
        },

        insertFestivity: function (callId, obj) {
            var deferred = $q.defer();
            $http({ method: 'POST', headers: { _callId: callId }, url: 'api/Settings/InsertFestivity', data: obj }).success(function (respData, status, headers, config) {
                deferred.resolve(respData);
            }).error(function (respData, status, headers, config) {
                deferred.reject(respData);
            });
            return deferred.promise;
        },

        deleteFestivity: function(callId, obj) {
            var deferred = $q.defer();
            $http({ method: 'POST', headers: { _callId: callId }, url: 'api/Settings/DeleteFestivity', data: obj }).success(function(respData, status, headers, config) {
                deferred.resolve(respData);
            }).error(function(respData, status, headers, config) {
                deferred.reject(respData);
            });
            return deferred.promise;
        }
    }
});
