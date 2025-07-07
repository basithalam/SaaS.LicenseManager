$(document).ready(function () {
    var countryCodeSelect = $("#CountryCode");

    // Populate dropdown from the local list
    countryCodes.forEach(function (country) {
        countryCodeSelect.append(`<option value="${country.dial_code}">${country.name} (${country.dial_code})</option>`);
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
