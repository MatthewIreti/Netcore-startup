userAccessModule.controller('registrationPageCtrl', function ($scope, $http) {
    $scope.emailExist = false; //"is-valid";
    $scope.processing = false;
    var maxFileSize = 2500000; // 2 Megabytes
    $scope.shareholders = [];
    $scope.directors = [];
    $scope.yearsOfExperience = [];
    $scope.staffs = []; //name,yearsofexperience,role,careersummary,custpropectre ==> CustProspectStaffList
    $scope.reportAttachment = '';

    $scope.legalStatuses = [
        { Name: "Sole Proprietorship", Value: "SoleProprietorship", },
        { Name: "Partnership", Value: "Partnership"},
        { Name: "Public Limited Liability", Value: "PublicLimitedLiability", },
        { Name: "Private Limited Liability", Value: "PrivateLimitedLiability"},
        { Name: "Cooperative Society", Value: "CooperativeSociety"},
        { Name: "Others (please sepcify)", Value: "Others"}
    ];

    $scope.convictedActs = [
        { Name: "N/A", Value: "N/A" },
        { Name: "Murder", Value: "Murder" },
        { Name: "Theft", Value: "Theft" },
        { Name: "Assault", Value: "Assault" },
        { Name: "Others", Value: "Others" }
    ];

    
    $scope.businessInfoDocumentsForUpload = {
        CertificateOfRegistrationFileName: '', CertificateOfRegistrationFileExtension: '', CertificateOfRegistrationBase64: '',
        CertificateOfIncorporationFileName: '', CertificateOfIncorporationFileExtension: '', CertificateOfIncorporationBase64: '',
        MemorandumArticlesOfAssociationFileName: '', MemorandumArticlesOfAssociationFileExtension: '', MemorandumArticlesOfAssociationBase64: '',
        DeedOfPartnershipFileName: '', DeedOfPartnershipFileExtension: '', DeedOfPartnershipBase64: '',
        DeedOfTrustFileName: '', DeedOfTrustFileExtension: '', DeedOfTrustBase64: ''
    };

    $scope.custProspectUploadsFileSizeCheckModel = {
        CertificationOfRegistrationFileSizeValid: '', CertificateOfIncorporationFileSizeValid: '', MemorandumArticlesOfAssociationFileSizeValid: '',
        DeedOfPartnershipFileSizeValid: '', DeedOfTrustFileSizeValid: ''
    };

    $scope.custProspectUploadsFileExtensionCheckModel = {
        CertificationOfRegistrationFileExtensionValid: '', CertificateOfIncorporationFileExtensionValid: '', MemorandumArticlesOfAssociationFileExtensionValid: '',
        DeedOfPartnershipFileExtensionValid: '', DeedOfTrustFileExtensionValid: ''
    };

    // custProspectUploads
    //$scope.businessInfoDocumentsForUpload = {
    //    CustProspectRecId: 0, CertificateOfRegistrationFileName: '', CertificateOfRegistrationFileExtension: '', CertificateOfRegistrationBase64: '',
    //    CertificateOfIncorporationFileName: '', CertificateOfIncorporationFileExtension: '', CertificateOfIncorporationBase64: ''
    //};

    $scope.registrationInformationModel = {
        BusinessName: '', Email: '', Password: '', RepeatPassword: '', Telephone: '+234', Mobile: '+234', PostalCode: '', WebAddress: 'https://www.', Address: '',
        AuthorizedRepName: '', AuthorizedRepEmail: '', AuthorizedRepMobile: '', AuthorizedRepPhysicalAddress: '', OtherLegalStatus: '', DirectorCriminalAct: '', DetailsOfConviction: '',
        SupportingDocuments: $scope.businessInfoDocumentsForUpload, CompanyLegalStatus: '', Shareholders: $scope.shareholders, Directors: $scope.directors, Staffs: $scope.staffs
    };

    $scope.initDirectorModel = function(){
        $scope.directorModel = {
            Name: '', Address: '', Nationality: '', CountryOfUsualResidence: ''
        };
        document.getElementById("DirectorAddress").value = '';
    };
    
    $scope.initShareHolderModel = function () {
        $scope.shareHolderModel = {
            Name: '', Address: '', Nationality: '', CountryOfUsualResidence: ''
        };
        document.getElementById("ShareHolderAddress").value = '';
    };

    $scope.initStaffModel = function () {
        $scope.staffModel = {
            Name: '', Role: '', YearsOfExperience: null, CareerSummary: '', CustProspect: 0
        };
        document.getElementById("staffCareerSummary").value = '';
    };
    
    $scope.checkSelectedLegalStatus = function () {
        console.log($scope.legalStatuses);
    };

    $scope.getCountries = function () {
        $http.get('https://restcountries.eu/rest/v2/all?fields=name').then(function (response) {
            $scope.countries = response.data;
        });
    };

    $scope.addShareholder = function () {
        var addressOfShareholder = document.getElementById("ShareHolderAddress").value;
        $scope.shareHolderModel.Address = addressOfShareholder;

        $scope.shareholders.push($scope.shareHolderModel);

        $scope.initShareHolderModel();
    };

    $scope.removeShareholder = function (objectToRemove) {
        var objectToRemovePosition = $scope.shareholders.indexOf(objectToRemove);
        $scope.shareholders.splice(objectToRemovePosition, 1);
    };

    $scope.addStaff = function () {
        var careerSummary = document.getElementById("staffCareerSummary").value;
        $scope.staffModel.CareerSummary = careerSummary;

        $scope.staffs.push($scope.staffModel);
        console.log($scope.staffModel);
        $scope.initStaffModel();
    };

    $scope.removeStaff = function (objectToRemove) {
        var objectToRemovePosition = $scope.staffs.indexOf(objectToRemove);
        $scope.staffs.splice(objectToRemovePosition, 1);
    };

    $scope.addDirector = function () {
        var addressOfDirector = document.getElementById("DirectorAddress").value;
        $scope.directorModel.Address = addressOfDirector;

        $scope.directors.push($scope.directorModel);

        $scope.initDirectorModel();
    };

    $scope.removeDirector = function (objectToRemove) {
        var objectToRemovePosition = $scope.directors.indexOf(objectToRemove);
        $scope.directors.splice(objectToRemovePosition, 1);
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

    $scope.evaluateLegalStatusSelection = function () {
        if ($scope.registrationInformationModel.CompanyLegalStatus === "Others") {
            $scope.registrationInformationModel.OtherLegalStatus = $scope.OtherLegalStatus;
        } else {
            $scope.registrationInformationModel.OtherLegalStatus = '';
        }
    };

    $scope.certificateOfRegistrationFileHandler = function () {
        var fi = document.getElementById('certificateOfRegistration');
        var selectedFile;

        if (fi.files.length > 0) {
            for (var i = 0; i <= fi.files.length - 1; i++) {
                var reader = new FileReader();
                selectedFile = fi.files.item(0);

                reader.readAsBinaryString(selectedFile);
                if (selectedFile !== undefined) {
                    const lastDot = selectedFile.name.lastIndexOf('.');

                    reader.onload = (function (theFile) {
                        return function (e) {
                            var binaryData = e.target.result;
                            var base64String = window.btoa(binaryData);
                            const fileNameFormatted = selectedFile.name.substring(0, lastDot);
                            const extension = extractExtensionFromFileName(selectedFile.name);

                            $scope.businessInfoDocumentsForUpload.CertificateOfRegistrationFileName = fileNameFormatted;
                            $scope.businessInfoDocumentsForUpload.CertificateOfRegistrationFileExtension = extension;
                            $scope.businessInfoDocumentsForUpload.CertificateOfRegistrationBase64 = base64String;

                            if (selectedFile.size > maxFileSize) {
                                $scope.custProspectUploadsFileSizeCheckModel.CertificationOfRegistrationFileSizeValid = false;
                                document.getElementById('certificateOfRegistrationWarningIndicator').innerHTML = "File size cannot be more than 2MB!";
                                
                            } else {
                                $scope.custProspectUploadsFileSizeCheckModel.CertificationOfRegistrationFileSizeValid = true;
                                document.getElementById('certificateOfRegistrationWarningIndicator').innerHTML = "";
                            }

                            if ($scope.custProspectUploadsFileSizeCheckModel.CertificationOfRegistrationFileSizeValid === true) {
                                if (extension === 'pdf') {
                                    $scope.custProspectUploadsFileSizeCheckModel.CertificationOfRegistrationFileExtensionValid = true;
                                    document.getElementById('certificateOfRegistrationWarningIndicator').innerHTML = "";
                                } else {
                                    $scope.custProspectUploadsFileSizeCheckModel.CertificationOfRegistrationFileExtensionValid = false;
                                    document.getElementById('certificateOfRegistrationWarningIndicator').innerHTML = "Only PDF files are allowed!";
                                }
                            }

                            
                            console.log($scope.custProspectUploadsFileSizeCheckModel.CertificationOfRegistrationFileSizeValid);
                        };
                    })(selectedFile);
                }
                console.log($scope.businessInfoDocumentsForUpload);
            }
        }
    };

    $scope.certificateOfIncorporationFileHandler = function () {
        var fi = document.getElementById('certificateOfIncorporation');
        var selectedFile;

        if (fi.files.length > 0) {
            for (var i = 0; i <= fi.files.length - 1; i++) {
                var reader = new FileReader();
                selectedFile = fi.files.item(0);

                reader.readAsBinaryString(selectedFile);
                if (selectedFile !== undefined) {
                    const lastDot = selectedFile.name.lastIndexOf('.');

                    reader.onload = (function (theFile) {
                        return function (e) {
                            var binaryData = e.target.result;
                            var base64String = window.btoa(binaryData);
                            const fileNameFormatted = selectedFile.name.substring(0, lastDot);
                            const extension = extractExtensionFromFileName(selectedFile.name);

                            $scope.businessInfoDocumentsForUpload.CertificateOfIncorporationFileName = fileNameFormatted;
                            $scope.businessInfoDocumentsForUpload.CertificateOfIncorporationFileExtension = extension;
                            $scope.businessInfoDocumentsForUpload.CertificateOfIncorporationBase64 = base64String;

                            if (selectedFile.size > maxFileSize) {
                                $scope.custProspectUploadsFileSizeCheckModel.CertificateOfIncorporationFileSizeValid = false;
                                document.getElementById('certificateOfIncorporationWarningIndicator').innerHTML = "File size cannot be more than 2MB!";

                            } else {
                                $scope.custProspectUploadsFileSizeCheckModel.CertificateOfIncorporationFileSizeValid = true;
                                document.getElementById('certificateOfIncorporationWarningIndicator').innerHTML = "";
                            }

                            if ($scope.custProspectUploadsFileSizeCheckModel.CertificateOfIncorporationFileSizeValid === true) {
                                if (extension === 'pdf') {
                                    $scope.custProspectUploadsFileSizeCheckModel.CertificateOfIncorporationFileExtensionValid = true;
                                    document.getElementById('certificateOfIncorporationWarningIndicator').innerHTML = "";
                                } else {
                                    $scope.custProspectUploadsFileSizeCheckModel.CertificateOfIncorporationFileExtensionValid = false;
                                    document.getElementById('certificateOfIncorporationWarningIndicator').innerHTML = "Only PDF files are allowed!";
                                }
                            }
                            console.log($scope.custProspectUploadsFileSizeCheckModel.CertificateOfIncorporationFileSizeValid);
                        };
                    })(selectedFile);
                }
                console.log($scope.businessInfoDocumentsForUpload);
            }
        }
    };

    $scope.memorandumOfAssociationFileHandler = function () {
        var fi = document.getElementById('memorandumOfAssociation');
        var selectedFile;

        if (fi.files.length > 0) {
            for (var i = 0; i <= fi.files.length - 1; i++) {
                var reader = new FileReader();
                selectedFile = fi.files.item(0);

                reader.readAsBinaryString(selectedFile);
                if (selectedFile !== undefined) {
                    const lastDot = selectedFile.name.lastIndexOf('.');

                    reader.onload = (function (theFile) {
                        return function (e) {
                            var binaryData = e.target.result;
                            var base64String = window.btoa(binaryData);
                            const fileNameFormatted = selectedFile.name.substring(0, lastDot);
                            const extension = extractExtensionFromFileName(selectedFile.name);

                            $scope.businessInfoDocumentsForUpload.MemorandumArticlesOfAssociationFileName = fileNameFormatted;
                            $scope.businessInfoDocumentsForUpload.MemorandumArticlesOfAssociationFileExtension = extension;
                            $scope.businessInfoDocumentsForUpload.MemorandumArticlesOfAssociationBase64 = base64String;

                            if (selectedFile.size > maxFileSize) {
                                $scope.custProspectUploadsFileSizeCheckModel.MemorandumArticlesOfAssociationFileSizeValid = false;
                                document.getElementById('memorandumOfAssociationIndicator').innerHTML = "File size cannot be more than 2MB!";

                            } else {
                                $scope.custProspectUploadsFileSizeCheckModel.MemorandumArticlesOfAssociationFileSizeValid = true;
                                document.getElementById('memorandumOfAssociationIndicator').innerHTML = "";
                            }

                            if ($scope.custProspectUploadsFileSizeCheckModel.MemorandumArticlesOfAssociationFileSizeValid === true) {
                                if (extension === 'pdf') {
                                    $scope.custProspectUploadsFileSizeCheckModel.MemorandumArticlesOfAssociationFileExtensionValid = true;
                                    document.getElementById('memorandumOfAssociationIndicator').innerHTML = "";
                                } else {
                                    $scope.custProspectUploadsFileSizeCheckModel.MemorandumArticlesOfAssociationFileExtensionValid = false;
                                    document.getElementById('memorandumOfAssociationIndicator').innerHTML = "Only PDF files are allowed!";
                                }
                            }
                            console.log($scope.custProspectUploadsFileSizeCheckModel.MemorandumArticlesOfAssociationFileSizeValid);
                        };
                    })(selectedFile);
                }
                console.log($scope.businessInfoDocumentsForUpload);
            }
        }
    };

    $scope.deedOfPartnershipFileHandler = function () {
        var fi = document.getElementById('deedOfPartnership');
        var selectedFile;

        if (fi.files.length > 0) {
            for (var i = 0; i <= fi.files.length - 1; i++) {
                var reader = new FileReader();
                selectedFile = fi.files.item(0);

                reader.readAsBinaryString(selectedFile);
                if (selectedFile !== undefined) {
                    const lastDot = selectedFile.name.lastIndexOf('.');

                    reader.onload = (function (theFile) {
                        return function (e) {
                            var binaryData = e.target.result;
                            var base64String = window.btoa(binaryData);
                            const fileNameFormatted = selectedFile.name.substring(0, lastDot);
                            const extension = extractExtensionFromFileName(selectedFile.name);

                            $scope.businessInfoDocumentsForUpload.DeedOfPartnershipFileName = fileNameFormatted;
                            $scope.businessInfoDocumentsForUpload.DeedOfPartnershipFileExtension = extension;
                            $scope.businessInfoDocumentsForUpload.DeedOfPartnershipBase64 = base64String;

                            if (selectedFile.size > maxFileSize) {
                                $scope.custProspectUploadsFileSizeCheckModel.DeedOfPartnershipFileSizeValid = false;
                                document.getElementById('deedOfPartnershipIndicator').innerHTML = "File size cannot be more than 2MB!";

                            } else {
                                $scope.custProspectUploadsFileSizeCheckModel.DeedOfPartnershipFileSizeValid = true;
                                document.getElementById('deedOfPartnershipIndicator').innerHTML = "";
                            }

                            if ($scope.custProspectUploadsFileSizeCheckModel.DeedOfPartnershipFileSizeValid === true) {
                                if (extension === 'pdf') {
                                    $scope.custProspectUploadsFileSizeCheckModel.DeedOfPartnershipFileExtensionValid = true;
                                    document.getElementById('deedOfPartnershipIndicator').innerHTML = "";
                                } else {
                                    $scope.custProspectUploadsFileSizeCheckModel.DeedOfPartnershipFileExtensionValid = false;
                                    document.getElementById('deedOfPartnershipIndicator').innerHTML = "Only PDF files are allowed!";
                                }
                            }
                            console.log($scope.custProspectUploadsFileSizeCheckModel.DeedOfPartnershipFileSizeValid);
                        };
                    })(selectedFile);
                }
                //console.clear();
                console.log($scope.businessInfoDocumentsForUpload);
            }
        }
    };

    $scope.deedOfTrustFileHandler = function () {
        var fi = document.getElementById('deedOfTrust');
        var selectedFile;

        if (fi.files.length > 0) {
            for (var i = 0; i <= fi.files.length - 1; i++) {
                var reader = new FileReader();
                selectedFile = fi.files.item(0);

                reader.readAsBinaryString(selectedFile);
                if (selectedFile !== undefined) {
                    const lastDot = selectedFile.name.lastIndexOf('.');

                    reader.onload = (function (theFile) {
                        return function (e) {
                            var binaryData = e.target.result;
                            var base64String = window.btoa(binaryData);
                            const fileNameFormatted = selectedFile.name.substring(0, lastDot);
                            const extension = extractExtensionFromFileName(selectedFile.name);

                            $scope.businessInfoDocumentsForUpload.DeedOfTrustFileName = fileNameFormatted;
                            $scope.businessInfoDocumentsForUpload.DeedOfTrustFileExtension = extension;
                            $scope.businessInfoDocumentsForUpload.DeedOfTrustBase64 = base64String;

                            if (selectedFile.size > maxFileSize) {
                                $scope.custProspectUploadsFileSizeCheckModel.DeedOfTrustFileSizeValid = false;
                                document.getElementById('deedOfTrustIndicator').innerHTML = "File size cannot be more than 2MB!";

                            } else {
                                $scope.custProspectUploadsFileSizeCheckModel.DeedOfTrustFileSizeValid = true;
                                document.getElementById('deedOfTrustIndicator').innerHTML = "";
                            }

                            if ($scope.custProspectUploadsFileSizeCheckModel.DeedOfTrustFileSizeValid === true) {
                                if (extension === 'pdf') {
                                    $scope.custProspectUploadsFileSizeCheckModel.DeedOfTrustFileExtensionValid = true;
                                    document.getElementById('deedOfTrustIndicator').innerHTML = "";
                                } else {
                                    $scope.custProspectUploadsFileSizeCheckModel.DeedOfTrustFileExtensionValid = false;
                                    document.getElementById('deedOfTrustIndicator').innerHTML = "Only PDF files are allowed!";
                                }
                            }
                            console.log($scope.custProspectUploadsFileSizeCheckModel.DeedOfTrustFileSizeValid);
                        };
                    })(selectedFile);
                }
                //console.clear();
                console.log($scope.businessInfoDocumentsForUpload);
            }
        }

        $scope.registrationInformationModel.CompanyLegalStatus = $scope.registrationInformationModel.CompanyLegalStatus;
    };

    $scope.saveBusinessAccountInformation = function () {
        $scope.processing = true;
        var businessAddress = document.getElementById("addressLocation").value;
        var authorizedRepAddress = document.getElementById("addressAuthorizedRepresentative").value;
        var detailsOfConviction = document.getElementById("DirectorCriminalActDetailsfConviction").value;

        $scope.registrationInformationModel.Address = businessAddress;
        $scope.registrationInformationModel.AuthorizedRepPhysicalAddress = authorizedRepAddress;
        $scope.registrationInformationModel.DetailsOfConviction = detailsOfConviction;

        $scope.registrationInformationModel.Shareholders = $scope.shareholders;
        $scope.registrationInformationModel.SupportingDocuments = $scope.businessInfoDocumentsForUpload;
        
        console.log($scope.registrationInformationModel);

        $http({
            method: 'POST',
            url: baseUrl + 'accounts/registeredbusiness',
            data: $scope.registrationInformationModel,
            dataType: 'json'
        }).then(function (response) {
            if (response.data === true) {
                window.location.href = "/account/registrationcomplete";
            }
        }, function (error) {
            console.log(error);
        });
    };

    $scope.loadYearsofExperience = function () {
        for (var i = 1; i < 50; i++) {
            $scope.yearsOfExperience.push(parseInt(i));
        }
    };

    $scope.tokenValidityHandler = function () {
        var token = extractToken();
        var tokenExpired = jwtHelper.isTokenExpired(token);
        console.log('Token expired? ' + tokenExpired);

        if (tokenExpired === true) {
            showSessionTimedOutDialog();
        }
    };

    $scope.loadYearsofExperience();
    $scope.initShareHolderModel();
    $scope.initDirectorModel();
    $scope.getCountries();
});