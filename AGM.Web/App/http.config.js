app.config(function($httpProvider) {
    $httpProvider.defaults.withCredentials = true;
    $httpProvider.defaults.xsrfHeaderName = 'SSID';
    $httpProvider.defaults.xsrfCookieName = 'SSID';

    $httpProvider.interceptors.push(function($q, $rootScope, $location, appHelper, authenticationHelper) {
        return {
            'request': function(config) {
                appHelper.setLoadingData(true, ((config.headers._callId) ? config.headers._callId : ''));

                config.headers = config.headers || {};
                if (authenticationHelper.getAuthToken()) {
                    config.headers.SSTKN = authenticationHelper.getAuthToken();
                }
                config.headers.requestResource = $location.$$url;

                return config;
            },
            'response': function(response) {
                if (response && response.data && response.data._authExp)
                    appHelper.setSESSIONEXP(response._authExp);
                appHelper.setLoadingData(false, ((response.config.headers._callId) ? response.config.headers._callId : ''));
                return response;
            },
            'requestError': function(rejection) {
                appHelper.setLoadingData(false, ((rejection.config.headers._callId) ? rejection.config.headers._callId : ''));
                return $q.reject(rejection);
            },
            'responseError': function(response) {
                //if (response && response.status != 200)
                //    appHelper.comFailed(response.status, response.data);
                appHelper.setLoadingData(false, ((response.config.headers._callId) ? response.config.headers._callId : ''));
                return $q.reject(response);
            }
        };
    });
});