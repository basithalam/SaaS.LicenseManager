using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;
using SaaS.LicenseManager.Models;
using SaaS.LicenseManager.Services;
using SaaS.LicenseManager.Helpers;

namespace SaaS.LicenseManager.Controllers
{
    [Route("webhook/stripe")]
    [ApiController]
    public class WebhookController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly StripeSettings _settings;
        private readonly EmailService _emailService;
        private readonly ILogger<WebhookController> _logger;

        public WebhookController(AppDbContext context, IOptions<StripeSettings> stripeOptions, EmailService emailService, ILogger<WebhookController> logger)
        {
            _context = context;
            _settings = stripeOptions.Value;
            _emailService = emailService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> StripeWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            _logger.LogInformation("Stripe webhook received.");

            try
            {
                var stripeEvent = EventUtility.ConstructEvent(
                    json,
                    Request.Headers["Stripe-Signature"],
                    _settings.WebhookSecret
                );
                _logger.LogInformation($"Stripe event type: {stripeEvent.Type}");

                if (stripeEvent.Type == "checkout.session.completed")
                {
                    _logger.LogInformation("Checkout session completed event.");
                    var sessionFromEvent = stripeEvent.Data.Object as Session;
                    var sessionService = new SessionService();
                    var session = sessionService.Get(sessionFromEvent.Id, new SessionGetOptions
                    {
                        Expand = new List<string> { "line_items", "payment_intent" }
                    });

                    var email = session?.CustomerEmail;
                    if (email != null)
                    {
                        _logger.LogInformation($"Processing email: {email}");
                        // Activate or create license for this customer
                        var customer = _context.Customers.FirstOrDefault(c => c.EmailAddress == email);

                        if (customer != null)
                        {
                            _logger.LogInformation($"Customer found: {customer.EmailAddress}. Updating license.");
                            // Enable and extend license
                            customer.IsActive = true;
                            customer.LicenseStart = DateTime.UtcNow;

                            customer.LicenseEnd = customer.LicenseType switch
                            {
                                LicenseType.Trial7Days => DateTime.UtcNow.AddDays(7),
                                LicenseType.Monthly => DateTime.UtcNow.AddMonths(1),
                                LicenseType.Yearly => DateTime.UtcNow.AddYears(1),
                                _ => DateTime.UtcNow
                            };

                            await _context.SaveChangesAsync();
                            await _emailService.SendLicenseEmail(customer.EmailAddress, customer.LicenseKey, customer.LicenseEnd.ToShortDateString());
                            _logger.LogInformation($"License updated and email sent for {customer.EmailAddress}");
                        }
                        else
                        {
                            _logger.LogInformation($"Creating new customer for {email}.");
                            try
                            {
                                var metadata = session.PaymentIntent.Metadata;
                                _logger.LogInformation($"Metadata received: LicenseType={metadata.GetValueOrDefault("LicenseType", "N/A")}, FullName={metadata.GetValueOrDefault("FullName", "N/A")}, Country={metadata.GetValueOrDefault("Country", "N/A")}, PhoneNumber={metadata.GetValueOrDefault("PhoneNumber", "N/A")}");

                                metadata.TryGetValue("FullName", out var fullName);
                                metadata.TryGetValue("Country", out var country);
                                metadata.TryGetValue("PhoneNumber", out var phoneNumber);
                                metadata.TryGetValue("LicenseType", out var licenseTypeStr);


                                var newCustomer = new SaaS.LicenseManager.Models.Customer
                                {
                                    EmailAddress = email,
                                    FullName = fullName ?? string.Empty,
                                    Country = country ?? string.Empty,
                                    PhoneNumber = phoneNumber ?? string.Empty,
                                    LicenseType = licenseTypeStr switch
                                    {
                                        "Monthly" => LicenseType.Monthly,
                                        "Yearly" => LicenseType.Yearly,
                                        _ => LicenseType.Trial7Days // Fallback
                                    },
                                    IsActive = true,
                                    LicenseStart = DateTime.UtcNow
                                };

                                newCustomer.LicenseEnd = newCustomer.LicenseType switch
                                {
                                    LicenseType.Trial7Days => DateTime.UtcNow.AddDays(7),
                                    LicenseType.Monthly => DateTime.UtcNow.AddMonths(1),
                                    LicenseType.Yearly => DateTime.UtcNow.AddYears(1),
                                    _ => DateTime.UtcNow
                                };

                                newCustomer.LicenseKey = LicenseKeyGenerator.GenerateKey(newCustomer.EmailAddress);
                                _logger.LogInformation($"New customer object created: Email={newCustomer.EmailAddress}, Name={newCustomer.FullName}, License={newCustomer.LicenseType}");

                                _context.Customers.Add(newCustomer);
                                _logger.LogInformation("Adding new customer to context.");

                                await _context.SaveChangesAsync();
                                _logger.LogInformation("SaveChangesAsync successful. Customer saved to database.");

                                await _emailService.SendLicenseEmail(newCustomer.EmailAddress, newCustomer.LicenseKey, newCustomer.LicenseEnd.ToShortDateString());
                                _logger.LogInformation($"License email sent to {newCustomer.EmailAddress}.");
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, $"EXCEPTION while creating new customer for {email}. Message: {ex.Message}\nStackTrace: {ex.StackTrace}");
                                // Rethrow the exception to be caught by the outer handler
                                throw;
                            }
                        }
                    }
                    else
                    {
                        _logger.LogWarning("Customer email is null in session.");
                    }
                }

                return Ok();
            }
            catch (StripeException e)
            {
                _logger.LogError(e, "Error processing Stripe webhook.");
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception in Stripe webhook.");
                return BadRequest();
            }
        }
    }
}