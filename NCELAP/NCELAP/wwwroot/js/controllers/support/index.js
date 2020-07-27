appModule.controller('TicketsList', function ($scope, $http) {
    $scope.getSupportTickets = function () {
        $http({
            method: 'GET',
            url: baseUrl + 'supportticket/getsupportticketsbyemployee/' + $scope.employeeRecId + '/' + $scope.companyRecId
        }).then(function (response) {
            $scope.tickets = response.data.data;
            console.log($scope.tickets);

            // if the datatable instance already exist, destroy before recreating, otherwise, just create
            if ($.fn.DataTable.isDataTable('#ticketsTable')) {
                $('#ticketsTable').DataTable().destroy();
            }

            angular.element(document).ready(function () {
                dTable = $('#ticketsTable');
                dTable.DataTable({
                    "aaSorting": [] // disables first colum auto-sorting
                });
            });
        }, function (error) {
            console.log(error);
        });
    };

    $scope.setRecIdParams = function () {
        var loggedInUser = localStorage.getItem('loggedInUser');
        $scope.loggedInUser = JSON.parse(loggedInUser);
        
        if ($scope.loggedInUser) {

            $scope.employeeRecId = $scope.loggedInUser.recordId;
            $scope.companyRecId = $scope.loggedInUser.custTableRecId;
            $scope.getSupportTickets();
        }
    };

    $scope.setRecIdParams();
});