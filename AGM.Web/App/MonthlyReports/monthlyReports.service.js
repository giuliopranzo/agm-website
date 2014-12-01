app.factory('monthlyReportsDataService', function ($http, $q) {
    return {
        test: function (authData) {
            var deferred = $q.defer();

            $http({ method: 'POST', url: 'api/Auth/Login', data: authData }).success(function (respData, status, headers, config) {
                authenticationHelper.setAuthToken(respData.token, true);
                deferred.resolve(respData);
            }).error(function (respData, status, headers, config) {
                deferred.reject(respData);
            });
            return deferred.promise;
        },
        getReportDetail: function (reportDetailId, month) {
            var deferred = $q.defer();

            if (!month)
                month = '';

            $http({ method: 'GET', url: 'api/MonthlyReports/Get?Id=' + reportDetailId + '&month=' + month}).success(function (respData, status, headers, config) {
                deferred.resolve(respData);
            }).error(function (respData, status, headers, config) {
                deferred.reject(respData);
            });
            return deferred.promise;
        }
    }
});