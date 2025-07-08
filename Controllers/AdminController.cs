using Microsoft.AspNetCore.Mvc;
using SaaS.LicenseManager.Models;
using BCrypt.Net;
using SaaS.LicenseManager.Filters;
using Microsoft.EntityFrameworkCore;

namespace SaaS.LicenseManager.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;
        private const string AdminSessionKey = "IsAdmin";
        private const string AdminUsernameSessionKey = "AdminUsername";

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = await _context.AdminUsers.FirstOrDefaultAsync(a => a.Username == username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                ViewBag.Error = "Invalid credentials";
                return View();
            }

            HttpContext.Session.SetString(AdminSessionKey, "true");
            HttpContext.Session.SetString(AdminUsernameSessionKey, user.Username);
            return RedirectToAction("Index", "Customer");
        }

        [AdminAuthorize]
        public async Task<IActionResult> Profile()
        {
            var username = HttpContext.Session.GetString(AdminUsernameSessionKey);
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login");
            }

            var adminUser = await _context.AdminUsers.FirstOrDefaultAsync(a => a.Username == username);
            if (adminUser == null)
            {
                return NotFound();
            }

            return View(adminUser);
        }

        [AdminAuthorize]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(string currentPassword, string newPassword, string confirmNewPassword)
        {
            var username = HttpContext.Session.GetString(AdminUsernameSessionKey);
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login");
            }

            var adminUser = await _context.AdminUsers.FirstOrDefaultAsync(a => a.Username == username);
            if (adminUser == null)
            {
                return NotFound();
            }

            if (!BCrypt.Net.BCrypt.Verify(currentPassword, adminUser.Password))
            {
                ModelState.AddModelError("currentPassword", "Invalid current password.");
                return View("Profile", adminUser);
            }

            if (newPassword != confirmNewPassword)
            {
                ModelState.AddModelError("newPassword", "New password and confirmation do not match.");
                return View("Profile", adminUser);
            }

            adminUser.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
            await _context.SaveChangesAsync();

            ViewBag.Success = "Password changed successfully!";
            return View("Profile", adminUser);
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

        // TEMPORARY: Use this action to generate a BCrypt hash for a password.
        // REMOVE THIS ACTION AFTER YOU HAVE UPDATED YOUR ADMIN PASSWORD IN THE DATABASE.
        //public ContentResult HashPassword(string password)
        //{
        //    if (string.IsNullOrEmpty(password))
        //    {
        //        return Content("Please provide a password to hash.");
        //    }
        //    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
        //    return Content($"Hashed Password: {hashedPassword}");
        //}
    }
}