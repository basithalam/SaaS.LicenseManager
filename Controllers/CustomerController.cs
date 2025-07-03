using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SaaS.LicenseManager.Filters;
using SaaS.LicenseManager.Helpers;
using SaaS.LicenseManager.Models;
using SaaS.LicenseManager.Services;
using Stripe.Checkout;

namespace SaaS.LicenseManager.Controllers
{
    public class CustomerController : Controller
    {
        private readonly AppDbContext _context;
        private readonly EmailService _emailService;

        public CustomerController(AppDbContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        [AdminAuthorize]

        public async Task<IActionResult> Index(string searchName, string searchCountry, string searchKey)
        {
            var query = _context.Customers.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchName))
                query = query.Where(c => c.FullName.Contains(searchName));

            if (!string.IsNullOrWhiteSpace(searchCountry))
                query = query.Where(c => c.Country.Contains(searchCountry));

            if (!string.IsNullOrWhiteSpace(searchKey))
                query = query.Where(c => c.LicenseKey.Contains(searchKey));

            var customers = await query.ToListAsync();
            return View(customers);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Customer customer)
        {
            customer.LicenseKey = LicenseKeyGenerator.GenerateKey(customer.EmailAddress);
            customer.LicenseStart = DateTime.UtcNow;
            _emailService.SendLicenseEmail(customer.EmailAddress, customer.LicenseKey);

            switch (customer.LicenseType)
            {
                case LicenseType.Trial7Days:
                    customer.LicenseEnd = customer.LicenseStart.AddDays(7); break;
                case LicenseType.Monthly:
                    customer.LicenseEnd = customer.LicenseStart.AddMonths(1); break;
                case LicenseType.Yearly:
                    customer.LicenseEnd = customer.LicenseStart.AddYears(1); break;
            }

            customer.IsActive = true;

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            // Email logic will go here (in next step)

            return RedirectToAction(nameof(Index));
        }

        // Toggle activation status
        [HttpPost]
        public async Task<IActionResult> Toggle(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null) return NotFound();

            customer.IsActive = !customer.IsActive;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // Update expiration date
        [HttpPost]
        public async Task<IActionResult> UpdateExpire(int id, DateTime newExpire)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
                return NotFound();

            customer.LicenseEnd = newExpire;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        // Create stripe session
        [HttpPost]
        public async Task<IActionResult> CreateStripeSession([FromBody] StripeSessionRequest req)
        {
            // Set license prices in USD
            decimal monthlyPrice = 10.00M;
            decimal yearlyPrice = 100.00M;

            decimal selectedPrice = req.LicenseType == "Monthly" ? monthlyPrice : yearlyPrice;

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                Mode = "payment",
                SuccessUrl = Url.Action("Create", "Customer", new { payment = "success" }, Request.Scheme),
                CancelUrl = Url.Action("Create", "Customer", new { payment = "cancel" }, Request.Scheme),
                CustomerEmail = req.Email,
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = "usd",
                            UnitAmount = (long)(selectedPrice * 100), // Convert to cents
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = $"{req.LicenseType} License"
                            }
                        },
                        Quantity = 1
                    }
                }
            };

            var service = new SessionService();
            Session session = await service.CreateAsync(options);
            return Json(new { sessionId = session.Id });
        }
    }

    public class StripeSessionRequest
    {
        public string LicenseType { get; set; }
        public string Email { get; set; }
    }

}