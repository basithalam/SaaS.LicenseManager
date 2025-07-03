using Microsoft.AspNetCore.Mvc;
using SaaS.LicenseManager.Models;

namespace SaaS.LicenseManager.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;
        private const string AdminSessionKey = "IsAdmin";

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var user = _context.AdminUsers.FirstOrDefault(a =>
                a.Username == username && a.Password == password);

            if (user == null)
            {
                ViewBag.Error = "Invalid credentials";
                return View();
            }

            HttpContext.Session.SetString(AdminSessionKey, "true");
            return RedirectToAction("Index", "Customer");
        }

        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetString(AdminSessionKey) != "true")
                return RedirectToAction("Login");

            var stats = new
            {
                Total = _context.Customers.Count(),
                Active = _context.Customers.Count(c => c.IsActive),
                Expired = _context.Customers.Count(c => c.LicenseEnd < DateTime.UtcNow)
            };

            return View(stats);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove(AdminSessionKey);
            return RedirectToAction("Login");
        }
    }
}