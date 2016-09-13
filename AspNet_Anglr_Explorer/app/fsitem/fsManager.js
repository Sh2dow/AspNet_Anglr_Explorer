(function () {
    'use strict';

    angular
        .module('app.fsitem')
        .factory('fsManager', fsManager);

    fsManager.$inject = ['$q', 'fsManagerClient', 'appInfo'];

    function fsManager($q, fsManagerClient, appInfo) {
        var service = {
            fsitems: [],
            load: load,
            fsitemExists: fsitemExists
        };

        return service;

        function load() {
            appInfo.setInfo({ busy: true, message: "loading fsitems" })

            service.fsitems.length = 0;

            return fsManagerClient.query()
                                .$promise
                                .then(function (result) {
                                    result.fsitems
                                            .forEach(function (fsitem) {
                                                service.fsitems.push(fsitem);
                                            });

                                    appInfo.setInfo({ message: "fsitems loaded successfully" });

                                    return result.$promise;
                                },
                                function (result) {
                                    appInfo.setInfo({ message: "something went wrong: " + result.data.message });
                                    return $q.reject(result);
                                })
                                ['finally'](
                                function () {
                                    appInfo.setInfo({ busy: false });
                                });
        }

        function fsitemExists(fsitemName) {
            var res = false;
            service.fsitems.forEach(function (fsitem) {
                if (fsitem.name === fsitemName) {
                    res = true;
                }
            });

            return res;
        }
    }
})();