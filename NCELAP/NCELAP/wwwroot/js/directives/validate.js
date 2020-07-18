appModule.directive('validation', function () {
    return{
        restrict: 'A',
        link: function (scope, element, attrs) {
            $(element).validate($.extend({
                errorElement: 'span',
                errorClass: 'help-block',
                focusInvalid: false,
                invalidHandler: function (event, validator) {
                    $('.alert-danger', $(element)).show();
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
            $(element).find('input').keypress(function (e) {
                if (e.which === 13) {
                    if ($(element).validate().form()) {
                        $(element).submit(); //form validation success, call ajax form submit
                    }
                    return false;
                }
            });
        }
    }
});