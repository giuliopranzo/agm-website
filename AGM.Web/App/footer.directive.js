app.directive('agmFooter', ['$timeout', function ($timeout) {
    return {
        template: '<hr /><footer><p>AGM Solutions srl &copy; ' + new Date().getFullYear() + ' - Area riservata</p></footer>',
        link: function (scope) {
            $timeout(function() { $('.nano').nanoScroller(); }, 0);
        }
    };
}]);