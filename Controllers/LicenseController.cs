using Microsoft.AspNetCore.Mvc;
using SaaS.LicenseManager.Models;

namespace SaaS.LicenseManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LicenseController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LicenseController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("Validate")]
        public IActionResult Validate(string key)
        {
            var customer = _context.Customers.FirstOrDefault(c =>
                c.LicenseKey == key &&
                c.IsActive &&
                c.LicenseEnd >= DateTime.UtcNow
            );

            if (customer == null)
                return NotFound(new { valid = false, message = "License key is invalid or expired." });

            return Ok(new
            {
                valid = true,
                fullName = customer.FullName,
                licenseType = customer.LicenseType.ToString(),
                expiresOn = customer.LicenseEnd.ToString("yyyy-MM-dd")
            });
        }

        
    }
}