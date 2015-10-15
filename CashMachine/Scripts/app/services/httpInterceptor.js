(function () {
    'use strict';

    angular.module('app').factory('httpInterceptor', function ($q, $location, identity, toaster) {
        return {
            request: function (config) {
                config.headers = config.headers || {};

                if (identity.isAuth()) {
                    config.headers.Authorization = 'Bearer ' + identity.getToken();
                }

                return config;
            },
            responseError: function (rejection) {
                if (rejection.status === 401) {
                    $location.path('/login');
                } else if (rejection.status == 404 || rejection.status == 500) {
                    toaster.error("Server error (" + rejection.status +")", rejection.data.message);
                }
                return $q.reject(rejection);
            }
        };
    });
})();