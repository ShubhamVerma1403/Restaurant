
    function setPasswordEditor(container, options) {
        $('<input type="password" id="password" name="' + options.field + '" title="Password" required="required" autocomplete="off" aria-labelledby="Password-form-label" data-bind="value: Password" aria-describedby="Password-form-hint"/>')
            .appendTo(container)
            .kendoTextBox();
        }
    function emailEditor(container, options) {
        $('<input type="email" id="email" name="' + options.field + '" title="Email" required="required" />')
            .appendTo(container)
            .kendoTextBox();
        }
    function firstNameEditor(container, options) {
        $('<input type="name" id="Firstname" name="' + options.field + '" title="First Name" required="required" />')
            .appendTo(container)
            .kendoTextBox();
        }
    function lastNameEditor(container, options) {
        $('<input type="name" id="Lastname" name="' + options.field + '" title="Last Name" required="required" />')
            .appendTo(container)
            .kendoTextBox();
        }
    function phoneEditor(container, options) {
        $('<input type="tel" id="phone" name="' + options.field + '" title="Phone No." required="required" />')
            .appendTo(container)
            .kendoTextBox();
        }

    function onFormValidateField(e) {
        $("#validation-success").html("");
    }

    function onFormSubmit(e) {
        e.preventDefault();
    $.ajax({
        url: 'https://localhost:44348/api/values/signup',
    method: 'POST',
    data: {
        lastName: $('#Lastname').val(),
    firstName: $('#Firstname').val(),
    email: $('#email').val(),
    password: $('#password').val(),
    phoneNo_: $('#phone').val()
            },
    success: function (result) {
        $("#validation-success").html("<div class='k-messagebox k-messagebox-success'>Registration Successfull! GO to SignIn Page</div>");
                
            },
    error: function (msg) {
        $("#validation-success").html("<div class='k-messagebox k-messagebox-error'>User already exist</div>");
            }
        })
    }

    function onFormClear(e) {
        $("#validation-success").html("");
    }

