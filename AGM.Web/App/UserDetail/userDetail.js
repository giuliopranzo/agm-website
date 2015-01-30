﻿app.controller('userDetail', function($rootScope, $scope, $alert, $location, $state, $filter, FileUploader, $http, appHelper, userSource, usersDataService, authenticationContainer) {

    $rootScope.$on('loader_show', function (event, callId) {
        if (callId == 'usr_detail')
            $scope.loading = true;
    });

    $rootScope.$on('loader_hide', function (event, callId) {
        if (callId == 'usr_detail')
            $scope.loading = false;
    });

    $scope.init = function() {
        if (userSource)
            $scope.user = userSource;
        else
            $state.go('Index');

        $scope.backToUsersVisible = (authenticationContainer.currentUser.id != $scope.user.id);
        $scope.deleteUserVisible = (authenticationContainer.currentUser.id != $scope.user.id && $scope.user.id != 0);
    };

    $scope.headersObj = {};
    $scope.headersObj[$http.defaults.xsrfHeaderName] = appHelper.getCookie($http.defaults.xsrfCookieName);

    var uploader = $scope.uploader = new FileUploader({
        scope: $scope,
        url: '../../backoffice/api/User/UploadAvatarImage/',
        headers: $scope.headersObj,
        filters: [
        {
            name: 'sizeFilter',
            fn: function (item)
                {
                    if (item.size > 4000000)
                        return false;
                    return true;
                }
        }],
        autoUpload: true,
        removeAfterUpload: true
    });

    uploader.onCompleteItem = function (item, response, status, headers) {
        if (response && response.succeed) {
            for (var i = 0; i < response.data.files.length; i++) {
                $scope.user.image = response.data.files[i].file.imageurl;
            }
            $scope.unsavedModifications = true;
        }
        item.remove;
    };

    $scope.save = function() {
        usersDataService.setUser('usr_detail', $scope.user).then(function (respData) {
            if ($scope.alert)
                $scope.alert.hide();

            if (respData.succeed) {
                if ($scope.user.id == 0) {
                    $scope.backToUsers();
                } else {
                    $scope.alert = $alert({
                        content: 'Profilo utente salvato',
                        animation: 'fadeZoomFadeDown',
                        type: 'info',
                        duration: 5
                    });
                }
            } else {
                $scope.alert = $alert({
                    content: 'Errore durante il salavataggio dei dati',
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
        }
        );
    }

    $scope.deleteUser = function() {
        if ($scope.alert && $scope.alert.$isShown && $scope.alert.$options.type != 'confirm_autocomplete')
            $scope.alert.hide();

        if (!$scope.alert || !$scope.alert.$isShown)
            $scope.alert = $alert({
                content: 'L\'utente visualizzato verrà cancellato, clicca nuovamente l\'icona per procedere',
                animation: 'fadeZoomFadeDown',
                type: 'confirm_autocomplete',
                duration: 10
            });
        else {
            $scope.alert.hide();
            usersDataService.deleteUser('usr_detail', $scope.user.id).then(
                function(respData) {
                    if (respData.succeed) {
                        $scope.backToUsers();
                    } else {
                        if ($scope.alert)
                            $scope.alert.hide();

                        $scope.alert = $alert({
                            content: 'Errore durante l\'aggiornamento dei dati',
                            animation: 'fadeZoomFadeDown',
                            type: 'error',
                            duration: 5
                        });
                    }
                }
            ).catch(function(respData) {
                if ($scope.alert)
                    $scope.alert.hide();
                $scope.alert = $alert({
                    content: 'Errore durante l\'aggiornamento dei dati',
                    animation: 'fadeZoomFadeDown',
                    type: 'error',
                    duration: 5
                });
            });
        }
    };

    $scope.backToUsers = function() {
        if ($scope.alert)
            $scope.alert.hide();

        $location.path('/Users');
    };

    $scope.init();
});

app.directive('toggleSwitch', [function () {
    return {
        scope: {
            val: '=ngModel'
        },
        link: function (scope, element, attrs) {
            element.bootstrapToggle();
            scope.$watch('val', function (newValue, oldValue) {
                    if (newValue == true && element.prop('checked') === false)
                        element.bootstrapToggle('on');
                    else if (newValue == false && element.prop('checked') === true)
                        element.bootstrapToggle('off');
            }, true);

            element.change(function() {
                var setVal = ($(this).prop('checked') === true)? 1:0;
                if (scope.val != setVal){
                    scope.val = setVal;
                    scope.$apply();
                }
            });
        }
    };
}]);