(function () {
    'use strict';

    angular.module('app').directive('numKey', numKeyDirective);

    function numKeyDirective() {
        return {
            restrict: 'EA',
            require: '^keyboard',
            templateUrl: 'Scripts/app/directives/numKey.tmpl.html',
            scope: {
                'value': '@',
                'position': '@'                
            },
            link: function ($scope, $elem, $attrs, $keyboardCtrl) {
                $scope.onKeyPressed = function () {
                    $keyboardCtrl.keyPress($scope.value);
                }

                $keyboardCtrl.ackKeyRenderCompleted();
            }
        };
    }
})();