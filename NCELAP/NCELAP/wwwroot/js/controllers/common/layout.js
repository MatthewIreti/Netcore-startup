appModule.controller('layoutCtrl', function ($scope/*$http, jwtHelper*/) {

    $scope.logout = function () {
        localStorage.clear();
        window.location.href = '/';
    };

    $scope.test = "sdnsdnb";
    $scope.getLoggedInUserDetails = function () {
        var loggedInUser = localStorage.getItem('loggedInUser');
        $scope.loggedInUser = JSON.parse(loggedInUser);

        

        if ($scope.loggedInUser.name !== '') {
            //alert($scope.loggedInUser.name);
            $scope.name = 'sdsdsdds';// $scope.loggedInUser.name;
        }

        if ($scope.loggedInUser.email !== '') {
            $scope.email = $scope.loggedInUser.email;
        }
        // alert($scope.licenseApplicationModel.Customer);
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