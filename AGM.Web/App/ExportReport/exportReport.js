﻿app.controller('exportReport', ['$scope', '$filter', '$state', 'exportSource', 'userSource', 'authenticationContainer', function ($scope, $filter, $state, exportSource, userSource, authenticationContainer) {
    $scope.init = function () {
        $scope.reportType = $state.current.data.reportType;
        $scope.currentUser = authenticationContainer.currentUser;
        $scope.reportRI = exportSource.data.retributionitems;
        $scope.reportMH = exportSource.data.hourreport;
        $scope.users = {};
        var usersOrdered = $filter('orderBy')(userSource, '+name');
        angular.forEach(usersOrdered, function (value, key) {
            if(value.idexport)
                this[value.name] = value;
        }, $scope.users);
        $scope.users[$scope.currentUser.name] = $scope.currentUser;
        $scope.retItemTypes = ['Buoni pasto', 'Rmborso spese', 'Trasf. IT', 'Trasf. IT 1/3', 'Trasf. IT 2/3', 'Trassf. EE', 'Trasf. EE 1/3', 'Trasf. EE 2/3', 'Acconto', 'Stage', 'Emolumento', 'CO.CO.CO.'];
        $scope.hourReasonTypes = ['Ordinarie', 'Straordinarie', 'r.o.l.', 'Ferie', 'Malattia', 'Infortunio', 'Donazione sangue', 'Congedo matrim.', 'D.Lgs. 151', 'Permessi exfest.', 'Permesso studio' ];
        $scope.hourReasonTypesValues = ['ordinarie', 'straordinarie (solo se approvate)', 'r.o.l.', 'ferie', 'malattia', 'infortunio', 'donazione sangue', 'congedo matrimoniale', 'd.lgs. 151', 'permessi ex-festività', 'permesso per studio'];
        $scope.retItemTypesValues = ['mealvoucher', 'expensesrefund', 'dailyallowanceitaly', 'dailyallowanceitalyonethird', 'dailyallowanceitalytwothird', 'dailyallowanceabroad', 'dailyallowanceabroadonethird', 'dailyallowanceabroadtwothird', 'deductforadvance', 'internship', 'compensation', 'freelancer'];

        $scope.retItemTypesLabels = { 'mealvoucher': 'Buoni pasto', 'expensesrefund': 'Rmborso spese', 'dailyallowanceitaly': 'Trasf. IT', 'dailyallowanceitalyonethird': 'Trasf. IT 1/3', 'dailyallowanceitalytwothird': 'Trasf. IT 2/3', 'dailyallowanceabroad': 'Trasf. EE', 'dailyallowanceabroadonethird': 'Trasf. EE 1/3', 'dailyallowanceabroadtwothird': 'Trasf. EE 2/3', 'deductforadvance': 'Acconto', 'internship': 'Stage', 'compensation': 'Emolumento', 'freelancer': 'CO.CO.CO.' };
    };

    $scope.backToExport = function() {
        $state.go('Root.Export', { selectedDate: $state.params.selectedDate });
    };

    $scope.init();
}]);