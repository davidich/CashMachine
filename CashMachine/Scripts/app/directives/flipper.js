(function () {
    'use strict';

angular.module('app')
	.directive('flipper', function() {
		return {
			restrict: 'EA',
			link: function($scope, $elem, $attrs) {
				$scope.flipped = false;
				var options = {
					flipDuration: ($attrs.flipDuration) ? $attrs.flipDuration : 400,
					timingFunction: 'ease-in-out',
				};

				// setting flip options
				angular.forEach(['front-side', 'back-side'], function(name) {
					var el = $elem.find(name);
					if (el.length == 1) {
						angular.forEach(['', '-ms-', '-webkit-'], function(prefix) {
							angular.element(el[0]).css(prefix + 'transition', 'all ' + options.flipDuration/1000 + 's ' + options.timingFunction);
						});
					}
				});				
			}
		};
	});
})();