userAccessModule.controller('loginPageCtrl', function ($scope, $http) {
    $scope.loginModel = { Email: '', Password: '' };
    $scope.accountdoesnotexist = false;
    $scope.loggingIn = false;
    $scope.accountnotactivated = false;

    $scope.showAccountActivationError = function () {
        $scope.accountnotactivated = true;
    };

    $scope.hideAccountActivationError = function () {
        $scope.accountnotactivated = false;
    };

    $scope.submitForm = function () {
        $scope.accountdoesnotexist = false;
        $scope.loggingIn = true;
        $scope.hideAccountActivationError();

        $http({
            method: 'POST',
            url: baseUrl + 'accounts/userlogin',
            data: $scope.loginModel,
            dataType: 'json'
        }).then(function (response) {
            $scope.loginResponse = response.data;
            if ($scope.loginResponse.recordId > 0 ) {
                // user account exists
                if ($scope.loginResponse.activated === 'Yes') {
                    localStorage.setItem("loggedInUser", JSON.stringify($scope.loginResponse));
                    $scope.loggingIn = false;
                    $scope.hideAccountActivationError();

                    window.location.href = '/companyoperator#!/application/list';
                } else {
                    // display account not activated response
                    $scope.showAccountActivationError();
                    $scope.loggingIn = false;
                }
            } else {
                // user account does not exist
                $scope.accountdoesnotexist = true;
                $scope.loggingIn = false;
            }
        }, function (error) {
            $scope.loggingIn = false;
            console.log(error);
        });
    };
});