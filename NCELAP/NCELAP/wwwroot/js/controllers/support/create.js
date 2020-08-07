appModule.controller('TicketCreate', function ($scope, $http) {
    $scope.supportPayload = {};
    var maxFileSize = 5000000; // 5 Megabytes

    $scope.caseseverities = [
        { Name: "Very High", Value: 1 },
        { Name: "High", Value: 2 },
        { Name: "Medium", Value: 3 },
        { Name: "Low", Value: 4 },
        { Name: "Very Low", Value: 5 }
    ];
    $scope.ticketModel = {
        Department: '', Priority: '', Subject: '', Attachment: '', Description: '', Message: '',
        CompanyName: '', EmployeeName: '', EmployeeEmail: '', ContactEmail: '', CompanyRecId: 0
    };

    $scope.setRecIdParams = function () {
        var loggedInUser = localStorage.getItem('loggedInUser');
        $scope.loggedInUser = JSON.parse(loggedInUser);

        if ($scope.loggedInUser) {
            $scope.ticketModel.CompanyName = $scope.loggedInUser.companyName;
            $scope.ticketModel.EmployeeName = $scope.loggedInUser.name;
            $scope.ticketModel.EmployeeEmail = $scope.loggedInUser.email;
            $scope.ticketModel.ContactEmail = $scope.loggedInUser.email;
            $scope.ticketModel.CompanyRecId = $scope.loggedInUser.custTableRecId;
            $scope.ticketModel.EmployeeRecId = $scope.loggedInUser.recordId;
        }
    };

    $scope.setMessageBody = function () {
        var messageBody = document.getElementById('message').value;
        $scope.ticketModel.Message = messageBody;
    };

    $scope.submitTicket = function () {
        var messageBody = document.getElementById('message').value;
        $scope.ticketModel.Message = messageBody;
        $scope.ticketModel.Priority = parseInt($scope.ticketModel.Priority);

        if (messageBody === '') {
            swal({
                title: "Warning",
                text: "Message body is still empty!",
                type: "wanring",
                confirmButtonText: "Ok"
            }).then((result) => {
                
            });
        } if ($scope.attachmentSizeValid === false) {
            swal({
                title: "Warning",
                text: "Attachment size is too large!",
                type: "wanring",
                confirmButtonText: "Ok"
            }).then((result) => {

            });
        } else {
            $scope.waiting++;
            console.log($scope.ticketModel);

            setTimeout(function () {
                $http({
                    method: 'POST',
                    url: baseUrl + 'supportticket/postsupporttickets',
                    data: $scope.ticketModel,
                    dataType: 'json'
                }).then(function (response) {
                    $scope.ticketCreateResponse = response.data;

                    if ($scope.ticketCreateResponse.success === true) {
                        $scope.waiting--;
                        swal({
                            title: "Success",
                            text: "Support ticket raised and sent successfully, you will be contacted shortly...",
                            type: "success",
                            confirmButtonText: "Ok"
                        }).then((result) => {
                            window.location.href = '/companyoperator#!/tickets/';
                        });
                    }

                }, function (error) {
                    console.log(error);
                    $scope.waiting--;
                });
            }, 2000);
        }
    };

    $scope.attachmentFileHandler = function () {
        var fi = document.getElementById('attachment');
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

                            $scope.ticketModel.Attachment = base64String;

                            if (selectedFile.size > maxFileSize) {
                                $scope.attachmentSizeValid = false;
                                document.getElementById('attachmentUploadSizeIndicator').innerHTML = "File size cannot be more than 5MB!";

                            } else {
                                $scope.attachmentSizeValid = true;
                                document.getElementById('attachmentUploadSizeIndicator').innerHTML = "";
                            }

                            //if ($scope.custProspectUploadsFileSizeCheckModel.CertificationOfRegistrationFileSizeValid === true) {
                            //    if (extension === 'pdf') {
                            //        $scope.custProspectUploadsFileSizeCheckModel.CertificationOfRegistrationFileExtensionValid = true;
                            //        document.getElementById('certificateOfRegistrationWarningIndicator').innerHTML = "";
                            //    } else {
                            //        $scope.custProspectUploadsFileSizeCheckModel.CertificationOfRegistrationFileExtensionValid = false;
                            //        document.getElementById('certificateOfRegistrationWarningIndicator').innerHTML = "Only PDF files are allowed!";
                            //    }
                            //}
                        };
                    })(selectedFile);
                }
                console.log($scope.ticketModel);
            }
        }
    };

    $scope.setRecIdParams();
});