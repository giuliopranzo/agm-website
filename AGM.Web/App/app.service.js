app.factory('appHelper', function ($rootScope, localStorageService, applicationGlobals) {
    var helper = {
        setSESSIONEXP: function (exp) {
            $.cookie('SESSIONEXP', exp, { expires: 7, path: '/' });
        },
        deleteSESSIONEXP: function () {
            $.removeCookie('SESSIONEXP', { path: '/' });
        },
        setCookie: function (name, value, path, cookieExpiration) {
            if (cookieExpiration)
                $.cookie(name, value, { expires: cookieExpiration, path: path });
            else
                $.cookie(name, value, { path: path });
        },
        deleteCookie: function (name, cookiePath) {
            $.removeCookie(name, { path: cookiePath });
        },
        getCookie: function (name) {
            return $.cookie(name);
        },
        setLocalStorage: function(name, value) {
            localStorageService.set(name, value);
        },
        getLocalStorage: function (name) {
            return localStorageService.get(name);
        },
        deleteLocalStorage: function (name) {
            localStorageService.remove(name);
        },
        loaderShow: function (value) {
            if (value)
                $rootScope.$broadcast("loader_show");
            else
                $rootScope.$broadcast("loader_hide");
        },
        setLoadingData: function (value) {
            if (value) {
                applicationGlobals.dataRequestCount++;
                this.loaderShow(true);
            }
            else {
                applicationGlobals.dataRequestCount--;
                if (!applicationGlobals.isLoadingData())
                    this.loaderShow(false);
            }
        },
        comFailed: function (status, message) {
            if (message.indexOf('[SysError]') == -1)
                $rootScope.$broadcast("com_error", { code: status, message: message });
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