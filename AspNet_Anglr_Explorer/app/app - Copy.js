(function (angular) {

    angular
        .module('app', ['ngResource', $scope])
        .controller('fsitems', $scope, fsitems)
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

        $scope.list() = fsManager.get();
    }

    angular
        .module('app')
        .factory('fsManager', fsManager);

    fsManager.$inject = ['$q', 'fsManagerClient'];

    function fsManager($q, fsManagerClient) {
        var service = {
            fsitems: [],
            get: get,
        };

        return service;

        function get(fsitem) {
            service.fsitems.length = 0;

            return fsManagerClient.get()
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
        }
    }

    angular
        .module('app')
        .factory('fsManagerClient', fsManagerClient);

    fsManagerClient.$inject = ['$resource'];

    function fsManagerClient($resource) {
        return $resource("api/fsitem/:path", { path: "" },
                {
                    query: { method: 'GET' },
                    get: { method: 'GET', url: 'api/fsitem/?path', params: { name: "" } }
                });
    }

})(window.angular);