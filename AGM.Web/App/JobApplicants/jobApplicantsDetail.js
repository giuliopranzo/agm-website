app.controller('jobApplicantsDetail', ['$rootScope', '$scope', '$filter', '$alert', '$state', 'applicant', 'jobCategories', 'locations', 'languages', 'statuses', 'interviewers', 'jobApplicantsDataService', function ($rootScope, $scope, $filter, $alert, $state, applicant, jobCategories, locations, languages, statuses, interviewers, jobApplicantsDataService) {
    $rootScope.$on('loader_show', function (event, callId) {
        if (callId == 'japp_main') {
            if (!$rootScope.loader)
                $rootScope.loader = {};
            if (!$rootScope.loader['japp_main'])
                $rootScope.loader['japp_main'] = 0;
            $rootScope.loader['japp_main'] = $rootScope.loader['japp_main'] + 1;
            $scope.loading = true;
        }
    });

    $rootScope.$on('loader_hide', function (event, callId) {
        if (callId == 'japp_main') {
            if ($rootScope.loader['japp_main'] > 0)
                $rootScope.loader['japp_main'] = $rootScope.loader['japp_main'] - 1;
            if ($rootScope.loader['japp_main'] == 0)
                $scope.loading = false;
        }
    });

    $scope.init = function () {
        $scope.applicant = applicant;
        $scope.interviewers = interviewers;

        if (!$scope.applicant.id)
        {
            $scope.applicant.jobcategoryid = 0;
            $scope.applicant.language1id = 15;
            $scope.applicant.language2id = 30;
            $scope.applicant.language3id = 30;
            $scope.applicant.worklocationid = 0;
            $scope.applicant.statusid = 0;
            $scope.applicant.statusreasonid = 1;
            $scope.applicant.interviewdate = new moment().format();
            $scope.applicant.userid = $filter('filter')($scope.interviewers, { iscurrent: true }, true)[0].value;
        }

        $scope.jobCategories = jobCategories;
        $scope.locations = locations;
        $scope.languages = languages;
    };

    $scope.kd = function (event) {
        var key = event.keyPress;
        applicant.language1id = 10;
    };

    $scope.getLanguageName = function(idIn){
        var languages = $filter('filter')($scope.languages, { id: idIn }, true);
        if (languages && languages.length > 0)
            return languages[0].name;
        return '';
    };

    $scope.getLanguageId = function (searchStr) {
        
    };

    $scope.save = function () {
        jobApplicantsDataService.save('japp_main', applicant).then(function (respData) {
            if ($scope.alert)
                $scope.alert.hide();

            if (respData.succeed) {
                $scope.backToListPage();
            } else {
                $scope.alert = $alert({
                    content: (respData.errors && respData.errors.length >= 0) ? respData.errors[0].message : 'Errore durante il salvataggio dei dati',
                    animation: 'fadeZoomFadeDown',
                    type: 'error',
                    duration: 5
                });
            }
        }).catch(function (respData) {
            if ($scope.alert)
                $scope.alert.hide();
            $scope.alert = $alert({
                content: 'Errore durante il salvataggio dei dati',
                animation: 'fadeZoomFadeDown',
                type: 'error',
                duration: 5
            });
        });
    };

    $scope.backToListPage = function () {
        $state.go('Root.JobApplicants');
    };

    $scope.init();
}]);