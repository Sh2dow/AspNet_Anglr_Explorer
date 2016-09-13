(function () {
    'use strict';

    angular
        .module('app.fsitem')
        .factory('fsManagerClient', fsManagerClient);

    fsManagerClient.$inject = ['$resource'];

    function fsManagerClient($resource) {
        return $resource("api/fsitem/:fileName",
                { id: "@fileName" },
                {
                    'query': { method: 'GET' }
                });
    }
})();