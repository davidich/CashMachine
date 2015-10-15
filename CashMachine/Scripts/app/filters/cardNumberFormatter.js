(function () {
    'use strict';

    angular.module('app').filter('cardNumberFormatter', cardNumberFilter);

    function cardNumberFilter() {
        return function (input) {
            if (!input)
                return '';

            var start = 0,
                end = 4,
                result = input.substring(start, end),
                part;
            
            do {
                start += 4;
                end += 4;

                part = input.substring(start, end);

                if (part) result += "-" + part;

            } while (part)            

            return result;           
        };
    }
})();