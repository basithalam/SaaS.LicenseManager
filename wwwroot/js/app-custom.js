$(function () {
    // Animate card on load (for login/register/create)
    $('.card').hide().fadeIn(700);

    // jQuery client-side validation and shake effect (for login/register/create)
    $('#adminLoginForm, #customerForm').on('submit', function (e) {
        var valid = true;
        $(this).find('input, select').each(function () {
            if (!this.checkValidity()) {
                $(this).addClass('is-invalid shake');
                valid = false;
            } else {
                $(this).removeClass('is-invalid shake');
            }
        });
        if (!valid) {
            e.preventDefault();
            setTimeout(function () {
                $('.shake').removeClass('shake');
            }, 400);
        }
    });

    // Remove invalid class on input
    $('input, select').on('input change', function () {
        if (this.checkValidity()) {
            $(this).removeClass('is-invalid');
        }
    });

    // Fade effect on status toggle (for index page)
    $(".toggle-form").on("submit", function (e) {
        var row = $(this).closest("tr");
        row.fadeTo(200, 0.3);
    });
});