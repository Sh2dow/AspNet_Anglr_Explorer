﻿(function (angular) {

    angular
        .module('app', ['ngResource'])
        .controller('fsitems', fsitems)
        .value('path', {
            load: function (path) {
                this.path = path;
            }
        });

    fsitems.$inject = ['fsManager'];

    function fsitems(fsManager) {
        /* jshint validthis:true */
        var vm = this;
        vm.title = 'fs manager';
        vm.fsitems = fsManager.fsitems;
        vm.get = fsManager.get;

        get('');

        function get(path) {
            var f = fsManager.get(path)
                .then(function () {
                    console.log(path);
                });
        }
    }

    angular
        .module('app')
        .factory('fsManager', fsManager);

    fsManager.$inject = ['$q', 'fsManagerClient'];

    function fsManager($q, fsManagerClient) {
        var service = {
            fsitems: [],
            get: get
        };

        return service;

        function get(path) {
            service.fsitems.length = 0;

            return fsManagerClient.get({path: path})
                                .$promise
                                .then(function (result) {
                                    result.fsitems
                                            .forEach(function (path) {
                                                service.fsitems.push(path);
                                            });

                                    return result.$promise;
                                })
                .then(function () {
                    console.log(path);
                });
        }
    }

        angular
            .module('app')
            .factory('fsManagerClient', fsManagerClient);

        fsManagerClient.$inject = ['$resource'];

        function fsManagerClient($resource) {
            return $resource("api/fsitem/:path", { id: "@path" },
                    {
                        get: { method: 'GET', url: '/api/fsitem/?path=:path', params: { path: '@path' } }
                    });
        }

    })(window.angular);