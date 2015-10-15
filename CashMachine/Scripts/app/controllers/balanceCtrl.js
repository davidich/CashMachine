(function () {
    'use strict';

    var app = angular.module('app');

    app.controller("balanceCtrl", function ($scope, cardService) {

        $scope.isLoaded = false;

        cardService.getBalanceInfoAsync()
            .then(function (info) {
                $scope.info = info;
                $scope.isLoaded = true;
            });
    });
})();