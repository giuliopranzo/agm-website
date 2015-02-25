app.factory('jobAdsDataService', function($http, $q) {
    return {
        getJobAds: function(callId) {
            var deferred = $q.defer();
            $http({ method: 'GET', headers: { _callId: callId }, url: 'api/JobAd/Get' }).success(function(respData, status, headers, config) {
                deferred.resolve(respData);
            }).error(function(respData, status, headers, config) {
                deferred.reject(respData);
            });
            return deferred.promise;
        },
        getJobAdText: function(callId, jobAdId) {
            var deferred = $q.defer();
            $http({ method: 'GET', headers: { _callId: callId }, url: 'api/JobAd/GetText?id=' + jobAdId }).success(function(respData, status, headers, config) {
                deferred.resolve(respData);
            }).error(function(respData, status, headers, config) {
                deferred.reject(respData);
            });
            return deferred.promise;
        },
        saveJobAd: function (callId, objToSave) {
            var deferred = $q.defer();
            $http({ method: 'POST', headers: { _callId: callId }, url: 'api/JobAd/Set', data: objToSave }).success(function (respData, status, headers, config) {
                deferred.resolve(respData);
            }).error(function (respData, status, headers, config) {
                deferred.reject(respData);
            });
            return deferred.promise;
        },
    }
});