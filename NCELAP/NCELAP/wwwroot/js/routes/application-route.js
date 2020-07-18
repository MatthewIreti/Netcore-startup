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
                },
                {
                    name: 'invoice',
                    url: '/invoice/:recordId',
                    controller: 'ApplicationGetDetails',
                    templateUrl: '/App/Application/Invoice.html'
                },
                {
                    name: 'details',
                    url: '/details/:recordId',
                    controller: 'ApplicationGetDetails',
                    templateUrl: '/App/Application/Details.html'
                }
            ]
        });
}]);
