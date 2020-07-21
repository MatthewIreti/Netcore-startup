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
                    controller: 'ApplicationInvoice',
                    templateUrl: '/App/Application/Invoice.html'
                },
                {
                    name: 'details',
                    url: '/details/:recordId',
                    controller: 'ApplicationGetDetails',
                    templateUrl: '/App/Application/Details.html'
                }
            ]
        })
        .state({
            name: 'site.payment',
            template: '<div ui-view>',
            url: '/payment',
            abstract: true,
            children: [
                {
                    name: 'list',
                    url: '/list',
                    templateUrl: '/App/Payment/List.html',
                    controller: 'PaymentList'
                }
            ]
        })
        .state({
            name: 'site.users',
            template: '<div ui-view>',
            url: '/users',
            abstract: true,
            children: [
                {
                    name: 'index',
                    url: '/',
                    templateUrl: '/App/Users/Index.html',
                    controller: 'UsersList'
                },
                {
                    name: 'create',
                    url: '/create',
                    controller: 'UsersCreate',
                    templateUrl: '/App/Users/Create.html'
                }
            ]
        })
        .state({
            name: 'site.tickets',
            template: '<div ui-view>',
            url: '/tickets',
            abstract: true,
            children: [
                {
                    name: 'index',
                    url: '/',
                    templateUrl: '/App/Support/Index.html',
                    controller: 'TicketsList'
                },
                {
                    name: 'create',
                    url: '/create',
                    controller: 'TicketCreate',
                    templateUrl: '/App/Support/Create.html'
                }
            ]
        });
}]);
