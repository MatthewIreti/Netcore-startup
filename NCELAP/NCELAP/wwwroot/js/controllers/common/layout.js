appModule.controller('layoutCtrl', function ($scope, $http) {
    $scope.contactFormModel = {};

    $scope.logout = function () {
        localStorage.clear();
        window.location.href = '/';
    };

    

    $scope.getLoggedInUserDetails = function () {
        var loggedInUser = localStorage.getItem('loggedInUser');
        $scope.loggedInUser = JSON.parse(loggedInUser);


        if ($scope.loggedInUser.name !== '') {
            //alert($scope.loggedInUser.name);
            $scope.name = $scope.loggedInUser.name;
        }

        if ($scope.loggedInUser.email !== '') {
            $scope.email = $scope.loggedInUser.email;
        }
        // alert($scope.licenseApplicationModel.Customer);
    };

    $scope.logoutAction = function () {
        //alert('yass');
        localStorage.clear();
        window.location.href = '/';
    };

    //$scope.tokenValidityHandler = function () {
    //    var token = extractToken();
    //    var tokenExpired = jwtHelper.isTokenExpired(token);
    //    console.log('Token expired? ' + tokenExpired);

    //    if (tokenExpired === true) {
    //        showSessionTimedOutDialog();
    //    }
    //};

    $scope.getLoggedInUserDetails();
});
