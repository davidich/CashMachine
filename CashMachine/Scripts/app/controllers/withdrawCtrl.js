(function () {
    'use strict';

    var app = angular.module('app');

    app.controller("withdrawCtrl",function ($scope, $state, cardService) {
        $scope.amount = "";
        $scope.isKeyboardLocked = false;
        $scope.proceed = function() {
            $scope.isKeyboardLocked = true;
            cardService.withdrawAsync($scope.amount)
                .then(function(result) {
                    if (result.status == "Success")
                        $state.go("app.withdrawReport", { id: result.operationId });
                    else if (result.status == "CardIsLocked")
                        $state.go("errorCard");
                    else if (result.status == "TooBigAmount")
                        $state.go("app.errorWithdraw");
                }).finally(function() {
                    $scope.isKeyboardLocked = false;
                });
        }
    });
})();