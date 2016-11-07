app.controller('index', ['$scope', 'authenticationContainer', 'usersDataService', 'messages', 'sentMessages', 'usersSource', function ($scope, authenticationContainer, usersDataService, messages, sentMessages, usersSource) {
    $scope.messages = messages;
    $scope.sentMessages = sentMessages;
    $scope.currentUser = authenticationContainer.currentUser;
    $scope.receivers = usersSource;
    $scope.receiverType = [{ value: 1, description: 'Tutti'}, { value: 2, description: 'Seleziona'}];

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

    $scope.init();
}]);