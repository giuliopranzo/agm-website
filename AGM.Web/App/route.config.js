app.config(function($stateProvider) {
    $stateProvider
        .state('Login', {
            url: "/Login?returnPath",
            views: {
                "content": {
                    templateUrl: resolveViewPath('Authentication/Login.html')
                },
                "modal": {
                    template: ''
                }
            }
        })
        .state('Logout', {
            url: "/Logout",
            views: {
                "content": {
                    templateUrl: resolveViewPath('Authentication/Login.html')
                },
                "modal": {
                    template: ''
                }
            }
        })
        .state('MonthlyReports', {
            url: "/MonthlyReports",
            views: {
                "content": {
                    templateUrl: resolveViewPath('MonthlyReports/MonthlyReports.html'),
                    controller: 'monthlyReports'
                },
                "modal": {
                    template: ''
                }
            },
            resolve: {
                usersSource: function(monthlyReportsDataService) {
                    return monthlyReportsDataService.getAllUsers().then(function(respData) {
                        return respData.data;
                    });
                }
            }
        })
        .state('MonthlyReports.detail', {
            url: "/:reportId/:selectedDate",
            resolve: {
                userReportSource: function($stateParams, monthlyReportsDataService) {
                    return monthlyReportsDataService.getReportDetail($stateParams.reportId, $stateParams.selectedDate).then(function (respData) {
                        $stateParams.userReportSource = respData.data;
                        //$scope.detail = respData.data;
                        //$scope.selectedDate = respData.data.currentmonth;
                        //$location.path('/MonthlyReports/' + $scope.reportId + '/' + $filter('date')($scope.selectedDate, 'yyyy-MM'));
                    });
                }
            }
        })
        .state('JobAds', {
            url: "/JobAds",
            views: {
                "content": {
                    template: ''
                },
                "modal": {
                    template: ''
                }
            }
        })
        .state('JobApplicants', {
            url: "/JobApplicants",
            views: {
                "content": {
                    template: ''
                },
                "modal": {
                    template: ''
                }
            }
        })
        .state('Profile', {
            url: "/Profile",
            views: {
                "content": {
                    template: ''
                },
                "modal": {
                    template: ''
                }
            }
        })
        .state('Festivity', {
            url: "/Festivity",
            views: {
                "content": {
                    template: ''
                },
                "modal": {
                    template: ''
                }
            }
        })
        .state('Index', {
            url: "/",
            views: {
                "content": {
                    template: ''
                },
                "modal": {
                    template: ''
                }
            }
        });
});