app.directive('agmFooter', ['$timeout', function ($timeout) {
    return {
        template: '<hr /><footer><p>AGM Solutions srl &copy; ' + new Date().getFullYear() + ' - Area riservata</p></footer>',
        //link: function (scope) {
        //    $timeout(function() { $('.nano').nanoScroller(); }, 0);
        //}
    };
}]);

app.directive('scrollToOption', [function () {
    return {
        scope: {
            value: '=scrollToOption'
        },
        link: function (scope, element, attrs) {
            scope.$watch('value', function (newValue, oldValue) {
                if (attrs.id) {
                    var el = $('button[id="' + attrs.id + '"] + ul > li > a > span:contains("' + newValue + '")');
                    if (el && el.length > 0)
                        el[0].offsetParent.scrollTop = el[0].offsetTop - 8;
                }
            }, true);
            
        }
    };
}]);