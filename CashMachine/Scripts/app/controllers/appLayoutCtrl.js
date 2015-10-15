(function () {
    'use strict';

    angular.module('app').controller('appLayoutCtrl', appLayoutCtrl);

    function appLayoutCtrl($scope, $state, authService) {
        $scope.logout = function() {
            authService.logout();
            $state.go("login");
        }
    }
})();