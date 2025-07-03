// stripe-checkout.js

// Initialize Stripe with your publishable key
const stripe = Stripe('pk_test_YOUR_PUBLISHABLE_KEY'); // Replace with your actual key

document.addEventListener("DOMContentLoaded", function () {
    const licenseType = document.getElementById('LicenseType');
    const paymentSection = document.getElementById('payment-section');
    const checkoutBtn = document.getElementById('stripe-checkout-btn');
    const registerBtn = document.querySelector('button[type="submit"].btn-success');

    function updatePaymentUI() {
        if (licenseType.value === 'Monthly' || licenseType.value === 'Yearly') {
            paymentSection.style.display = '';
            if (registerBtn) registerBtn.style.display = 'none';
        } else {
            paymentSection.style.display = 'none';
            if (registerBtn) registerBtn.style.display = '';
        }
    }

    if (licenseType) {
        licenseType.addEventListener('change', updatePaymentUI);
        updatePaymentUI(); // Initial UI setup
    }

    if (checkoutBtn) {
        checkoutBtn.addEventListener('click', function (e) {
            e.preventDefault();

            const email = document.getElementById('EmailAddress').value;

            fetch('/Customer/CreateStripeSession', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({
                    licenseType: licenseType.value,
                    email: email
                })
            })
                .then(res => res.json())
                .then(data => {
                    if (data.sessionId) {
                        stripe.redirectToCheckout({ sessionId: data.sessionId });
                    } else {
                        document.getElementById('payment-message').innerText = 'Payment error. Please try again.';
                    }
                });
        });
    }
});