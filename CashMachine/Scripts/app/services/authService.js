(function () {
    'use strict';

    angular.module('app').service('authService', authService);

    function authService($http, $q, identity) {
        var self = this;

        self.validateCardNumberAsync = function (number) {
            var url = 'api/auth/validate-card-number';
            var getParams = { number: number };

            var deferred = $q.defer();

            $http.get(url, { params: getParams })
                .then(function (response) {
                    deferred.resolve(response.data);
                }, function () {
                    deferred.reject();
                });

            return deferred.promise;
        }

        self.validateCardPinAsync = function (number, pin) {

            var loginUrl = 'validate-pin';
            var authData = "grant_type=password&username=" + number + "&password=" + pin;
            var extraParams = { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } };

            var deferred = $q.defer();

            $http.post(loginUrl, authData, extraParams)
                 .then(function (response) { // success
                     if (!response.data.access_token)
                         throw "authService.validateCardPinAsync() failed as AccessToken was missing in the server response";
                     identity.setToken(response.data.access_token);
                     deferred.resolve();
                 }, function (response) {   // failure
                     self.logout();
                     var isCardLocked = response.data.error == 'locked_card';
                     deferred.reject(isCardLocked);
                 });

            return deferred.promise;
        };

        self.isTokenValidAsync = function () {
            var url = 'api/auth/is-valid-token';

            var deferred = $q.defer();

            if (!identity.isAuth()) {
                deferred.resolve(false);
            } else {
                $http.get(url).then(function (response) {
                    if (response.data) {
                        deferred.resolve(true);
                    } else {
                        identity.clear();
                        deferred.resolve(false);
                    }
                }, function () {
                    identity.clear();
                    deferred.resolve(false);
                });
            }

            return deferred.promise;
        }

        self.logout = function () {
            identity.clear();
        };
    }
})();