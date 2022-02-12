// Đối tượng Validator
function Validator(options){ //? options là tham số object của hàm Validator

    function getParent(element,selector){
        while (element.parentElement){
            if (element.parentElement.matches(selector)){
                return element.parentElement;
            }
            element = element.parentElement;
        }
    }

    var selectorRules = {};

    function validate(inputElement,rule){
        var errorElement = getParent(inputElement, options.formGroupSelector).querySelector(options.errorSelector); 
        var errorMessage;

        // Lấy ra các rules của selector
        var rules = selectorRules[rule.selector];

        // Lặp qua từng rule và kiểm tra, nếu có lỗi thì dừng việc kiểm tra
        for(var i = 0; i < rules.length; ++i){
            switch (inputElement.type) {
                case 'radio':
                case 'checkbox':
                    errorMessage = rules[i](
                        formElement.querySelector(rule.selector + ':checked')
                    );
                    break;
                default:
                    errorMessage = rules[i](inputElement.value);
                    break;
            }
            if(errorMessage) break;
        }

        if(errorMessage){
            errorElement.innerText = errorMessage;
            getParent(inputElement, options.formGroupSelector).classList.add('invalid');
        }else{
            errorElement.innerText = '';
            getParent(inputElement, options.formGroupSelector).classList.remove('invalid');
        }
        return !errorMessage;
    }

    // Lấy element của form cần validate
    var formElement = document.querySelector(options.form);
    if(formElement){

        // Khi submit form
        formElement.onsubmit = function(e){
            // Hủy hành vi mặc định(submit) của form
            e.preventDefault();

            var isFormValid = true;

            // Lặp qua từng rules và validate
            options.rules.forEach(function(rule){
                var inputElement = formElement.querySelector(rule.selector);
                var isValid = validate(inputElement,rule);
                if(!isValid){
                    isFormValid = false;
                }
            });

            if (isFormValid){
                // Trường hợp submit với javascript
                if (typeof options.onSubmit === 'function'){
                    var enableInputs = formElement.querySelectorAll('[name]'); 
                    var formValues = Array.from(enableInputs).reduce(function (values,input){
                        
                        switch (input.type) {
                            case 'radio':
                                values[input.name] = formElement.querySelector('input[name="'+ input.name +'"]:checked').value;
                                break;
                            case 'checkbox':
                                if(!input.matches(':checked')) {
                                    values[input.name] = '';
                                    return values;
                                }
                                if(!Array.isArray(values[input.name])){
                                    values[input.name] = [];
                                } 
                                values[input.name].push(input.value);
                                break;
                            case 'file':
                                    values[input.name] = input.files;
                                    break;    
                            default:
                                values[input.name] = input.value;
                                break;
                        }

                        return values;
                    },{});
                    options.onSubmit(formValues);
                }
                // Trường hợp không có lỗi, trả lại hành vi mặc định của form là submit
                else {
                    formElement.submit();
                }
            }
        }

        // Lặp qua mỗi rule và xử lý (lắng nghe sự kiện blue, input,...)
        options.rules.forEach(function(rule){
            if (Array.isArray(selectorRules[rule.selector])) {
                selectorRules[rule.selector].push(rule.test);
            }else {
                selectorRules[rule.selector] = [rule.test];
            }

            var inputElements = formElement.querySelectorAll(rule.selector); 

            Array.from(inputElements).forEach(function(inputElement){
                if(inputElement){
                    //Xử lí trường hợp blur khỏi input
                    inputElement.onblur = function (){
                        validate(inputElement,rule)
                    }
    
                    // Xử lí mỗi khi người dùng nhập vào input
                    inputElement.oninput = function(){
                        var errorElement = getParent(inputElement, options.formGroupSelector).querySelector(options.errorSelector); // lấy span lỗi
                        errorElement.innerText = '';
                        getParent(inputElement, options.formGroupSelector).classList.remove('invalid');
                    }
                }
            });


        });

    }
}

// Định nghĩa rules
// Nguyên tắc của các rules:
/*
    1. Khi có lỗi => trả ra message lỗi
    2.khi hợp lệ => không trả ra gì cả (undefined)

*/ 
Validator.isRequired = function (selector, message){
    return{
        selector :selector,
        test: function(value){
            return value ? undefined : message || 'This field can\'t be blank'
        }
    };
}
Validator.isEmail = function (selector, message){
    return{
        selector :selector,
        test: function(value){
            var regex = /^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$/
            return regex.test(value) ? undefined : message || 'Please enter a valid email.'
        }
    };
}
Validator.minLength = function (selector, min, message){
    return{
        selector :selector,
        test: function(value){
            return value.length >= min ? undefined : message || `Password is too short (minimum is ${min} characters)`
        }
    };
}

Validator.isConfirmed = function (selector, getConfirmValue, message){
    return {
        selector: selector,
        test: function(value){
            return value === getConfirmValue()? undefined : message || 'Giá trị nhập vào không chính xác'
        }
    }
}
Validator.isDate = function (selector, message) {
    return {
        selector: selector,
        test: function (value) {
            var regex = /^([0-9]{4}|[0-9]{4})[./-]([0]?[1-9]|[1][0-2])[./-]([0]?[1-9]|[1|2][0-9]|[3][0|1])$/
            return regex.test(value) ? undefined : message || 'Please enter a valid date.'
        }
    };
}