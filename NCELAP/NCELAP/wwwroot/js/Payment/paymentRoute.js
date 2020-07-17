angular.module('ncLasPortalApp').config(['stateHelperProvider', function (stateHelperProvider) {
    var viewPath = '';
    stateHelperProvider
        .state({
            name: 'site.application',
            template: '<div ui-view>',
            url: '/application',
            abstract: true,
            children: [
                {
                    name: 'list',
                    url: '/list',
                    templateUrl: '/App/Application/List.html',
                    controller: 'ApplicationList'
                },
                {
                    name: 'create',
                    url: '/create',
                    controller: 'ApplicationCreate',
                    templateUrl: '/App/Application/CreateEdit.html'
                }
            ]
        }).state({
            name: 'site.payment',
            template: '<div ui-view>',
            abstract: true,
            url: '/payment',
            children: [
                {
                    name: 'list',
                    url: '/list',
                    templateUrl: '/App/Payment/List.html'
                }
            ]
        });
}]);
