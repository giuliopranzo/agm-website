app.factory('appHelper', function ($rootScope, $location, $anchorScroll, localStorageService, applicationGlobals) {
    var helper = {
        setSESSIONEXP: function(exp) {
            $.cookie('SESSIONEXP', exp, { expires: 7, path: '/' });
        },
        deleteSESSIONEXP: function() {
            $.removeCookie('SESSIONEXP', { path: '/' });
        },
        setCookie: function(name, value, path, cookieExpiration) {
            if (cookieExpiration)
                $.cookie(name, value, { expires: cookieExpiration, path: path });
            else
                $.cookie(name, value, { path: path });
        },
        deleteCookie: function(name, cookiePath) {
            $.removeCookie(name, { path: cookiePath });
        },
        getCookie: function(name) {
            return $.cookie(name);
        },
        setLocalStorage: function(name, value) {
            localStorageService.set(name, value);
        },
        getLocalStorage: function(name) {
            return localStorageService.get(name);
        },
        deleteLocalStorage: function(name) {
            localStorageService.remove(name);
        },
        loaderShow: function(value, callId) {
            if (value)
                $rootScope.$broadcast("loader_show", callId);
            else
                $rootScope.$broadcast("loader_hide", callId);
        },
        setLoadingData: function(value, callId) {
            if (value) {
                applicationGlobals.dataRequestCount++;
                this.loaderShow(true, callId);
            } else {
                applicationGlobals.dataRequestCount--;
                //if (!applicationGlobals.isLoadingData())
                this.loaderShow(false, callId);
            }
        },
        comFailed: function(status, message) {
            if (message.indexOf('[SysError]') == -1)
                $rootScope.$broadcast("com_error", { code: status, message: message });
        },
        scrollTo: function(id) {
            var currLocation = $location.url();
            $location.hash(id);
            $anchorScroll();
            $location.url(currLocation);
        }
    }
    return helper;
});

app.factory('applicationGlobals', function () {
    var applicationGlobals = {
        isLoadingData: function () { return (this.dataRequestCount > 0); },
        dataRequestCount: 0,
        dataErrorCallback: function (data) { console.log('data error callback'); },
    }

    return applicationGlobals;
});

app.factory('appDataService', function($http, $q) {
    return {
        getLocation: function(callId, viewValue) {
            var deferred = $q.defer();
            var params = {callback: 'JSON_CALLBACK', name_startsWith: viewValue, lang: 'IT', featureClass: 'A', featureCode: 'ADM3', country: 'it', orderby: 'relevance', username: 'nandowalter' };
            $http.jsonp('http://api.geonames.org/searchJSON',{ method: 'GET', headers: { _callId: callId }, params: params }).success(function (respData, status, headers, config) {
                deferred.resolve(respData);
            }).error(function(respData, status, headers, config) {
                deferred.reject(respData);
            });
            return deferred.promise;

        }
    }
});