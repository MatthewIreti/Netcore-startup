userAccessModule.controller('passwordResetPageCtrl', function ($scope, $http) {
    $scope.emailModel = {
        Value: ''
    };
    $scope.buttonText = 'Send Activation Code';

    $scope.emailCheck = function () {
        $scope.veriyingEmail = true;
        $scope.emaildoesnotexist = false;
        $scope.buttonText = 'Verifying Email Address';

        $http({
            method: 'POST',
            url: baseUrl + 'accounts/emailcheck',
            data: $scope.emailModel,
            dataType: 'json'
        }).then(function (response) {
            $scope.emailCheckResponse = response.data;
            if ($scope.emailCheckResponse === true) {
                // user account exists, send activation code
                $scope.sendActivationCode();
            } else {
                // email does not exist, display an error message
                $scope.emaildoesnotexist = true;
                $scope.veriyingEmail = false;
                $scope.buttonText = 'Send Activation Code';
            }
        }, function (error) {
            $scope.veriyingEmail = false;
            console.log(error);
        });
    };

    $scope.sendActivationCode = function () {
        $scope.veriyingEmail = true;
        $http({
            method: 'POST',
            url: baseUrl + 'accounts/sendactivationcode',
            data: $scope.emailModel,
            dataType: 'json'
        }).then(function (response) {
            
        }, function (error) {
            $scope.veriyingEmail = false;
            console.log(error);
        });
    };
});