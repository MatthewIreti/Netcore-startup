appModule.controller('UsersList', function ($scope, $http) {

    $scope.getUserAccounts = function () {
        $http({
            method: 'GET',
            url: baseUrl + 'accounts/usersbycreatorrecid/' + $scope.createdByRecId
        }).then(function (response) {
            $scope.userAccounts = response.data;
            console.log($scope.userAccounts);

            // if the datatable instance already exist, destroy before recreating, otherwise, just create
            if ($.fn.DataTable.isDataTable('#userAccountsTable')) {
                $('#userAccountsTable').DataTable().destroy();
            }

            angular.element(document).ready(function () {
                dTable = $('#userAccountsTable');
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
        
        $scope.createdByRecId = $scope.loggedInUser.recordId;
        if ($scope.createdByRecId) {
            $scope.getUserAccounts();
        }
    };

    $scope.setRecIdParams();
});