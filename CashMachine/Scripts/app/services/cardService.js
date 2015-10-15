(function () {
    'use strict';

    angular.module('app').service('cardService', cardService);

    function cardService($http, $q) {
        var self = this;

        self.getBalanceInfoAsync = function () {

            var deferred = $q.defer();

            $http.get('api/card/balance')
                .then(function (response) {
                    deferred.resolve(response.data);
                })
                .catch(function (response) {
                    deferred.reject(response.data);
                });

            return deferred.promise;
        }

        self.withdrawAsync = function (amount) {
            var deferred = $q.defer();

            $http.post('api/card/withdraw', { amount: amount })
                .then(function (response) {
                    deferred.resolve(response.data);
                })
                .catch(function (response) {
                    deferred.reject(response.data);
                });

            return deferred.promise;
        }

        self.getWithdrawReportAsync = function (id) {

            var deferred = $q.defer();

            $http.get('api/card/withdraw-report', { params: { id: id } })
                .then(function (response) {
                    deferred.resolve(response.data);
                })
                .catch(function (response) {
                    deferred.reject(response.status);
                });

            return deferred.promise;
        }

    }
})();