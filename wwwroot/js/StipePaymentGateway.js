// stripe-checkout.js

// Initialize Stripe with your publishable key
let stripe;
let monthlyPriceDisplay = document.getElementById('monthly-price');
let yearlyPriceDisplay = document.getElementById('yearly-price');

document.addEventListener("DOMContentLoaded", function () {
    fetch('/Customer/GetStripePublishableKey')
        .then(response => response.json())
        .then(data => {
            stripe = Stripe(data.publishableKey);
        })
        .catch(error => {
            console.error('Error fetching Stripe publishable key:', error);
            document.getElementById('payment-message').innerText = 'Error initializing payment. Please try again.';
        });

    // Fetch and display prices
    fetch('/Customer/GetStripePrices')
        .then(response => response.json())
        .then(data => {
            if (data.monthly) {
                monthlyPriceDisplay.innerText = `${data.monthly.toFixed(2)}`;
            }
            if (data.yearly) {
                yearlyPriceDisplay.innerText = `${data.yearly.toFixed(2)}`;
            }
        })
        .catch(error => {
            console.error('Error fetching Stripe prices:', error);
            document.getElementById('payment-message').innerText = 'Error fetching prices. Please try again.';
        });

    const licenseType = document.getElementById('LicenseType');
    const paymentSection = document.getElementById('payment-section');
    const checkoutBtn = document.getElementById('stripe-checkout-btn');
    const registerBtn = document.getElementById('register-btn'); // Use the new ID

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
                    email: email,
                    fullName: document.getElementById('FullName').value,
                    country: document.getElementById('Country').value,
                    phoneNumber: document.getElementById('CountryCode').value + document.getElementById('PhoneNumberInput').value
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