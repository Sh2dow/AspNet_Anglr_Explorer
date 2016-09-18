(function () {
    'use strict';

    angular
        .module('app')
        .factory('fsManagerClient', fsManagerClient);

    fsManagerClient.$inject = ['$resource'];

    function fsManagerClient($resource) {
        return $resource("api/fsitem/:fileName", { path: "@fileName" },
                {
                    query: { method: 'GET', params: {}, isArray: true }
                    //'query': { method: 'GET', url: 'api/fsitem/:fileName', params: { name: '@fileName' } }
                });
    }
})();