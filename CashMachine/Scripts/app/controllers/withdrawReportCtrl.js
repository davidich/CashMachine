(function () {
    'use strict';

    var app = angular.module('app');

    app.controller("withdrawReportCtrl", function ($scope, $state, $stateParams, cardService) {

        $scope.isLoaded = false;

        cardService.getWithdrawReportAsync($stateParams.id)
            .then(function (info) {
                $scope.info = info;
                $scope.isLoaded = true;
            }).catch(function (status) {
                if (status == 400 || status == 403)
                    $state.go("errorOperation");
            });
    });
})();