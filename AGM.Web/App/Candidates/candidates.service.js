app.factory('candidatesDataService', function($http, $q) {
    return {
        getCandidates: function(callId) {
            var deferred = $q.defer();
            $http({ method: 'GET', headers: { _callId: callId }, url: 'api/Candidate/Get' }).success(function(respData, status, headers, config) {
                deferred.resolve(respData);
            }).error(function(respData, status, headers, config) {
                deferred.reject(respData);
            });
            return deferred.promise;
        }
    }
});