app.factory('monthlyReportsDataService', function ($http, $q) {
    return {
        getReportDetail: function (callId, reportDetailId, month) {
            var deferred = $q.defer();

            if (!month)
                month = '';

            $http({ method: 'GET', headers: { _callId: callId }, url: 'api/MonthlyReport/Get?Id=' + reportDetailId + '&month=' + month }).success(function (respData, status, headers, config) {
                deferred.resolve(respData);
            }).error(function (respData, status, headers, config) {
                deferred.reject(respData);
            });
            return deferred.promise;
        },
        insertReportDetail: function (callId, reportDetail) {
            var deferred = $q.defer();
            $http({ method: 'POST', headers: { _callId: callId }, url: 'api/MonthlyReport/Insert', data: reportDetail }).success(function (respData, status, headers, config) {
                deferred.resolve(respData);
            }).error(function (respData, status, headers, config) {
                deferred.reject(respData);
            });
            return deferred.promise;
        },
        deleteReportDetail: function (callId, obj) {
            var deferred = $q.defer();
            $http({ method: 'POST', headers: { _callId: callId }, url: 'api/MonthlyReport/Delete', data: obj }).success(function (respData, status, headers, config) {
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
        },
        getRecurringNotes: function(userId, viewValue) {
            var deferred = $q.defer();
            $http({ method: 'GET', url: 'api/MonthlyReport/GetRecurringNotes?id=' + userId + '&value=' + viewValue }).success(function (respData, status, headers, config) {
                deferred.resolve(respData);
            }).error(function (respData, status, headers, config) {
                deferred.reject(respData);
            });
            return deferred.promise;
        },
        autocompleteReport: function (callId, reportDetailId, month) {
            var deferred = $q.defer();

            $http({ method: 'POST', headers: { _callId: callId }, url: 'api/MonthlyReport/Autocomplete', data: { id: reportDetailId, month: month } }).success(function (respData, status, headers, config) {
                deferred.resolve(respData);
            }).error(function (respData, status, headers, config) {
                deferred.reject(respData);
            });
            return deferred.promise;
        },
    }
});