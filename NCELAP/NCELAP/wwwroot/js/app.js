﻿var appModule = angular.module('ncLasPortalApp', [/*'angular-jwt'*/]);
var userAccessModule = angular.module('ncLasPortalAppRegistrationPageModule', []);


appModule.directive('ngUploadChange', function () {
    return {
        scope: {
            ngUploadChange: "=",
            tag: "="
        },
        link: function ($scope, $element, $attrs) {
            $element.on("change", function (event) {
                var maxFileSize = 2500000;
                var file = event.target.files[0];
                $scope.ngUploadChange(event, $attrs.tag);
            })
            $scope.$on("$destroy", function () {
                $element.off();
            });
        }
    }
});

// add floating-number-only as an attribute e.g <input type="text" floating-number-only />
appModule.directive("floatingNumberOnly", function () {
    return {
        require: 'ngModel',
        link: function (scope, ele, attr, ctrl) {

            ctrl.$parsers.push(function (inputValue) {
                console.log(inputValue);
                var pattern = new RegExp("(^[0-9]{1,9})+(\.[0-9]{1,8})?$", "g");
                if (inputValue === '')
                    return '';
                var dotPattern = /^[.]*$/;

                if (dotPattern.test(inputValue)) {
                    console.log("inside dot Pattern");
                    ctrl.$setViewValue('');
                    ctrl.$render();
                    return '';
                }

                var newInput = inputValue.replace(/[^0-9.]/g, '');
                // newInput=inputValue.replace(/.+/g,'.');

                if (newInput !== inputValue) {
                    ctrl.$setViewValue(newInput);
                    ctrl.$render();
                }
                //******************************************
                //***************Note***********************
                /*** If a same function call made twice,****
                 *** erroneous result is to be expected ****/
                //******************************************
                //******************************************

                var result;
                var dotCount;
                var newInputLength = newInput.length;
                if (result === pattern.test(newInput)) {
                    console.log("pattern " + result);
                    dotCount = newInput.split(".").length - 1; // count of dots present
                    if (dotCount === 0 && newInputLength > 9) { //condition to restrict "integer part" to 9 digit count
                        newInput = newInput.slice(0, newInputLength - 1);
                        ctrl.$setViewValue(newInput);
                        ctrl.$render();
                    }
                } else { //pattern failed
                    console.log("pattern " + result);
                    // console.log(newInput.length);

                    dotCount = newInput.split(".").length - 1; // count of dots present
                    console.log("dotCount  :  " + dotCount);
                    if (newInputLength > 0 && dotCount > 1) { //condition to accept min of 1 dot
                        console.log("length>0");
                        newInput = newInput.slice(0, newInputLength - 1);
                        console.log("newInput  : " + newInput);

                    }
                    if ((newInput.slice(newInput.indexOf(".") + 1).length) > 8) { //condition to restrict "fraction part" to 8 digit count only.
                        newInput = newInput.slice(0, newInputLength - 1);
                        console.log("newInput  : " + newInput);

                    }
                    ctrl.$setViewValue(newInput);
                    ctrl.$render();
                }

                return newInput;
            });
        }
    };
});