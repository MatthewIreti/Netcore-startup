userAccessModule.controller('activationPageCtrl', function ($scope, $http) {
    $scope.settingUpAccount = false;
    $scope.userinfoloaded = false;
    $scope.activatingaccount = false;
    
    // Dynamic Password Strength Validation
    var result = $("#strength");

    $('#user-password').keyup(function () {
        $(".result").html(checkStrength($('#user-password').val()));
    });

    function checkStrength(password) {
        //initial strength
        var strength = 0;

        if (password.length === 0) {
            result.removeClass();
            return '';
        }
        //if the password length is less than 6, return message.
        if (password.length < 8) {
            result.removeClass();
            result.addClass('short');
            $scope.passwordok = false;
            return 'too short';
        }

        //length is ok, lets continue.

        //if length is 8 characters or more, increase strength value
        if (password.length > 9) strength += 1;

        //if password contains both lower and uppercase characters, increase strength value
        if (password.match(/([a-z].*[A-Z])|([A-Z].*[a-z])/)) strength += 1;

        //if it has numbers and characters, increase strength value
        if (password.match(/([a-zA-Z])/) && password.match(/([0-9])/)) strength += 1;

        //if it has one special character, increase strength value
        if (password.match(/([!,%,&,@,#,$,^,*,?,_,~])/)) strength += 1;

        //if it has two special characters, increase strength value
        if (password.match(/(.*[!,%,&,@,#,$,^,*,?,_,~].*[!,",%,&,@,#,$,^,*,?,_,~])/)) strength += 1;

        //now we have calculated strength value, we can return messages

        //if value is less than 2
        if (strength < 2) {
            result.removeClass();
            result.addClass('weak');
            $scope.passwordok = false;
            return 'weak';
        } else if (strength === 2) {
            result.removeClass();
            result.addClass('good');
            $scope.passwordok = true;
            return 'good';
        } else {
            result.removeClass();
            result.addClass('strong');
            $scope.passwordok = true;
            return 'strong';
        }
    }


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