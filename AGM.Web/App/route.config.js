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
                    return usersDataService.getAllUsers('usr_detail').then(function (respData) {
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
                    return usersDataService.getUserDetail('usr_detail', $stateParams.userId).then(function (respData) {
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
                    });
                }
            }
        })
        .state('JobAds', {
            url: "/JobAds",
            views: {
                "content": {
                    templateUrl: resolveViewPath('JobAds/JobAds.html'),
                    controller: 'jobAds'
                },
                "modal": {
                    template: ''
                }
            },
            resolve: {
                jobAdsSource: function ($stateParams, jobAdsDataService) {
                    return jobAdsDataService.getJobAds('ja_main').then(function (respData) {
                        return respData.data;
                    });
                }
            }
        })
        .state('JobAds.Detail', {
            url: "/:adId",
            views: {
                "ja_detail": {
                    templateUrl: resolveViewPath('JobAds/Detail.html')
                }
            },
            resolve: {
                jobAdTextSource: function ($stateParams, jobAdsDataService) {
                    return jobAdsDataService.getJobAdText('ja_main', $stateParams.adId).then(function (respData) {
                        if (respData.succeed) {
                            $stateParams.jobAdTextSource = respData.data;
                        } else {
                            $stateParams.jobAdTextSource = null;
                        }
                    });
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
        .state('Settings', {
            url: "/Settings",
            views: {
                "content": {
                    templateUrl: resolveViewPath('Settings/Settings.html'),
                    controller: 'settings'
                },
                "modal": {
                    template: ''
                }
            },
            resolve: {
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