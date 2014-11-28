app.controller('monthlyReports', function($scope, $alert, $location) {
    $scope.users = [
        { "id": 0, "name": "A-Utente1", "company": "Azienda1" },
        { "id": 1, "name": "A-Utente2", "company": "Azienda2" },
        { "id": 2, "name": "A-Utente3", "company": "Azienda3" },
        { "id": 3, "name": "B-Utente4", "company": "Azienda4" }
    ];
    $scope.detailVisible = false;
    $scope.currentLetter = '';

    $scope.showLetter = function (name) {
        var letter = name.substr(0, 1);
        var res = (letter != $scope.currentLetter);
        $scope.currentLetter = letter;
        return res;
    };

    $scope.getLetter = function(name) {
        return name.substr(0, 1);
    }

    $scope.showDetail = function(id) {
        $location.path('/MonthlyReports/' + id);
    }
});