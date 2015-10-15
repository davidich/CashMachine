(function () {
    'use strict';

    var app = angular.module('app', ["ngAnimate", "ui.router", "LocalStorageModule", "toaster"]);

    app.config(function ($stateProvider, $locationProvider, $httpProvider) {
        var viewRoot = "Scripts/app/views/";

        $stateProvider
            .state('login', {
                url: "/login",
                allowAnonymous: true,
                controller: "loginCtrl",
                templateUrl: "Scripts/app/views/" + "login.html"
            })
            .state('errorCard', {
                url: "/error-card",
                allowAnonymous: true,
                templateUrl: viewRoot + "error-card.html"
            })
            .state('errorPin', {
                url: "/error-pin",
                allowAnonymous: true,
                templateUrl: viewRoot + "error-pin.html"
            })
            .state('app.errorWithdraw', {
                url: "/error-withdraw",
                templateUrl: viewRoot + "error-withdraw.html"
            })
            .state('errorOperation', {
                url: "/error-operation",
                templateUrl: viewRoot + "error-operation.html"
            })
            .state('app', {
                abstract: true,
                controller: "appLayoutCtrl",
                templateUrl: viewRoot + "app.layout.html"
            })
            .state('app.operations', {
                url: "/",
                template: "<span style='font-style: italic;'>Please, select some operation</span>"
            })
            .state('app.balance', {
                url: "/balance",
                controller: "balanceCtrl",
                templateUrl: viewRoot + "balance.html"
            })
            .state('app.withdraw', {
                url: "/withdraw",
                controller: "withdrawCtrl",
                templateUrl: viewRoot + "withdraw.html"
            })
            .state('app.withdrawReport', {
                url: "/withdraw-report/:id",
                controller: "withdrawReportCtrl",
                templateUrl: viewRoot + "withdraw-report.html"
            });

        $locationProvider.html5Mode(true);

        $httpProvider.interceptors.push("httpInterceptor");
    });

    // make sure not authorized users will not get into the app
    app.run(function ($rootScope, $state, $http, $urlRouter, identity, authService) {        
        var validationStarted = false;
        var tokenValidationResult = authService.isTokenValidAsync();

        var unsubscribeFromEvent = $rootScope.$on('$stateChangeStart', function (event, toState, toParams) {
            if (!validationStarted) {
                event.preventDefault();
                validationStarted = true;

                tokenValidationResult
                    .then(function (isValid) {
                        if (isValid)
                            $state.go(toState.name, toParams);
                        else
                            $state.go("login");
                    }).finally(function () {
                        unsubscribeFromEvent();
                    });
            }
        });
    });
})();

// http://blog.oneunicorn.com/2013/05/28/database-initializer-and-migrations-seed-methods/
// http://bitoftech.net/2014/06/01/token-based-authentication-asp-net-web-api-2-owin-asp-net-identity/
// http://bitoftech.net/2014/06/09/angularjs-token-authentication-using-asp-net-web-api-2-owin-asp-net-identity/
// http://aspnetboilerplate.com/Pages/Documents/Introduction
// http://iranreyes.com/starting-angularjs-project-with-visual-studio/
// http://blogs.msdn.com/b/visualstudio/archive/2015/02/05/using-angularjs-in-visual-studio-2013.aspx

//отошёл от требования хранить сумму в таблице карт