userAccessModule.controller('registrationPageCtrl', function ($scope, $http) {
    $scope.emailExist = false; //"is-valid";
    $scope.shareholders = [];
    $scope.directors = [];
    $scope.legalStatuses = [
        { Name: "Sole Proprietorship", Checked: false },
        { Name: "Partnership Proprietorship", Checked: true },
        { Name: "Public Limited Liability Company", Checked: false },
        { Name: "Cooperative Society", Checked: false },
        { Name: "Other (please sepcify)", Checked: false }
    ];

    $scope.custProspectUploads = {
        CertificateOfRegistrationFileName: '', CertificateOfRegistrationFileExtension: '', CertificateOfRegistrationBase64: '',
        CertificateOfIncorporationFileName: '', CertificateOfIncorporationFileExtension: '', CertificateOfIncorporationBase64: '',
        MemorandumArticlesOfAssociationFileName: '', MemorandumArticlesOfAssociationFileExtension: '', MemorandumArticlesOfAssociationBase64: '',
        DeedOfPartnershipFileName: '', DeedOfPartnershipFileExtension: '', DeedOfPartnershipBase64: '',
        DeedOfTrustFileName: '', DeedOfTrustFileExtension: '', DeedOfTrustBase64: ''
    };

    $scope.registrationInformationModel = {
        BusinessName: '', Email: '', Password: '', RepeatPassword: '', Telephone: '+234', Mobile: '+234', PostalCode: '', WebAddress: 'https://www.', Address: '',
        AuthorizedRepName: '', AuthorizedRepEmail: '', AuthorizedRepMobile: '', AuthorizedRepPhysicalAddress: '', LegalStatus: $scope.legalStatuses
    };

    $scope.shareHolderModel = {
        Name: '', Address: '', Nationality: '', CountryOfUsualResidence: ''
    };

    $scope.shareHolderModel = {
        Name: '', Address: '', Nationality: '', CountryOfUsualResidence: ''
    };


    $scope.checkSelectedLegalStatus = function () {
        console.log($scope.legalStatuses);
    };

    $scope.submitForm = function () {
        alert('hey');
    };

    $scope.logout = function () {
        localStorage.clear();
        window.location.href = '/';
    };

    $scope.passwordsMatch = function () {
        if ($scope.registrationInformationModel.Password.length === $scope.registrationInformationModel.RepeatPassword.length) {
            if ($scope.registrationInformationModel.Password !== $scope.registrationInformationModel.RepeatPassword) {
                $scope.passwordsDoesNotMatch = true;
            } else {
                $scope.passwordsDoesNotMatch = false;
            }
        }
    };

    $scope.saveBusinessAccountInformation = function () {
        var businessAddress = document.getElementById("addressLocation").value;
        var authorizedRepAddress = document.getElementById("addressAuthorizedRepresentative").value;

        $scope.registrationInformationModel.Address = businessAddress;
        $scope.registrationInformationModel.AuthorizedRepPhysicalAddress = authorizedRepAddress;

        console.log($scope.registrationInformationModel);

        $http({
            method: 'POST',
            url: baseUrl + 'accounts/registeredbusiness',
            //data: $scope.inventoryReportModel,
            data: $scope.registrationInformationModel,
            //'headers': { 'Authorization': 'Bearer ' + authToken },
            dataType: 'json'
        }).then(function (response) {
            if (response.data) {
                alert('Save');
            }
        }, function (error) {
            console.log(error);
            // $scope.hideProcessSpinner();
            // $scope.errorNotification(error);
            //errorHandler(error);
        });
    };

    $scope.tokenValidityHandler = function () {
        var token = extractToken();
        var tokenExpired = jwtHelper.isTokenExpired(token);
        console.log('Token expired? ' + tokenExpired);

        if (tokenExpired === true) {
            showSessionTimedOutDialog();
        }
    };

    console.log($scope.legalStatuses.length);
});