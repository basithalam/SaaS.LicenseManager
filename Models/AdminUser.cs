namespace SaaS.LicenseManager.Models
{
    public class AdminUser
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty; // In production, hash this!
    }
}