(function () {
    'use strict';

    angular.module('app').directive('keyboard', keyboardDirective);

    function keyboardDirective() {
        return {
            restrict: 'EA',
            templateUrl: 'Scripts/app/directives/keyboard.tmpl.html',
            scope: {
                'ok': '&',
                'isLocked': '=',
                'minLength': '=',
                'maxLength': '=',
                'buffer': '='
            },
            controller: function ($scope, $timeout) {
                this.keyPress = function (key) {
                    if (!$scope.isLocked && $scope.buffer.length < parseInt($scope.maxLength))
                        $scope.buffer += key;
                }

                $scope.renderedKeyCnt = 0;
                this.ackKeyRenderCompleted = function() {
                    $scope.renderedKeyCnt++;                    
                }

                $scope.clearPress = function () {
                    if (!$scope.isLocked)
                        $scope.buffer = '';
                }

                $scope.okPress = function () {
                    if (!$scope.isLocked && $scope.canProceed()) {
                        $scope.ok()();
                    }
                }

                $scope.canProceed = function () {
                    return $scope.buffer.length >= parseInt($scope.minLength);
                }

                $scope.getOkButtonTooltip = function () {
                    return $scope.canProceed() ? 'Click to proceed' : 'Value is not entered';
                }
            },
        };
    }
})();