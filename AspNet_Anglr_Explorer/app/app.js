(function () {
    'use strict';

    var app = angular.module('app', ['ngResource','ngRoute','app.fsitem']);

    app.config(['$routeProvider', function ($routeProvider) {

        //$routeProvider.when('/welcome', {
        //    templateUrl: 'app/welcome.html',
        //    controller: 'welcome',
        //    controllerAs: 'vm',
        //    caseInsensitiveMatch: true
        //});
        $routeProvider.when('/fsitems', {
            templateUrl: 'app/fsitem/fsitems.html',
            controller: 'fsitems',
            controllerAs: 'vm',
            caseInsensitiveMatch: true
        });
        $routeProvider.otherwise({
            redirectTo: '/fsitems'
        });
    }]);


    // Handle routing errors and success events
    app.run([function () {
    }]);
})();