app.config(['$stateProvider', function($stateProvider) {
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
        .state('Root.Users', {
            url: "/Users",
            views: {
                "content@": {
                    templateUrl: resolveViewPath('Users/Users.html'),
                    controller: 'users'
                },
                "modal@": {
                    template: ''
                }
            },
            resolve: {
                usersSource: ['usersDataService', function (usersDataService) {
                    return usersDataService.getAllUsers('usr_detail').then(function (respData) {
                        if (respData.succeed)
                            return respData.data;
                        
                        return null;
                    });
                }]
            }
        })
        .state('Root.UserDetail', {
            url: "/Users/:userId",
            views: {
                "content@": {
                    templateUrl: resolveViewPath('UserDetail/UserDetail.html'),
                    controller: 'userDetail'
                },
                "modal@": {
                    template: ''
                }
            },
            resolve: {
                userSource: ['$stateParams', 'usersDataService', function ($stateParams, usersDataService) {
                    return usersDataService.getUserDetail('usr_detail', $stateParams.userId).then(function (respData) {
                        if (respData.succeed)
                            return respData.data;

                        return null;
                    });
                }]
            }
        })
        .state('Root.MonthlyReports', {
            url: "/MonthlyReports/:reportId/:selectedDate",
            views: {
                "content@": {
                    templateUrl: resolveViewPath('MonthlyReports/MonthlyReports.html'),
                    controller: 'monthlyReports'
                },
                "modal@": {
                    template: ''
                }
            },
            resolve: {
                userReportSource: ['$stateParams', 'monthlyReportsDataService', function($stateParams, monthlyReportsDataService) {
                    return monthlyReportsDataService.getReportDetail('mr_detail', $stateParams.reportId, $stateParams.selectedDate).then(function (respData) {
                        $stateParams.userReportSource = respData.data;
                    });
                }]
            }
        })
        .state('Root.JobAds', {
            url: "/JobAds",
            views: {
                "content@": {
                    templateUrl: resolveViewPath('JobAds/JobAds.html'),
                    controller: 'jobAds'
                },
                "modal@": {
                    template: ''
                }
            },
            resolve: {
                jobAdsSource: ['$stateParams', 'jobAdsDataService', function ($stateParams, jobAdsDataService) {
                    return jobAdsDataService.getJobAds('ja_main').then(function (respData) {
                        return respData.data;
                    });
                }]
            }
        })
        .state('Root.JobAds.Detail', {
            url: "/:adId",
            views: {
                "ja_detail": {
                    templateUrl: resolveViewPath('JobAds/Detail.html'),
                    controller: 'jobAdsDetail'
                }
            },
            resolve: {
                jobAdTextSource: ['$state', '$stateParams', 'jobAdsDataService', function ($state, $stateParams, jobAdsDataService) {
                    return jobAdsDataService.getJobAdText('ja_main', $stateParams.adId).then(function (respData) {
                        if (respData.succeed) {
                            $stateParams.jobAdTextSource = respData.data;
                        } else {
                            $stateParams.jobAdTextSource = null;
                            $state.go('Root.JobAds');
                        }
                    });
                }]
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
        .state('Root.Settings', {
            url: "/Settings",
            views: {
                "content@": {
                    templateUrl: resolveViewPath('Settings/Settings.html'),
                    controller: 'settings'
                },
                "modal@": {
                    template: ''
                },
                "hourReasonSection@Settings": {
                    templateUrl: resolveViewPath('Settings/HourReason.html'),
                    controller: 'hourReason'
                },
                "festivitySection@Settings": {
                    templateUrl: resolveViewPath('Settings/Festivity.html'),
                    controller: 'festivity'
                },
                "mealVoucherSection@Settings": {
                    templateUrl: resolveViewPath('Settings/MealVoucher.html'),
                    controller: 'mealVoucher'
                }
            },
            resolve: {
                hourReasonSource: ['settingsDataService', function (settingsDataService) {
                    return settingsDataService.getHourReasons('se_main').then(function (respData) {
                        if (respData.succeed) {
                            return respData.data;
                        } else {
                            return null;
                        }
                    });
                }],
                festivitySource: ['settingsDataService', function(settingsDataService) {
                    return settingsDataService.getFestivities('se_main').then(function (respData) {
                        if (respData.succeed) {
                            return respData.data;
                        } else {
                            return null;
                        }
                    });
                }],
                mealVoucherSource: ['settingsDataService', function (settingsDataService) {
                    return settingsDataService.getMealVoucherOptions('se_main').then(function (respData) {
                        if (respData.succeed) {
                            return respData.data;
                        } else {
                            return null;
                        }
                    });
                }]
            }
        })
        .state('Root.Export', {
            url: "/Export/{selectedDate:[0-9]{6}}",
            views: {
                "content@": {
                    templateUrl: resolveViewPath('Export/Export.html'),
                    controller: 'export'
                },
                "modal@": {
                    template: ''
                }
            },
            resolve: {
                exportSource: ['$filter', '$stateParams', 'exportDataService', function ($filter, $stateParams, exportDataService) {
                    var dataFiltered = $stateParams.selectedDate;
                    if (!dataFiltered)
                        dataFiltered = $filter('date')(new Date(), 'yyyyMM');
                    return exportDataService.getExportMonth('mr_detail', dataFiltered).then(function (res) {
                        return {
                            data: (res.succeed) ? res.data : null,
                            month: dataFiltered,
                            selectedDate: new Date(dataFiltered.substr(0, 4) + '-' + dataFiltered.substr(4, 2))
                        }
                    });
                }],
                userSource: ['usersDataService', function (usersDataService) {
                    return usersDataService.getAllUsers('mr_detail').then(function(respData) {
                        if (respData.succeed)
                            return respData.data;

                        return null;
                    });
                }]
            }
        })
        .state('Root.Export.ReportMH', {
            url: "/ReportMH",
            views: {
                "content@": {
                    templateUrl: resolveViewPath('ExportReport/ExportReport.html'),
                    controller: 'exportReport'
                },
                "modal@": {
                    template: ''
                }
            },
            data: {
                reportType: 'MH'
            }
        })
        .state('Root.Export.ReportRI', {
            url: "/ReportRI",
            views: {
                "content@": {
                    templateUrl: resolveViewPath('ExportReport/ExportReport.html'),
                    controller: 'exportReport'
                },
                "modal@": {
                    template: ''
                }
            },
            data: {
                reportType: 'RI'
            }
        })
        .state('Root.Index', {
            url: "/",
            views: {
                "content@": {
                    templateUrl: resolveViewPath('Index/Index.html')
                },
                "modal@": {
                    template: ''
                }
            }
        })
        .state('Root', {
            abstract: true,
            views: {
                "menu": {
                    templateUrl: resolveViewPath('Menu/Menu.html')
                }
            }
        });
}]);