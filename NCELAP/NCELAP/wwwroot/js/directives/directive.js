appModule.directive('customWizard', function () {
    return {
        link: function (scope, element) {
            var progressBar = $(element).find(".progress-bar-step");
            var submitBtn = $("#submitApplication");
            var form = $(element).find('form');
            var steps = {
                stepActive: $(element).find(".step-wizard.active").index(),
                stepsTotal: $(element).find(".step-wizard-container").children().length,
                calcProgressWidth: function () {
                    this.stepActive = $(element).find(".step-wizard.active").index();
                    var containerPadding = (($(element).find("ul.step-wizard-container").css("padding-left").split("px").shift() * 200) / $("ul.step-wizard-container").width()) - 1.1;
                    var stepFinal = (this.stepsTotal == this.stepActive + 1) ? 10 : 5;
                    var stepElemnet = (($("li.step-wizard").width() * 100) / $("ul.step-wizard-container").outerWidth());
                    return (((100 - 10 - (stepElemnet * this.stepsTotal)) / (this.stepsTotal - 1)) * this.stepActive) + ((this.stepActive + 1) * (stepElemnet)) + stepFinal;
                },
                getStepActive: function () {
                    this.stepActive = $(".step-wizard.active").index();
                    return this.stepActive;
                }
            };
            form.on('keyup keypress', function (e) {
                var code = e.keyCode || e.which;
                if (code === 13) {
                    e.preventDefault();
                    return false;
                }
            });

            var validator = form.validate($.extend({
                errorElement: 'span',
                errorClass: 'help-block',
                focusInvalid: false,
                invalidHandler: function (event, validator) {
                    var msg = validator.errorList.map(function (item) {
                        return item.message;
                    });
                    scope.$apply(function () {
                        scope.setError(msg);
                    });
                },
                highlight: function (e) {
                    if ($(e).closest('.form-group').length)
                        $(e).closest('.form-group').addClass('has-error'); // set error class to the control group
                    else {
                        $(e).closest('div').addClass('has-error');
                    }
                },
                success: function (label) {
                    if (label.closest('.form-group').length)
                        label.closest('.form-group').removeClass('has-error');
                    else {
                        label.closest('div').removeClass('has-error');
                    }
                    label.remove();
                },
                errorPlacement: function (error, e) {
                    error.addClass('invalid-feedback');
                    if ($(e).parent('label').length) {
                        error.insertAfter($(e).parent('label'));
                    }
                    else {
                        error.insertAfter(e);
                    }
                },
                submitHandler: function (form) {
                    scope.$apply(function () {
                        scope.submitForm();
                    });
                }
            }, scope.validationRule));
            progressBar.width(steps.calcProgressWidth() + "%");
            submitBtn.hide();
            $("#previous").click(function () {
                if (steps.getStepActive() == 0) return false;
                headerActive = $(".step-wizard.active");
                contentActive = $(".step-wizard-content.active");

                headerActive.removeClass("active");
                headerActive.prev().addClass("active");
                contentActive.removeClass("active");
                contentActive.prev().addClass("active");
                $(".progress-bar-step").width(steps.calcProgressWidth() + "%");

                if (steps.getStepActive() == 0)
                    $(this).addClass("disabled");
                $(this).siblings().removeClass("disabled");
                $("#next").show();
                $("#submitApplication").hide();
            });
            $("#next").click(function () {
                $("#submitApplication").hide();
                if (steps.stepsTotal == steps.getStepActive() + 1) return false;
                headerActive = $(".step-wizard.active");
                contentActive = $(".step-wizard-content.active");
                headerActive.removeClass("active");
                headerActive.next().addClass("active");
                contentActive.removeClass("active");
                contentActive.next().addClass("active");
                $(".progress-bar-step").width(steps.calcProgressWidth() + "%");
                if (steps.stepsTotal == steps.getStepActive() + 1)
                    //$(this).addClass("disabled");
                    $(this).hide();
                $("#submitApplication").show();
                $(this).siblings().removeClass("disabled");

            });
        }
    }

});

appModule.directive('waiting', function () {
    return {
        restrict: 'A',
        link: function (scope, element, attr) {
            scope.$watch('waiting', function (newValue) {
                if (newValue >= 1) {
                    $(element).waitMe({
                        effect: 'roundBounce',
                        text: 'Processing request...',
                        bg: 'rgba(255,255,255,0.7)',
                        color: '#198a4b'
                    });
                }
                else {
                    $(element).waitMe('hide');
                }
            });
        }
    }

});