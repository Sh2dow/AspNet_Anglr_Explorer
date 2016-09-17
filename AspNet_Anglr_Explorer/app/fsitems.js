(function () {
    'use strict';

    angular
        .module('app')
        .controller('fsitems', fsitems);

    fsitems.$inject =['fsManager'];

    function fsitems(fsManager) {
        /* jshint validthis:true */
        var vm = this;
        vm.title = 'fs manager';
        vm.fsitems = fsManager.fsitems;

        activate();

        function activate() {
            fsManager.load();
        }
    }
})();
