app.controller('index', ['$scope', 'authenticationContainer', 'usersDataService', 'messages', 'sentMessages', 'usersSource', 'notices', function ($scope, authenticationContainer, usersDataService, messages, sentMessages, usersSource, notices) {
    var counter = 0;
    checkAuthenticationContainer();

    function checkAuthenticationContainer()
    {
        counter++;
        if ((authenticationContainer.currentUser == undefined || authenticationContainer.currentUser == null || authenticationContainer.currentUser == "") && counter < 10)
        {
            setTimeout(checkAuthenticationContainer, 100);
        }
        else {
            $scope.currentUser = authenticationContainer.currentUser;
            $scope.showAddNotice = ($scope.currentUser.usertype == 1);
        }
    }

    $scope.messages = messages;
    $scope.sentMessages = sentMessages;
    $scope.receivers = usersSource;
    $scope.notices = notices;
    $scope.receiverType = [{ value: 1, description: 'Tutti'}, { value: 2, description: 'Seleziona'}];

    $scope.isNoticeCollapsed = [];
    
    resetNoticesView($scope.notices != null && $scope.notices.length > 0 && $scope.notices[0] != null && $scope.notices[0].id != null ? $scope.notices[0].id : 0);

    $scope.init = function () {
        $scope.messageIn = {
            sendToAll: 2,
            toUserIds: []
        };

        $scope.tabs = {
            activeTab: 'Send'
        };
    };

    $scope.sendMessage = function () {
        usersDataService.setMessage('mr_detail', $scope.messageIn).then(function (respData) {
            if (respData.succeed)
                $scope.messageIn = {
                    sendToAll: 2,
                    toUserIds: []
                };

        });
    };

    $scope.deleteMessage = function (id) {
        usersDataService.deleteMessage('mr_detail', id).then(function (respData) {
            if (respData.succeed)
                usersDataService.getSentMessages('mr_detail').then(function (respData) {
                    if (respData.succeed)
                        $scope.sentMessages = respData.data;
                });
        });
    };

    $scope.noticeCollapse = function (id) {
        resetNoticesView(id);
    }

    $scope.addNotice = function () {
        usersDataService.addNotice('mr_detail', $scope.noticeToAdd).then(function (respData) {
            if (respData.succeed)
                usersDataService.getNotices('mr_detail').then(function (respData) {
                    if (respData.succeed)
                    {
                        $scope.noticeToAdd.subject = "";
                        $scope.noticeToAdd.text = "";
                        $scope.isNoticeCollapsed['0'] = true;
                        $scope.notices = respData.data;
                        
                    }
                });
        });
    };

    $scope.deleteNotice = function (id) {
        usersDataService.deleteNotice('mr_detail', id).then(function (respData) {
            if (respData.succeed)
                usersDataService.getNotices('mr_detail').then(function (respData) {
                    if (respData.succeed)
                    {
                        $scope.notices = respData.data;
                        resetNoticesView($scope.notices != null && $scope.notices.length > 0 && $scope.notices[0] != null && $scope.notices[0].id != null ? $scope.notices[0].id : 0);
                    }
                });
        });
    };

    function resetNoticesView(id)
    {
        if ($scope.isNoticeCollapsed.length == 0) {
            $scope.isNoticeCollapsed['0'] = !(id == 0);
            for (var i = 0; i < $scope.notices.length; i++) {
                $scope.isNoticeCollapsed[$scope.notices[i].id.toString()] = !(i == 0);
            }
        }
        else {
            if ($scope.isNoticeCollapsed[id.toString()] || ($scope.isNoticeCollapsed[id.toString()] == null)) {
                if (id != 0)
                    $scope.isNoticeCollapsed['0'] = true;
                for (var i = 0; i < $scope.notices.length; i++) {
                    if ($scope.notices[i].id != id || id == 0)
                        $scope.isNoticeCollapsed[$scope.notices[i].id.toString()] = true;
                }
            }
            $scope.isNoticeCollapsed[id.toString()] = !($scope.isNoticeCollapsed[id.toString()] || ($scope.isNoticeCollapsed[id.toString()] == null));
        }
    }

    $scope.init();
}]);