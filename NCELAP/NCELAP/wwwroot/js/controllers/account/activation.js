userAccessModule.controller('activationPageCtrl', function ($scope, $http) {
    $scope.settingUpAccount = false;
    $scope.userinfoloaded = false;
    $scope.activatingaccount = false;
    

    $scope.extractUserId = function () {
        var urlParts = window.location.href.split('/');
        $scope.userRecId = urlParts[5];

        $scope.getUserInfoByRecordId();
    };

    $scope.activationModel = {
        Email: '',
        Password: '',
        RepeatPassword: ''
    };

    $scope.activateAccount = function () {
        console.log($scope.loginModel);
        $scope.activatingaccount = true;
        
        setTimeout(function () {
            $http({
                method: 'POST',
                url: baseUrl + 'accounts/activate',
                data: $scope.activationModel,
                dataType: 'json'
            }).then(function (response) {
                $scope.accountActivationResponseResponse = response.data;
                if ($scope.accountActivationResponseResponse === true) {
                    // user account exists
                    localStorage.setItem("loggedInUser", JSON.stringify($scope.loginResponse));
                    $scope.activatingaccount = false;
                    window.location.href = '/account/login';
                } 
            }, function (error) {
                    $scope.activatingaccount = false;
                console.log(error);
            });
        }, 3000);
        
    };

    $scope.getUserInfoByRecordId = function () {
        $http({
            method: 'GET',
            url: baseUrl + 'accounts/useraccount/' + $scope.userRecId
        }).then(function (response) {
            $scope.usernfo = response.data;
            $scope.activationModel.Email = $scope.usernfo.email;
            if ($scope.usernfo.name) {
                $scope.userinfoloaded = true;
            }
            console.log($scope.headerDetail);
        }, function (error) {
            console.log(error);
        });
    };

    $scope.extractUserId();
});