
    function setPasswordEditor(container, options) {
        $('<input type="password" id="password" name="' + options.field + '" title="Password" required="required" autocomplete="off" aria-labelledby="Password-form-label" data-bind="value: Password" aria-describedby="Password-form-hint"/>')
            .appendTo(container)
            .kendoTextBox();
    }
    function EmailEditor(container, options) {
        $('<input type="email" id="email" name="' + options.field + '" title="Email" required="required" />')
            .appendTo(container)
            .kendoTextBox();
    }

    function onFormValidateField(e) {
        $("#validation-success").html("");
    }

    function onFormSubmit(e) {
        e.preventDefault();

    $.ajax({
        url: '/FoodHub/SignIn',
    method: 'POST',
    data: {

        email: $('#email').val(),
    password: $('#password').val()

            },
    success: function (result) {
                if (result == "success") {
        window.location.href = "FoodHub/Main";
                } else {
        $("#validation-success").html("<div class='k-messagebox k-messagebox-error'>Wrong Email or Password</div>");
                }
            },
    error: function (jqXHR) {

    }
        })
    }

    function onFormClear(e) {
        $("#validation-success").html("");
    }