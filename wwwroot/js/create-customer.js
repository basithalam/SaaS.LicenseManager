$(document).ready(function () {
    var countryCodeSelect = $("#CountryCode");
    var countrySelect = $("#Country");

    // Populate country code dropdown
    countryCodes.forEach(function (country) {
        countryCodeSelect.append(`<option value="${country.dial_code}">${country.name} (${country.dial_code})</option>`);
    });

    // Populate country dropdown
    countryCodes.forEach(function (country) {
        countrySelect.append(`<option value="${country.name}">${country.name}</option>`);
    });

    // Combine country code and phone number before form submission
    $("#customerForm").submit(function () {
        var countryCode = $("#CountryCode").val();
        var phoneNumber = $("#PhoneNumberInput").val();
        if (countryCode && phoneNumber) {
            $("input[name='PhoneNumber']").val(countryCode + phoneNumber);
        }
    });
});
