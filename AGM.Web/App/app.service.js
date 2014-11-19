app.factory('AppHelper', function ($rootScope, ApplicationGlobals) {
    var helper = {
        setSESSIONEXP: function (exp) {
            $.cookie('SESSIONEXP', exp, { expires: 7, path: '/' });
        },
        deleteSESSIONEXP: function () {
            $.removeCookie('SESSIONEXP', { path: '/' });
        },
        setCookie: function (name, value, cookeExpiration, cookiePath) {
            $.cookie(name, value, { expires: cookeExpiration, path: cookiePath });
        },
        deleteCookie: function (name, cookiePath) {
            $.removeCookie(name, { path: cookiePath });
        },
        getCookie: function (name) {
            return $.cookie(name);
        },
        loaderShow: function (value) {
            if (value)
                $rootScope.$broadcast("loader_show");
            else
                $rootScope.$broadcast("loader_hide");
        },
        setLoadingData: function (value) {
            if (value) {
                ApplicationGlobals.dataRequestCount++;
                this.loaderShow(true);
            }
            else {
                ApplicationGlobals.dataRequestCount--;
                if (!ApplicationGlobals.isLoadingData())
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

app.factory('ApplicationGlobals', function () {
    var applicationGlobals = {
        isLoadingData: function () { return (this.dataRequestCount > 0); },
        dataRequestCount: 0,
        dataErrorCallback: function (data) { console.log('data error callback'); },
    }

    return applicationGlobals;
});