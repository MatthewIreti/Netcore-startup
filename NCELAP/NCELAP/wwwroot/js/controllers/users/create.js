appModule.controller('usersCreateCtrl', function ($scope, $http) {

    $scope.userCreateFormModel = { Name: '', Email: '', Department: '', CreatedByRecId: 0, CustTableRecId: 0 };
    $scope.userEmailModel = { Value: '' };
    $scope.verifyingEmail = false;

    $scope.setCRecIdParams = function () {
        var loggedInUser = localStorage.getItem('loggedInUser');
        $scope.loggedInUser = JSON.parse(loggedInUser);

        $scope.userCreateFormModel.CustTableRecId = $scope.loggedInUser.custTableRecId;
        $scope.userCreateFormModel.CreatedByRecId = $scope.loggedInUser.recordId;

    };

    $scope.createUserAccount = function () {
        console.log($scope.userCreateFormModel);

        setTimeout(function () {
            $http({
                method: 'POST',
                url: baseUrl + 'accounts/createuser',
                data: $scope.userCreateFormModel,
                dataType: 'json'
            }).then(function (response) {
                $scope.userCreateResponse = response.data;

            }, function (error) {
                console.log(error);
            });
        }, 2000);
        
    };

    $scope.emailCheck = function () {
        console.log($scope.userEmailModel);
        $scope.userEmailModel.Value = $scope.userCreateFormModel.Email;
        $scope.verifyingEmail = true;


        setTimeout(function () {
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
        }, 2000);
        
    };

    $scope.toastr = function () {
        toastr.success('Have fun storming the castle!', 'Miracle Max Says');
    };

    $scope.setCRecIdParams();
});
