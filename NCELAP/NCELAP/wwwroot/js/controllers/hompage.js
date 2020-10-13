homepageModule.controller('indexPageCtrl', function ($scope, $http) {
    $scope.contactFormModel = {};

    $scope.submittinContactForm = function () {
        console.log($scope.contactFormModel);
        //$.LoadingOverlay("show");
        $("#contactForm").LoadingOverlay("show", {
            background: "rgba(165, 190, 100, 0.5)"
        });

        var message = document.getElementById('supportMessage').value;
        $scope.contactFormModel.Message = message;

        setTimeout(function () {
            $http({
                method: 'POST',
                url: baseUrl + 'accounts/contactsupport',
                data: $scope.contactFormModel,
                dataType: 'json'
            }).then(function (response) {
                $scope.supportResponse = response.data;

                if ($scope.supportResponse === true) {
                    $("#contactForm").LoadingOverlay("hide", {
                    });

                    swal({
                        title: "Success",
                        text: "Message sent successfully, you will be contacted shortly...",
                        type: "success",
                        confirmButtonText: "Ok"
                    }).then((result) => {
                        $scope.contactFormModel = {};
                        window.location.href = '/';
                    });
                }

            }, function (error) {
                $("#contactForm").LoadingOverlay("hide", {
                });
                console.log(error);
            });
        }, 2000);
    };
});