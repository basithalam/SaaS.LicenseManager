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
        public async Task<IActionResult> Login(AdminUser model)
        {
            var user = await _context.AdminUsers.FirstOrDefaultAsync(a => a.Username == model.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
            {
                ViewBag.Error = "Invalid credentials";
                return View();
            }

            HttpContext.Session.SetString(AdminSessionKey, "true");
            HttpContext.Session.SetString(AdminUsernameSessionKey, user.Username);
            return RedirectToAction("Dashboard", "Admin");
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

        [AdminAuthorize]
        public IActionResult Dashboard()
        {
            return RedirectToAction("Index", "Customer");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove(AdminSessionKey);
            return RedirectToAction("Login");
        }

        [AdminAuthorize]
        public IActionResult AddAdmin()
        {
            return View();
        }

        [AdminAuthorize]
        [HttpPost]
        public async Task<IActionResult> AddAdmin(AdminUser adminUser)
        {
            if (ModelState.IsValid)
            {
                if (await _context.AdminUsers.AnyAsync(a => a.Username == adminUser.Username || a.Email == adminUser.Email))
                {
                    ModelState.AddModelError("", "Admin user with this username or email already exists.");
                    return View(adminUser);
                }

                adminUser.Password = BCrypt.Net.BCrypt.HashPassword(adminUser.Password);
                _context.AdminUsers.Add(adminUser);
                await _context.SaveChangesAsync();
                ViewBag.Success = "New admin user added successfully!";
                return RedirectToAction("Profile");
            }
            return View(adminUser);
        }
    }
}