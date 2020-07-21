appModule.controller('PaymentList', function ($scope, $http) {
    $scope.title = "Applications";
    $scope.breadcrumb.splice(0, $scope.breadcrumb.length);
    $scope.breadcrumb.push(
        {
            title: $scope.title
        }
    );
    if ($scope.loggedInUser.custTableRecId) {
        $scope.waiting++;
        getCustomerApplications($scope.loggedInUser.custTableRecId);
        $scope.waiting--;
    }


    function getCustomerApplications(custrecid) {


        $http({
            method: 'GET',
            url: baseUrl + 'payment/customerPayments/' + custrecid
        }).then(function (response) {
            $scope.items = response.data;
           
            // if the datatable instance already exist, destroy before recreating, otherwise, just create
            if ($.fn.DataTable.isDataTable('#applicationsTable')) {
                $('#applicationsTable').DataTable().destroy();
            }

            angular.element(document).ready(function () {
                dTable = $('#applicationsTable');
                dTable.DataTable({
                    "aaSorting": [] // disables first colum auto-sorting
                });
            });
        }, function (error) {
            console.log(error);
        });
    }
});
