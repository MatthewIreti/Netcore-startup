appModule.controller('ApplicationCreate', function ($scope, $http, $state) {
    $scope.breadcrumb.splice(0, $scope.breadcrumb.length);
    $scope.title = "New Application";
    $scope.stakeholdersLocations = [{}];
    $scope.takeOffPoints = [{}];
    $scope.gasShipperCustomers = [{}];
    $scope.licenseFees = [];
    $scope.submittingInformation === 'false';
    $scope.submitButtonText = 'Submit Application';
    $scope.breadcrumb.push(
        {
            title: 'License Applications',
            link: 'site.application.list'
        },
        {
            title: $scope.title,
        }
    );
    var maxFileSize = 2500000; // 2 Megabytes
    $scope.licenseApplicationUpload = {
        HoldRelatedLicenseFileName: '', HoldRelatedLicenseFileExtension: '', HoldRelatedLicenseBase64: '',
        HasRelatedLicenseFileName: '', HasRelatedLicenseFileExtension: '', HasRelatedLicenseBase64: '',
        HasLicenseRevokedFileName: '', HasLicenseRevokedFileExtension: '', HasLicenseRevokedLicenseBase64: '',
        HasLicenseRefusedFileName: '', HasLicenseRefusedFileExtension: '', HasLicenseRefusedLicenseBase64: '',
        ProposedArrangementAttachmentFileName: '', ProposedArrangementAttachmentFileExtension: '', ProposedArrangementAttachmentBase64: '',
        DeclarationSignatureFileName: '', DeclarationSignatureFileExtension: '', DeclarationSignatureBase64: '',
        OPLFileName: '', OPLFileExtension: '', OPLBase64: '',
        SafetyCaseFileName: '', SafetyCaseFileExtension: '', SafetyCaseBase64: '',
        SCADAFileName: '', SCADAFileExtension: '', SCADABase64: '',
        GTSFileName: '', GTSFileExtension: '', GTSBase64: '',
        TechnicalAttributeFileName: '', TechnicalAttributeFileExtension: '', TechnicalAttributeBase64: '',
        AuxiliarySystemFileName: '', AuxiliarySystemFileExtension: '', AuxiliarySystemBase64: '',
        TariffAndPricingFileName: '', TariffAndPricingFileExtension: '', TariffAndPricingBase64: '',
        RiskManagmentFileName: '', RiskManagmentFileExtension: '', RiskManagmentBase64: '',
        CommunityMOUFileName: '', CommunityMOUFileExtension: '', CommunityMOUBase64: '',
        NetworkAgentOPLFileName: '', NetworkAgentOPLFileExtension: '', NetworkAgentOPLBase64: '',
        GasShipperOPLFileName: '', GasShipperOPLFileExtension: '', GasShipperOPLBase64: ''
    };
    $scope.licenseApplicationModel = {
        CompanyName: '', Customer: 0, CustomerTier: '', SubmittedBy: 0, CustApplicationNum: '', CustLicenseType: '', EffectiveDate: '', HoldRelatedLicense: '', RelatedLicenseDetail: '',
        HasRelatedLicense: '', RelatedLicenseType: '', HasLicenseRevoked: '', RevokedLicenseType: '', HasGasApplicationRefused: '',
        RefusedLicenseType: '', AgentShipperName: '', AgentLocationOfShipper: '', EntryPoint: '', ExitPoint: '', Location: '',
        MaximumNominatedCapacity: 0.0, PipelineAndGasTransporterName: '', GasPipelineNetwork: '', InstalledCapacity: '', DeclarationName: '', DeclarationCapacity: '',
        DeclarationDate: '', ProposedArrangementLicensingActivity: '', HasStandardModificationRequest: '', ModificationRequestDetails: '',
        ModificationRequestReason: '', CustLicenseApplicationStatus: '',
        FileUploads: $scope.licenseApplicationUpload, StakeholderLocations: $scope.stakeholdersLocations, TakeOffPoints: $scope.takeOffPoints, GasShipperCustomers: $scope.gasShipperCustomers
    };
    $scope.licenseApplicationModel.EffectiveDate = new Date();
    $scope.licensetypes = [
        { "Name": "Network Agent", "Value": "NetworkAgent", "Selected": true },
        { "Name": "Gas Shipper License", "Value": "GasShipperLicense", "Selected": false },
        { "Name": "Gas Transporter License", "Value": "GasTransporterLicense", "Selected": false }
    ];

    $scope.GasShipperPointType = [
        { "Name": "Delivery", "Value": "Delivery" },
        { "Name": "Take Off", "Value": "TakeOff" }
    ];

    $scope.GasShipperCustCategory = [
        { "Name": "Buyer", "Value": "Buyer" },
        { "Name": "Seller", "Value": "Seller" }

    ];
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
    $scope.addCustomerStakeholder = function () {
        $scope.stakeholdersLocations.push({});
    };
    $scope.removeCustomerStakeholder = function (objectToRemove) {
        var objectToRemovePosition = $scope.stakeholdersLocations.indexOf(objectToRemove);
        $scope.stakeholdersLocations.splice(objectToRemovePosition, 1);
    };

    $scope.addGasShipperCustomer = function () {
        $scope.gasShipperCustomers.push({});
    };

    $scope.removeGasShipperCustomer = function (objectToRemove) {
        var objectToRemovePosition = $scope.gasShipperCustomers.indexOf(objectToRemove);
        $scope.gasShipperCustomers.splice(objectToRemovePosition, 1);
    };

    $scope.addTakeOffPoint = function () {
        $scope.takeOffPoints.push({});
    };

    $scope.removeTakeOffPoint = function (objectToRemove) {
        var objectToRemovePosition = $scope.takeOffPoints.indexOf(objectToRemove);
        $scope.takeOffPoints.splice(objectToRemovePosition, 1);
    };
    $scope.submitButtonText = 'Submitting Application...';
    $scope.submittingInformation = 'true';
    $scope.setFileRecord = function ($event, tag) {
        var file = $event.target.files[0];
        var filename = file.name;
        var extension = extractExtensionFromFileName(filename);
        var reader = new FileReader();

        reader.readAsBinaryString(file);
        reader.onload = (function () {
            return function (e) {
                var base64Str = window.btoa(e.target.result);
                if (tag === "proposedDetail") {
                    $scope.licenseApplicationUpload.ProposedArrangementAttachmentFileName = filename;
                    $scope.licenseApplicationUpload.ProposedArrangementAttachmentFileExtension = extension;
                    $scope.licenseApplicationUpload.ProposedArrangementAttachmentBase64 = base64Str;
                }
                else if (tag === "OPLLicense") {
                    $scope.licenseApplicationUpload.OPLFileName = filename;
                    $scope.licenseApplicationUpload.OPLFileExtension = extension;
                    $scope.licenseApplicationUpload.OPLBase64 = base64Str;
                }
                else if (tag === "SafetyCase") {
                    $scope.licenseApplicationUpload.SafetyCaseFileName = filename;
                    $scope.licenseApplicationUpload.SafetyCaseFileExtension = extension;
                    $scope.licenseApplicationUpload.SafetyCaseBase64 = base64Str;
                }
                else if (tag === "SCADA") {
                    $scope.licenseApplicationUpload.SCADAFileName = filename;
                    $scope.licenseApplicationUpload.SCADAFileExtension = extension;
                    $scope.licenseApplicationUpload.SCADABase64 = base64Str;
                }
                else if (tag === "GTS") {
                    $scope.licenseApplicationUpload.GTSFileName = filename;
                    $scope.licenseApplicationUpload.GTSFileExtension = extension;
                    $scope.licenseApplicationUpload.GTSBase64 = base64Str;
                }
                else if (tag === "technicalAttr") {
                    $scope.licenseApplicationUpload.TechnicalAttributeFileName = filename;
                    $scope.licenseApplicationUpload.TechnicalAttributeFileExtension = extension;
                    $scope.licenseApplicationUpload.TechnicalAttributeBase64 = base64Str;
                }
                else if (tag === "auxSystems") {
                    $scope.licenseApplicationUpload.AuxiliarySystemFileName = filename;
                    $scope.licenseApplicationUpload.AuxiliarySystemFileExtension = extension;
                    $scope.licenseApplicationUpload.AuxiliarySystemBase64 = base64Str;
                }
                else if (tag === "tariff") {
                    $scope.licenseApplicationUpload.TariffAndPricingFileName = filename;
                    $scope.licenseApplicationUpload.TariffAndPricingFileExtension = extension;
                    $scope.licenseApplicationUpload.TariffAndPricingBase64 = base64Str;
                }
                else if (tag === "riskManagement") {
                    $scope.licenseApplicationUpload.RiskManagmentFileName = filename;
                    $scope.licenseApplicationUpload.RiskManagmentFileExtension = extension;
                    $scope.licenseApplicationUpload.RiskManagmentBase64 = base64Str;
                }
                else if (tag === "MOU") {
                    $scope.licenseApplicationUpload.CommunityMOUFileName = filename;
                    $scope.licenseApplicationUpload.CommunityMOUFileExtension = extension;
                    $scope.licenseApplicationUpload.CommunityMOUBase64 = base64Str;
                }
                else if (tag === "NetworkAgentOPL") {
                    $scope.licenseApplicationUpload.NetworkAgentOPLFileName = filename;
                    $scope.licenseApplicationUpload.NetworkAgentOPLFileExtension = extension;
                    $scope.licenseApplicationUpload.NetworkAgentOPLBase64 = base64Str;
                }
                else if (tag === "GasShipperOPL") {
                    $scope.licenseApplicationUpload.GasShipperOPLFileName = filename;
                    $scope.licenseApplicationUpload.GasShipperOPLFileExtension = extension;
                    $scope.licenseApplicationUpload.GasShipperOPLBase64 = base64Str;
                }
                else if (tag === "Signature") {
                    $scope.licenseApplicationUpload.DeclarationSignatureFileName = filename;
                    $scope.licenseApplicationUpload.DeclarationSignatureFileExtension = extension;
                    $scope.licenseApplicationUpload.DeclarationSignatureBase64 = base64Str;
                }
                else if (tag == "holdRelatedLicense") {
                    $scope.licenseApplicationUpload.HoldRelatedLicenseFileName = filename;
                    $scope.licenseApplicationUpload.HoldRelatedLicenseFileExtension = extension;
                    $scope.licenseApplicationUpload.HoldRelatedLicenseBase64 = base64Str;
                }
                else if ("hasRelatedLicense") {
                    $scope.licenseApplicationUpload.HasRelatedLicenseFileName = filename;
                    $scope.licenseApplicationUpload.HasRelatedLicenseFileExtension = extension;
                    $scope.licenseApplicationUpload.HasRelatedLicenseBase64 = base64Str;
                }
                else if ("hasLicenseRevoked") {
                    $scope.licenseApplicationUpload.HasLicenseRevokedFileName = filename;
                    $scope.licenseApplicationUpload.HasLicenseRevokedFileExtension = extension;
                    $scope.licenseApplicationUpload.HasLicenseRevokedLicenseBase64 = base64Str;
                }
                else if ("hasRefusedLicense") {
                    $scope.licenseApplicationUpload.HasLicenseRefusedFileName = filename;
                    $scope.licenseApplicationUpload.HasLicenseRefusedFileExtension = extension;
                    $scope.licenseApplicationUpload.HasLicenseRefusedLicenseBase64 = base64Str;
                }
            };
        })(file);
    };

    $scope.licenseApplicationUploadsFileSizeCheckModel = {
        HoldRelatedLicenseFileSizeValid: '', HasRelatedLicenseFileSizeValid: '', HasLicenseRevokedFileSizeValid: '', HasLicenseRefusedFileSizeValid: '', DeclarationSignatureValid: ''
    };

    $scope.holdRelatedLicenseSelectionHandler = function () {
        if ($scope.licenseApplicationModel.HoldRelatedLicense !== 'true') {
            $scope.licenseApplicationModel.RelatedLicenseDetail = '';
            $scope.licenseApplicationUpload.HoldRelatedLicenseFileName = '';
            $scope.licenseApplicationUpload.HoldRelatedLicenseFileExtension = '';
            $scope.licenseApplicationUpload.HoldRelatedLicenseBase64 = '';
        }
    };

    $scope.hasRelatedLicenseSelectionHandler = function () {
        if ($scope.licenseApplicationModel.HasRelatedLicense !== 'true') {
            $scope.licenseApplicationModel.RelatedLicenseType = '';
            $scope.licenseApplicationUpload.HasRelatedLicenseFileName = '';
            $scope.licenseApplicationUpload.HasRelatedLicenseFileExtension = '';
            $scope.licenseApplicationUpload.HasRelatedLicenseBase64 = '';
        }
    };

    $scope.hasLicenseRevokedHandler = function () {
        if ($scope.licenseApplicationModel.HasLicenseRevoked !== 'true') {
            $scope.licenseApplicationModel.RevokedLicenseType = '';
            $scope.licenseApplicationUpload.HasLicenseRevokedFileName = '';
            $scope.licenseApplicationUpload.HasLicenseRevokedFileExtension = '';
            $scope.licenseApplicationUpload.HasLicenseRevokedLicenseBase64 = '';
        }
    };

    $scope.hasLicenseRefusedHandler = function () {
        if ($scope.licenseApplicationModel.HasGasApplicationRefused !== 'true') {
            $scope.licenseApplicationModel.RefusedLicenseType = '';
            $scope.licenseApplicationUpload.HasLicenseRefusedFileName = '';
            $scope.licenseApplicationUpload.HasLicenseRefusedFileExtension = '';
            $scope.licenseApplicationUpload.HasLicenseRefusedLicenseBase64 = '';
            document.getElementById("hasRefusedLicense").value = "";
        }
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
            }
        }
    };
   
    $scope.getLicenseFees = function () {
        $http({
            method: 'GET',
            url: baseUrl + 'applications/licensefees/'
        }).then(function (response) {
            $scope.licenseFees = response.data;
        }, function (error) {
            console.log(error);
        });
    };
    $scope.getLicenseFees();
    $scope.setCustomerRecId = function () {
        var loggedInUser = localStorage.getItem('loggedInUser');
        $scope.loggedInUser = JSON.parse(loggedInUser);

        $scope.licenseApplicationModel.Customer = $scope.loggedInUser.custTableRecId;
        $scope.licenseApplicationModel.CompanyName = $scope.loggedInUser.companyName;

        if ($scope.loggedInUser.recordId) {
            $scope.licenseApplicationModel.SubmittedBy = $scope.loggedInUser.recordId;
        }
    };
    $scope.setCustomerRecId();
    //set errors from the validation directive;
    $scope.setError = function (errors) {
        $scope.errors = [];
        $scope.errors = errors;
    };

    $scope.submitForm = function () {
        $scope.errors = [];
        var proposedArrangementDetails = document.getElementById("proposedArrangementDetails").value;
        var effectiveDate = document.getElementById("effectiveDate").value;
        var declarationDate = document.getElementById("declarationDate").value;

        $scope.licenseApplicationModel.ProposedArrangementLicensingActivity = proposedArrangementDetails;
        $scope.licenseApplicationModel.EffectiveDate = effectiveDate;
        $scope.licenseApplicationModel.DeclarationDate = declarationDate;
        $scope.licenseApplicationModel.MaximumNominatedCapacity = parseFloat($scope.licenseApplicationModel.MaximumNominatedCapacity);
        $scope.submittingInformation = 'true';
        console.log(JSON.stringify($scope.licenseApplicationModel));
        $scope.waiting++;
        $http({
            method: 'POST',
            url: baseUrl + 'applications/savelicenseapplication',
            data: $scope.licenseApplicationModel,
            dataType: 'json'
        }).then(function (response) {
            if (response.data === true) {
                $state.go('site.application.list');
                $scope.waiting--;
            }
        }, function (error) {
            $scope.waiting--;
            console.log(error);
        });
    };
});


appModule.controller('ApplicationList', function ($scope, $http, $state) {
    $scope.title = "Applications";
    $scope.breadcrumb.splice(0, $scope.breadcrumb.length);
    $scope.breadcrumb.push(
        {
            title: $scope.title
        }
    );
    if ($scope.loggedInUser.custTableRecId) {
        $scope.waiting++;
        getCustomerApplications($scope.loggedInUser.custTableRecId);
        $scope.waiting--;
    }


    function getCustomerApplications(custrecid) {
        $http({
            method: 'GET',
            url: baseUrl + 'applications/customer/' + custrecid
        }).then(function (response) {
            $scope.applications = response.data;
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
    }
});


appModule.controller('ApplicationGetDetails', function ($scope, $http, $state) {
    $scope.title = "Details";
    $scope.breadcrumb.splice(0, $scope.breadcrumb.length);
    $scope.breadcrumb.push(
        {
            title: 'License Applications',
            link: 'site.application.list'
        },
        {
            title: $scope.title,
            link: '#'
        }
    );
    $scope.waiting++;
    $http({
        method: 'GET',
        url: baseUrl + 'applications/licenseapplicationdetails/' + $state.params.recordId
    }).then(function (response) {
        $scope.item = response.data;
        $scope.waiting--;
    }, function (error) {
        $scope.waiting--;
    });
});

appModule.controller('ApplicationInvoice', function ($scope, $http, $state) {

    $scope.waiting++;
    $http({
        method: 'GET',
        url: baseUrl + 'applications/licenseapplicationdetails/' + $state.params.recordId

    }).then(function (response) {
        $scope.item = response.data;

        $http({
            method: 'GET',
            url: baseUrl + 'applications/licensefees/'
        }).then(function (response) {
            $scope.waiting--;
            $scope.licenseFees = response.data;
            initPaymentData($scope.item)
        }, function (error) {
            console.log(error);
        });
    }, function (error) {
        $scope.waiting--;
    });



    var initPaymentData = function (application) {

        if ($scope.licenseFees.length > 0) {
            for (var i = 0; i < $scope.licenseFees.length - 1; i++) {
                var currentLicenseFee = $scope.licenseFees[i];

                if (currentLicenseFee.licenseType === application.custLicenseType && application.maximumNominatedCapacity >= currentLicenseFee.minimum && application.maximumNominatedCapacity <= currentLicenseFee.maximum) {
                    $scope.statutoryFee = currentLicenseFee.statutory;
                    $scope.serviceTypeId = currentLicenseFee.StatutoryFeeServiceTypeId;
                    $scope.processingFee = currentLicenseFee.processingFee;
                }
            }
        }

        var req = {
            method: 'POST',
            url: baseUrl + 'Payment/getRRRPayload',
            data: {
                serviceTypeId: "40816498",
                amount: $scope.processingFee,
                orderId: application.applicationNum,
                payerName: application.declarationName,
                payerEmail: "dflont@gmail.com",
                description: 'Payment for A' + application.custLicenseType
            }
        }
        console.log(req);
        $http(req).then(function (response) {
            $scope.remita = response.data;
            $scope.remita.responseUrl = "";
            $scope.remita.action = "http://remitademo.net/remita/ecomm/finalize.reg";
            $scope.remita.rrr = "330007846039";
            console.log($scope.remita);
            var rrrReq = {
                method: 'POST',
                headers: {
                    'Authorization': 'remitaConsumerKey=' + response.data.merchantId + ',remitaConsumerToken=' + response.data.hash
                },
                url: response.data.url,
                data: req.data
            }
            //$http(rrrReq).then(function (response) {
            //    console.log(response);
            //      var rrrResponse = response.replace("jsonp (", "").replace(")", "");
            //     var jsonResponse = JSON.parse(rrrResponse);
            //     console.log(jsonResponse);
            //    console.log(rrrResponse);
            //}, function (error) {
            //    console.log(error);
            //});

        }, function (error) {
            console.log(error);
        });
    }
});