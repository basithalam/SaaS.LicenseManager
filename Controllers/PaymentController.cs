//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Options;
//using SaaS.LicenseManager.Models;
//using Stripe.Checkout;

//namespace SaaS.LicenseManager.Controllers
//{
//    public class PaymentController : Controller
//    {
//        private readonly StripeSettings _settings;

//        public PaymentController(IOptions<StripeSettings> options)
//        {
//            _settings = options.Value;
//        }


//        [HttpPost]
//        public IActionResult CreateCheckoutSession(string email, string plan)
//        {
//            var domain = "https://localhost:5001"; // Change to your deployed URL

//            var priceId = plan switch
//            {
//                "Trial" => "price_7days_id",
//                "Monthly" => "price_monthly_id",
//                "Yearly" => "price_yearly_id",
//                _ => throw new Exception("Invalid plan")
//            };

//            var options = new SessionCreateOptions
//            {
//                CustomerEmail = email,
//                PaymentMethodTypes = new List<string> { "card" },
//                LineItems = new List<SessionLineItemOptions>
//                {
//                    new SessionLineItemOptions
//                    {
//                        Price = priceId,
//                        Quantity = 1
//                    }
//                },
//                Mode = "payment",
//                SuccessUrl = $"{domain}/Customer/Success?session_id={{CHECKOUT_SESSION_ID}}",
//                CancelUrl = $"{domain}/Customer/Cancel"
//            };

//            var service = new SessionService();
//            Session session = service.Create(options);

//            return Redirect(session.Url);
//        }
//    }
//}