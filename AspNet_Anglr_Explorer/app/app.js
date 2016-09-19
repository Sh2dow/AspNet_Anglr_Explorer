(function (angular) {

    angular
        .module('app', ['ngResource'])
        .controller('fsitems', fsitems)
        //.value('path', {
        //    load: function (path) {
        //        this.path = path;
        //    }
        //});

    fsitems.$inject = ['fsManager'];

    function fsitems(fsManager) {
        /* jshint validthis:true */
        var vm = this;
        vm.title = 'fs manager';
        vm.fsitems = fsManager.fsitems;
        vm.get = fsManager.get;

        get();

        function get(fsitem) {
            var f = fsManager.get(fsitem)
                .then(function () {
                    console.log(fsitem.relPath);
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

        function get(fsitem) {
            service.fsitems.length = 0;

            return fsManagerClient.get(fsitem)
                                .$promise
                                .then(function (result) {
                                    result.fsitems
                                            .forEach(function (fsitem) {
                                                service.fsitems.push(fsitem);
                                            });

                                    return result.$promise;
                                },
                                function (result) {
                                    return $q.reject(result);
                                })
                .then(function () {
                    console.log(fsitem);
                });
        }
    }

        angular
            .module('app')
            .factory('fsManagerClient', fsManagerClient);

        fsManagerClient.$inject = ['$resource'];

        function fsManagerClient($resource) {
            return $resource("api/fsitem/:path", { path: '@path' },
                    {
                        get: { method: 'GET', url: 'api/fsitem/?path', params: { path: '@path' } }
                    });
        }

    })(window.angular);