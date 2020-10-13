appModule.controller('UsersCreate', function ($scope, $http) {

    //$scope.userCreateFormModel = { Name: '', Email: '', Department: '', CreatedByRecId: 0, CustTableRecId: 0 };
    $scope.userEmailModel = { Value: '' };
    $scope.verifyingEmail = false;

    $scope.setCRecIdParams = function () {
        var loggedInUser = localStorage.getItem('loggedInUser');
        $scope.loggedInUser = JSON.parse(loggedInUser);

        $scope.userCreateFormModel.CustTableRecId = $scope.loggedInUser.custTableRecId;
        $scope.userCreateFormModel.CreatedByRecId = $scope.loggedInUser.recordId;
    };

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

    $scope.emailCheck = function () {
        $scope.userEmailModel.Value = $scope.userCreateFormModel.Email;
        $scope.verifyingEmail = true;
        $scope.emailCheckResponse = false;

        //alert($scope.userEmailModel.Value);

        $http({
            method: 'POST',
            url: baseUrl + 'accounts/emailcheck',
            data: $scope.userEmailModel,
            dataType: 'json'
        }).then(function (response) {
            $scope.emailCheckResponse = response.data;
            console.log('Email account exists?: ' + $scope.emailCheckResponse);
            $scope.verifyingEmail = false;

        }, function (error) {
            console.log(error);
            $scope.verifyingEmail = false;
        });
        
    };

    $scope.initUserModel = function () {
        $scope.userCreateFormModel = { Name: '', Email: '', Department: '', CreatedByRecId: 0, CustTableRecId: 0 };
    };

    $scope.initUserModel();
    $scope.setCRecIdParams();
});
