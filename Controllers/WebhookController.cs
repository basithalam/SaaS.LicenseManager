//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Options;
//using Stripe;
//using Stripe.Checkout;
//using SaaS.LicenseManager.Models;

//namespace SaaS.LicenseManager.Controllers
//{
//    [Route("webhook/stripe")]
//    [ApiController]
//    public class WebhookController : ControllerBase
//    {
//        private readonly AppDbContext _context;
//        private readonly StripeSettings _settings;

//        public WebhookController(AppDbContext context, IOptions<StripeSettings> stripeOptions)
//        {
//            _context = context;
//            _settings = stripeOptions.Value;
//        }

//        [HttpPost]
//        public async Task<IActionResult> StripeWebhook()
//        {
//            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

//            try
//            {
//                var stripeEvent = EventUtility.ConstructEvent(
//                    json,
//                    Request.Headers["Stripe-Signature"],
//                    _settings.WebhookSecret
//                );

//                //if (stripeEvent.Type == Events.CheckoutSessionCompleted)
//                //{
//                //    var session = stripeEvent.Data.Object as Session;

//                //    var email = session?.CustomerEmail;
//                //    if (email != null)
//                //    {
//                //        // Activate or create license for this customer
//                //        var customer = _context.Customers.FirstOrDefault(c => c.EmailAddress == email);

//                //        if (customer != null)
//                //        {
//                //            // Enable and extend license
//                //            customer.IsActive = true;
//                //            customer.LicenseStart = DateTime.UtcNow;

//                //            customer.LicenseEnd = customer.LicenseType switch
//                //            {
//                //                LicenseType.Trial7Days => DateTime.UtcNow.AddDays(7),
//                //                LicenseType.Monthly => DateTime.UtcNow.AddMonths(1),
//                //                LicenseType.Yearly => DateTime.UtcNow.AddYears(1),
//                //                _ => DateTime.UtcNow
//                //            };

//                //            await _context.SaveChangesAsync();
//                //        }
//                //        else
//                //        {
//                //            // Optional: Create a new record if not found
//                //        }
//                //    }
//                //}

//                return Ok();
//            }
//            catch (Exception ex)
//            {
//                return BadRequest($"Webhook Error: {ex.Message}");
//            }
//        }
//    }
//}