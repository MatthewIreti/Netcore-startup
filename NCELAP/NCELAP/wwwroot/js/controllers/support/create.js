appModule.controller('TicketCreate', function ($scope, $http) {
    $scope.supportPayload = {};
    $scope.dd = "smdnsmdnsd";
    $scope.caseseverities = [
        { Name: "Veyh High", Value: 1 },
        { Name: "High", Value: 2 },
        { Name: "Medium", Value: 3 },
        { Name: "Low", Value: 4 },
        { Name: "Very Low", Value: 5 }
    ];
    //$scope.ticketModel = { Department: '', Priority: '', Subject: '', Description: '', Message: '' };

    $scope.setCRecIdParams = function () {
        var loggedInUser = localStorage.getItem('loggedInUser');
        $scope.loggedInUser = JSON.parse(loggedInUser);

        $scope.userCreateFormModel.CustTableRecId = $scope.loggedInUser.custTableRecId;
        $scope.userCreateFormModel.CreatedByRecId = $scope.loggedInUser.recordId;
    };

    $scope.submitTicket = function () {
        $scope.waiting++;
        console.log($scope.userCreateFormModel);

        setTimeout(function () {
            $http({
                method: 'POST',
                url: baseUrl + 'accounts/createuser',
                data: $scope.userCreateFormModel,
                dataType: 'json'
            }).then(function (response) {
                $scope.userCreateResponse = response.data;

                if ($scope.userCreateResponse === true) {
                    toastr.success('User account created successfully! <br /> An activation email has been sent to ' + $scope.userCreateFormModel.Email, 'Success', {});
                    $scope.initUserModel();
                    $scope.waiting--;
                }

            }, function (error) {
                console.log(error);
                $scope.waiting--;
            });
        }, 2000);
    };

});