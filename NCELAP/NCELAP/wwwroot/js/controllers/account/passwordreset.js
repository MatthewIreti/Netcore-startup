userAccessModule.controller('passwordResetPageCtrl', function ($scope, $http) {
    $scope.emailModel = {
        Value: ''
    };
    $scope.emailPassword = {};

    $scope.buttonText = 'Proceed';
    $scope.showEmailInputDiv = true;

    // Dynamic Password Strength Validation
    var result = $("#strength");

    $('#userPassword').keyup(function () {
        $(".result").html(checkStrength($('#userPassword').val()));
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

    $scope.emailCheck = function () {
        $scope.busy = true;
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
                $scope.busy = false;
                $scope.buttonText = 'Proceed';
            }
        }, function (error) {
            $scope.busy = false;
            console.log(error);
        });
    };

    $scope.sendActivationCode = function () {
        $scope.busy = true;
        $scope.buttonText = 'Sending Password Reset Code...';

        $http({
            method: 'POST',
            url: baseUrl + 'accounts/sendpassresetcode',
            data: $scope.emailModel,
            dataType: 'json'
        }).then(function (response) {
            $scope.activationCodeResponse = response.data;
            if ($scope.activationCodeResponse.status === true) {
                // password reset code sent successfully
                $scope.showEmailInputDiv = false;

                $scope.passresetsent = true;
                toastr.success('Password reset code sent successfully!', 'Success!', {});
                $scope.activationCodeSent = true;
            }
            $scope.busy = false;
        }, function (error) {
                $scope.busy = false;
                toastr.error("Could not sent password reset code. Please try again...", 'Oops!', {});

            console.log(error);
        });
    };

    $scope.showPassResetForm = function () {
        if ($scope.activationCodeResponse.activationCode === $scope.activationCode) {
            // show password reset form segment
            $scope.activationCodeValid = true;
            $scope.activationCodeSent = false;
            $scope.passwordok = 'false';
        }
    };

    $scope.resetNcelasUserPassword = function () {
        $scope.busy = true;
        $scope.buttonText = 'Resetting Password...';
        $scope.emailPassword.Password = $scope.newpassword;
        $scope.emailPassword.Email = $scope.emailModel.Value;

        $http({
            method: 'POST',
            url: baseUrl + 'accounts/resetncelasuserpassword',
            data: $scope.emailPassword,
            dataType: 'json'
        }).then(function (response) {
            if (response.data === true) {
                // password reset code sent successfully
                $scope.passresetsent = true;
                toastr.success('Password reset was successful!', 'Success!', {});
                $scope.activationCodeSent = true;
                $scope.hideContent = true;
                swal({
                    title: "Success",
                    text: "Password reset was successful!",
                    type: "success",
                    confirmButtonText: "Ok"
                }).then((result) => {
                    window.location.href = '/account/login/';
                });
            }
            $scope.busy = false;
        }, function (error) {
            $scope.busy = false;
            toastr.error("Could not sent password reset code. Please try again...", 'Oops!', {});

            console.log(error);
        });
    };
});