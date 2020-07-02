appModule.controller('layoutCtrl', function ($scope, $http, jwtHelper) {

    $scope.logout = function () {
        localStorage.clear();
        window.location.href = '/';
    };

    $scope.tokenValidityHandler = function () {
        var token = extractToken();
        var tokenExpired = jwtHelper.isTokenExpired(token);
        console.log('Token expired? ' + tokenExpired);

        if (tokenExpired === true) {
            showSessionTimedOutDialog();
        }
    };
});