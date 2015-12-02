app.factory('appHelper', ['$rootScope', '$location', '$anchorScroll', '$filter', 'localStorageService', 'applicationGlobals', function ($rootScope, $location, $anchorScroll, $filter, localStorageService, applicationGlobals) {
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
        },
        getFormattedDate: function (date, formatString, ifNullValue) {
            var dateFormatted = $filter('date')(date, 'dd-MM-yyyy');
            return (dateFormatted != '01-01-0001')? dateFormatted : null;
        }
    }
    return helper;
}]);

app.factory('applicationGlobals', [function () {
    var applicationGlobals = {
        isLoadingData: function () { return (this.dataRequestCount > 0); },
        dataRequestCount: 0,
        dataErrorCallback: function (data) { console.log('data error callback'); },
    }

    return applicationGlobals;
}]);

app.factory('appDataService', ['$http', '$q', 'appHelper', function($http, $q, appHelper) {
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
}]);

app.service('pagerUp', ['$filter', function ($filter) {
    var data = [];
    var filteredData = [];
    var currentPageData = [];
    var searchFilter = "";
    var order = "";
    var pageSize = 1;
    var pageIndex = 0;
    var maxItems = 0;
    var pages = [];
    var advancedFilterFunction = null;

    this.setData = function (dataInput) {
        data = dataInput;
        setCurrentPageData();
    };

    this.getData = function () {
        return data;
    }

    this.getDataCount = function () {
        return data.length;
    }

    this.getFilteredData = function () {
        return filteredData;
    }

    var setCurrentPageData = function()
    {
        filteredData = $filter('filter')(data, searchFilter, false);
        if (advancedFilterFunction) {
            filteredData = _.filter(filteredData, advancedFilterFunction);
        }

        var tempData = $filter('orderBy')(filteredData, order);
        if (maxItems > 0)
            tempData = $filter('limitTo')(tempData, maxItems);

        pages = new Array();
        for (var i = 1; i <= Math.ceil(tempData.length / pageSize); i++) {
            pages.push(i);
        }

        tempData = $filter('limitTo')(tempData, pageSize, pageIndex * pageSize);
        currentPageData = tempData;
    };

    this.getCurrentPageData = function (searchFilterInput) {
        if (searchFilterInput != searchFilter)
        {
            searchFilter = searchFilterInput;
            pageIndex = 0;
            setCurrentPageData();
        }
        
        return currentPageData;
    };

    this.getPages = function () {
        return pages;
    };

    this.prevPage = function () {
        if (pageIndex > 0)
            pageIndex--;
        setCurrentPageData();
    };

    this.nextPage = function () {
        if (pageIndex < pages.length - 1)
            pageIndex++;
        setCurrentPageData();
    };

    this.setPageIndex = function (index) {
        pageIndex = index;
        setCurrentPageData();
    };

    this.getPageIndex = function () {
        return pageIndex;
    };

    this.setPageSize = function (size) {
        pageSize = size;
        setCurrentPageData();
    };

    this.getPageSize = function () {
        return pageSize;
    };

    //this.setSearchFilter = function (search) {
    //    searchFilter = search;
    //    setCurrentPageData();
    //};

    this.getSearchFilter = function () {
        return searchFilter;
    };

    this.setOrder = function (field, initialValue) {
        if (!initialValue && order == '+' + field) {
            order = '-' + field;
        }
        else {
            order = '+' + field;
        }
        setCurrentPageData();
    };

    this.getOrder = function () {
        return order;
    };

    this.setMaxItems = function (n) {
        if (n >= 0) {
            maxItems = n;
        }
    };

    this.getMaxItems = function () {
        return maxItems;
    };

    this.setAdvancedFilterFunction = function (func) {
        advancedFilterFunction = func;
    };
}]);