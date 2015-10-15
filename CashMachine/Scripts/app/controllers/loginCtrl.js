(function () {
    'use strict';

    angular.module('app').controller('loginCtrl', loginCtrl);

    function loginCtrl($scope, $state, authService) {
        $scope.cardSide = null;
        $scope.inputLength = null;
        $scope.keyboardBuffer = null;
        $scope.cardNumber = $scope.cardPin = "";
        //$scope.cardNumber = "0000000000000001"; $scope.cardPin = "0001";
        showNumberSide();

        $scope.$watch('keyboardBuffer', function (newValue) {
            if ($scope.cardSide == 'number') {
                $scope.cardNumber = newValue;
                $scope.inputLength = 16;
            } else {
                $scope.cardPin = newValue;
                $scope.inputLength = 4;
            }
        });

        $scope.proceed = function () {
            if ($scope.cardSide == 'number')
                validateNumber($scope.cardNumber);
            else
                login($scope.cardNumber, $scope.cardPin);
        }

        $scope.goBack = function () {
            showNumberSide();
        }

        function showPinSide() {
            $scope.cardSide = 'pin';
            $scope.keyboardBuffer = $scope.cardPin;
        }

        function showNumberSide() {
            $scope.cardSide = 'number';
            $scope.keyboardBuffer = $scope.cardNumber;
        }

        function validateNumber(number) {
            // lock keyboard until validation completes
            $scope.isKeyboardLocked = true;

            // validate card number
            authService.validateCardNumberAsync(number)
                .then(function (result) {
                    if (result == "Ok") 
                        showPinSide();
                    else
                        $state.go("errorCard");
                })
                .finally(function () {
                    $scope.isKeyboardLocked = false;
                });
        }

        function login(number, pin) {
            // lock keyboard until validation completes
            $scope.isKeyboardLocked = true;

            // exchange card number and pin for AuthToken
            authService.validateCardPinAsync(number, pin)
                .then(function () {
                    $scope.isKeyboardLocked = false;
                    $state.go("app.operations");
                }, function (isCardLocked) {
                    if (isCardLocked)
                        $state.go("errorCard");
                    else
                        $state.go("errorPin");
                });
        }
    }
})();