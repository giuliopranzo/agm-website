app.factory('monthlyReportsDataService', function ($http, $q) {
    return {
        getReportDetail: function (reportDetailId, month) {
            var deferred = $q.defer();

            if (!month)
                month = '';

            $http({ method: 'GET', url: 'api/MonthlyReport/Get?Id=' + reportDetailId + '&month=' + month}).success(function (respData, status, headers, config) {
                deferred.resolve(respData);
            }).error(function (respData, status, headers, config) {
                deferred.reject(respData);
            });
            return deferred.promise;
        },
        insertReportDetail: function(reportDetail) {
            var deferred = $q.defer();
            $http({ method: 'POST', url: 'api/MonthlyReport/Insert', data: reportDetail }).success(function (respData, status, headers, config) {
                deferred.resolve(respData);
            }).error(function (respData, status, headers, config) {
                deferred.reject(respData);
            });
            return deferred.promise;
        },
        deleteReportDetail: function (obj) {
            var deferred = $q.defer();
            $http({ method: 'POST', url: 'api/MonthlyReport/Delete', data: obj }).success(function (respData, status, headers, config) {
                deferred.resolve(respData);
            }).error(function (respData, status, headers, config) {
                deferred.reject(respData);
            });
            return deferred.promise;
        },
        getAllUsers: function () {
            var deferred = $q.defer();
            $http({ method: 'GET', url: 'api/User/GetAll' }).success(function (respData, status, headers, config) {
                deferred.resolve(respData);
            }).error(function (respData, status, headers, config) {
                deferred.reject(respData);
            });
            return deferred.promise;
        }
    }
});