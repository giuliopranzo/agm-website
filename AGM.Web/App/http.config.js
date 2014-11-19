app.config(function($httpProvider) {
    $httpProvider.defaults.withCredentials = true;
    $httpProvider.defaults.xsrfHeaderName = 'SOMHTOKEN';
    $httpProvider.defaults.xsrfCookieName = 'SOMSESSID';

    $httpProvider.interceptors.push(function($q, $rootScope, AppHelper) {
        return {
            'request': function(config) {
                AppHelper.setLoadingData(true);
                return config;
            },
            'response': function(response) {
                if (response && response.data && response.data._authExp)
                    AppHelper.setSOMSESSEXP(response._authExp);
                AppHelper.setLoadingData(false);
                return response;
            },
            'requestError': function(rejection) {
                AppHelper.setLoadingData(false);
                return $q.reject(rejection);
            },
            'responseError': function(response) {
                if (response && response.status != 200)
                    AppHelper.comFailed(response.status, response.data);
                AppHelper.setLoadingData(false);
                return $q.reject(response);
            }
        };
    });
});