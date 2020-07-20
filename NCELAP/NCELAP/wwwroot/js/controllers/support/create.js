appModule.controller('SupportCreate', function ($scope, $http) {
    $scope.ticketModel = { Department: '', Priority: '', Subject: '', Description: '', Message: '', };

    $scope.createUserAccount = function () {
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