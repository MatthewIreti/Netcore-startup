appModule.controller('companyoperatorIndexCtrl', function ($scope, $http) {
    $scope.stakeholdersLocations = [];
    $scope.licenseFees = [];
    var maxFileSize = 2500000; // 2 Megabytes
    $scope.submittingInformation === 'false';
    $scope.submitButtonText = 'Submit Application';
    $scope.licensetypes = [
        { "Name": "Network Agent", "Value": "NetworkAgent", "Selected": true },
        { "Name": "Gas Shipper License", "Value": "GasShipperLicense", "Selected": false },
        { "Name": "Gas Transporter License", "Value":"GasTransporterLicense", "Selected": false }
    ];
    //$scope.selectedLicenseType = 'NetworkAgent';

    $scope.licenseApplicationUpload = {
        HoldRelatedLicenseFileName: '', HoldRelatedLicenseFileExtension: '', HoldRelatedLicenseBase64: '',
        HasRelatedLicenseFileName: '', HasRelatedLicenseFileExtension: '', HasRelatedLicenseBase64: '',
        HasLicenseRevokedFileName: '', HasLicenseRevokedFileExtension: '', HasLicenseRevokedLicenseBase64: '',
        HasLicenseRefusedFileName: '', HasLicenseRefusedFileExtension: '', HasLicenseRefusedLicenseBase64: '',
        ProposedArrangementAttachmentFileName: '', ProposedArrangementAttachmentFileExtension: '', ProposedArrangementAttachmentBase64: '',
        DeclarationSignatureFileName: '', DeclarationSignatureFileExtension: '', DeclarationSignatureBase64: ''
    };

    $scope.setCustomerRecId = function () {
        var loggedInUser = localStorage.getItem('loggedInUser');
        $scope.loggedInUser = JSON.parse(loggedInUser);

        $scope.licenseApplicationModel.Customer = $scope.loggedInUser.custTableRecId;
        $scope.licenseApplicationModel.CompanyName = $scope.loggedInUser.companyName;

        if ($scope.loggedInUser.custTableRecId) {
            $scope.getCustomerApplications($scope.loggedInUser.custTableRecId);
        }

        if ($scope.loggedInUser.recordId) {
            $scope.licenseApplicationModel.SubmittedBy = $scope.loggedInUser.recordId;
        }
        // alert($scope.licenseApplicationModel.Customer);
    };

    $scope.licenseApplicationModel = {
        CompanyName: '', Customer: 0, CustomerTier: '', SubmittedBy: 0, CustApplicationNum: '', CustLicenseType: '', EffectiveDate: '', HoldRelatedLicense: '', RelatedLicenseDetail: '',
        HasRelatedLicense: '', RelatedLicenseType: '', HasLicenseRevoked: '', RevokedLicenseType: '', HasGasApplicationRefused: '',
        RefusedLicenseType: '', AgentShipperName: '', AgentLocationOfShipper: '', EntryExitPoint: '', Location: '',
        MaximumNominatedCapacity: 0.0, PipelineAndGasTransporterName: '', GasPipelineNetwork: '', InstalledCapacity: '', DeclarationName: '', DeclarationCapacity: '', 
        DeclarationDate: '', ProposedArrangementLicensingActivity: '', HasStandardModificationRequest: '', ModificationRequestDetails: '',
        ModificationRequestReason: '', CustLicenseApplicationStatus: '',
        FileUploads: $scope.licenseApplicationUpload, StakeholderLocations: $scope.stakeholdersLocations
    };
    
    $scope.licenseApplicationUploadsFileSizeCheckModel = {
        HoldRelatedLicenseFileSizeValid: '', HasRelatedLicenseFileSizeValid: '', HasLicenseRevokedFileSizeValid: '', HasLicenseRefusedFileSizeValid: '', DeclarationSignatureValid: ''
    };

    $scope.holdRelatedLicenseSelectionHandler = function () {
        console.log($scope.licenseApplicationUpload);
        if ($scope.licenseApplicationModel.HoldRelatedLicense !== 'true') {
            $scope.licenseApplicationModel.RelatedLicenseDetail = '';
            $scope.licenseApplicationUpload.HoldRelatedLicenseFileName = '';
            $scope.licenseApplicationUpload.HoldRelatedLicenseFileExtension = '';
            $scope.licenseApplicationUpload.HoldRelatedLicenseBase64 = '';
            document.getElementById("holdRelatedLicense").value = "";

            console.log($scope.licenseApplicationUpload);
        }
    };

    $scope.hasRelatedLicenseSelectionHandler = function () {
        console.log($scope.licenseApplicationUpload);
        if ($scope.licenseApplicationModel.HasRelatedLicense !== 'true') {
            $scope.licenseApplicationModel.RelatedLicenseType = '';
            $scope.licenseApplicationUpload.HasRelatedLicenseFileName = '';
            $scope.licenseApplicationUpload.HasRelatedLicenseFileExtension = '';
            $scope.licenseApplicationUpload.HasRelatedLicenseBase64 = '';
            document.getElementById("hasRelatedLicense").value = "";

            console.log($scope.licenseApplicationUpload);
        }
    };

    $scope.hasLicenseRevokedHandler = function () {
        console.log($scope.licenseApplicationUpload);
        if ($scope.licenseApplicationModel.HasLicenseRevoked !== 'true') {
            $scope.licenseApplicationModel.RevokedLicenseType = '';
            $scope.licenseApplicationUpload.HasLicenseRevokedFileName = '';
            $scope.licenseApplicationUpload.HasLicenseRevokedFileExtension = '';
            $scope.licenseApplicationUpload.HasLicenseRevokedLicenseBase64 = '';
            document.getElementById("hasLicenseRevoked").value = "";

            console.log($scope.licenseApplicationUpload);
        }
    };

    $scope.hasLicenseRefusedHandler = function () {
        console.log($scope.licenseApplicationUpload);
        if ($scope.licenseApplicationModel.HasGasApplicationRefused !== 'true') {
            $scope.licenseApplicationModel.RefusedLicenseType = '';
            $scope.licenseApplicationUpload.HasLicenseRefusedFileName = '';
            $scope.licenseApplicationUpload.HasLicenseRefusedFileExtension = '';
            $scope.licenseApplicationUpload.HasLicenseRefusedLicenseBase64 = '';
            document.getElementById("hasRefusedLicense").value = "";

            console.log($scope.licenseApplicationUpload);
        }
    };

    $scope.getLicenseFees = function () {
        $http({
            method: 'GET',
            url: baseUrl + 'applications/licensefees/'
        }).then(function (response) {
            $scope.licenseFees = response.data;
            console.log($scope.licenseFees);
        }, function (error) {
            console.log(error);
        });
    };

    $scope.getCustomerApplications = function (custrecid) {
        $http({
            method: 'GET',
            url: baseUrl + 'applications/customer/' + custrecid
        }).then(function (response) {
            $scope.applications = response.data;
            console.log($scope.applications);

            // if the datatable instance already exist, destroy before recreating, otherwise, just create
            if ($.fn.DataTable.isDataTable('#applicationsTable')) {
                $('#applicationsTable').DataTable().destroy();
            }

            angular.element(document).ready(function () {
                dTable = $('#applicationsTable');
                dTable.DataTable({
                    "aaSorting": [] // disables first colum auto-sorting
                });
            });
        }, function (error) {
            console.log(error);
        });
    };

    $scope.evaluateGasShipperNominalCapacity = function (capacity) {
        $scope.licenseFeeEvaluated = false;

        for (var i = 0; i < $scope.licenseFees.length - 1; i++) {
            var currentLicenseFee = $scope.licenseFees[i];

            if (currentLicenseFee.licenseType === 'GasShipperLicense' && capacity >= currentLicenseFee.minimum && capacity <= currentLicenseFee.maximum) {
                $scope.licenseFeeEvaluated = true;
                $scope.statutoryFee = currentLicenseFee.statutory;
                $scope.processingFee = currentLicenseFee.processingFee;
                $scope.totalFee = parseInt($scope.statutoryFee) + parseInt($scope.processingFee);
                $scope.licenseApplicationModel.CustomerTier = currentLicenseFee.categoryDescription;
                console.log('licanse fee: ');
                console.log(currentLicenseFee);
            }
        }
    };

    $scope.evaluateGasTransporterNominalCapacity = function (capacity) {
        $scope.licenseFeeEvaluated = false;

        for (var i = 0; i < $scope.licenseFees.length - 1; i++) {
            var currentLicenseFee = $scope.licenseFees[i];

            if (currentLicenseFee.licenseType === 'GasTransporterLicense' && capacity >= currentLicenseFee.minimum && capacity <= currentLicenseFee.maximum) {
                $scope.licenseFeeEvaluated = true;
                $scope.statutoryFee = currentLicenseFee.statutory;
                $scope.processingFee = currentLicenseFee.processingFee;
                $scope.totalFee = parseInt($scope.statutoryFee) + parseInt($scope.processingFee);
                console.log('licanse fee: ');
                console.log(currentLicenseFee);
            }
        }
    };

    $scope.submitLicenseApplication = function () {
        var proposedArrangementDetails = document.getElementById("proposedArrangementDetails").value;
        var effectiveDate = document.getElementById("effectiveDate").value;
        var declarationDate = document.getElementById("declarationDate").value;

        $scope.licenseApplicationModel.ProposedArrangementLicensingActivity = proposedArrangementDetails;
        $scope.licenseApplicationModel.EffectiveDate = effectiveDate;
        $scope.licenseApplicationModel.DeclarationDate = declarationDate;
        $scope.licenseApplicationModel.MaximumNominatedCapacity = parseFloat($scope.licenseApplicationModel.MaximumNominatedCapacity);
        $scope.submitButtonText = 'Submitting Application...';
        $scope.submittingInformation = 'true';

        console.log($scope.licenseApplicationModel);
        $http({
            method: 'POST',
            url: baseUrl + 'applications/savelicenseapplication',
            data: $scope.licenseApplicationModel,
            dataType: 'json'
        }).then(function (response) {
            if (response.data === true) {
                window.location.href = "/companyoperator";
            }
        }, function (error) {
            console.log(error);
        });
    };

    $scope.addCustomerStakeholder = function (stakeholderName, stakeholderLocation) {
        var customerStakeholderObj = {
            "Customer": stakeholderName, "Location": stakeholderLocation
        };

        $scope.stakeholdersLocations.push(customerStakeholderObj);
        $scope.stakeholderName = '';
        $scope.stakeholderLocation = '';
        console.log($scope.stakeholdersLocations);
    };

    $scope.removeCustomerStakeholder = function (objectToRemove) {
        var objectToRemovePosition = $scope.stakeholdersLocations.indexOf(objectToRemove);
        $scope.stakeholdersLocations.splice(objectToRemovePosition, 1);
    };

    $scope.holdRelatedLicenseFileHandler = function () {
        var fi = document.getElementById('holdRelatedLicense');
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

                            $scope.licenseApplicationUpload.HoldRelatedLicenseFileName = fileNameFormatted;
                            $scope.licenseApplicationUpload.HoldRelatedLicenseFileExtension = extension;
                            $scope.licenseApplicationUpload.HoldRelatedLicenseBase64 = base64String;

                            if (selectedFile.size > maxFileSize) {
                                $scope.licenseApplicationUploadsFileSizeCheckModel.HoldRelatedLicenseFileSizeValid = false;
                                document.getElementById('holdRelatedLicenseWarningIndicator').innerHTML = "File size cannot be more than 2MB!";

                            } else {
                                $scope.licenseApplicationUploadsFileSizeCheckModel.HoldRelatedLicenseFileSizeValid = true;
                                document.getElementById('holdRelatedLicenseWarningIndicator').innerHTML = "";
                            }

                            if ($scope.licenseApplicationUploadsFileSizeCheckModel.HoldRelatedLicenseFileSizeValid === true) {
                                if (extension === 'pdf') {
                                    $scope.licenseApplicationUploadsFileSizeCheckModel.HoldRelatedLicenseFileSizeValid = true;
                                    document.getElementById('holdRelatedLicenseWarningIndicator').innerHTML = "";
                                } else {
                                    $scope.licenseApplicationUploadsFileSizeCheckModel.HoldRelatedLicenseFileSizeValid = false;
                                    document.getElementById('holdRelatedLicenseWarningIndicator').innerHTML = "Only PDF files are allowed!";
                                }
                            }


                            console.log('Hold related license?: ' +$scope.licenseApplicationUploadsFileSizeCheckModel.HoldRelatedLicenseFileSizeValid);
                        };
                    })(selectedFile);
                }
                console.log($scope.licenseApplicationUpload);
            }
        }
    };

    $scope.hasRelatedLicenseFileHandler = function () {
        var fi = document.getElementById('hasRelatedLicense');
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

                            $scope.licenseApplicationUpload.HasRelatedLicenseFileName = fileNameFormatted;
                            $scope.licenseApplicationUpload.HasRelatedLicenseFileExtension = extension;
                            $scope.licenseApplicationUpload.HasRelatedLicenseBase64 = base64String;

                            if (selectedFile.size > maxFileSize) {
                                $scope.licenseApplicationUploadsFileSizeCheckModel.HasRelatedLicenseFileSizeValid = false;
                                document.getElementById('hasRelatedLicenseWarningIndicator').innerHTML = "File size cannot be more than 2MB!";

                            } else {
                                $scope.licenseApplicationUploadsFileSizeCheckModel.HasRelatedLicenseFileSizeValid = true;
                                document.getElementById('hasRelatedLicenseWarningIndicator').innerHTML = "";
                            }

                            if ($scope.licenseApplicationUploadsFileSizeCheckModel.HasRelatedLicenseFileSizeValid === true) {
                                if (extension === 'pdf') {
                                    $scope.licenseApplicationUploadsFileSizeCheckModel.HasRelatedLicenseFileSizeValid = true;
                                    document.getElementById('hasRelatedLicenseWarningIndicator').innerHTML = "";
                                } else {
                                    $scope.licenseApplicationUploadsFileSizeCheckModel.HasRelatedLicenseFileSizeValid = false;
                                    document.getElementById('hasRelatedLicenseWarningIndicator').innerHTML = "Only PDF files are allowed!";
                                }
                            }


                            console.log('Related license valid?: ' +$scope.licenseApplicationUploadsFileSizeCheckModel.HasRelatedLicenseFileSizeValid);
                        };
                    })(selectedFile);
                }
                console.log($scope.licenseApplicationUpload);
            }
        }
    };

    $scope.hasLicenseRevokedFileHandler = function () {
        var fi = document.getElementById('hasLicenseRevoked');
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

                            $scope.licenseApplicationUpload.HasLicenseRevokedFileName = fileNameFormatted;
                            $scope.licenseApplicationUpload.HasLicenseRevokedFileExtension = extension;
                            $scope.licenseApplicationUpload.HasLicenseRevokedLicenseBase64 = base64String;

                            if (selectedFile.size > maxFileSize) {
                                $scope.licenseApplicationUploadsFileSizeCheckModel.HasLicenseRevokedFileSizeValid = false;
                                document.getElementById('hasLicenseRevokedWarningIndicator').innerHTML = "File size cannot be more than 2MB!";

                            } else {
                                $scope.licenseApplicationUploadsFileSizeCheckModel.HasLicenseRevokedFileSizeValid = true;
                                document.getElementById('hasLicenseRevokedWarningIndicator').innerHTML = "";
                            }

                            if ($scope.licenseApplicationUploadsFileSizeCheckModel.HasLicenseRevokedFileSizeValid === true) {
                                if (extension === 'pdf') {
                                    $scope.licenseApplicationUploadsFileSizeCheckModel.HasLicenseRevokedFileSizeValid = true;
                                    document.getElementById('hasLicenseRevokedWarningIndicator').innerHTML = "";
                                } else {
                                    $scope.licenseApplicationUploadsFileSizeCheckModel.HasLicenseRevokedFileSizeValid = false;
                                    document.getElementById('hasLicenseRevokedWarningIndicator').innerHTML = "Only PDF files are allowed!";
                                }
                            }


                            console.log('License revoked valid?: ' +$scope.licenseApplicationUploadsFileSizeCheckModel.HasLicenseRevokedFileSizeValid);
                        };
                    })(selectedFile);
                }
                console.log($scope.licenseApplicationUpload);
            }
        }
    };

    $scope.hasLicenseRefusedFileHandler = function () {
        var fi = document.getElementById('hasRefusedLicense');
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

                            $scope.licenseApplicationUpload.HasLicenseRefusedFileName = fileNameFormatted;
                            $scope.licenseApplicationUpload.HasLicenseRefusedFileExtension = extension;
                            $scope.licenseApplicationUpload.HasLicenseRefusedLicenseBase64 = base64String;

                            if (selectedFile.size > maxFileSize) {
                                $scope.licenseApplicationUploadsFileSizeCheckModel.HasLicenseRefusedFileSizeValid = false;
                                document.getElementById('hasLicenseRefusedWarningIndicator').innerHTML = "File size cannot be more than 2MB!";

                            } else {
                                $scope.licenseApplicationUploadsFileSizeCheckModel.HasLicenseRefusedFileSizeValid = true;
                                document.getElementById('hasLicenseRefusedWarningIndicator').innerHTML = "";
                            }

                            if ($scope.licenseApplicationUploadsFileSizeCheckModel.HasLicenseRefusedFileSizeValid === true) {
                                if (extension === 'pdf') {
                                    $scope.licenseApplicationUploadsFileSizeCheckModel.HasLicenseRefusedFileSizeValid = true;
                                    document.getElementById('hasLicenseRefusedWarningIndicator').innerHTML = "";
                                } else {
                                    $scope.licenseApplicationUploadsFileSizeCheckModel.HasLicenseRefusedFileSizeValid = false;
                                    document.getElementById('hasLicenseRefusedWarningIndicator').innerHTML = "Only PDF files are allowed!";
                                }
                            }

                            console.log('Refused License valid?: ' + $scope.licenseApplicationUploadsFileSizeCheckModel.HasLicenseRefusedFileSizeValid);
                        };
                    })(selectedFile);
                }
                console.log($scope.licenseApplicationUpload);
            }
        }
    };

    $scope.declarationSignatureFileHandler = function () {
        var fi = document.getElementById('declarationSignature');
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

                            $scope.licenseApplicationUpload.DeclarationSignatureFileName = fileNameFormatted;
                            $scope.licenseApplicationUpload.DeclarationSignatureFileExtension = extension;
                            $scope.licenseApplicationUpload.DeclarationSignatureBase64 = base64String;

                            if (selectedFile.size > maxFileSize) {
                                $scope.licenseApplicationUploadsFileSizeCheckModel.DeclarationSignatureValid = false;
                                document.getElementById('declarationSignatureWarningIndicator').innerHTML = "File size cannot be more than 2MB!";

                            } else {
                                $scope.licenseApplicationUploadsFileSizeCheckModel.DeclarationSignatureValid = true;
                                document.getElementById('declarationSignatureWarningIndicator').innerHTML = "";
                            }

                            if ($scope.licenseApplicationUploadsFileSizeCheckModel.DeclarationSignatureValid === true) {
                                if (extension === 'pdf') {
                                    $scope.licenseApplicationUploadsFileSizeCheckModel.DeclarationSignatureValid = true;
                                    document.getElementById('declarationSignatureWarningIndicator').innerHTML = "";
                                } else {
                                    $scope.licenseApplicationUploadsFileSizeCheckModel.DeclarationSignatureValid = false;
                                    document.getElementById('declarationSignatureWarningIndicator').innerHTML = "Only PDF files are allowed!";
                                }
                            }

                            console.log('Declaration signature valid?: '+ $scope.licenseApplicationUploadsFileSizeCheckModel.DeclarationSignatureValid);
                        };
                    })(selectedFile);
                }
                console.log($scope.licenseApplicationUpload);
            }
        }
    };

    $scope.makePayment = function () {
        var paymentEngine = RmPaymentEngine.init({
            key: 'b2x1ZmVtaW95ZWRlcG9AZ21haWwuY29tfDQyNjIwMjMzfDIxZDM2YjdkYWJlYjZmNjJmMDRiZTY0OTU0NmJiYWMxNTg0MjQyZWM4ZmQ2NWEzMmY1NjgyNWJmYzQyZDRiMmNlNmYyNTI0YWM5NjgwZGEwZGJkNGI4Zjg1MmFiY2YwZThiZWE2YTE4ZjE2NzUyOTU0NjliYzM4YjMyM2Y5YzQ2',
            customerId: "4216551965",
            firstName: "Yetunde",
            lastName: "Salau",
            email: "ysalau@wragbysolutions.com",
            narration: "Remitta DPR Demo",
            amount: 15000,
            onSuccess: function (response) {
                console.log('callback Successful Response', response);
            },
            onError: function (response) {
                console.log('callback Error Response', response);
            },
            onClose: function () {
                console.log("closed");
            }
        });
        paymentEngine.showPaymentWidget();
    };

    

    $scope.setCustomerRecId();
    $scope.getLicenseFees();

});