angular.module('fwg.pagerUp', []);

angular.module('fwg.pagerUp').service('fwgPagerUpService', ['$filter', function ($filter) {
    var pagerUpObj = function(data, pageSizeIn, orderIn, onChangeIn) {
        var pageIndex = 0;
        var pageSize = (pageSizeIn)? pageSizeIn : 10;
        var searchFilter = '';
        var order = (orderIn)? orderIn : '';
        var dataSource = data;

        this.filteredData = [];
        this.pages = [];
        this.currentPageData = [];
        this.onChange = (onChangeIn)? onChangeIn : null;

        this.getPageIndex = function() {
            return pageIndex;
        };

        this.setPageIndex = function (index) {
            pageIndex = index;
            this.setCurrentPageData();
        };

        this.prevPage = function () {
            if (this.getPageIndex() > 0)
                pageIndex--;
            this.setCurrentPageData();
        };

        this.nextPage = function () {
            if (this.getPageIndex() < this.pages.length - 1)
                pageIndex++;
            this.setCurrentPageData();
        };

        this.getPageSize = function () {
            return pageSize;
        };

        this.setPageSize = function (size) {
            pageSize = size;
            this.setPageIndex(0);
            this.setCurrentPageData();
        };

        this.getSearchFilter = function () {
            return searchFilter;
        };

        this.setSearchFilter = function (filter) {
            searchFilter = filter;
            this.setCurrentPageData();
        };

        this.getOrder = function () {
            return order;
        };

        this.setOrder = function (orderIn) {
            order = orderIn;
            this.setCurrentPageData();
        };

        this.getDataSource = function () {
            return dataSource;
        };

        this.setDataSource = function (data) {
            dataSource = data;
            this.pages = new Array();
            this.setPageIndex(0);
            this.setCurrentPageData();
        };

        this.setCurrentPageData = function () {
            this.filteredData = (this.getSearchFilter() != '') ? $filter('filter')(this.getDataSource(), this.getSearchFilter(), false) : angular.copy(this.getDataSource());
            
            //Init pages collection only if number of pages changed
            if (!this.pages || this.pages.length != Math.ceil(this.filteredData.length / this.getPageSize())) {
                this.pages = new Array();
                for (var i = 1; i <= Math.ceil(this.filteredData.length / this.getPageSize()) ; i++) {
                    this.pages.push({
                        pageNumber: i,
                        searchFilter: this.getSearchFilter(),
                        order: this.getOrder(),
                        data: []
                    });
                };
            }
            
            if (this.pages[this.getPageIndex()].data.length == 0 || this.pages[this.getPageIndex()].searchFilter != this.getSearchFilter() || this.pages[this.getPageIndex()].order != this.getOrder()) {
                var tempData = (this.getOrder()) ? $filter('orderBy')(this.filteredData, this.getOrder()) : this.filteredData;
                this.pages[this.getPageIndex()].data = tempData.splice(this.getPageIndex() * this.getPageSize(), this.getPageSize());
                this.pages[this.getPageIndex()].searchFilter = this.getSearchFilter();
                this.pages[this.getPageIndex()].order = this.getOrder();
            }

            this.currentPageData = this.pages[this.getPageIndex()].data;
            if (this.onChange)
                this.onChange({
                    currentPageData: this.currentPageData
                });
        };

        this.setCurrentPageData();
    };

    this.init = function(data, pageSize, order, onChange) {
        return new pagerUpObj(data, pageSize, order, onChange);
    }
}]);

angular.module('fwg.pagerUp').directive('fwgPager', [function() {
    return {
        template: '<div style="margin-bottom: 15px;" ng-show="pager.filteredData.length > 0"> <div class="btn-group" style="float: left; margin: 0 0 0 15px;"> <label class="btn btn-default" ng-disabled="pager.getPageIndex()==0" ng-click="pager.prevPage()"><i class="fa fa-angle-left"></i></label> <label class="btn pagerPageButton" ng-class="{\'btn-primary\':pager.getPageIndex()==$index, \'btn-default\':pager.getPageIndex()!=$index}" ng-disabled="pager.getPageIndex()==$index" ng-click="pager.setPageIndex($index)" ng-repeat="page in pager.pages" ng-bind="page.pageNumber"></label> <label class="btn btn-default" ng-disabled="pager.getPageIndex()==pager.pages.length-1" ng-click="pager.nextPage()"><i class="fa fa-angle-right"></i></label>  </div> <div class="clearfix"></div> </div>  <div class="clearfix"></div>',
        restrict: 'E',
        scope: {
            fwgPagerData: '=',
            fwgPagerOrder: '=',
            fwgPagerPageSize: '=',
            fwgPagerOnChange: '&'
},
        controller: ['$scope', 'fwgPagerUpService', function($scope, fwgPagerUpService) {
            $scope.init = function() {
                if ($scope.fwgPagerData) {
                    $scope.pager = fwgPagerUpService.init($scope.fwgPagerData, $scope.fwgPagerPageSize, $scope.fwgPagerOrder, $scope.fwgPagerOnChange);
                    //$scope.pager.onChange = $scope.fwgPagerOnChange;
                    //if ($scope.fwgPagerOrder) {
                    //    $scope.pager.setOrder($scope.fwgPagerOrder);
                    //}
                }

                $scope.$watch('fwgPagerData', function (newValue, oldValue) {
                    if (newValue)
                        $scope.pager.setDataSource(newValue);
                }, true);

                $scope.$watch('fwgPagerOrder', function (newValue, oldValue) {
                    if (newValue)
                        $scope.pager.setOrder(newValue);
                }, true);
            };

            $scope.init();
        }]
    }
}]);