(function () {
    'use strict';

    angular.module('app').factory('identity', function (localStorageService) {

        var storeKey = "cashMachineToken";

        return {
            isAuth: function () {
                var token = localStorageService.get(storeKey);
                return !!token;
            },
            getToken: function () {
                return localStorageService.get(storeKey);
            }, setToken: function (token) {
                localStorageService.set(storeKey, token);
            },
            clear: function () {
                localStorageService.remove(storeKey);
            }
        }       
    });
})();