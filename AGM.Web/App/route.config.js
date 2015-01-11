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
        .state('Users', {
            url: "/Users",
            views: {
                "content": {
                    templateUrl: resolveViewPath('Users/Users.html'),
                    controller: 'users'
                },
                "modal": {
                    template: ''
                }
            },
            resolve: {
                usersSource: function (usersDataService) {
                    return usersDataService.getAllUsers().then(function (respData) {
                        if (respData.succeed)
                            return respData.data;
                        
                        return null;
                    });
                }
            }
        })
        .state('UserDetail', {
            url: "/Users/:userId",
            views: {
                "content": {
                    templateUrl: resolveViewPath('UserDetail/UserDetail.html'),
                    controller: 'userDetail'
                },
                "modal": {
                    template: ''
                }
            },
            resolve: {
                userSource: function ($stateParams, usersDataService) {
                    return usersDataService.getUserDetail($stateParams.userId).then(function (respData) {
                        if (respData.succeed)
                            return respData.data;

                        return null;
                    });
                }
            }
        })
        .state('MonthlyReports', {
            url: "/MonthlyReports/:reportId/:selectedDate",
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
                userReportSource: function($stateParams, monthlyReportsDataService) {
                    return monthlyReportsDataService.getReportDetail('mr_detail', $stateParams.reportId, $stateParams.selectedDate).then(function (respData) {
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