app.config(function($httpProvider) {
    $httpProvider.defaults.withCredentials = true;
    $httpProvider.defaults.xsrfHeaderName = 'SSID';
    $httpProvider.defaults.xsrfCookieName = 'SSID';

    $httpProvider.interceptors.push(function($q, $rootScope, $location, appHelper, authenticationHelper) {
        return {
            'request': function(config) {
                appHelper.setLoadingData(true);

                config.headers = config.headers || {};
                if (authenticationHelper.getAuthToken()) {
                    config.headers.SSTKN = authenticationHelper.getAuthToken();
                }
                config.headers.requestResource = $location.$$url;

                return config;
            },
            'response': function(response) {
                if (response && response.data && response.data._authExp)
                    appHelper.setSOMSESSEXP(response._authExp);
                appHelper.setLoadingData(false);
                return response;
            },
            'requestError': function(rejection) {
                appHelper.setLoadingData(false);
                return $q.reject(rejection);
            },
            'responseError': function(response) {
                //if (response && response.status != 200)
                //    appHelper.comFailed(response.status, response.data);
                appHelper.setLoadingData(false);
                return $q.reject(response);
            }
        };
    });
});